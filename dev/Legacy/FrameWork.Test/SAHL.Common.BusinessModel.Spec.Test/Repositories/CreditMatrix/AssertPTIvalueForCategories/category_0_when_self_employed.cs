using Machine.Specifications;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.AssertPTIvalueForCategories
{
    public class category_0_when_self_employed : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaAssertTest> creditCriteriaTestPacks = new List<CreditCriteriaAssertTest>();

        private Establish context = () =>
        {
            //Cat 0 - SelfEmployed - new purchase - 1pt6 million
            CreditCriteriaAssertTest test_Newpurchase_loan_amt_1p6_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_1p6_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - switch - 1pt6 million
            CreditCriteriaAssertTest test_switch_loan_amt_1p6_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_1p6_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - refinance - 1pt6 million
            CreditCriteriaAssertTest test_refinance_loan_amt_1p6_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_refinance_loan_amt_1p6_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - new purchase - 2pt5 million
            CreditCriteriaAssertTest test_Newpurchase_loan_amt_2pt5_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_2pt5_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - switch - 2pt5 million
            CreditCriteriaAssertTest test_switch_loan_amt_2pt5_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_switch_loan_amt_2pt5_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - refinance - 2pt5 million
            CreditCriteriaAssertTest test_refinance_loan_amt_2pt5_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_refinance_loan_amt_2pt5_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - new purchase - 3pt5 million
            CreditCriteriaAssertTest test_Newpurchase_loan_amt_3pt5_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 3500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_loan_amt_3pt5_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - switch - 3pt5 million
            CreditCriteriaAssertTest test_switch_loan_amt_3pt5_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 3500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_switch_loan_amt_3pt5_mil_SelfEmployed);

            //Cat 0 - SelfEmployed - refinance - 3pt5 million
            CreditCriteriaAssertTest test_refinance_loan_amt_3pt5_mil_SelfEmployed = new CreditCriteriaAssertTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 3500000D,
                CategoryKey = (int)SAHL.Common.Globals.Categories.Category0,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance,
                ExpectedPTI = 25
            };
            creditCriteriaTestPacks.Add(test_refinance_loan_amt_3pt5_mil_SelfEmployed);
        };

        private Because of = () =>
        {
            GetCreditCriteriaForCreditCriteriaAssertTest(creditCriteriaTestPacks);
        };

        private It should_return_expected_PTI_for_category_0 = () =>
        {
            CheckAssertionTests(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}