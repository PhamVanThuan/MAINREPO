using Common.Enums;
using System;

namespace Automation.DataModels
{
    public sealed class TransactionType
    {
        public int TransactionTypeDataAccessKey { get; set; }

        public string ADCredentials { get; set; }

        public TransactionTypeEnum TransactionTypeKey { get; set; }

        public string Description { get; set; }

        public string MBSDescription { get; set; }

        public Int16 CurrentDrCr { get; set; }

        public Int16 ArrearDrCr { get; set; }

        public Int16 MBSDrCr { get; set; }

        public bool Memo { get; set; }

        public int ScreenBatch { get; set; }

        public string GLAccount { get; set; }

        public string GLContra { get; set; }

        public string SecuritisationDescription { get; set; }

        public string HTMLColour { get; set; }

        public string ForeColour { get; set; }

        public TransactionTypeEnum ReversalTransactionTypeKey { get; set; }
    }
}