using System;
using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.SharedServices.PerformEworkActionCommandHandlerSpecs
{
    [Subject(typeof(PerformEWorkActionCommandHandler))]
    public class When_unable_to_find_ework_action_for_case : DomainServiceSpec<PerformEWorkActionCommand, PerformEWorkActionCommandHandler>
    {
        private static IDebtCounsellingRepository debtCounsellingRepository;

        private static Exception exception;

        Establish context = () =>
        {
            var eWorkEngine = An<IeWork>();
            debtCounsellingRepository = An<IDebtCounsellingRepository>();

            command = new PerformEWorkActionCommand("1", "", 1, "", "");
            handler = new PerformEWorkActionCommandHandler(eWorkEngine, debtCounsellingRepository);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}