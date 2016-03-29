using System;

namespace SAHL.Shared.BusinessModel.Models
{
    public class TransactionRuleModel
    {
        public DateTime EffectiveDate { get; set; }

        public int TransactionTypeKey { get; set; }

        public int TransactionKey { get; set; }
    }
}
