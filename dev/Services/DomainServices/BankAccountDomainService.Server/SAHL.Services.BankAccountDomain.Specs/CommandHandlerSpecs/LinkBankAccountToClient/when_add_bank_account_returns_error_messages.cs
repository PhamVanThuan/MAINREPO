using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.BankAccountDomain.CommandHandlers;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.LinkBankAccountToClient
{
    public class when_add_bank_account_returns_error_messages : WithCoreFakes
    {
        private static LinkBankAccountToClientCommand command;
        private static LinkBankAccountToClientCommandHandler handler;
        private static IBankAccountDataManager bankAccountDataManager;
        private static BankAccountModel bankAccountModel;
        private static int clientKey;
        private static Guid clientBankAccountGuid, bankAccountGuid;
        private static IEnumerable<LegalEntityBankAccountDataModel> existingClientBankAccount;
        private static IEnumerable<BankAccountDataModel> existingBankAccounts;
        private static ISystemMessageCollection subCommandMessages;

        private Establish context = () =>
        {
            existingClientBankAccount = Enumerable.Empty<LegalEntityBankAccountDataModel>();
            existingBankAccounts = Enumerable.Empty<BankAccountDataModel>();
            bankAccountDataManager = An<IBankAccountDataManager>();

            clientKey = 1234;

            subCommandMessages = SystemMessageCollection.Empty();
            subCommandMessages.AddMessage(new SystemMessage("Failed to save bank account.", SystemMessageSeverityEnum.Error));

            clientBankAccountGuid = combGuid.Generate();
            bankAccountModel = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "CT Speed", "System");

            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
            handler = new LinkBankAccountToClientCommandHandler(bankAccountDataManager, linkedKeyManager, serviceCommandRouter, combGuid, unitOfWorkFactory, eventRaiser);

            bankAccountDataManager.WhenToldTo(x => x.FindExistingBankAccount(bankAccountModel)).Return(existingBankAccounts);

            bankAccountGuid = combGuid.Generate();
            combGuid.WhenToldTo(x => x.Generate()).Return(bankAccountGuid);

            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddBankAccountCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(subCommandMessages);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_bank_account_exists = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingBankAccount(bankAccountModel));
        };

        private It should_add_bank_account_to_system = () =>
        {
            serviceCommandRouter.WasToldTo(x => x.HandleCommand<AddBankAccountCommand>(
                Arg.Is<AddBankAccountCommand>(y => y.BankAccountGuid == bankAccountGuid && y.BankAccountModel.AccountNumber == bankAccountModel.AccountNumber),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_try_retrieve_the_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_not_remove_the_linked_key = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_not_find_existing_client_bank_account = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.FindExistingClientBankAccount(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_reactivate_client_bank_account=()=>
        {
            bankAccountDataManager.WasNotToldTo(x=>x.ReactivateClientBankAccount(Param.IsAny<int>()));
        };

        private It should_not_add_client_bank_account = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.AddBankAccountToClient(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_return_error_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("Failed to save bank account.");
        };
    }
}