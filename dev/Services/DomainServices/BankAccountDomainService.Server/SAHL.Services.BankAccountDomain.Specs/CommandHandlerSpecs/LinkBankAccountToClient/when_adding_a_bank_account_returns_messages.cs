using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
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

namespace SAHL.Services.BankAccountDomain.Specs.CommandHandlerSpecs.LinkBankAccountToClient
{
    public class when_adding_a_bank_account_returns_messages : WithCoreFakes
    {
        private static LinkBankAccountToClientCommand command;
        private static LinkBankAccountToClientCommandHandler handler;
        private static IBankAccountDataManager bankAccountDataManager;
        private static BankAccountModel bankAccountModel;
        private static int clientKey;
        private static Guid clientBankAccountGuid;
        private static IEnumerable<LegalEntityBankAccountDataModel> existingClientBankAccount;
        private static IEnumerable<BankAccountDataModel> existingBankAccounts;
        private static Guid generatedGuid;
        private static ISystemMessageCollection subCommandMessages;

        private Establish context = () =>
        {
            generatedGuid = CombGuid.Instance.Generate();
            existingClientBankAccount = Enumerable.Empty<LegalEntityBankAccountDataModel>();
            existingBankAccounts = Enumerable.Empty<BankAccountDataModel>();
            bankAccountDataManager = An<IBankAccountDataManager>();
            clientKey = 1234;
            subCommandMessages = SystemMessageCollection.Empty();
            subCommandMessages.AddMessage(new SystemMessage("message", SystemMessageSeverityEnum.Exception));
            clientBankAccountGuid = CombGuid.Instance.Generate();
            bankAccountModel = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "CT Speed", "System");
            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
            handler = new LinkBankAccountToClientCommandHandler(bankAccountDataManager, linkedKeyManager, serviceCommandRouter, combGuid, unitOfWorkFactory, eventRaiser);
            bankAccountDataManager.WhenToldTo(x => x.FindExistingBankAccount(bankAccountModel)).Return(existingBankAccounts);
            serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param.IsAny<AddBankAccountCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                .Return(subCommandMessages);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        It should_not_try_retrieve_the_guid = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.RetrieveLinkedKey(Param.IsAny<Guid>()));
        };

        It should_not_add_the_client_bank_account = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.AddBankAccountToClient(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        It should_not_remove_the_linked_key = () =>
        {
            linkedKeyManager.WasNotToldTo(x => x.DeleteLinkedKey(Param.IsAny<Guid>()));
        };

        private It should_should_not_raise_an_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<Event>(),
                    Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };

    }
}
