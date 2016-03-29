using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination.TestCategories;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
	public class category_1_when_self_employed_and_switch_loan : CreditCriteriaTestBase
	{	

		protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
		  			
            CreditCriteriaDetermineTest test_self_employed_ltv_80_income_13000_loan_amt_1800000_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = 2,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1800000,
                LTV = 80,
                Income = 13000,
                ExpectedCategoryKey = 1,
                MortgageLoanPurposeKey = 2,
				CreditCriteriaAttributeTypeKey = 1
            };
            creditCriteriaTestPacks.Add(test_self_employed_ltv_80_income_13000_loan_amt_1800000_mil);
					
            CreditCriteriaDetermineTest test_self_employed_ltv_80_income_13000_loan_amt_2750000_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = 2,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 2750000,
                LTV = 80,
                Income = 13000,
                ExpectedCategoryKey = 1,
                MortgageLoanPurposeKey = 2,
				CreditCriteriaAttributeTypeKey = 1
            };
            creditCriteriaTestPacks.Add(test_self_employed_ltv_80_income_13000_loan_amt_2750000_mil);
				};

		Because of = () =>
		{
			GetCreditCriteria(creditCriteriaTestPacks);
		};

		It should_return_category_1 = () =>
		{
			CheckDeterminedCategory(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
		};
	}
}