using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class BankAccount_BackupDataModel :  IDataModel
    {
        public BankAccount_BackupDataModel(int bankAccountKey, string aCBBranchCode, string accountNumber, int aCBTypeNumber, string accountName, string userID, DateTime? changeDate)
        {
            this.BankAccountKey = bankAccountKey;
            this.ACBBranchCode = aCBBranchCode;
            this.AccountNumber = accountNumber;
            this.ACBTypeNumber = aCBTypeNumber;
            this.AccountName = accountName;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public int BankAccountKey { get; set; }

        public string ACBBranchCode { get; set; }

        public string AccountNumber { get; set; }

        public int ACBTypeNumber { get; set; }

        public string AccountName { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }
    }
}