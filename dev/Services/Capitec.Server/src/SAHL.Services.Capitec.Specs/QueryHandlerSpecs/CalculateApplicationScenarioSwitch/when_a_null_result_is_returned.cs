using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Models;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.CalculateApplicationScenarioSwitch
{
    public class when_a_null_result_is_returned : WithFakes
    {
        private static CalculateApplicationScenarioSwitchQuery query;
        private static CalculateApplicationScenarioQueryResult queryResult;
        private static CalculateApplicationScenarioSwitchQueryHandler handler;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static ILookupManager lookupService;
        private static SwitchLoanDetails loanDetails;
        private static Guid occupancyType, incomeType;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            decisionTreeService = An<IDecisionTreeServiceClient>();
            messages = An<ISystemMessageCollection>();
            lookupService = An<ILookupManager>();
            occupancyType = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
            incomeType = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
            loanDetails = new SwitchLoanDetails(occupancyType, incomeType, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, 240, false);
            query = new CalculateApplicationScenarioSwitchQuery(loanDetails);
            handler = new CalculateApplicationScenarioSwitchQueryHandler(decisionTreeService, lookupService);

            var capitecOriginationCreditPricing_QueryResult = new CapitecOriginationCreditPricing_QueryResult[] { new CapitecOriginationCreditPricing_QueryResult(){
                     InterestRate = Param<double>.IsAnything,
                     Instalment = Param<double>.IsAnything,
                     LoanAmount = Param<double>.IsAnything,
                     LoantoValue = Param<double>.IsAnything,
                     PaymenttoIncome = Param<double>.IsAnything,
                     EligibleApplication = true
                } };
            IServiceQueryResult<CapitecOriginationCreditPricing_QueryResult> result = new ServiceQueryResult<CapitecOriginationCreditPricing_QueryResult>(capitecOriginationCreditPricing_QueryResult);

            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Callback<CapitecOriginationCreditPricing_Query>(y =>
            {
                y.Result = null;
            });
        };

        private Because of = () =>
        {
            messages = handler.HandleQuery(query);
            queryResult = query.Result;
        };

        private It should_return_eligible_application_as_false = () =>
        {
            queryResult.EligibleApplication.ShouldBeFalse();
        };

        private It should_add_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An error occurred while running the Capitec Origination Credit Pricing Decision Tree.");
        };
    }
}