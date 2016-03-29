using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DisabilityTypeDataModel :  IDataModel
    {
        public DisabilityTypeDataModel(int disabilityTypeKey, string description)
        {
            this.DisabilityTypeKey = disabilityTypeKey;
            this.Description = description;
		
        }		

        public int DisabilityTypeKey { get; set; }

        public string Description { get; set; }
    }
}