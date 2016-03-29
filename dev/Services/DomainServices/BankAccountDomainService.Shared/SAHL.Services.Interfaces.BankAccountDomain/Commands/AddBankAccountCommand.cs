using SAHL.Core.Services;
using SAHL.Services.Interfaces.BankAccountDomain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.BankAccountDomain.Commands
{
    public class AddBankAccountCommand : ServiceCommand, IBankAccountDomainCommand
    {
        [Required]
        public BankAccountModel BankAccountModel { get; protected set; }

        [Required]
        public Guid BankAccountGuid { get; protected set; }

        public AddBankAccountCommand(Guid bankAccountGuid, BankAccountModel bankAccountModel)
        {
            this.BankAccountGuid = bankAccountGuid;
            this.BankAccountModel = bankAccountModel;
        }
    }
}
