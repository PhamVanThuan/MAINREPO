using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditFinancialTranPostedDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditFinancialTranPostedDataModel(int loanNumber, short transactionTypeNumber, DateTime transactionEffectiveDate, DateTime transactionInsertDate, double transactionAmount, string transactionReference, string userID)
        {
            this.LoanNumber = loanNumber;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.TransactionEffectiveDate = transactionEffectiveDate;
            this.TransactionInsertDate = transactionInsertDate;
            this.TransactionAmount = transactionAmount;
            this.TransactionReference = transactionReference;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public AuditFinancialTranPostedDataModel(decimal auditNumber, int loanNumber, short transactionTypeNumber, DateTime transactionEffectiveDate, DateTime transactionInsertDate, double transactionAmount, string transactionReference, string userID)
        {
            this.AuditNumber = auditNumber;
            this.LoanNumber = loanNumber;
            this.TransactionTypeNumber = transactionTypeNumber;
            this.TransactionEffectiveDate = transactionEffectiveDate;
            this.TransactionInsertDate = transactionInsertDate;
            this.TransactionAmount = transactionAmount;
            this.TransactionReference = transactionReference;
            this.UserID = userID;
		
        }		

        public decimal AuditNumber { get; set; }

        public int LoanNumber { get; set; }

        public short TransactionTypeNumber { get; set; }

        public DateTime TransactionEffectiveDate { get; set; }

        public DateTime TransactionInsertDate { get; set; }

        public double TransactionAmount { get; set; }

        public string TransactionReference { get; set; }

        public string UserID { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}