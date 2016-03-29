using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Events;
using System;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.CommandHandlers
{
    public class LinkBankAccountToClientCommandHandler : IDomainServiceCommandHandler<LinkBankAccountToClientCommand, BankAccountLinkedToClientEvent>
    {
        private IBankAccountDataManager bankAccountDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IServiceCommandRouter serviceCommandRouter;
        private ICombGuid comGuid;
        private BankAccountLinkedToClientEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public LinkBankAccountToClientCommandHandler(IBankAccountDataManager bankAccountDataManager, ILinkedKeyManager linkedKeyManager,
            IServiceCommandRouter serviceCommandRouter, ICombGuid combGuid, IUnitOfWorkFactory uowFactory
            , IEventRaiser eventRaiser)
        {
            this.bankAccountDataManager = bankAccountDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.comGuid = combGuid;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(LinkBankAccountToClientCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessageCollection = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                var existingBankAccount = bankAccountDataManager.FindExistingBankAccount(command.BankAccountModel);
                int bankAccountKey = existingBankAccount.Any() ? existingBankAccount.First().BankAccountKey : 0;
                int clientBankAccountKey = 0;

                if (!existingBankAccount.Any())
                {
                    var bankAccountGuid = comGuid.Generate();
                    var addBankMetada = new ServiceRequestMetadata();
                    var addBankCommand = new AddBankAccountCommand(bankAccountGuid, command.BankAccountModel);
                    var response = serviceCommandRouter.HandleCommand<AddBankAccountCommand>(addBankCommand, addBankMetada);
                    systemMessageCollection.AddMessages(response.AllMessages);

                    if (!systemMessageCollection.HasErrors)
                    {
                        bankAccountKey = linkedKeyManager.RetrieveLinkedKey(bankAccountGuid);
                        linkedKeyManager.DeleteLinkedKey(bankAccountGuid);
                    }
                }
                else
                {
                    var existingClientBankAccountLink = bankAccountDataManager.FindExistingClientBankAccount(command.ClientKey, bankAccountKey);
                    if (existingClientBankAccountLink.Any())
                    {
                        if (existingClientBankAccountLink.First().GeneralStatusKey != (int)SAHL.Core.BusinessModel.Enums.GeneralStatus.Active)
                        {
                            bankAccountDataManager.ReactivateClientBankAccount(existingClientBankAccountLink.First().LegalEntityBankAccountKey);
                        }
                        clientBankAccountKey = existingClientBankAccountLink.First().LegalEntityBankAccountKey;
                    }
                }

                if (clientBankAccountKey == 0 && !systemMessageCollection.HasErrors)
                {
                    clientBankAccountKey = bankAccountDataManager.AddBankAccountToClient(command.ClientKey, bankAccountKey);
                }

                if (clientBankAccountKey != 0)
                {
                    @event = new BankAccountLinkedToClientEvent(DateTime.Now, bankAccountKey, command.ClientKey, clientBankAccountKey, command.BankAccountModel.AccountName, 
                        command.BankAccountModel.AccountNumber, command.BankAccountModel.BranchCode, command.BankAccountModel.BranchName);
                    eventRaiser.RaiseEvent(DateTime.Now, @event, clientBankAccountKey, (int)GenericKeyType.LegalEntityBankAccount, metadata);
                    linkedKeyManager.LinkKeyToGuid(clientBankAccountKey, command.ClientBankAccountGuid);
                }

                uow.Complete();
            }
            return systemMessageCollection;
        }
    }
}