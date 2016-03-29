using System.Collections.Generic;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using NUnit.Framework;
using System.Reflection;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
    //[Subject(typeof(CreditCriteriaRepository))]
    public class exceptions_ltv_above_90_income_16000_loan_amt_above_1pt8_mil : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
            //Multiple LTV, income and loan amount scenarios have been covered in a single test by using the min value
            //For every boundary the test has been repeated for all applicable EmploymentType-LoanPurpose combinations

            CreditCriteriaDetermineTest test_Newpurchase_Salaried_ltv_above_90_income_16000_loan_amt_above_1pt8_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1800001D,
                LTV = 90.1D,
                Income = 18000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Salaried_ltv_above_90_income_16000_loan_amt_above_1pt8_mil);

            CreditCriteriaDetermineTest test_Switchloan_Salaried_ltv_above_90_income_16000_loan_amt_above_1pt8_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1800001D,
                LTV = 90.1D,
                Income = 18000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Salaried_ltv_above_90_income_16000_loan_amt_above_1pt8_mil);

            //Refinance scenario catered for in Refinance_ltv_above_90_income_16000 above

            CreditCriteriaDetermineTest test_Newpurchase_Subsidised_ltv_above_90_income_16000_loan_amt_above_1pt8_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1800001D,
                LTV = 90.1D,
                Income = 18000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Subsidised_ltv_above_90_income_16000_loan_amt_above_1pt8_mil);

            CreditCriteriaDetermineTest test_Switchloan_Subsidised_ltv_above_90_income_16000_loan_amt_above_1pt8_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1800001D,
                LTV = 90.1D,
                Income = 18000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Subsidised_ltv_above_90_income_16000_loan_amt_above_1pt8_mil);

            //Refinance scenario catered for in Refinance_ltv_above_90_income_16000 above

            //SelfEmployed scenario's catered for in Refinance_SelfEmployed_ltv_above_80_income_16000 below
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