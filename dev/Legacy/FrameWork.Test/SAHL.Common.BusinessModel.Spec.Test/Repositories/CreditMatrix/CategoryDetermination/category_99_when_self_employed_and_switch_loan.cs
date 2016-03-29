using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination.TestCategories;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
	public class category_99_when_self_employed_and_switch_loan : CreditCriteriaTestBase
	{	

		protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
		  			
            CreditCriteriaDetermineTest test_self_employed_ltv_98_income_20000_loan_amt_7000000_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = 2,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 7000000,
                LTV = 98,
                Income = 20000,
                ExpectedCategoryKey = 99,
                MortgageLoanPurposeKey = 2,
				CreditCriteriaAttributeTypeKey = 1
            };
            creditCriteriaTestPacks.Add(test_self_employed_ltv_98_income_20000_loan_amt_7000000_mil);
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