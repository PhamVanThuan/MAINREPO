using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DeedsPropertyTypeDataModel :  IDataModel
    {
        public DeedsPropertyTypeDataModel(int deedsPropertyTypeKey, string description)
        {
            this.DeedsPropertyTypeKey = deedsPropertyTypeKey;
            this.Description = description;
		
        }		

        public int DeedsPropertyTypeKey { get; set; }

        public string Description { get; set; }
    }
}