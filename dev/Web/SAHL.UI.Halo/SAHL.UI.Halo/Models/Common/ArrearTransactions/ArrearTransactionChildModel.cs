using System;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Common.ArrearTransactions
{
    public class ArrearTransactionChildModel : IHaloTileModel
    {
        public DateTime InsertDate { get; set; }
        public string TransactionTypeDescription { get; set; }
        public decimal Amount { get; set; }
    }
}