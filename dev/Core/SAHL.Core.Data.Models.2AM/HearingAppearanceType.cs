using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HearingAppearanceTypeDataModel :  IDataModel
    {
        public HearingAppearanceTypeDataModel(int hearingAppearanceTypeKey, int hearingTypeKey, string description)
        {
            this.HearingAppearanceTypeKey = hearingAppearanceTypeKey;
            this.HearingTypeKey = hearingTypeKey;
            this.Description = description;
		
        }		

        public int HearingAppearanceTypeKey { get; set; }

        public int HearingTypeKey { get; set; }

        public string Description { get; set; }
    }
}