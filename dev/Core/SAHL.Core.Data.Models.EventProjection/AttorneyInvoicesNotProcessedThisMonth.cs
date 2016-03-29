using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class AttorneyInvoicesNotProcessedThisMonthDataModel :  IDataModel
    {
        public AttorneyInvoicesNotProcessedThisMonthDataModel(int count, decimal value)
        {
            this.Count = count;
            this.Value = value;
		
        }		

        public int Count { get; set; }

        public decimal Value { get; set; }
    }
}