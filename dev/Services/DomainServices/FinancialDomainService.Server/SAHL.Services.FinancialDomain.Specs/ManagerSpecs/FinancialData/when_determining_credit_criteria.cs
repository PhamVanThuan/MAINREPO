using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.FinancialDomain.Managers.Statements;

namespace SAHL.Services.FinancialDomain.Specs.ManagerSpecs.FinancialData
{
    public class when_determining_credit_criteria : WithCoreFakes
    {
        private static FinancialDataManager financialDataManager;
        private static FakeDbFactory dbFactory;

        private static MortgageLoanPurpose mortgageLoanPurpose = MortgageLoanPurpose.Newpurchase;
        private static EmploymentType employmentType = EmploymentType.Salaried;
        private static decimal totalLoanAmount = 9;
        private static decimal ltv = 0;
        private static OriginationSource originationSource = OriginationSource.SAHomeLoans;
        private static Product product = Product.NewVariableLoan;
        private static decimal householdIncome = 0;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            financialDataManager = new FinancialDataManager(dbFactory);

            mortgageLoanPurpose = MortgageLoanPurpose.Newpurchase;
            employmentType = EmploymentType.Salaried;
            totalLoanAmount = 900000;
            ltv = 0.9m;
            originationSource = OriginationSource.SAHomeLoans;
            product = Product.NewVariableLoan;
            householdIncome = 30000;
        };

        private Because of = () =>
        {
            financialDataManager.DetermineCreditCriteria(mortgageLoanPurpose, employmentType, totalLoanAmount, ltv, originationSource, product, householdIncome);
        };

        private It should_retrieve_credit_criteria_based_on_parameters = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne<CreditCriteriaDataModel>(Arg.Is<GetCreditCriteriaStatement>(y => y.MortgageLoanPurposeKey == (int)mortgageLoanPurpose
                                                                                                              && y.EmploymentTypeKey == (int)employmentType
                                                                                                              && y.Income == householdIncome
                                                                                                              && y.LTV == ltv
                                                                                                              && y.OriginationSourceKey == (int)originationSource
                                                                                                              && y.ProductKey == (int)product
                                                                                                              && y.TotalLoanAmount == totalLoanAmount)));
        };
    }
}