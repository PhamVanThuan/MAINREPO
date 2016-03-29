using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using SAHL.V3.Framework.Model;
using SAHL.V3.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Specs.ServicesSpecs.DecisionTreeSpecs.DetermineNCRGuidelineMinMonthlyFixedExpenses
{
    public class when_calling_the_NCR_Assessment_Tree_fails_with_exception : WithFakes
    {
        private static IDecisionTreeServiceClient decisionTreeServiceClient;
        private static IDecisionTreeService decisionTreeService;

        private static DetermineNCRGuidelineMinMonthlyFixedExpensesQuery query;

        private static Exception exception;

        private static decimal grossIncome;

        Establish context = () =>
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Clear();
            grossIncome = 34000m;
            query = new DetermineNCRGuidelineMinMonthlyFixedExpensesQuery(grossIncome);
            decisionTreeServiceClient = An<IDecisionTreeServiceClient>();
            decisionTreeServiceClient.WhenToldTo(x => x.PerformQuery(Param.IsAny<NCRAffordabilityAssessment_1Query>())).Throw(new Exception("Invalid Gross Income"));
            decisionTreeService = new DecisionTreeService(decisionTreeServiceClient);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                decisionTreeService.DetermineNCRGuidelineMinMonthlyFixedExpenses(query);
            });
        };

        It should_catch_the_exception = () =>
        {
            exception.ShouldNotBeNull();
        };

        It should_have_the_exception_messages_in_the_principal_cache = () =>
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            spc.DomainMessages.Count().ShouldEqual(2);
            spc.DomainMessages.Clear();
        };
    }
}

