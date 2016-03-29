using System;
using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common;

namespace DomainService2.Specs.SharedServices.Common.PerformEworkActionCommandHandlerSpecs
{
    [Subject(typeof(PerformEWorkActionCommandHandler))]
    public class When_able_to_find_ework_action_for_case : DomainServiceSpec<PerformEWorkActionCommand, PerformEWorkActionCommandHandler>
    {
        private static PerformEWorkActionCommand command1;
        private static PerformEWorkActionCommand command2;
        private static PerformEWorkActionCommand command3;
        private static PerformEWorkActionCommand command4;
        private static PerformEWorkActionCommand command5;
        private static PerformEWorkActionCommand command6;
        private static PerformEWorkActionCommand command7;
        private static PerformEWorkActionCommand command8;
        private static PerformEWorkActionCommand command9;
        private static PerformEWorkActionCommand command10;
        private static PerformEWorkActionCommand command11;
        private static PerformEWorkActionCommand command12;
        private static PerformEWorkActionCommand command13;

        private static IDebtCounsellingRepository debtCounsellingRepository;

        private static Exception exception;

        Establish context = () =>
        {
            IeWork eWorkEngine = An<IeWork>();
            debtCounsellingRepository = An<IDebtCounsellingRepository>();

            command1 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2ClientWonOver, 1, "", "");
            command2 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2ClientRefused, 1, "", "");
            command3 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2NTUAdvise, 1, "", "");
            command4 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2UNREGISTER, 1, "", "");
            command5 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2REINSTRUCTED, 1, "", "");
            command6 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2ARCHIVE, 1, "", "");
            command7 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2ROLLBACKDISBURSEMENT, 1, "", "");
            command8 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2DISBURSEMENTTIMER, 1, "", "");
            command9 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2RESUB, 1, "", "");
            command10 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2HOLDOVER, 1, "", "");
            command11 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2DECLINEFINAL, 1, "", "");

            command12 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2DebtCounselling, 1, "", "Current Stage");
            command13 = new PerformEWorkActionCommand("1", Constants.EworkActionNames.X2ReturnDebtCounselling, 1, "", "Debt Counselling");

            handler = new PerformEWorkActionCommandHandler(eWorkEngine, debtCounsellingRepository);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command1));
            exception = Catch.Exception(() => handler.Handle(messages, command2));
            exception = Catch.Exception(() => handler.Handle(messages, command3));
            exception = Catch.Exception(() => handler.Handle(messages, command4));
            exception = Catch.Exception(() => handler.Handle(messages, command5));
            exception = Catch.Exception(() => handler.Handle(messages, command6));
            exception = Catch.Exception(() => handler.Handle(messages, command7));
            exception = Catch.Exception(() => handler.Handle(messages, command8));
            exception = Catch.Exception(() => handler.Handle(messages, command9));
            exception = Catch.Exception(() => handler.Handle(messages, command10));
            exception = Catch.Exception(() => handler.Handle(messages, command11));

            exception = Catch.Exception(() => handler.Handle(messages, command12));
            exception = Catch.Exception(() => handler.Handle(messages, command13));
        };

        It should_return_false = () =>
        {
            command1.Result.ShouldBeTrue();
            command2.Result.ShouldBeTrue();
            command3.Result.ShouldBeTrue();
            command4.Result.ShouldBeTrue();
            command5.Result.ShouldBeTrue();
            command6.Result.ShouldBeTrue();
            command7.Result.ShouldBeTrue();
            command8.Result.ShouldBeTrue();
            command9.Result.ShouldBeTrue();
            command10.Result.ShouldBeTrue();
            command11.Result.ShouldBeTrue();
            command12.Result.ShouldBeTrue();
            command13.Result.ShouldBeTrue();
        };
    }
}