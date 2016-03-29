using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PropertyTitleDeedDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PropertyTitleDeedDataModel(int propertyKey, string titleDeedNumber, int? deedsOfficeKey)
        {
            this.PropertyKey = propertyKey;
            this.TitleDeedNumber = titleDeedNumber;
            this.DeedsOfficeKey = deedsOfficeKey;
		
        }
		[JsonConstructor]
        public PropertyTitleDeedDataModel(int propertyTitleDeedKey, int propertyKey, string titleDeedNumber, int? deedsOfficeKey)
        {
            this.PropertyTitleDeedKey = propertyTitleDeedKey;
            this.PropertyKey = propertyKey;
            this.TitleDeedNumber = titleDeedNumber;
            this.DeedsOfficeKey = deedsOfficeKey;
		
        }		

        public int PropertyTitleDeedKey { get; set; }

        public int PropertyKey { get; set; }

        public string TitleDeedNumber { get; set; }

        public int? DeedsOfficeKey { get; set; }

        public void SetKey(int key)
        {
            this.PropertyTitleDeedKey =  key;
        }
    }
}