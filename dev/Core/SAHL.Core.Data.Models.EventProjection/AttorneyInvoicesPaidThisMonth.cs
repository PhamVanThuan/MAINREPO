using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class AttorneyInvoicesPaidThisMonthDataModel :  IDataModel
    {
        public AttorneyInvoicesPaidThisMonthDataModel(int count, decimal value)
        {
            this.Count = count;
            this.Value = value;
		
        }		

        public int Count { get; set; }

        public decimal Value { get; set; }
    }
}