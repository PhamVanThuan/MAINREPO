using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AreaClassificationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AreaClassificationDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public AreaClassificationDataModel(int areaClassificationKey, string description)
        {
            this.AreaClassificationKey = areaClassificationKey;
            this.Description = description;
		
        }		

        public int AreaClassificationKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.AreaClassificationKey =  key;
        }
    }
}