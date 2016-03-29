using Machine.Specifications;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.AssertPTIvalueForCategories
{
    public class category_1_when_salaried : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaAssertTest> creditCriteriaTestPacks = new List<CreditCriteriaAssertTest>();

        private Establish context = () =>
        {
            //Cat 0 - salaried - new purchase - 1pt6 million
            CreditCriteriaAssertTest test_Newpurchase_loan_amt_1p6_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category1,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                ExpectedPTI = 30
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_1p6_mil_salaried);

            //Cat 0 - salaried - switch - 1pt6 million
            CreditCriteriaAssertTest test_switch_loan_amt_1p6_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category1,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan,
                ExpectedPTI = 30
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_1p6_mil_salaried);

            //Cat 0 - salaried - refinance - 1pt6 million
            CreditCriteriaAssertTest test_refinance_loan_amt_1p6_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category1,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_refinance_loan_amt_1p6_mil_salaried);

            //Cat 0 - salaried - new purchase - 2pt5 million
            CreditCriteriaAssertTest test_Newpurchase_loan_amt_2pt5_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category1,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_2pt5_mil_salaried);

            //Cat 0 - salaried - switch - 2pt5 million
            CreditCriteriaAssertTest test_switch_loan_amt_2pt5_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category1,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_switch_loan_amt_2pt5_mil_salaried);

            //Cat 0 - salaried - refinance - 2pt5 million
            CreditCriteriaAssertTest test_refinance_loan_amt_2pt5_mil_salaried = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category1,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_refinance_loan_amt_2pt5_mil_salaried);
        };

        private Because of = () =>
        {
            GetCreditCriteriaForCreditCriteriaAssertTest(creditCriteriaTestPacks);
        };

        private It should_return_expected_PTI_for_category_1 = () =>
        {
            CheckAssertionTests(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}