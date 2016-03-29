using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using System;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddRefinanceApplicationCommandHandler : IDomainServiceCommandHandler<AddRefinanceApplicationCommand, RefinanceApplicationAddedEvent>
    {
        private IServiceCommandRouter serviceCommandRouter;
        private IApplicationDataManager applicationDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;
        private IApplicationManager applicationManager;
        private IDomainRuleManager<RefinanceApplicationModel> domainRuleContext;

        public AddRefinanceApplicationCommandHandler(IServiceCommandRouter serviceCommandRouter, IApplicationDataManager applicationDataManager, ILinkedKeyManager linkedKeyManager,
            IUnitOfWorkFactory uowFactory, IEventRaiser eventRaiser, IApplicationManager applicationManager, IDomainRuleManager<RefinanceApplicationModel> domainRuleContext)
        {
            this.serviceCommandRouter = serviceCommandRouter;
            this.applicationDataManager = applicationDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
            this.applicationManager = applicationManager;
            this.domainRuleContext = domainRuleContext;

            domainRuleContext.RegisterRule(new RefinanceRequestedLoanAmountMustBeAboveMinimumRequiredRule(this.applicationDataManager));
        }

        public ISystemMessageCollection HandleCommand(AddRefinanceApplicationCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            
            domainRuleContext.ExecuteRules(messages, command.RefinanceApplicationModel);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var uow = uowFactory.Build())
            {
                // reserve an accountkey
                int reservedAccountKey = applicationDataManager.GetReservedAccountNumber();

                int applicationNumber = applicationManager.SaveApplication(command.RefinanceApplicationModel.ApplicationType,
                                                                       command.RefinanceApplicationModel.ApplicationStatus,
                                                                       DateTime.Now, command.RefinanceApplicationModel.ApplicationSourceKey,
                                                                       reservedAccountKey,
                                                                       SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans,
                                                                       command.RefinanceApplicationModel.Reference,
                                                                       command.RefinanceApplicationModel.ApplicantCount);

                applicationManager.SaveApplicationMortgageLoan(applicationNumber,
                                                                     MortgageLoanPurpose.Refinance,
                                                                     command.RefinanceApplicationModel.ApplicantCount,
                                                                     null,
                                                                     command.RefinanceApplicationModel.EstimatedPropertyValue,
                                                                     null);

                int offerInformationKey = applicationManager.SaveApplicationInformation(DateTime.Now, applicationNumber, OfferInformationType.OriginalOffer,
                    command.RefinanceApplicationModel.Product);

                applicationManager.SaveRefinanceApplicationInformationVariableLoan(offerInformationKey, command.RefinanceApplicationModel.Term,
                    command.RefinanceApplicationModel.EstimatedPropertyValue, command.RefinanceApplicationModel.CashOut, command.RefinanceApplicationModel.LoanAmountNoFees);

                applicationManager.SaveApplicationInformationInterestOnly(offerInformationKey);

                // set the external originator attribute for non sahl origination sources
                if (command.RefinanceApplicationModel.OriginationSource != SAHL.Core.BusinessModel.Enums.OriginationSource.SAHomeLoans)
                {
                    SetExternalOriginatorAttributeCommand setExternalOriginatorAttributeCommand = new SetExternalOriginatorAttributeCommand(applicationNumber,
                        command.RefinanceApplicationModel.OriginationSource);
                    messages = serviceCommandRouter.HandleCommand(setExternalOriginatorAttributeCommand, metadata);
                    if (messages.HasErrors)
                    {
                        uow.Complete();
                        return messages;
                    }
                }
                applicationManager.SaveApplicationInformationQuickCash(offerInformationKey);

                // we need to link the guid to the application
                linkedKeyManager.LinkKeyToGuid(applicationNumber, command.ApplicationId);

                var date = DateTime.Now;
                var RefinanceApplicationAddedEvent = new RefinanceApplicationAddedEvent(date, applicationNumber, command.RefinanceApplicationModel.ApplicationType,
                    command.RefinanceApplicationModel.ApplicationStatus, command.RefinanceApplicationModel.ApplicationSourceKey, command.RefinanceApplicationModel.OriginationSource
                    , command.RefinanceApplicationModel.EstimatedPropertyValue, command.RefinanceApplicationModel.Term, command.RefinanceApplicationModel.CashOut,
                    command.RefinanceApplicationModel.Product);
                eventRaiser.RaiseEvent(date, RefinanceApplicationAddedEvent, applicationNumber, (int)GenericKeyType.Offer, metadata);
                uow.Complete();
            }

            return messages;
        }
    }
}