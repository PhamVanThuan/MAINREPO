using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace DomainService2.Specs.SharedServices.Common.IsRCSAccountCommandHandlerSpecs
{
	[Subject(typeof(IsRCSAccountCommandHandler))]
	public class when_account_is_not_an_rcs_account : DomainServiceSpec<IsRCSAccountCommand, IsRCSAccountCommandHandler>
	{
		protected static IAccountRepository accountRepository;
		protected static IAccount account;
		protected static bool isRCSAccount;
		protected static IOriginationSource originationSource;

		Establish context = () =>
		{
			accountRepository = An<IAccountRepository>();
			account = An<IAccount>();
			originationSource = An<IOriginationSource>();
			originationSource.WhenToldTo(x => x.Key).Return((int)OriginationSources.SAHomeLoans);
			accountRepository.WhenToldTo(x => x.GetAccountByKey(Param.IsAny<int>())).Return(account);
			account.WhenToldTo(x => x.OriginationSource).Return(originationSource);

			command = new IsRCSAccountCommand(Param.IsAny<int>());
			handler = new IsRCSAccountCommandHandler(accountRepository);
		};

		Because of = () =>
		{
			handler.Handle(messages, command);
			isRCSAccount = command.Result;
		};

		It result_should_be_false = () =>
		{
			isRCSAccount.ShouldBeFalse();
		};
	}
}
