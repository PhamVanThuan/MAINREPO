using Common.Enums;
using System;

namespace Automation.DataModels
{
    public sealed class LoanDetail
    {
        public int AccountKey { get; set; }

        public DetailTypeEnum DetailTypeKey { get; set; }

        public double Amount { get; set; }

        public DateTime DetailDate { get; set; }

        public string Description { get; set; }

        public DetailType LoanDetailType { get; set; }
    }
}