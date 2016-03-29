using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Models;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using SAHL.Shared.BusinessModel.Calculations;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinancialDomain.Specs.CommandHandlersSpec.PriceNewBusinessApplication
{
    public class when_pricing_a_new_purchase_application : WithCoreFakes
    {
        private static PriceNewBusinessApplicationCommand command;
        private static PriceNewBusinessApplicationCommandHandler handler;
        private static IFinancialDataManager financialDataManager;
        private static IDomainRuleManager<IApplicationModel> domainRuleContext;
        private static IApplicationCalculator applicationCalculator;
        private static IFinancialManager financialManager;
        private static ILoanCalculations functionsUtils;

        private static int applicationNumber;
        public static OfferDataModel application;
        private static OfferInformationDataModel applicationInformation;
        private static MortgageLoanApplicationInformationModel applicationMortgageLoanInformation;
        private static FeeApplicationAttributesModel applicationFeeAttributes;
        private static decimal ltvBeforeFees;
        private static OriginationFeesModel applicationFees;
        private static OfferInformationVariableLoanDataModel applicationInformationVariableLoan;
        private static PricedMortgageLoanApplicationInformationModel pricedApplication;
        private static PricedCreditCriteriaModel pricedCreditCriteria;
        private static RateConfigurationValuesModel rateConfigurationValues;
        private static decimal loanAmountNoFees;
        private static decimal loanAgreementAmount;
        private static decimal instalment;
        private static decimal ltvAfterFees;
        private static decimal pti;
        private static DateTime offerStartDate;
        private static List<GetOfferAttributesModel> determinedOfferAttributes;

        private Establish context = () =>
        {
            domainRuleContext = An<IDomainRuleManager<IApplicationModel>>();
            applicationCalculator = An<IApplicationCalculator>();
            financialDataManager = An<IFinancialDataManager>();
            financialManager = An<IFinancialManager>();
            functionsUtils = An<ILoanCalculations>();

            messages = SystemMessageCollection.Empty();
            offerStartDate = DateTime.Now;
            applicationNumber = 1;

            application = new OfferDataModel(1, 1, 1, offerStartDate, DateTime.Now, 0, "", 1, 1, 1, 1, 1);
            financialDataManager.WhenToldTo(x => x.GetApplication(applicationNumber)).Return(application);

            applicationInformation = new OfferInformationDataModel(DateTime.Now, applicationNumber, (int)OfferInformationType.OriginalOffer, "System", DateTime.Now, (int)Product.NewVariableLoan);
            financialDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(applicationInformation);

            applicationMortgageLoanInformation = new MortgageLoanApplicationInformationModel(750000, 0, OfferType.NewPurchaseLoan, 0, 28500, EmploymentType.Salaried, 1000000, MortgageLoanPurpose.Newpurchase, OriginationSource.SAHomeLoans, 240);
            financialDataManager.WhenToldTo(x => x.GetApplicationInformationMortgageLoan(applicationInformation.OfferInformationKey)).Return(applicationMortgageLoanInformation);

            applicationFeeAttributes = new FeeApplicationAttributesModel(true, false, false, false, false, false);
            financialDataManager.WhenToldTo(x => x.GetFeeApplicationAttributes(applicationNumber)).Return(applicationFeeAttributes);

            ltvBeforeFees = functionsUtils.CalculateLTV(applicationMortgageLoanInformation.LoanAmountNoFees, applicationMortgageLoanInformation.PropertyValuation);

            applicationFees = new OriginationFeesModel(0, 0, 5500, 1030000, 7500, 0, true, false);
            financialDataManager.WhenToldTo(x => x.CalculateOriginationFees(applicationMortgageLoanInformation.LoanAmountNoFees, applicationMortgageLoanInformation.BondToRegister.Value, applicationMortgageLoanInformation.OfferType, applicationMortgageLoanInformation.RequestedCashAmount.Value,
                0, applicationFeeAttributes.CapitaliseFees, applicationFeeAttributes.QuickPayLoan, applicationMortgageLoanInformation.HouseholdIncome, applicationMortgageLoanInformation.EmploymentType.Value, ltvBeforeFees, applicationFeeAttributes.StaffHomeLoan,
                applicationFeeAttributes.DiscountedInitiationFeeReturningClient, offerStartDate, applicationFeeAttributes.CapitaliseInitiationFee, applicationFeeAttributes.GEPF)).Return(applicationFees);

            pricedCreditCriteria = new PricedCreditCriteriaModel(1, 1, 1);
            rateConfigurationValues = new RateConfigurationValuesModel(1, (decimal)0.03, (decimal)0.06);

            loanAmountNoFees = applicationMortgageLoanInformation.LoanAmountNoFees + applicationFees.InterimInterest;

            loanAgreementAmount = loanAmountNoFees;
            if (applicationFeeAttributes.CapitaliseFees)
            {
                loanAgreementAmount += applicationFees.TotalFees();
            }

            ltvAfterFees = functionsUtils.CalculateLTV(loanAgreementAmount, applicationMortgageLoanInformation.PropertyValuation);
            instalment = functionsUtils.CalculateInstalment(loanAgreementAmount, rateConfigurationValues.MarketRateValue + rateConfigurationValues.MarginValue, 240, false);
            pti = functionsUtils.CalculatePTI(instalment, applicationMortgageLoanInformation.HouseholdIncome);

            pricedApplication = new PricedMortgageLoanApplicationInformationModel(loanAgreementAmount, applicationMortgageLoanInformation.LoanAmountNoFees, ltvAfterFees, rateConfigurationValues, instalment, pti, pricedCreditCriteria);
            applicationCalculator.WhenToldTo(x => x.PriceApplication(applicationMortgageLoanInformation, applicationFees)).Return(pricedApplication);

            applicationInformationVariableLoan = new OfferInformationVariableLoanDataModel(applicationInformation.OfferInformationKey, null, 240, 0, 250000, 1000000, 28500, null, null, null, null, null, null, null, null, null, null, 750000, 0, null, null,
                null, null, null, null, null, null, null, null, null);
            financialDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey)).Return(applicationInformationVariableLoan);

            determinedOfferAttributes = new List<GetOfferAttributesModel>
            {
                new GetOfferAttributesModel { OfferAttributeTypeKey = 26, Remove = false }
            };
            financialDataManager.WhenToldTo(x => x.DetermineApplicationAttributes(applicationNumber, ltvAfterFees, applicationMortgageLoanInformation.EmploymentType.Value, applicationMortgageLoanInformation.HouseholdIncome, false, false)).Return(determinedOfferAttributes);

            command = new PriceNewBusinessApplicationCommand(applicationNumber);
            handler = new PriceNewBusinessApplicationCommandHandler(financialDataManager, unitOfWorkFactory, eventRaiser, applicationCalculator,
                domainRuleContext, financialManager, functionsUtils);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleContext.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<PriceNewBusinessApplicationCommand>()));
        };

        private It should_fetch_latest_application_information = () =>
        {
            financialDataManager.WasToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber));
        };

        private It should_fetch_the_application_mortgage_loan = () =>
        {
            financialDataManager.WasToldTo(x => x.GetApplicationInformationMortgageLoan(applicationInformation.OfferInformationKey));
        };

        private It should_fetch_the_application_fee_attributes = () =>
        {
            financialDataManager.WasToldTo(x => x.GetFeeApplicationAttributes(applicationNumber));
        };

        private It should_calculate_the_application_fees = () =>
        {
            financialDataManager.WasToldTo(x => x.CalculateOriginationFees(applicationMortgageLoanInformation.LoanAmountNoFees, 0, applicationMortgageLoanInformation.OfferType, applicationMortgageLoanInformation.RequestedCashAmount.Value,
                0, applicationFeeAttributes.CapitaliseFees, applicationFeeAttributes.QuickPayLoan, applicationMortgageLoanInformation.HouseholdIncome, applicationMortgageLoanInformation.EmploymentType.Value, ltvBeforeFees,
                applicationFeeAttributes.StaffHomeLoan, applicationFeeAttributes.DiscountedInitiationFeeReturningClient, offerStartDate, applicationFeeAttributes.CapitaliseInitiationFee, applicationFeeAttributes.GEPF));
        };

        private It should_save_the_applications_expenses = () =>
        {
            financialDataManager.WasToldTo(x => x.SetApplicationExpenses(applicationNumber, applicationFees));
        };

        private It should_price_the_application = () =>
        {
            applicationCalculator.WasToldTo(x => x.PriceApplication(applicationMortgageLoanInformation, applicationFees));
        };

        private It should_fetch_the_application_information_variable_loan = () =>
        {
            financialDataManager.WasToldTo(x => x.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey));
        };

        private It should_save_the_application_information_variable_loan = () =>
        {
            financialDataManager.WasToldTo(x => x.SaveOfferInformationVariableLoan(applicationInformationVariableLoan));
        };

        private It should_update_the_interim_interest_value = () =>
        {
            applicationInformationVariableLoan.InterimInterest.ShouldEqual((double)applicationFees.InterimInterest);
        };

        private It should_update_the_loan_amount_excluding_fees = () =>
        {
            applicationInformationVariableLoan.LoanAmountNoFees.ShouldEqual((double)pricedApplication.LoanAmountNoFees);
        };

        private It should_update_the_ltv_value = () =>
        {
            applicationInformationVariableLoan.LTV.ShouldEqual((double)pricedApplication.LTV);
        };

        private It should_update_the_bond_to_register_value = () =>
        {
            applicationInformationVariableLoan.BondToRegister.ShouldEqual((double)applicationFees.BondToRegister);
        };

        private It should_update_the_credit_criteria_key = () =>
        {
            applicationInformationVariableLoan.CreditCriteriaKey.ShouldEqual(pricedApplication.ApplicationCreditCriteria.CreditCriteriaKey);
        };

        private It should_update_the_credit_matrix_key = () =>
        {
            applicationInformationVariableLoan.CreditMatrixKey.ShouldEqual(pricedApplication.ApplicationCreditCriteria.CreditMatrixKey);
        };

        private It should_update_the_category_key = () =>
        {
            applicationInformationVariableLoan.CategoryKey.ShouldEqual(pricedApplication.ApplicationCreditCriteria.CategoryKey);
        };

        private It should_update_the_total_fees_value = () =>
        {
            applicationInformationVariableLoan.FeesTotal.ShouldEqual((double)applicationFees.TotalFees());
        };

        private It should_update_the_market_rate_value = () =>
        {
            applicationInformationVariableLoan.MarketRate.ShouldEqual((double)pricedApplication.RateConfigurationValues.MarketRateValue);
        };

        private It should_update_the_rate_configuration_key = () =>
        {
            applicationInformationVariableLoan.RateConfigurationKey.ShouldEqual(pricedApplication.RateConfigurationValues.RateConfigurationKey);
        };

        private It should_update_the_monthly_instalment_value = () =>
        {
            applicationInformationVariableLoan.MonthlyInstalment.ShouldEqual((double)pricedApplication.MonthlyInstalment);
        };

        private It should_update_the_pti_value = () =>
        {
            applicationInformationVariableLoan.PTI.ShouldEqual((double)pricedApplication.PTI);
        };

        private It should_update_the_loan_agreement_amount_value = () =>
        {
            applicationInformationVariableLoan.LoanAgreementAmount.ShouldEqual((double)pricedApplication.LoanAgreementAmount);
        };

        private It should_determine_the_application_attributes = () =>
        {
            financialDataManager.WasToldTo(x => x.DetermineApplicationAttributes(applicationNumber, pricedApplication.LTV, applicationMortgageLoanInformation.EmploymentType.Value, applicationMortgageLoanInformation.HouseholdIncome, applicationFeeAttributes.StaffHomeLoan, applicationFeeAttributes.GEPF));
        };

        private It should_add_or_remove_the_determined_application_attributes_from_the_application = () =>
        {
            financialManager.WasToldTo(x => x.SyncApplicationAttributes(applicationNumber, determinedOfferAttributes));
        };
    }
}