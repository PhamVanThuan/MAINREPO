using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.BankAccountDomain.CommandHandlers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.LinkBankAccountToClient
{
    public class when_no_link_or_bank_account_exists : WithCoreFakes
    {
        private static LinkBankAccountToClientCommand command;
        private static LinkBankAccountToClientCommandHandler handler;
        private static IBankAccountDataManager bankAccountDataManager;
        private static BankAccountModel bankAccountModel;
        private static int clientKey;
        private static Guid clientBankAccountGuid;
        private static IEnumerable<BankAccountDataModel> existingBankAccounts;
        private static int clientBankAccountKey;
        private static int newBankAccountKey;
        private static Guid bankAccountGuid;

        private Establish context = () =>
        {
            clientBankAccountKey = 9999;
            bankAccountDataManager = An<IBankAccountDataManager>();
            clientKey = 1234;
            clientBankAccountGuid = CombGuid.Instance.Generate();
            bankAccountModel = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "CT Speed", "System");
            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
            handler = new LinkBankAccountToClientCommandHandler(bankAccountDataManager, linkedKeyManager, serviceCommandRouter, combGuid, unitOfWorkFactory, eventRaiser);

            //find existing bank account
            existingBankAccounts = Enumerable.Empty<BankAccountDataModel>();
            bankAccountDataManager.WhenToldTo(x => x.FindExistingBankAccount(bankAccountModel)).Return(existingBankAccounts);

            //generate bank account guid
            bankAccountGuid = CombGuid.Instance.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(bankAccountGuid);

            //add bank account
            ISystemMessageCollection serviceMessageCollection = SystemMessageCollection.Empty();
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand<AddBankAccountCommand>(Param.IsAny<AddBankAccountCommand>(), Param.IsAny<IServiceRequestMetadata>())).Return(serviceMessageCollection);

            //retrieve linked bank account key
            newBankAccountKey = 1234;
            linkedKeyManager.WhenToldTo(x => x.RetrieveLinkedKey(bankAccountGuid)).Return(newBankAccountKey);

            //Add bank account to client
            bankAccountDataManager.WhenToldTo(x => x.AddBankAccountToClient(clientKey, newBankAccountKey)).Return(clientBankAccountKey);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_bank_account_exists = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingBankAccount(bankAccountModel));
        };

        private It should_add_bank_account_to_system = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddBankAccountCommand>(
                Arg.Is<AddBankAccountCommand>(y => y.BankAccountGuid == bankAccountGuid && y.BankAccountModel.AccountNumber == bankAccountModel.AccountNumber),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_retrieve_the_linked_bank_account_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(bankAccountGuid));
        };

        private It should_delete_the_linked_bank_account_key = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(bankAccountGuid));
        };

        private It should_not_check_if_the_client_bank_account_exists = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.FindExistingClientBankAccount(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_reactivate_client_bank_account = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.ReactivateClientBankAccount(Param.IsAny<int>()));
        };

        private It should_add_the_bank_account_to_the_client = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.AddBankAccountToClient(clientKey, newBankAccountKey));
        };

        private It should_link_new_client_bank_account_key_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(clientBankAccountKey, clientBankAccountGuid)).OnlyOnce();
        };

        private It should_raise_a_bank_account_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<BankAccountLinkedToClientEvent>
                (y => y.AccountNumber == command.BankAccountModel.AccountNumber &&
                      y.ClientKey == command.ClientKey &&
                      y.ClientBankAccountKey == clientBankAccountKey &&
                      y.BankAccountKey == newBankAccountKey &&
                      y.AccountName == command.BankAccountModel.AccountName),
                    clientBankAccountKey, (int)GenericKeyType.LegalEntityBankAccount, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_raise_event_once = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<BankAccountLinkedToClientEvent>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>())).OnlyOnce();
        };

        private It should_delete_linked_key_once = () =>
        {
            linkedKeyManager.WasToldTo(x => x.DeleteLinkedKey(Param.IsAny<Guid>())).OnlyOnce();
        };

        private It should_retrieve_linked_key_once = () =>
        {
            linkedKeyManager.WasToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>())).OnlyOnce();
        };
    }
}