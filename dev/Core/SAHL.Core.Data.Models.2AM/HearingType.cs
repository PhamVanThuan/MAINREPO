using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HearingTypeDataModel :  IDataModel
    {
        public HearingTypeDataModel(int hearingTypeKey, string description)
        {
            this.HearingTypeKey = hearingTypeKey;
            this.Description = description;
		
        }		

        public int HearingTypeKey { get; set; }

        public string Description { get; set; }
    }
}