using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationClassificationDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValuationClassificationDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ValuationClassificationDataModel(int valuationClassificationKey, string description)
        {
            this.ValuationClassificationKey = valuationClassificationKey;
            this.Description = description;
		
        }		

        public int ValuationClassificationKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ValuationClassificationKey =  key;
        }
    }
}