using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PaymentTypeDataModel :  IDataModel
    {
        public PaymentTypeDataModel(int paymentTypeKey, string description)
        {
            this.PaymentTypeKey = paymentTypeKey;
            this.Description = description;
		
        }		

        public int PaymentTypeKey { get; set; }

        public string Description { get; set; }
    }
}