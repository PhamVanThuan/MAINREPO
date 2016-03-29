using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CitizenTypeDataModel :  IDataModel
    {
        public CitizenTypeDataModel(int citizenTypeKey, string description)
        {
            this.CitizenTypeKey = citizenTypeKey;
            this.Description = description;
		
        }		

        public int CitizenTypeKey { get; set; }

        public string Description { get; set; }
    }
}