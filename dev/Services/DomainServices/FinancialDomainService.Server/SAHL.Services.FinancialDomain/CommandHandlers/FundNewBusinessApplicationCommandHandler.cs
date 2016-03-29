using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.FinancialDomain.CommandHandlers
{
    public class FundNewBusinessApplicationCommandHandler : IDomainServiceCommandHandler<FundNewBusinessApplicationCommand, NewBusinessApplicationFundedEvent>
    {
        private readonly IFinancialDataManager financialDataManager;
        private readonly IFinancialManager financialManager;
        private readonly IEventRaiser eventRaiser;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IDomainRuleManager<IApplicationModel> domainRuleContext;
        private IServiceQueryRouter serviceQueryRouter;

        public FundNewBusinessApplicationCommandHandler(IServiceQueryRouter serviceQueryRouter, IFinancialDataManager financialDataManager, IFinancialManager financialManager,
            IEventRaiser eventRaiser, IUnitOfWorkFactory unitOfWorkFactory, IDomainRuleManager<IApplicationModel> domainRuleContext)
        {
            this.serviceQueryRouter = serviceQueryRouter;
            this.financialDataManager = financialDataManager;
            this.financialManager = financialManager;
            this.eventRaiser = eventRaiser;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.domainRuleContext = domainRuleContext;

            this.domainRuleContext.RegisterRule(new ApplicationMayNotBeAcceptedWhenFundingNewBusinessApplicationRule(financialDataManager));
            this.domainRuleContext.RegisterRule(new ApplicationMustBeANewBusinessMortgageLoanWhenFundingRule(financialDataManager));
        }

        public ISystemMessageCollection HandleCommand(FundNewBusinessApplicationCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            // run the rules
            domainRuleContext.ExecuteRules(messages, command);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var unitOfWork = unitOfWorkFactory.Build())
            {
                // fetch the application information data
                var applicationInformation = financialDataManager.GetLatestApplicationOfferInformation(command.ApplicationNumber);
                // fetch the application loan information, applied fee settings and application data
                var applicationVariableLoanInformation = financialDataManager.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey);
                var applicationFeeAttributes = financialDataManager.GetFeeApplicationAttributes(command.ApplicationNumber);
                var application = financialDataManager.GetApplication(command.ApplicationNumber);

                // determine the SPV
                var determineSPVQuery = new DetermineSPVQuery(command.ApplicationNumber, (EmploymentType)applicationVariableLoanInformation.EmploymentTypeKey,
                    (decimal)applicationVariableLoanInformation.HouseholdIncome, applicationFeeAttributes.StaffHomeLoan, (decimal)applicationVariableLoanInformation.LTV, applicationFeeAttributes.GEPF);

                messages.Aggregate(this.serviceQueryRouter.HandleQuery(determineSPVQuery));
                if (!messages.HasErrors && determineSPVQuery.Result.Results.Count() == 1)
                {
                    // set the applications SPV and Reset Configuration
                    var determinedSPV = determineSPVQuery.Result.Results.SingleOrDefault();
                    financialManager.SetApplicationInformationSPVKey(applicationInformation.OfferInformationKey, determinedSPV);
                    financialDataManager.SetApplicationResetConfiguration(command.ApplicationNumber, determinedSPV, applicationInformation.ProductKey.Value);

                    // raise the command event
                    var newBusinessApplicationFundedEvent = new NewBusinessApplicationFundedEvent(DateTime.Now, command.ApplicationNumber);
                    eventRaiser.RaiseEvent(DateTime.Now, newBusinessApplicationFundedEvent, command.ApplicationNumber, (int)GenericKeyType.Offer, metadata);

                    // complete the unit of work
                    unitOfWork.Complete();
                }
            }

            return messages;
        }
    }
}