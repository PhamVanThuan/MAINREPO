using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MonthlyFeeToBeRaisedDataModel :  IDataModel
    {
        public MonthlyFeeToBeRaisedDataModel(int? accountKey, int? financialServiceKey, DateTime? grantedDate, DateTime? dateDisbursed)
        {
            this.AccountKey = accountKey;
            this.FinancialServiceKey = financialServiceKey;
            this.GrantedDate = grantedDate;
            this.DateDisbursed = dateDisbursed;
		
        }		

        public int? AccountKey { get; set; }

        public int? FinancialServiceKey { get; set; }

        public DateTime? GrantedDate { get; set; }

        public DateTime? DateDisbursed { get; set; }
    }
}