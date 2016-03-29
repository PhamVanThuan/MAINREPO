using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.CreditMatrix.CategoryDetermination
{
    public class when_determining_categories_with_further_lending_non_alpha_housing_credit_criteria_attributes : CreditCriteriaTestBase
    {
        protected static List<CreditCriteriaDetermineTest> creditCriteriaTestPacks = new List<CreditCriteriaDetermineTest>();
        Establish context = () =>
        {
            var furtherLendingNonAlphaHousing = new CreditCriteriaDetermineTest
            {
                EmploymentTypeKey = (int)SAHL.Common.Globals.EmploymentTypes.Salaried,
                OriginationSourceKey = (int)SAHL.Common.Globals.OriginationSources.SAHomeLoans,
                ProductKey = (int)SAHL.Common.Globals.Products.NewVariableLoan,
                TotalLoanAmount = 500000D,
                LTV = 85.01D,
                Income = 20000D,
                ExpectedCategoryKey = 4,
                MortgageLoanPurposeKey = (int)SAHL.Common.Globals.MortgageLoanPurposes.Newpurchase,
                CreditCriteriaAttributeTypeKey = (int)SAHL.Common.Globals.CreditCriteriaAttributeTypes.FurtherLendingNonAlphaHousing
            };
            creditCriteriaTestPacks.Add(furtherLendingNonAlphaHousing);
        };
        Because of = () =>
        {
            GetCreditCriteria(creditCriteriaTestPacks);
        };

        It should_return_category_5 = () =>
        {
            CheckDeterminedCategory(creditCriteriaTestPacks, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        };
    }
}
