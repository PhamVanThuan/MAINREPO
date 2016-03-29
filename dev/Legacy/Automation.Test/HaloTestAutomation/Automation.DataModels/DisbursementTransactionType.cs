using Common.Enums;

namespace Automation.DataModels
{
    public class DisbursementTransactionType
    {
        public DisbursementTransactionTypeEnum DisbursementTransactionTypeKey { get; set; }

        public string Description { get; set; }

        public int TransactionTypeNumber { get; set; }
    }
}