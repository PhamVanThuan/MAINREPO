using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CAPPaymentOptionDataModel :  IDataModel
    {
        public CAPPaymentOptionDataModel(int cAPPaymentOptionKey, string description)
        {
            this.CAPPaymentOptionKey = cAPPaymentOptionKey;
            this.Description = description;
		
        }		

        public int CAPPaymentOptionKey { get; set; }

        public string Description { get; set; }
    }
}