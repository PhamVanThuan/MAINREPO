using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisabilityPaymentStatusDataModel :  IDataModel
    {
        public DisabilityPaymentStatusDataModel(int disabilityPaymentStatusKey, string description)
        {
            this.DisabilityPaymentStatusKey = disabilityPaymentStatusKey;
            this.Description = description;
		
        }		

        public int DisabilityPaymentStatusKey { get; set; }

        public string Description { get; set; }
    }
}