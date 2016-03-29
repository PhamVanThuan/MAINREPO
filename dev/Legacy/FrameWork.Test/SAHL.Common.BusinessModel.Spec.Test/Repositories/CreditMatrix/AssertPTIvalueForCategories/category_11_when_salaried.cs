using Machine.Specifications;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.AssertPTIvalueForCategories
{
    public class category_11_when_salaried : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaAssertTest> creditCriteriaTestPacks = new List<CreditCriteriaAssertTest>();

        private Establish context = () =>
        {
            //Cat 11 - salaried - new purchase - 1pt6 million
            CreditCriteriaAssertTest test_Newpurchase_loan_amt_1p6_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category11,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                ExpectedPTI = 20
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_1p6_mil_salaried);
        };

        private Because of = () =>
        {
            GetCreditCriteriaForCreditCriteriaAssertTest(creditCriteriaTestPacks);
        };

        private It should_return_expected_PTI_for_category_4 = () =>
        {
            CheckAssertionTests(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}