using System.Collections.Generic;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using NUnit.Framework;
using System.Reflection;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
    //[Subject(typeof(CreditCriteriaRepository))]
    public class exceptions_selfemployed_ltv_above_85_income_16000 : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
            //Multiple LTV, income and loan amount scenarios have been covered in a single test by using the min value
            //For every boundary the test has been repeated for all applicable EmploymentType-LoanPurpose combinations
            CreditCriteriaDetermineTest test_Newpurchase_SelfEmployed_ltv_above_85_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 85.1D,
                Income = 16000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_SelfEmployed_ltv_above_85_income_16000);
        };

        Because of = () =>
        {
            GetCreditCriteria(creditCriteriaTestPacks);
        };

        It should_return_category_exception = () =>
        {
            CheckDeterminedCategory(creditCriteriaTestPacks, MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}