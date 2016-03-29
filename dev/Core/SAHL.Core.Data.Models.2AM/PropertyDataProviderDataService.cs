using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class PropertyDataProviderDataServiceDataModel :  IDataModel
    {
        public PropertyDataProviderDataServiceDataModel(int propertyDataProviderDataServiceKey, int dataProviderDataServiceKey)
        {
            this.PropertyDataProviderDataServiceKey = propertyDataProviderDataServiceKey;
            this.DataProviderDataServiceKey = dataProviderDataServiceKey;
		
        }		

        public int PropertyDataProviderDataServiceKey { get; set; }

        public int DataProviderDataServiceKey { get; set; }
    }
}