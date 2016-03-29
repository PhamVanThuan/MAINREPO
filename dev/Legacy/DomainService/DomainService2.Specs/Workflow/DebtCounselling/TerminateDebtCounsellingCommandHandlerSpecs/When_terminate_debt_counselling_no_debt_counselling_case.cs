namespace DomainService2.Specs.Workflow.DebtCounselling.TerminateDebtCounsellingCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.DebtCounselling;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using X2DomainService.Interface.Common;

    [Subject(typeof(TerminateDebtCounsellingCommandHandler))]
    public class When_terminate_debt_counselling_no_debt_counselling_case : DomainServiceSpec<TerminateDebtCounsellingCommand, TerminateDebtCounsellingCommandHandler>
    {
        static Exception exception;
        static ICommonRepository commonRepository;


        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            MockRepository your_mother_ees_ur_hamster = new MockRepository();
            IDebtCounselling debtCounselling = null;
            IDebtCounsellingRepository debtCounsellingRepository = An<IDebtCounsellingRepository>();
            ICommon commonWorkflowService = An<ICommon>();
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);
            command = new TerminateDebtCounsellingCommand(Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new TerminateDebtCounsellingCommandHandler(debtCounsellingRepository, commonWorkflowService, commonRepository);
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
    }
}