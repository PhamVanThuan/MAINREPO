using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCHistoryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HOCHistoryDataModel(int financialServiceKey, int hOCInsurerKey, DateTime? commencementDate, DateTime? cancellationDate, DateTime? changeDate, string userID)
        {
            this.FinancialServiceKey = financialServiceKey;
            this.HOCInsurerKey = hOCInsurerKey;
            this.CommencementDate = commencementDate;
            this.CancellationDate = cancellationDate;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }
		[JsonConstructor]
        public HOCHistoryDataModel(int hOCHistoryKey, int financialServiceKey, int hOCInsurerKey, DateTime? commencementDate, DateTime? cancellationDate, DateTime? changeDate, string userID)
        {
            this.HOCHistoryKey = hOCHistoryKey;
            this.FinancialServiceKey = financialServiceKey;
            this.HOCInsurerKey = hOCInsurerKey;
            this.CommencementDate = commencementDate;
            this.CancellationDate = cancellationDate;
            this.ChangeDate = changeDate;
            this.UserID = userID;
		
        }		

        public int HOCHistoryKey { get; set; }

        public int FinancialServiceKey { get; set; }

        public int HOCInsurerKey { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CancellationDate { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string UserID { get; set; }

        public void SetKey(int key)
        {
            this.HOCHistoryKey =  key;
        }
    }
}