using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.BankAccountDomain.CommandHandlers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.LinkBankAccountToClient
{
    public class when_no_link_exists_and_the_bank_account_does_exist : WithCoreFakes
    {
        private static LinkBankAccountToClientCommand command;
        private static LinkBankAccountToClientCommandHandler handler;
        private static IBankAccountDataManager bankAccountDataManager;
        private static BankAccountModel bankAccountModel;
        private static int clientKey, bankAccountKey;
        private static Guid clientBankAccountGuid;
        private static IEnumerable<LegalEntityBankAccountDataModel> existingClientBankAccount;
        private static IEnumerable<BankAccountDataModel> existingBankAccounts;
        private static int clientBankAccountKey;

        private Establish context = () =>
        {
            clientBankAccountKey = 9999;
            bankAccountKey = 1234;
            existingClientBankAccount = Enumerable.Empty<LegalEntityBankAccountDataModel>();
            existingBankAccounts = new BankAccountDataModel[] { new BankAccountDataModel(bankAccountKey, "1352", "1352087464", (int)ACBType.Current, "CT Speed", "System", null) };
            bankAccountDataManager = An<IBankAccountDataManager>();
            clientKey = 1234;
            clientBankAccountGuid = CombGuid.Instance.Generate();
            bankAccountModel = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "CT Speed", "System");
            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
            handler = new LinkBankAccountToClientCommandHandler(bankAccountDataManager, linkedKeyManager, serviceCommandRouter, combGuid, unitOfWorkFactory, eventRaiser);

            bankAccountDataManager.WhenToldTo(x => x.FindExistingBankAccount(bankAccountModel)).Return(existingBankAccounts);

            bankAccountDataManager.WhenToldTo(x => x.FindExistingClientBankAccount(clientKey, existingBankAccounts.First().BankAccountKey)).Return(existingClientBankAccount);

            bankAccountDataManager.WhenToldTo(x => x.AddBankAccountToClient(clientKey, existingBankAccounts.First().BankAccountKey)).Return(clientBankAccountKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_bank_account_exists = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingBankAccount(bankAccountModel));
        };

        private It should_not_add_the_bank_account = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AddBankAccountCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_try_retrieve_the_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_not_remove_the_linked_key = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_check_if_the_client_bank_account_exists = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingClientBankAccount(clientKey, existingBankAccounts.First().BankAccountKey));
        };

        private It should_not_reactivate_any_existing_link = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.ReactivateClientBankAccount(Param.IsAny<int>()));
        };

        private It should_add_the_bank_account_to_the_client = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.AddBankAccountToClient(clientKey, existingBankAccounts.First().BankAccountKey));
        };

        private It should_link_new_client_bank_account_key_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(clientBankAccountKey, clientBankAccountGuid));
        };

        private It should_raise_a_bank_account_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<BankAccountLinkedToClientEvent>
                (y => y.AccountNumber == command.BankAccountModel.AccountNumber &&
                      y.ClientKey == command.ClientKey &&
                      y.ClientBankAccountKey == clientBankAccountKey &&
                      y.BankAccountKey == bankAccountKey &&
                      y.AccountName == command.BankAccountModel.AccountName &&
                      y.BranchCode == command.BankAccountModel.BranchCode &&
                      y.BranchName == command.BankAccountModel.BranchName),
                    clientBankAccountKey, (int)GenericKeyType.LegalEntityBankAccount, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}