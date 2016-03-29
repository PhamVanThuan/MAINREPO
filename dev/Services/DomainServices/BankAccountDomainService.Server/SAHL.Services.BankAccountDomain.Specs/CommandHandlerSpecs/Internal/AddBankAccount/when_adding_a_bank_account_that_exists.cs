using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.BankAccountDomain.CommandHandlers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.AddBankAccount
{
    internal class when_adding_a_bank_account_that_exists : WithCoreFakes
    {
        private static IBankAccountDataManager bankAccountDataManager;
        private static AddBankAccountCommand command;
        private static AddBankAccountCommandHandler handler;
        private static BankAccountModel bankAccountModel;
        private static Guid bankAccountGuid;
        private static int bankAccountKey;
        private static IDomainRuleManager<BankAccountModel> domainRuleManager;
        private static IServiceRequestMetadata serviceRequestMetaData;
        private static ICdvValidationManager validationManager;

        private Establish context = () =>
        {
            bankAccountDataManager = An<IBankAccountDataManager>();
            validationManager = An<ICdvValidationManager>();
            domainRuleManager = An<IDomainRuleManager<BankAccountModel>>();
            serviceRequestMetaData = An<IServiceRequestMetadata>();
            bankAccountGuid = CombGuid.Instance.Generate();
            bankAccountModel = new BankAccountModel("051", "Berea", "323232", SAHL.Core.BusinessModel.Enums.ACBType.Current, "Mr John Doe", "5005");
            command = new AddBankAccountCommand(bankAccountGuid, bankAccountModel);
            handler = new AddBankAccountCommandHandler(bankAccountDataManager, linkedKeyManager, domainRuleManager, validationManager, unitOfWorkFactory);
            bankAccountDataManager.WhenToldTo(x => x.FindExistingBankAccount(bankAccountModel))
                .Return(new List<BankAccountDataModel>() { new BankAccountDataModel("051", "323232", (int)SAHL.Core.BusinessModel.Enums.ACBType.Current, "Mr John Doe", "5005", DateTime.Now) });
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_for_existing_bank_account = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingBankAccount(bankAccountModel));
        };

        private It should_not_save_bank_account = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.SaveBankAccount(bankAccountModel));
        };

        private It should_not_link_bank_account_key_bank_account_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.LinkKeyToGuid(bankAccountKey, bankAccountGuid));
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Bank Account already exists.");
        };
    }
}