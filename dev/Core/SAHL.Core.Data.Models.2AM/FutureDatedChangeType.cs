using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FutureDatedChangeTypeDataModel :  IDataModel
    {
        public FutureDatedChangeTypeDataModel(int futureDatedChangeTypeKey, string description)
        {
            this.FutureDatedChangeTypeKey = futureDatedChangeTypeKey;
            this.Description = description;
		
        }		

        public int FutureDatedChangeTypeKey { get; set; }

        public string Description { get; set; }
    }
}