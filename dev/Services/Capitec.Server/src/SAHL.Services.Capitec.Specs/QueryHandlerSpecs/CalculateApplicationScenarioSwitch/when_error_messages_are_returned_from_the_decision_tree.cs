﻿using Machine.Fakes;
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
    public class when_error_messages_are_returned_from_the_decision_tree : WithFakes
    {
        private static CalculateApplicationScenarioSwitchQuery query;
        private static CalculateApplicationScenarioQueryResult queryResult;
        private static CalculateApplicationScenarioSwitchQueryHandler handler;
        private static IDecisionTreeServiceClient decisionTreeService;
        private static ILookupManager lookupService;
        private static SwitchLoanDetails loanDetails;
        private static Guid occupancyType, incomeType;
        private static ISystemMessageCollection messages;
        private static ISystemMessageCollection decisionTreeMessages;

        private Establish context = () =>
        {
            decisionTreeService = An<IDecisionTreeServiceClient>();
            messages = An<ISystemMessageCollection>();
            lookupService = An<ILookupManager>();
            decisionTreeMessages = new SystemMessageCollection();
            decisionTreeMessages.AddMessage(new SystemMessage("test warning message", SystemMessageSeverityEnum.Warning));
            decisionTreeMessages.AddMessage(new SystemMessage("test info message", SystemMessageSeverityEnum.Info));
            occupancyType = Guid.Parse(OccupancyTypeEnumDataModel.OWNER_OCCUPIED);
            incomeType = Guid.Parse(EmploymentTypeEnumDataModel.SALARIED);
            loanDetails = new SwitchLoanDetails(occupancyType, incomeType, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, Param<decimal>.IsAnything, 240, false);
            query = new CalculateApplicationScenarioSwitchQuery(loanDetails);

            decisionTreeService.WhenToldTo(x => x.PerformQuery(Param.IsAny<CapitecOriginationCreditPricing_Query>())).Return(decisionTreeMessages);
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

        private It should_contain_the_decision_tree_warning_messages_in_the_query_result = () =>
        {
            queryResult.DecisionTreeMessages.WarningMessages().First().Message.ShouldEqual("test warning message");
        };

        private It should_contain_the_decision_tree_info_messages_in_the_query_result = () =>
        {
            queryResult.DecisionTreeMessages.InfoMessages().First().Message.ShouldEqual("test info message");
        };
    }
}