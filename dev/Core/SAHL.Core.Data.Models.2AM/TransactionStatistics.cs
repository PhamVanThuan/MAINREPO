using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TransactionStatisticsDataModel :  IDataModel
    {
        public TransactionStatisticsDataModel(string yearMonth, int? transactionTypeKey, int? sPVKey, decimal? transactionAmount, string oldSPV, string newSPV)
        {
            this.YearMonth = yearMonth;
            this.TransactionTypeKey = transactionTypeKey;
            this.SPVKey = sPVKey;
            this.TransactionAmount = transactionAmount;
            this.OldSPV = oldSPV;
            this.NewSPV = newSPV;
		
        }		

        public string YearMonth { get; set; }

        public int? TransactionTypeKey { get; set; }

        public int? SPVKey { get; set; }

        public decimal? TransactionAmount { get; set; }

        public string OldSPV { get; set; }

        public string NewSPV { get; set; }
    }
}