using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
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
    public class when_the_link_exists_and_is_active : WithCoreFakes
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
            serviceCommandRouter = An<IServiceCommandRouter>();
            bankAccountDataManager = An<IBankAccountDataManager>();
            clientKey = 1111;
            bankAccountKey = 2222;
            clientBankAccountKey = 3333;
            existingClientBankAccount = new LegalEntityBankAccountDataModel[] 
                                        { 
                                            new LegalEntityBankAccountDataModel(clientBankAccountKey, clientKey, bankAccountKey, (int)GeneralStatus.Active, "5050", null) 
                                        };

            existingBankAccounts = new BankAccountDataModel[] { new BankAccountDataModel(bankAccountKey, "1352", "1352087464", (int)ACBType.Current, "CT Speed", "System", null) };
            clientBankAccountGuid = CombGuid.Instance.Generate();
            bankAccountModel = new BankAccountModel("1352", "BranchName", "1352087464", Core.BusinessModel.Enums.ACBType.Current, "CT Speed", "System");
            command = new LinkBankAccountToClientCommand(bankAccountModel, clientKey, clientBankAccountGuid);
            handler = new LinkBankAccountToClientCommandHandler(bankAccountDataManager, linkedKeyManager, serviceCommandRouter, combGuid, unitOfWorkFactory, eventRaiser);
            bankAccountDataManager.WhenToldTo(x => x.FindExistingBankAccount(bankAccountModel)).Return(existingBankAccounts);
            bankAccountDataManager.WhenToldTo(x => x.FindExistingClientBankAccount(clientKey, existingBankAccounts.First().BankAccountKey)).Return(existingClientBankAccount);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_bank_account_exists = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingBankAccount(bankAccountModel));
        };

        private It should_check_if_the_client_bank_account_exists = () =>
        {
            bankAccountDataManager.WasToldTo(x => x.FindExistingClientBankAccount(clientKey, existingBankAccounts.First().BankAccountKey));
        };

        private It should_not_add_the_bank_account = () =>
        {
            serviceCommandRouter.WasNotToldTo(x => x.HandleCommand(Param.IsAny<AddBankAccountCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_not_reactivate_the_link = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.ReactivateClientBankAccount(Param.IsAny<int>()));
        };

        private It should_not_add_the_bank_account_to_the_client = () =>
        {
            bankAccountDataManager.WasNotToldTo(x => x.AddBankAccountToClient(Param.IsAny<int>(), Param.IsAny<int>()));
        };

        private It should_link_the_existing_key_to_the_guid = () =>
        {
            linkedKeyManager.WasToldTo(x => x.LinkKeyToGuid(existingClientBankAccount.First().LegalEntityBankAccountKey, clientBankAccountGuid));
        };

        private It should_raise_a_bank_account_linked_to_client_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<BankAccountLinkedToClientEvent>
                (y => y.AccountNumber == command.BankAccountModel.AccountNumber &&
                      y.ClientKey == command.ClientKey &&
                      y.ClientBankAccountKey == clientBankAccountKey &&
                      y.BankAccountKey == bankAccountKey &&
                      y.AccountName == command.BankAccountModel.AccountName),
                    existingClientBankAccount.First().LegalEntityBankAccountKey, (int)GenericKeyType.LegalEntityBankAccount, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}