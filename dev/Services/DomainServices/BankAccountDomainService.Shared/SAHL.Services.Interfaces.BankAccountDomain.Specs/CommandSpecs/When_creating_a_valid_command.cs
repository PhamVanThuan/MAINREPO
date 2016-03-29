using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.BankAccountDomain.Specs.CommandSpecs
{
    class When_creating_a_valid_command
    {
        private static LinkBankAccountToClientCommand command;
        private static BankAccountModel bankAccountModel;
        private static int clientKey;
        private static Guid clientBankAccountGuid;

        Establish context = () =>
        {
            clientBankAccountGuid = CombGuid.Instance.Generate();
            clientKey = 1234;
            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
        };
        Because of = () =>
        {
            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
        };

        It should_contain_a_valid_clientKey = () =>
        {
            command.ShouldBeAssignableTo(typeof(IRequiresClient));
        };
    }
}
