namespace Automation.DataModels
{
    public class DisbursementFinancialTransaction
    {
        public DisbursementFinancialTransaction()
        {
            DisbursementFinancialTransactionKey = 0;
            DisbursementKey = 0;
            FinancialTransactionKey = 0;
            LoanTransaction = new LoanTransaction();
        }

        public int? DisbursementFinancialTransactionKey { get; set; }

        public int? DisbursementKey { get; set; }

        public int? FinancialTransactionKey { get; set; }

        public LoanTransaction LoanTransaction { get; set; }
    }
}