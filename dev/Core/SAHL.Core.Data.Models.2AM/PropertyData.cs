using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PropertyDataDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public PropertyDataDataModel(int propertyKey, int propertyDataProviderDataServiceKey, string propertyID, string data, DateTime insertDate)
        {
            this.PropertyKey = propertyKey;
            this.PropertyDataProviderDataServiceKey = propertyDataProviderDataServiceKey;
            this.PropertyID = propertyID;
            this.Data = data;
            this.InsertDate = insertDate;
		
        }
		[JsonConstructor]
        public PropertyDataDataModel(int propertyDataKey, int propertyKey, int propertyDataProviderDataServiceKey, string propertyID, string data, DateTime insertDate)
        {
            this.PropertyDataKey = propertyDataKey;
            this.PropertyKey = propertyKey;
            this.PropertyDataProviderDataServiceKey = propertyDataProviderDataServiceKey;
            this.PropertyID = propertyID;
            this.Data = data;
            this.InsertDate = insertDate;
		
        }		

        public int PropertyDataKey { get; set; }

        public int PropertyKey { get; set; }

        public int PropertyDataProviderDataServiceKey { get; set; }

        public string PropertyID { get; set; }

        public string Data { get; set; }

        public DateTime InsertDate { get; set; }

        public void SetKey(int key)
        {
            this.PropertyDataKey =  key;
        }
    }
}