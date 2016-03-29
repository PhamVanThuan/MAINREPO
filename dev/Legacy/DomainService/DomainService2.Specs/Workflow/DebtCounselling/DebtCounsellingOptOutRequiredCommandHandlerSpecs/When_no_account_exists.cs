using DomainService2.Workflow.DebtCounselling;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.DebtCounselling.DebtCounsellingOptOutRequiredCommandHandlerSpecs
{
    [Subject(typeof(DebtCounsellingOptOutRequiredCommandHandler))]
    public class When_no_account_exists : WithFakes
    {
        static DebtCounsellingOptOutRequiredCommand command;
        static DebtCounsellingOptOutRequiredCommandHandler handler;
        static IDomainMessageCollection messages;
        static IAccountRepository accountRepository;
        static IAccount account;

        Establish context = () =>
        {
            accountRepository = An<IAccountRepository>();
            account = An<IAccount>();

            account = null;
            accountRepository.WhenToldTo(x => x.GetAccountByKey(Param.IsAny<int>())).Return(account);

            messages = new DomainMessageCollection();
            command = new DebtCounsellingOptOutRequiredCommand(Param.IsAny<int>());
            handler = new DebtCounsellingOptOutRequiredCommandHandler(accountRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}