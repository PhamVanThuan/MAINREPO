using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DataProviderDataServiceDataModel :  IDataModel
    {
        public DataProviderDataServiceDataModel(int dataProviderDataServiceKey, int dataProviderKey, int dataServiceKey)
        {
            this.DataProviderDataServiceKey = dataProviderDataServiceKey;
            this.DataProviderKey = dataProviderKey;
            this.DataServiceKey = dataServiceKey;
		
        }		

        public int DataProviderDataServiceKey { get; set; }

        public int DataProviderKey { get; set; }

        public int DataServiceKey { get; set; }
    }
}