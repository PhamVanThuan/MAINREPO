using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class QuickCashPaymentTypeDataModel :  IDataModel
    {
        public QuickCashPaymentTypeDataModel(int quickCashPaymentTypeKey, string description)
        {
            this.QuickCashPaymentTypeKey = quickCashPaymentTypeKey;
            this.Description = description;
		
        }		

        public int QuickCashPaymentTypeKey { get; set; }

        public string Description { get; set; }
    }
}