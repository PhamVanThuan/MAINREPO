using System.Collections.Generic;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
    //[Subject(typeof(CreditCriteriaRepository))]
    public class category_exceptions_when_salaried_and_switch : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();
        Establish context = () =>
        {
            #region Min exceptions category income
            //changed ltv from 98 to 70 to cover all ltv ranges
            //changed income from 4000 to 7999 to be on the boundary
            CreditCriteriaDetermineTest test_Newpurchase_Salaried_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Salaried_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Switchloan_Salaried_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Salaried_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Refinance_Salaried_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_Salaried_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Newpurchase_Subsidised_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Subsidised_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Switchloan_Subsidised_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Subsidised_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Refinance_Subsidised_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_Subsidised_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Newpurchase_SelfEmployed_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_SelfEmployed_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Switchloan_SelfEmployed_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_SelfEmployed_ltv_70_income_7999);

            CreditCriteriaDetermineTest test_Refinance_SelfEmployed_ltv_70_income_7999 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_SelfEmployed_ltv_70_income_7999);
            #endregion
            #region Max exceptions category LTV
            CreditCriteriaDetermineTest test_Newpurchase_Salaried_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Salaried_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Switchloan_Salaried_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Salaried_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Refinance_Salaried_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_Salaried_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Newpurchase_Subsidised_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_Subsidised_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Switchloan_Subsidised_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_Subsidised_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Refinance_Subsidised_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SalariedwithDeduction,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_Subsidised_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Newpurchase_SelfEmployed_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
            };
            creditCriteriaTestPacks.Add(test_Newpurchase_SelfEmployed_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Switchloan_SelfEmployed_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Switchloan
            };
            creditCriteriaTestPacks.Add(test_Switchloan_SelfEmployed_ltv_99_income_16000);

            CreditCriteriaDetermineTest test_Refinance_SelfEmployed_ltv_99_income_16000 = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.SelfEmployed,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000D,
                LTV = 70D,
                Income = 7999D,
                ExpectedCategoryKey = (int)SAHL.Common.Globals.Categories.ExceptionCategory,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Refinance
            };
            creditCriteriaTestPacks.Add(test_Refinance_SelfEmployed_ltv_99_income_16000);
            #endregion
        };

        Because of = () =>
        {
            GetCreditCriteria(creditCriteriaTestPacks);
        };

        It should_return_category_99 = () =>
        {
            CheckDeterminedCategory(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}