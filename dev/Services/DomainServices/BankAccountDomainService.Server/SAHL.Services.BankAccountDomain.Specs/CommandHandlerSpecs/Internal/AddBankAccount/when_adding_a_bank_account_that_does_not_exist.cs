using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.CommandHandlers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Linq;
using SAHL.Core.Testing;

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.AddBankAccount
{
    public class when_adding_a_bank_account_that_does_not_exist : WithCoreFakes
    {
        private static IBankAccountDataManager bankAccountDataManager;
        private static AddBankAccountCommand command;
        private static AddBankAccountCommandHandler handler;
        private static BankAccountModel bankAccountModel;
        private static Guid bankAccountGuid;
        private static int bankAccountKey;
        private static IDomainRuleManager<BankAccountModel> domainRuleManager;
        private static ICdvValidationManager validationManager;

        private Establish context = () =>
        {
            bankAccountKey = 1;
            validationManager = An<ICdvValidationManager>();
            bankAccountDataManager = An<IBankAccountDataManager>();
            domainRuleManager = An<IDomainRuleManager<BankAccountModel>>();

            bankAccountGuid = CombGuid.Instance.Generate();
            bankAccountModel = new BankAccountModel("051", "Berea", "323232", SAHL.Core.BusinessModel.Enums.ACBType.Current, "Mr John Doe", "5005");
            command = new AddBankAccountCommand(bankAccountGuid, bankAccountModel);
            handler = new AddBankAccountCommandHandler(bankAccountDataManager, linkedKeyManager, domainRuleManager, validationManager, unitOfWorkFactory);
            bankAccountDataManager.WhenToldTo(x => x.SaveBankAccount(bankAccountModel)).Return(bankAccountKey);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_for_an_existing_bank_account = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingBankAccount(bankAccountModel));
        };

        private It should_save_the_new_bank_account = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.SaveBankAccount(bankAccountModel));
        };

        private It should_link_the_bank_account_guid_to_the_inserted_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(bankAccountKey, bankAccountGuid));
        };

        private It should_execute_the_rules = () =>
        {
            domainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), command.BankAccountModel));
        };
    }
}