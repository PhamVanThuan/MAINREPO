using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class ManualDebitOrder
    {
        public int manualDebitOrderKey { get; set; }

        public decimal Amount { get; set; }

        public string Reference { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime ActionDate { get; set; }

        public string Notes { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }

        public int GeneralStatusKey { get; set; }

        public int BankAccountKey { get; set; }

        public Automation.DataModels.BankAccount BankAccount { get; set; }

        public string UserID { get; set; }
    }
}