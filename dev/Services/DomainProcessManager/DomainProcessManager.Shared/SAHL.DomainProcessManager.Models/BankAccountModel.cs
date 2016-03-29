using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class BankAccountModel : IDataModel
    {
        public BankAccountModel(string branchCode, string branchName, string accountNumber, ACBType accountType, string accountName, string userID, bool isDebitOrderBankAccount)
        {
            this.BranchCode              = branchCode;
            this.BranchName              = branchName;
            this.AccountNumber           = accountNumber;
            this.AccountType             = accountType;
            this.AccountName             = accountName;
            this.UserID                  = userID;
            this.IsDebitOrderBankAccount = isDebitOrderBankAccount;
        }

        [DataMember]
        public string BranchCode { get; set; }

        [DataMember]
        public string BranchName { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public ACBType AccountType { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public bool IsDebitOrderBankAccount { get; set; }
    }
}
