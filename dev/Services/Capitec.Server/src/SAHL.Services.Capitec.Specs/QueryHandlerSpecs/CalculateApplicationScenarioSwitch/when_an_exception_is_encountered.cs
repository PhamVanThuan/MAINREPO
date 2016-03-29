using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Capitec.QueryHandlers;
using SAHL.Services.Interfaces.Capitec.Queries;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using SAHL.Services.Interfaces.DecisionTree;
using SAHL.Services.Interfaces.DecisionTree.Models;
using SAHL.Services.Interfaces.DecisionTree.Queries;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.QueryHandlerSpecs.CalculateApplicationScenarioSwitch
{
    public class when_an_exception_is_encountered : WithFakes
    {
        private static CalculateApplicationScenarioSwitchQuery query;
        private static CalculateApplicationScenarioSwitchQueryHandler handler;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static ILookupManager lookupService;
        private static SwitchLoanDetails loanDetails;
        private static Guid occupancyType, incomeType;
        private static System.Exception exception;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            decisionTreeService = An<IDecisionTreeServiceClient>();
            messages = An<ISystemMessageCollection>();
            lookupService = An<ILookupManager>();
            occupancyType = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
            incomeType = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
            loanDetails = new SwitchLoanDetails(occupancyType, incomeType, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, 240, true);
            query = new CalculateApplicationScenarioSwitchQuery(loanDetails);
            handler = new CalculateApplicationScenarioSwitchQueryHandler(decisionTreeService, lookupService);

            IServiceQueryResult<CapitecOriginationCreditPricing_QueryResult> result = null;

            decisionTreeService.WhenToldTo<IDecisionTreeServiceClient>(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Throw(new NullReferenceException());
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => messages = handler.HandleQuery(query));
        };

        private It should_return_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Could not contact the Capitec Origination Credit Pricing Decision Tree.");
        };
    }
}