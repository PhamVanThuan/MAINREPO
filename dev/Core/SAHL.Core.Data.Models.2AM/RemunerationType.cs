using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RemunerationTypeDataModel :  IDataModel
    {
        public RemunerationTypeDataModel(int remunerationTypeKey, string description)
        {
            this.RemunerationTypeKey = remunerationTypeKey;
            this.Description = description;
		
        }		

        public int RemunerationTypeKey { get; set; }

        public string Description { get; set; }
    }
}