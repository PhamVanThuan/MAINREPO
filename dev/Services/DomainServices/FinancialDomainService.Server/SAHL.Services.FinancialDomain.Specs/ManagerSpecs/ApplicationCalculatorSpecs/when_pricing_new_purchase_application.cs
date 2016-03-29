using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using SAHL.Shared.BusinessModel.Calculations;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.ApplicationCalculatorSpecs
{
    public class when_pricing_new_purchase_application : WithCoreFakes
    {
        private static IApplicationCalculator applicationCalculator;
        private static IFinancialDataManager financialDataManager;
        private static ILoanCalculations functionsUtils;

        private static PricedMortgageLoanApplicationInformationModel pricedApplication;
        private static MortgageLoanApplicationInformationModel mortgageLoanApplicationInformation;
        private static OriginationFeesModel originationFees;
        private static CreditCriteriaDataModel creditCriteria;
        private static RateConfigurationValuesModel rateConfigurationValues;

        private static int marginKey = 22;
        private static int creditCriteriaKey = 33;
        private static int creditMatrixKey = 44;
        private static int categoryKey = 2;

        private static decimal loanAmountNoFees;
        private static decimal totalLoanAmount;
        private static decimal ltv;
        private static decimal ltvPercentage;
        private static decimal interimInterest = 0;
        private static bool capitaliseFees = false;
        private static bool capitaliseInitiationFees = false;
        private static decimal monthlyInstalment;
        private static decimal paymentToIncome;

        private Establish context = () =>
        {
            financialDataManager = An<IFinancialDataManager>();
            functionsUtils = An<ILoanCalculations>();

            applicationCalculator = new ApplicationCalculator(financialDataManager, functionsUtils);

            mortgageLoanApplicationInformation = new MortgageLoanApplicationInformationModel(1000000,
                0,
                OfferType.NewPurchaseLoan,
                0,
                25000,
                EmploymentType.Salaried,
                4000000,
                MortgageLoanPurpose.Newpurchase,
                OriginationSource.SAHomeLoans,
                240);

            originationFees = new OriginationFeesModel(interimInterest, 6, 7, 8, 9, 10, capitaliseFees, capitaliseInitiationFees);

            creditCriteria = new CreditCriteriaDataModel(creditCriteriaKey, creditMatrixKey, marginKey, categoryKey, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0, 0);

            totalLoanAmount = loanAmountNoFees = mortgageLoanApplicationInformation.LoanAmountNoFees + originationFees.InterimInterest;

            ltv = functionsUtils.CalculateLTV(totalLoanAmount, mortgageLoanApplicationInformation.PropertyValuation);
            ltvPercentage = ltv * 100;
            //FinancialDataManager.DetermineCreditCriteria
            financialDataManager.WhenToldTo(x => x.DetermineCreditCriteria(mortgageLoanApplicationInformation.MortgageLoanPurpose, mortgageLoanApplicationInformation.EmploymentType.Value, totalLoanAmount, ltvPercentage, mortgageLoanApplicationInformation.OriginationSource, Product.NewVariableLoan, mortgageLoanApplicationInformation.HouseholdIncome)).Return(creditCriteria);

            rateConfigurationValues = new RateConfigurationValuesModel(12, 0.21m, 0.7m);
            //FinancialDataManager.GetRateConfigurationValues
            financialDataManager.WhenToldTo(x => x.GetRateConfigurationValues(creditCriteria.MarginKey, (int)MarketRates.ThreeMonthJIBARRounded)).Return(rateConfigurationValues);

            monthlyInstalment = functionsUtils.CalculateInstalment(totalLoanAmount, (rateConfigurationValues.MarginValue + rateConfigurationValues.MarketRateValue), mortgageLoanApplicationInformation.Term, false);
            paymentToIncome = functionsUtils.CalculatePTI(monthlyInstalment, mortgageLoanApplicationInformation.HouseholdIncome);
        };

        private Because of = () =>
        {
            pricedApplication = applicationCalculator.PriceApplication(mortgageLoanApplicationInformation, originationFees);
        };

        private It should_return_a_priced_mortgage_loan_application = () =>
        {
            pricedApplication.ShouldNotBeNull();
        };

        private It should_calculate_the_loan_amount_without_fees = () =>
        {
            pricedApplication.LoanAmountNoFees.ShouldEqual(loanAmountNoFees);
        };

        private It should_calculate_the_loan_to_property_value_percentage = () =>
        {
            ;
            pricedApplication.LTV.ShouldEqual(ltv);
        };

        private It should_determine_the_credit_criteria = () =>
        {
            financialDataManager.WasToldTo(x => x.DetermineCreditCriteria(mortgageLoanApplicationInformation.MortgageLoanPurpose, mortgageLoanApplicationInformation.EmploymentType.Value, totalLoanAmount, ltvPercentage, mortgageLoanApplicationInformation.OriginationSource, Product.NewVariableLoan, mortgageLoanApplicationInformation.HouseholdIncome));
        };

        private It should_return_the_priced_credit_criteria = () =>
        {
            pricedApplication.ApplicationCreditCriteria.CategoryKey.ShouldEqual(categoryKey);
            pricedApplication.ApplicationCreditCriteria.CreditMatrixKey.ShouldEqual(creditMatrixKey);
            pricedApplication.ApplicationCreditCriteria.CreditCriteriaKey.ShouldEqual(creditCriteriaKey);
        };

        private It should_get_the_rate_configuration = () =>
        {
            financialDataManager.WasToldTo(x => x.GetRateConfigurationValues(creditCriteria.MarginKey, (int)MarketRates.ThreeMonthJIBARRounded));
        };

        private It should_get_the_rate_configuration_values = () =>
        {
            pricedApplication.RateConfigurationValues.ShouldEqual(rateConfigurationValues);
        };

        private It should_calculate_the_monthly_instalment = () =>
        {
            pricedApplication.MonthlyInstalment.ShouldEqual(monthlyInstalment);
        };

        private It should_calculate_the_payment_to_income_ration = () =>
        {
            pricedApplication.PTI.ShouldEqual(paymentToIncome);
        };
    }
}