using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceDataModel :  IDataModel
    {
        public OriginationSourceDataModel(int originationSourceKey, string description)
        {
            this.OriginationSourceKey = originationSourceKey;
            this.Description = description;
		
        }		

        public int OriginationSourceKey { get; set; }

        public string Description { get; set; }
    }
}