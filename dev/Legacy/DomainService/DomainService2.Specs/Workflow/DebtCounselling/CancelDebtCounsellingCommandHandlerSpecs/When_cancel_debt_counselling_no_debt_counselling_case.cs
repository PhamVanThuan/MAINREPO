namespace DomainService2.Specs.Workflow.DebtCounselling.CancelDebtCounsellingCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.DebtCounselling;
    using Machine.Fakes;
    using Machine.Specifications;
    using Rhino.Mocks;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using X2DomainService.Interface.Common;

    [Subject(typeof(CancelDebtCounsellingCommandHandler))]
    public class When_cancel_debt_counselling_no_debt_counselling_case : DomainServiceSpec<CancelDebtCounsellingCommand, CancelDebtCounsellingCommandHandler>
    {
        static Exception exception;

        Establish context = () =>
        {
            MockRepository your_mother_ees_ur_hamster = new MockRepository();
            IDebtCounselling debtCounselling = null;
            IDebtCounsellingRepository debtCounsellingRepository = An<IDebtCounsellingRepository>();
            IReasonRepository reasonRepository = An<IReasonRepository>();
            ICommon commonWorkflowService = An<ICommon>();
            debtCounsellingRepository.WhenToldTo(x => x.GetDebtCounsellingByKey(Param<int>.IsAnything)).Return(debtCounselling);
            command = new CancelDebtCounsellingCommand(Param<int>.IsAnything, Param<int>.IsAnything);
            handler = new CancelDebtCounsellingCommandHandler(debtCounsellingRepository, reasonRepository, commonWorkflowService);
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