using System;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class AttorneyInvoicesPaidLastMonthDataModel : IDataModel
    {
        public AttorneyInvoicesPaidLastMonthDataModel(int count, decimal value)
        {
            this.Count = count;
            this.Value = value;

        }

        public int Count { get; set; }

        public decimal Value { get; set; }
    }
}
