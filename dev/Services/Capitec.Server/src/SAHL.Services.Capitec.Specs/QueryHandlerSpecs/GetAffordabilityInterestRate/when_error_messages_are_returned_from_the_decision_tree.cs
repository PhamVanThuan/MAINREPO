using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.GetAffordabilityInterestRate
{
    public class when_error_messages_are_returned_from_the_decision_tree : WithFakes
    {
        private static IDecisionTreeServiceClient decisionTreeService;
        private static GetAffordabilityInterestRateQuery query;
        private static GetAffordabilityInterestRateQueryHandler handler;
        private static CapitecAffordabilityInterestRate_QueryResult decisionTreeResult;

        private static bool interestRateQuery;
        private static decimal calcRate, deposit, householdIncome;

        private static decimal instalment = 4550;
        private static decimal interestRate = 0.102m;
        private static decimal propertyPrice = 500000;
        private static decimal qualified = 450000;

        private static ISystemMessageCollection treeMessages;
        private static ISystemMessageCollection results;
        Establish context = () =>
        {
            decisionTreeService = An<IDecisionTreeServiceClient>();
            treeMessages = new SystemMessageCollection();
            treeMessages.AddMessage(new SystemMessage("test error message", SystemMessageSeverityEnum.Error));
            interestRateQuery = false;
            calcRate = 0.102m;
            deposit = 50000;
            householdIncome = 15000;
            handler = new GetAffordabilityInterestRateQueryHandler(decisionTreeService);
            query = new GetAffordabilityInterestRateQuery(householdIncome, deposit, calcRate, interestRateQuery);

            decisionTreeResult = new CapitecAffordabilityInterestRate_QueryResult();
            decisionTreeResult.AmountQualifiedFor = (double)qualified;
            decisionTreeResult.Instalment = (double)instalment;
            decisionTreeResult.InterestRate = (double)interestRate;
            decisionTreeResult.PropertyPriceQualifiedFor = (double)propertyPrice;
            decisionTreeService.WhenToldTo(x => x.PerformQuery(Param.IsAny<CapitecAffordabilityInterestRate_Query>())).Return(treeMessages);
            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecAffordabilityInterestRate_Query>())).Callback<CapitecAffordabilityInterestRate_Query>(
                y => { y.Result = new ServiceQueryResult<CapitecAffordabilityInterestRate_QueryResult>(new CapitecAffordabilityInterestRate_QueryResult[] { decisionTreeResult }); });
        };

        Because of = () =>
        {
            results = handler.HandleQuery(query);
        };

        It should_call_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Param<CapitecAffordabilityInterestRate_Query>.Matches(m =>
                m.HouseholdIncome == (double)householdIncome &&
                m.Deposit == (double)deposit &&
                m.CalcRate == (double)calcRate &&
                m.InterestRateQuery == interestRateQuery)));
        };

        It should_add_the_messages_from_tree_to_the_result = () =>
        {
            results.ErrorMessages().First().Message.ShouldEqual("test error message");
        };


    }
}
