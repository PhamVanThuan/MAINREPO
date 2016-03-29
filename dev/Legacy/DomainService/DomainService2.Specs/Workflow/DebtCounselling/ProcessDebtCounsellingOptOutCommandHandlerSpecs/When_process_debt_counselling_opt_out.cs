using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.ProcessDebtCounsellingOptOutCommandHandlerSpecs
{
    [Subject(typeof(ProcessDebtCounsellingOptOutCommandHandler))]
    public class When_process_debt_counselling_opt_out : WithFakes
    {
        static ProcessDebtCounsellingOptOutCommand command;
        static ProcessDebtCounsellingOptOutCommandHandler handler;
        static IDomainMessageCollection messages;
        static IDebtCounsellingRepository debtCounsellingRepository;
        static ICommonRepository commonRepository;


        Establish context = () =>
        {
            commonRepository = An<ICommonRepository>();
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            commonRepository = An<ICommonRepository>();
            messages = new DomainMessageCollection();
            command = new ProcessDebtCounsellingOptOutCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new ProcessDebtCounsellingOptOutCommandHandler(debtCounsellingRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_the_repository_method = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.ProcessDebtCounsellingOptOut(Param.IsAny<int>(), Param.IsAny<string>()));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}