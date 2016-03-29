using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Rules;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Events;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using SAHL.Shared.BusinessModel.Calculations;
using System;

namespace SAHL.Services.FinancialDomain.CommandHandlers
{
    public class PriceNewBusinessApplicationCommandHandler : IDomainServiceCommandHandler<PriceNewBusinessApplicationCommand, NewBusinessApplicationPricedEvent>
    {
        private readonly IFinancialDataManager financialDataManager;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IEventRaiser eventRaiser;
        private readonly IApplicationCalculator applicationCalculator;
        private readonly IDomainRuleManager<IApplicationModel> domainRuleContext;
        private readonly IFinancialManager financialManager;
        private readonly ILoanCalculations functionsUtils;

        public PriceNewBusinessApplicationCommandHandler(IFinancialDataManager financialDataManager,
                                                         IUnitOfWorkFactory unitOfWorkFactory,
                                                         IEventRaiser eventRaiser,
                                                         IApplicationCalculator applicationCalculator,
                                                         IDomainRuleManager<IApplicationModel> domainRuleContext,
                                                         IFinancialManager financialManager, ILoanCalculations functionsUtils)
        {
            this.financialDataManager = financialDataManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.eventRaiser = eventRaiser;
            this.applicationCalculator = applicationCalculator;
            this.domainRuleContext = domainRuleContext;
            this.financialManager = financialManager;
            this.functionsUtils = functionsUtils;

            this.domainRuleContext.RegisterRule(new ApplicationMayNotBeAcceptedWhenPricingNewBusinessApplicationRule(financialDataManager));
            this.domainRuleContext.RegisterRule(new ApplicationMustBeANewBusinessMortgageLoanWhenPricingRule(financialDataManager));
        }

        public ISystemMessageCollection HandleCommand(PriceNewBusinessApplicationCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();

            // run the rules
            domainRuleContext.ExecuteRules(messages, command);
            if (messages.HasErrors)
            {
                return messages;
            }

            using (var unitOfWork = unitOfWorkFactory.Build())
            {
                var application = financialDataManager.GetApplication(command.ApplicationNumber);
                // fetch the basic the application information data
                var applicationInformation = financialDataManager.GetLatestApplicationOfferInformation(command.ApplicationNumber);
                // fetch the application loan information and applied fee settings
                var applicationInformationMortgageLoan = financialDataManager.GetApplicationInformationMortgageLoan(applicationInformation.OfferInformationKey);
                var applicationFeeAttributes = financialDataManager.GetFeeApplicationAttributes(command.ApplicationNumber);

                var employmentType = applicationInformationMortgageLoan.EmploymentType.HasValue ? applicationInformationMortgageLoan.EmploymentType.Value : EmploymentType.Salaried;

                // determine the application fees
                var ltvBeforeFees = functionsUtils.CalculateLTV(applicationInformationMortgageLoan.LoanAmountNoFees, applicationInformationMortgageLoan.PropertyValuation);
                var applicationFees = financialDataManager.CalculateOriginationFees(applicationInformationMortgageLoan.LoanAmountNoFees,
                                    applicationInformationMortgageLoan.BondToRegister.HasValue ? applicationInformationMortgageLoan.BondToRegister.Value : 0,
                                    (OfferType)applicationInformationMortgageLoan.OfferType,
                                    applicationInformationMortgageLoan.RequestedCashAmount.HasValue ? applicationInformationMortgageLoan.RequestedCashAmount.Value : 0,
                                    0,
                                    applicationFeeAttributes.CapitaliseFees,
                                    applicationFeeAttributes.QuickPayLoan,
                                    applicationInformationMortgageLoan.HouseholdIncome,
                                    employmentType,
                                    ltvBeforeFees,
                                    applicationFeeAttributes.StaffHomeLoan,
                                    applicationFeeAttributes.DiscountedInitiationFeeReturningClient, application.OfferStartDate.Value, applicationFeeAttributes.CapitaliseInitiationFee, applicationFeeAttributes.GEPF);

                // setup the application expenses
                financialDataManager.SetApplicationExpenses(command.ApplicationNumber, applicationFees);

                // Price application
                var pricedapplication = applicationCalculator.PriceApplication(applicationInformationMortgageLoan, applicationFees);

                // fetch the record to update
                var applicationInformationVariableLoan = financialDataManager.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey);

                applicationInformationVariableLoan.InterimInterest = (double)applicationFees.InterimInterest;
                applicationInformationVariableLoan.LoanAmountNoFees = (double)pricedapplication.LoanAmountNoFees;
                applicationInformationVariableLoan.LTV = (double)pricedapplication.LTV;
                applicationInformationVariableLoan.BondToRegister = (double)applicationFees.BondToRegister;
                applicationInformationVariableLoan.CreditCriteriaKey = pricedapplication.ApplicationCreditCriteria.CreditCriteriaKey;
                applicationInformationVariableLoan.CreditMatrixKey = pricedapplication.ApplicationCreditCriteria.CreditMatrixKey;
                applicationInformationVariableLoan.CategoryKey = pricedapplication.ApplicationCreditCriteria.CategoryKey;
                applicationInformationVariableLoan.FeesTotal = (double)applicationFees.TotalFees();
                applicationInformationVariableLoan.MarketRate = (double)pricedapplication.RateConfigurationValues.MarketRateValue;
                applicationInformationVariableLoan.RateConfigurationKey = pricedapplication.RateConfigurationValues.RateConfigurationKey;
                applicationInformationVariableLoan.MonthlyInstalment = (double)pricedapplication.MonthlyInstalment;
                applicationInformationVariableLoan.PTI = (double)pricedapplication.PTI;
                applicationInformationVariableLoan.LoanAgreementAmount = (double)pricedapplication.LoanAgreementAmount;

                // save the changes
                financialDataManager.SaveOfferInformationVariableLoan(applicationInformationVariableLoan);

                // Determine and persist Application Attributes
                var determinedApplicationAttributes = financialDataManager.DetermineApplicationAttributes(command.ApplicationNumber,
                                                                                                          pricedapplication.LTV,
                                                                                                          employmentType,
                                                                                                          applicationInformationMortgageLoan.HouseholdIncome,
                                                                                                          applicationFeeAttributes.StaffHomeLoan,
                                                                                                          applicationFeeAttributes.GEPF);

                financialManager.SyncApplicationAttributes(command.ApplicationNumber, determinedApplicationAttributes);

                // raise the event
                var newBusinessApplicationPricedEvent = new NewBusinessApplicationPricedEvent(DateTime.Now, command.ApplicationNumber);
                eventRaiser.RaiseEvent(DateTime.Now, newBusinessApplicationPricedEvent, command.ApplicationNumber, (int)GenericKeyType.Offer, metadata);

                // complete the unit of work
                unitOfWork.Complete();
            }

            return messages;
        }
    }
}