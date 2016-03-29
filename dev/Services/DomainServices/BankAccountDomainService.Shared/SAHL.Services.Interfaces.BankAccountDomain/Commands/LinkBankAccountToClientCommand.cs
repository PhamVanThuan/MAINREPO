using SAHL.Core.Services;
using SAHL.DomainServiceChecks.Checks;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.BankAccountDomain.Commands
{
    public class LinkBankAccountToClientCommand : ServiceCommand, IRequiresClient, IBankAccountDomainCommand
    {
        [Required]
        public BankAccountModel BankAccountModel { get; protected set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid ClientKey.")]
        public int ClientKey { get; protected set; }

        [Required]
        public Guid ClientBankAccountGuid { get; protected set; }

        public LinkBankAccountToClientCommand(BankAccountModel bankAccountModel, int clientKey, Guid clientBankAccountGuid)
        {
            this.BankAccountModel = bankAccountModel;
            this.ClientKey = clientKey;
            this.ClientBankAccountGuid = clientBankAccountGuid;
        }
    }
}