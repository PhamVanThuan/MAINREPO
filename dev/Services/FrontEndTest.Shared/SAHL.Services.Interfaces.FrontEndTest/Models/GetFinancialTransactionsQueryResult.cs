using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetFinancialTransactionsQueryResult
    {
        public GetFinancialTransactionsQueryResult(int financialTransactionKey,
            int financialServiceKey,
            int transactionTypeKey,
            DateTime insertDate,
            DateTime effectiveDate,
            DateTime correctionDate,
            decimal interestRate,
            decimal activeMarketRate,
            decimal amount,
            decimal balance,
            string reference,
            string userID,
            int spvKey,
            bool isRolledBack,
            int loanTransactionNumber
            )
        {
            this.FinancialTransactionKey = financialTransactionKey;
            this.FinancialServiceKey = financialServiceKey;
            this.TransactionTypeKey = transactionTypeKey;
            this.InsertDate = insertDate;
            this.EffectiveDate = effectiveDate;
            this.CorrectionDate = correctionDate;
            this.InterestRate = interestRate;
            this.ActiveMarketRate = activeMarketRate;
            this.Amount = amount;
            this.Balance = balance;
            this.Reference = reference;
            this.UserID = userID;
            this.SPVKey = spvKey;
            this.IsRolledBack = isRolledBack;
            this.LoanTransactionNumber = loanTransactionNumber;
        }

        public int FinancialTransactionKey { get; protected set; }

        public int FinancialServiceKey { get; protected set; }

        public int TransactionTypeKey { get; protected set; }

        public DateTime InsertDate { get; protected set; }

        public DateTime EffectiveDate { get; protected set; }

        public DateTime CorrectionDate { get; protected set; }

        public decimal InterestRate { get; protected set; }

        public decimal ActiveMarketRate { get; protected set; }

        public decimal Amount { get; protected set; }

        public decimal Balance { get; protected set; }

        public string Reference { get; protected set; }

        public string UserID { get; protected set; }

        public int SPVKey { get; protected set; }

        public bool IsRolledBack { get; protected set; }

        public int LoanTransactionNumber { get; protected set; }
    }
}