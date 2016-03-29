using System;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Common.LoanTransaction
{
    public class LoanTransactionTileModel : IHaloTileModel
    {
        public DateTime InsertDate { get; set; }
        public string TransactionTypeDescription { get; set; }
        public decimal Amount { get; set; }
    }
}
