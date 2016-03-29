using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.BankAccountDomain.Models
{
    public class BankAccountModel : ValidatableModel
    {
        public BankAccountModel(string branchCode, string branchName, string accountNumber, ACBType accountType, string accountName, string userID)
        {
            this.BranchCode = branchCode;
            this.BranchName = branchName;
            this.AccountNumber = accountNumber;
            this.AccountType = accountType;
            this.AccountName = accountName;
            this.UserID = userID;
            Validate();
        }

        [Required]
        public string BranchCode { get; protected set; }

        [Required]
        public string BranchName { get; protected set; }

        [Required]
        public string AccountNumber { get; protected set; }

        [Required]
        public ACBType AccountType { get; protected set; }

        [Required]
        public string AccountName { get; protected set; }

        public string UserID { get; protected set; }

    }
}
