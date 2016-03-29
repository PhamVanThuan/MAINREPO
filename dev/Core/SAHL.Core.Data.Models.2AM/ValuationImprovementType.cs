using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuationImprovementTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValuationImprovementTypeDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ValuationImprovementTypeDataModel(int valuationImprovementTypeKey, string description)
        {
            this.ValuationImprovementTypeKey = valuationImprovementTypeKey;
            this.Description = description;
		
        }		

        public int ValuationImprovementTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ValuationImprovementTypeKey =  key;
        }
    }
}