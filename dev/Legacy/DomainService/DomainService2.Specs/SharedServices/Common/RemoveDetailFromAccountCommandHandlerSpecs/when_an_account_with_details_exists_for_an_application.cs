using System.Collections.Generic;
using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.SharedServices.Common.RemoveDetailFromAccountCommandHandlerSpecs
{
    [Subject(typeof(RemoveDetailFromAccountCommandHandler))]
    public class when_an_account_with_details_exists_for_an_application : DomainServiceSpec<RemoveDetailFromAccountCommand, RemoveDetailFromAccountCommandHandler>
    {
        protected static IAccountRepository accountRepository;

        // Arrange
        Establish context = () =>
            {
                accountRepository = An<IAccountRepository>();
                IDetail detail = An<IDetail>();
                IAccount account = An<IAccount>();
                int detailTypeKey = 1;
                account.WhenToldTo(x => x.Key).Return(1);
                IReadOnlyEventList<IDetail> details = new ReadOnlyEventList<IDetail>(new List<IDetail> { detail });

                accountRepository.WhenToldTo(x => x.GetDetailByAccountKeyAndDetailType(Param<int>.IsAnything, Param<int>.IsAnything))
                    .Return(details);

                accountRepository.WhenToldTo(x => x.GetDetailTypeKeyByDescription(Param<string>.IsAnything))
                    .Return(detailTypeKey);

                accountRepository.WhenToldTo(x => x.DeleteDetail(Param<IDetail>.IsAnything));

                accountRepository.WhenToldTo(x => x.GetAccountByApplicationKey(Param<int>.IsAnything))
                    .Return(account);

                command = new RemoveDetailFromAccountCommand(Param<int>.IsAnything, Param<string>.IsAnything);
                handler = new RemoveDetailFromAccountCommandHandler(accountRepository);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_remove_detail_from_the_account = () =>
            {
                accountRepository.WasToldTo(x => x.DeleteDetail(Param<IDetail>.IsAnything));
            };
    }
}