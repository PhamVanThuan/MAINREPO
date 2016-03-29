using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OccupancyTypeDataModel :  IDataModel
    {
        public OccupancyTypeDataModel(int occupancyTypeKey, string description)
        {
            this.OccupancyTypeKey = occupancyTypeKey;
            this.Description = description;
		
        }		

        public int OccupancyTypeKey { get; set; }

        public string Description { get; set; }
    }
}