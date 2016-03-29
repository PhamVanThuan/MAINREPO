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
    public class When_variable_loan_and_interest_only : WithFakes
    {
        static DebtCounsellingOptOutRequiredCommand command;
        static DebtCounsellingOptOutRequiredCommandHandler handler;
        static IDomainMessageCollection messages;
        static IAccountRepository accountRepository;
        static IAccount account;
        static IAccountInstallmentSummary accountInstallmentSummary;

        Establish context = () =>
        {
            accountRepository = An<IAccountRepository>();
            account = An<IAccount>();
            accountInstallmentSummary = An<IAccountInstallmentSummary>();

            accountRepository.WhenToldTo(x => x.GetAccountByKey(Param.IsAny<int>())).Return(account);
            account.WhenToldTo(x => x.Product.Key).Return((int)SAHL.Common.Globals.Products.VariableLoan);
            account.WhenToldTo(x => x.InstallmentSummary).Return(accountInstallmentSummary);
            accountInstallmentSummary.WhenToldTo(x => x.IsInterestOnly).Return(true);

            messages = new DomainMessageCollection();
            command = new DebtCounsellingOptOutRequiredCommand(Param.IsAny<int>());
            handler = new DebtCounsellingOptOutRequiredCommandHandler(accountRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}