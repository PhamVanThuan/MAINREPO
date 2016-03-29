using System;

namespace Automation.DataModels
{
    public class LoanTransaction
    {
        public LoanTransaction()
        {
            FinancialTransactionKey = 0;
            FinancialServiceKey = 0;
            TransactionTypeKey = 0;
            InsertDate = new DateTime();
            EffectiveDate = new DateTime();
            InterestRate = 0;
            Amount = 0;
            Balance = 0;
            Reference = string.Empty;
            UserID = string.Empty;
            SPVKey = 0;
            IsRolledBack = false;
        }

        public int FinancialTransactionKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int TransactionTypeKey { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public decimal InterestRate { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public string Reference { get; set; }

        public string UserID { get; set; }

        public int SPVKey { get; set; }

        public bool IsRolledBack { get; set; }
    }
}