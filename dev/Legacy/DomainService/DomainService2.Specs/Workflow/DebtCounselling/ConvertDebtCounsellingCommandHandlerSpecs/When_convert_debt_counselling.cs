using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.ConvertDebtCounsellingCommandHandlerSpecs
{
    [Subject(typeof(ConvertDebtCounsellingCommandHandler))]
    public class When_convert_debt_counselling : WithFakes
    {
        static ConvertDebtCounsellingCommand command;
        static ConvertDebtCounsellingCommandHandler handler;
        static IDomainMessageCollection messages;
        static IDebtCounsellingRepository debtCounsellingRepository;
        static ICommonRepository commonRepository;


        Establish context = () =>
        {
            debtCounsellingRepository = An<IDebtCounsellingRepository>();
            commonRepository = An<ICommonRepository>();
            messages = new DomainMessageCollection();
            command = new ConvertDebtCounsellingCommand(Param.IsAny<int>(), Param.IsAny<string>());
            handler = new ConvertDebtCounsellingCommandHandler(debtCounsellingRepository, commonRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_the_repository_method = () =>
        {
            debtCounsellingRepository.WasToldTo(x => x.ConvertDebtCounselling(Param.IsAny<int>(), Param.IsAny<string>()));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}