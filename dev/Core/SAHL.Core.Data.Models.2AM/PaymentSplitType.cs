using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PaymentSplitTypeDataModel :  IDataModel
    {
        public PaymentSplitTypeDataModel(int paymentSplitTypeKey, string description)
        {
            this.PaymentSplitTypeKey = paymentSplitTypeKey;
            this.Description = description;
		
        }		

        public int PaymentSplitTypeKey { get; set; }

        public string Description { get; set; }
    }
}