using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Repositories;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SAHL.V3.Framework.Specs.RepositorySpecs.DecisionTreeRepositorySpecs
{
    public class when_asked_to_determine_NCR_guideline_min_monthly_fixed_expenses : BaseSpec
    {
        private static decimal grossMonthlyIncome;
        private static decimal minMonthlyFixedExpenses;
        private new static decimal result;

        Establish context = () =>
        {
            grossMonthlyIncome = 9000;
            minMonthlyFixedExpenses = 1234;
            decisionTreeService.WhenToldTo<IDecisionTreeService>(x => x.DetermineNCRGuidelineMinMonthlyFixedExpenses(Param.IsAny<DetermineNCRGuidelineMinMonthlyFixedExpensesQuery>())).Callback<DetermineNCRGuidelineMinMonthlyFixedExpensesQuery>(y =>
            {
                y.SetResult(minMonthlyFixedExpenses, SystemMessageCollection.Empty());
            });
        };

        Because of = () =>
        {
            result = decisionTreeRepo.DetermineNCRGuidelineMinMonthlyFixedExpenses(grossMonthlyIncome);
        };

        It should_ask_the_decisiontreeservice_to_determine_NCR_guideline_minimum_monthly_fixed_expenses_for_the_provided_gross_monthly_income = () =>
        {
            decisionTreeService.WasToldTo(x => x.DetermineNCRGuidelineMinMonthlyFixedExpenses(Param.IsAny<DetermineNCRGuidelineMinMonthlyFixedExpensesQuery>()));
        };

        It should_return_a_valid_result = () =>
        {
            result.ShouldEqual(minMonthlyFixedExpenses);
        };
    }
}
