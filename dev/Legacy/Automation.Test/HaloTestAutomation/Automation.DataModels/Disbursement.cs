using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class Disbursement : BankAccount
    {
        public int DisbursementKey { get; set; }

        public int AccountKey { get; set; }

        public int ACBBankCode { get; set; }

        public string ACBBranchCode { get; set; }

        public int ACBTypeNumber { get; set; }

        public DateTime PreparedDate { get; set; }

        public DateTime ActionDate { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public double Amount { get; set; }

        public DisbursementStatusEnum DisbursementStatusKey { get; set; }

        public DisbursementStatus DisbursementStatus { get; set; }

        public DisbursementTransactionTypeEnum DisbursementTransactionTypeKey { get; set; }

        public DisbursementTransactionType DisbursementTransactionType { get; set; }

        public double CapitalAmount { get; set; }

        public double GuaranteeAmount { get; set; }

        public double InterestRate { get; set; }

        public DateTime InterestStartDate { get; set; }

        public string InterestApplied { get; set; }

        public double PaymentAmount { get; set; }

        public DisbursementFinancialTransaction DisbursementLoanTransaction { get; set; }
    }
}