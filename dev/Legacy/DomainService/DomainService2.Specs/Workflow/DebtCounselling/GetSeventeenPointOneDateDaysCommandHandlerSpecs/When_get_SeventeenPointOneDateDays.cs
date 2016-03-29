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
    public class When_get_SeventeenPointOneDateDays : WithFakes
    {
        protected static GetSeventeenPointOneDateDaysCommand command;
        protected static GetSeventeenPointOneDateDaysCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static DateTime? seventeenPointOneDatePlusDays = DateTime.Now;
        protected static DateTime? today = DateTime.Now;
        protected static IStageDefinitionRepository stageDefinitionRepository;
        protected static ICommonRepository commonRepository;
        protected static IStageTransition latestStageTransition;

        Establish context = () =>
        {
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            commonRepository = An<ICommonRepository>();
            IList<IStageTransition> stList = new List<IStageTransition>();
            latestStageTransition = An<IStageTransition>();

            stageDefinitionRepository.WhenToldTo(x => x.GetStageTransitionList(Param<int>.IsAnything, Param<int>.IsAnything, Param<List<int>>.IsAnything)).Return(stList);
            stList.Add(latestStageTransition);

            latestStageTransition.WhenToldTo(x => x.EndTransitionDate).Return(today);

            commonRepository.WhenToldTo(x => x.GetnWorkingDaysFromDate(1, today.Value)).Return(seventeenPointOneDatePlusDays.Value);

            messages = new DomainMessageCollection();
            command = new GetSeventeenPointOneDateDaysCommand(1, 1);
            handler = new GetSeventeenPointOneDateDaysCommandHandler(stageDefinitionRepository, commonRepository);
        };
        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_return_no_error_message = () =>
        {
            messages.Count.Equals(0);
        };
        It should_return_valid_date = () =>
        {
            command.SeventeenPointOneDatePlusDaysResult.ShouldEqual<DateTime?>(seventeenPointOneDatePlusDays);
        };
    }
}