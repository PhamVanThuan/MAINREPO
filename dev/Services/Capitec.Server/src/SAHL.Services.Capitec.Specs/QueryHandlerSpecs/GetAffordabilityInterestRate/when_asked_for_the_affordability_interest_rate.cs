using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.GetAffordabilityInterestRate
{
    public class when_asked_for_the_affordability_interest_rate : WithFakes
    {
        private static IDecisionTreeServiceClient decisionTreeService;
        private static GetAffordabilityInterestRateQuery query;
        private static GetAffordabilityInterestRateQueryHandler handler;
        private static CapitecAffordabilityInterestRate_QueryResult decisionTreeResult;

        private static bool interestRateQuery;
        private static decimal calcRate;
        private static decimal deposit;
        private static decimal householdIncome;

        private static decimal instalment = 4550;
        private static decimal interestRate = 0.102m;
        private static decimal propertyPrice = 500000;
        private static decimal qualified = 450000;
        Establish context = () =>
        {
            decisionTreeService = An<IDecisionTreeServiceClient>();
            interestRateQuery = false;
            calcRate = 0.102m;
            deposit = 50000;
            householdIncome = 15000;
            handler = new GetAffordabilityInterestRateQueryHandler(decisionTreeService);
            query = new GetAffordabilityInterestRateQuery(householdIncome, deposit, calcRate, interestRateQuery);

            decisionTreeResult = new CapitecAffordabilityInterestRate_QueryResult();
            decisionTreeResult.AmountQualifiedFor = (double)qualified;
            decisionTreeResult.Instalment = (double)instalment ;
            decisionTreeResult.InterestRate = (double)interestRate;
            decisionTreeResult.PropertyPriceQualifiedFor = (double)propertyPrice;
            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecAffordabilityInterestRate_Query>())).Callback<CapitecAffordabilityInterestRate_Query>(
                y=> { y.Result = new ServiceQueryResult<CapitecAffordabilityInterestRate_QueryResult>(new CapitecAffordabilityInterestRate_QueryResult[] { decisionTreeResult }); });
        };
        Because of = () =>
        {
            handler.HandleQuery(query);
        };
        It should_call_the_decision_tree = () =>
        {
            decisionTreeService.WasToldTo(x => x.PerformQuery(Param<CapitecAffordabilityInterestRate_Query>.Matches(m => 
                m.HouseholdIncome == (double)householdIncome &&
                m.Deposit == (double) deposit &&
                m.CalcRate == (double)calcRate &&
                m.InterestRateQuery == interestRateQuery)));
        };
        It should_return_the_expected_result = () =>
        {
            query.Result.Instalment.ShouldEqual(instalment);
            query.Result.InterestRate.ShouldEqual(interestRate);
            query.Result.PropertyPriceQualifiedFor.ShouldEqual(propertyPrice);
            query.Result.AmountQualifiedFor.ShouldEqual(qualified);
        };

    }
}
