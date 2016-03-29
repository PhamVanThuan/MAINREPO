using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationDataProviderDataServiceDataModel :  IDataModel
    {
        public ValuationDataProviderDataServiceDataModel(int valuationDataProviderDataServiceKey, int dataProviderDataServiceKey)
        {
            this.ValuationDataProviderDataServiceKey = valuationDataProviderDataServiceKey;
            this.DataProviderDataServiceKey = dataProviderDataServiceKey;
		
        }		

        public int ValuationDataProviderDataServiceKey { get; set; }

        public int DataProviderDataServiceKey { get; set; }
    }
}