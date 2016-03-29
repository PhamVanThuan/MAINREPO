using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination.TestCategories;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
	public class category_4_when_salaried_and_new_purchase : CreditCriteriaTestBase
	{	

		protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
		  			
            CreditCriteriaDetermineTest test_salaried_ltv_90_income_20000_loan_amt_1800000_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = 1,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1800000,
                LTV = 90,
                Income = 20000,
                ExpectedCategoryKey = 4,
                MortgageLoanPurposeKey = 3,
				CreditCriteriaAttributeTypeKey = 1
            };
            creditCriteriaTestPacks.Add(test_salaried_ltv_90_income_20000_loan_amt_1800000_mil);
					
            CreditCriteriaDetermineTest test_salaried_ltv_90_income_20000_loan_amt_2750000_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = 1,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750000,
                LTV = 90,
                Income = 20000,
                ExpectedCategoryKey = 4,
                MortgageLoanPurposeKey = 3,
				CreditCriteriaAttributeTypeKey = 1
            };
            creditCriteriaTestPacks.Add(test_salaried_ltv_90_income_20000_loan_amt_2750000_mil);
				};

		Because of = () =>
		{
			GetCreditCriteria(creditCriteriaTestPacks);
		};

		It should_return_category_4 = () =>
		{
			CheckDeterminedCategory(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
		};
	}
}