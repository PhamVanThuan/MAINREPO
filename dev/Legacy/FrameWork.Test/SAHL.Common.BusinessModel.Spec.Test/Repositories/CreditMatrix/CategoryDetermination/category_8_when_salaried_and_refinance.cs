using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination.TestCategories;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
	public class category_8_when_salaried_and_refinance : CreditCriteriaTestBase
	{	

		protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();

        Establish context = () =>
        {
		  			
            CreditCriteriaDetermineTest test_salaried_ltv_95_income_8000_loan_amt_1600000_mil = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = 1,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 1600000,
                LTV = 95,
                Income = 8000,
                ExpectedCategoryKey = 8,
                MortgageLoanPurposeKey = 4,
				CreditCriteriaAttributeTypeKey = 1
            };
            creditCriteriaTestPacks.Add(test_salaried_ltv_95_income_8000_loan_amt_1600000_mil);
				};

		Because of = () =>
		{
			GetCreditCriteria(creditCriteriaTestPacks);
		};

		It should_return_category_8 = () =>
		{
			CheckDeterminedCategory(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
		};
	}
}