using System.Collections.Generic;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using NUnit.Framework;
using System.Reflection;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
    //[Subject(typeof(CreditCriteriaRepository))]
    public class exceptions_ltv_above_70_income_16000_loan_amt_above_2pt75_mil : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
            //Multiple LTV, income and loan amount scenarios have been covered in a single test by using the min value
            //For every boundary the test has been repeated for all applicable EmploymentType-LoanPurpose combinations

            //test_ltv_above_70_income_16000_loan_amt_above_2pt75_mil
            CreditCriteriaDetermineTest test_Newpurchase_Salaried_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Salaried_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Switchloan_Salaried_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Salaried_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Refinance_Salaried_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_Salaried_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Newpurchase_Subsidised_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Subsidised_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Switchloan_Subsidised_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Subsidised_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Refinance_Subsidised_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_Subsidised_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Newpurchase_SelfEmployed_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_SelfEmployed_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Switchloan_SelfEmployed_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_SelfEmployed_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);

            CreditCriteriaDetermineTest test_Refinance_SelfEmployed_ltv_above_70_income_16000_loan_amt_above_2pt75_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750001D,
                LTV = 70.1D,
                Income = 13000,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_SelfEmployed_ltv_above_70_income_16000_loan_amt_above_2pt75_mil);
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