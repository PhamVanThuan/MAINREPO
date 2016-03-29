namespace Automation.DataModels
{
    public sealed class DOTransaction : Record, IDataModel
    {
        public int DOTransactionKey { get; set; }

        public int AccountKey { get; set; }

        public string AccountNumber { get; set; }

        public string ACBBranchCode { get; set; }

        public int ACBTypeNumber { get; set; }

        public string AccountName { get; set; }

        public decimal Amount { get; set; }

        public int SPVKey { get; set; }

        public int SPVBankAccountKey { get; set; }

        public int TransactionStatusKey { get; set; }

        public int ErrorKey { get; set; }

        public int BatchKey { get; set; }

        public int BatchTotalKey { get; set; }
    }
}