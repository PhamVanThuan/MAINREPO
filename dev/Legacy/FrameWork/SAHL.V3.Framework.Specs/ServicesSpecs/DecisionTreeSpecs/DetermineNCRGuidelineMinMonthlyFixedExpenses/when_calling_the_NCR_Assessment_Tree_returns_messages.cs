using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
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
    public class when_calling_the_NCR_Assessment_Tree_returns_messages : WithFakes
    {
        private static IDecisionTreeServiceClient decisionTreeServiceClient;
        private static IDecisionTreeService decisionTreeService;
        private static DetermineNCRGuidelineMinMonthlyFixedExpensesQuery query;
        private static decimal grossIncome;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            grossIncome = 34000m;
            query = new DetermineNCRGuidelineMinMonthlyFixedExpensesQuery(grossIncome);

            messages = SystemMessageCollection.Empty();
            messages.AddMessage(new SystemMessage("Error Message 1", SystemMessageSeverityEnum.Error));

            decisionTreeServiceClient = An<IDecisionTreeServiceClient>();
            decisionTreeServiceClient.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<NCRAffordabilityAssessment_1Query>())).Callback<NCRAffordabilityAssessment_1Query>(y =>
            {
                var result = new NCRAffordabilityAssessment_1QueryResult();
                result.MinMonthlyFixedExpenses = 9000;

                var results = new List<NCRAffordabilityAssessment_1QueryResult>(){
                    result 
                };

                y.Result = new ServiceQueryResult<NCRAffordabilityAssessment_1QueryResult>(results);
            });

            decisionTreeServiceClient.WhenToldTo(x => x.PerformQuery(Param.IsAny<NCRAffordabilityAssessment_1Query>())).Return(messages);

            decisionTreeService = new DecisionTreeService(decisionTreeServiceClient);
        };

        Because of = () =>
        {
            
            decisionTreeService.DetermineNCRGuidelineMinMonthlyFixedExpenses(query);
        };

        It should_return_messages_in_query_result = () =>
        {
            query.Messages.AllMessages.Count().ShouldEqual(1);
        };
    }
}
