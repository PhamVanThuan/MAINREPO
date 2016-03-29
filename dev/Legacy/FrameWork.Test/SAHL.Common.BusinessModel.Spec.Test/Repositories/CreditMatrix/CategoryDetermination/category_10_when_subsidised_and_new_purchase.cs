using System.Collections.Generic;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination.TestCategories;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
    public class category_10_when_salaried_deduction_and_new_purchase : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();
        Establish context = () =>
        {
            CreditCriteriaDetermineTest test_Newpurchase_ltv_100_income_20000_loan_amt_1pt6_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 100D,
                Income = Category10Config.MinIncome,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.Category10,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_ltv_100_income_20000_loan_amt_1pt6_mil);

            CreditCriteriaDetermineTest test_Newpurchase_ltv_101_income_20000_loan_amt_1pt6_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 101D,
                Income = Category10Config.MinIncome,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_ltv_101_income_20000_loan_amt_1pt6_mil);
        };

        Because of = () =>
        {
            GetCreditCriteria(creditCriteriaTestPacks);
        };

        It should_return_category_10 = () =>
        {
            CheckDeterminedCategory(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}