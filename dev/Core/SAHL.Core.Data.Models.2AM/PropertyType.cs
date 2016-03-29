using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PropertyTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PropertyTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public PropertyTypeDataModel(int propertyTypeKey, string description)
        {
            this.PropertyTypeKey = propertyTypeKey;
            this.Description = description;
		
        }		

        public int PropertyTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.PropertyTypeKey =  key;
        }
    }
}