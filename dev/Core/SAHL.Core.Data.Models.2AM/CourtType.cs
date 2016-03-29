using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CourtTypeDataModel :  IDataModel
    {
        public CourtTypeDataModel(int courtTypeKey, string description)
        {
            this.CourtTypeKey = courtTypeKey;
            this.Description = description;
		
        }		

        public int CourtTypeKey { get; set; }

        public string Description { get; set; }
    }
}