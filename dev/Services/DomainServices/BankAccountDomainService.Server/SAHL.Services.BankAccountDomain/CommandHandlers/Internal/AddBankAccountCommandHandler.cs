using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.BankAccountDomain.Managers;
using SAHL.Services.BankAccountDomain.Rules;
using SAHL.Services.BankAccountDomain.Utils;
using SAHL.Services.Interfaces.BankAccountDomain.Commands;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System.Linq;

namespace SAHL.Services.BankAccountDomain.CommandHandlers
{
    public class AddBankAccountCommandHandler : IServiceCommandHandler<AddBankAccountCommand>
    {
        private IBankAccountDataManager bankAccountDataManager;
        private ILinkedKeyManager linkedKeyManager;
        private IDomainRuleManager<BankAccountModel> domainRuleManager;
        private IUnitOfWorkFactory uowFactory;

        public AddBankAccountCommandHandler(IBankAccountDataManager bankAccountDataManager, ILinkedKeyManager linkedKeyManager, IDomainRuleManager<BankAccountModel> domainRuleManager, 
            ICdvValidationManager validationManager, IUnitOfWorkFactory uowFactory)
        {
            this.bankAccountDataManager = bankAccountDataManager;
            this.linkedKeyManager = linkedKeyManager;
            this.domainRuleManager = domainRuleManager;
            this.uowFactory = uowFactory;
            this.domainRuleManager.RegisterRule(new BankAccountMustAdhereToCDVValidationRule(validationManager));
        }

        public ISystemMessageCollection HandleCommand(AddBankAccountCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessageCollection = SystemMessageCollection.Empty();

            using (var uow = uowFactory.Build())
            {
                domainRuleManager.ExecuteRules(systemMessageCollection, command.BankAccountModel);
                if (!systemMessageCollection.HasErrors)
                {
                    var bankAccounts = bankAccountDataManager.FindExistingBankAccount(command.BankAccountModel);

                    if (!bankAccounts.Any())
                    {
                        int bankAccountKey = bankAccountDataManager.SaveBankAccount(command.BankAccountModel);
                        linkedKeyManager.LinkKeyToGuid(bankAccountKey, command.BankAccountGuid);
                    }
                    else
                    {
                        systemMessageCollection.AddMessage(new SystemMessage("Bank Account already exists.", SystemMessageSeverityEnum.Error));
                    }
                }
                uow.Complete();
            }

            return systemMessageCollection;
        }


    }
}
