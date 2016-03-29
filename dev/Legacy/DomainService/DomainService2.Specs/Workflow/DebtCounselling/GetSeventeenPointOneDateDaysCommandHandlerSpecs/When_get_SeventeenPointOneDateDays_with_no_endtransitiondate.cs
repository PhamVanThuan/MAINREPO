using System;
using System.Collections.Generic;
using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.GetSeventeenPointOneDateDaysCommandHandlerSpecs
{
    [Subject(typeof(GetSeventeenPointOneDateDaysCommandHandler))]
    public class When_get_SeventeenPointOneDateDays_with_no_endtransitiondate : WithFakes
    {
        protected static GetSeventeenPointOneDateDaysCommand command;
        protected static GetSeventeenPointOneDateDaysCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static DateTime? seventeenPointOneDatePlusDays;
        protected static IStageDefinitionRepository stageDefinitionRepository;
        protected static ICommonRepository commonRepository;
        protected static IStageTransition latestStageTransition;
        static Exception exception;

        Establish context = () =>
        {
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            commonRepository = An<ICommonRepository>();
            IList<IStageTransition> stList = new List<IStageTransition>();
            latestStageTransition = An<IStageTransition>();

            stageDefinitionRepository.WhenToldTo(x => x.GetStageTransitionList(Param<int>.IsAnything, Param<int>.IsAnything, Param<List<int>>.IsAnything)).Return(stList);
            stList.Add(latestStageTransition);

            seventeenPointOneDatePlusDays = null;

            latestStageTransition.WhenToldTo(x => x.EndTransitionDate).Return(seventeenPointOneDatePlusDays);

            messages = new DomainMessageCollection();
            command = new GetSeventeenPointOneDateDaysCommand(1, 1);
            handler = new GetSeventeenPointOneDateDaysCommandHandler(stageDefinitionRepository, commonRepository);
        };
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_throw_exception = () =>
        {
            exception.ShouldBeOfType(typeof(Exception));
            exception.ShouldNotBeNull();
        };
        It should_return_null_date = () =>
        {
            command.SeventeenPointOneDatePlusDaysResult.ShouldEqual<DateTime?>(null);
        };
    }
}