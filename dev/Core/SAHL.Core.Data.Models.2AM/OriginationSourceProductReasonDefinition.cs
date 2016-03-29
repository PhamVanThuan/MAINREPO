using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceProductReasonDefinitionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceProductReasonDefinitionDataModel(int originationSourceProductKey, int reasonDefinitionKey)
        {
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.ReasonDefinitionKey = reasonDefinitionKey;
		
        }
		[JsonConstructor]
        public OriginationSourceProductReasonDefinitionDataModel(int originationSourceProductReasonDefinitionKey, int originationSourceProductKey, int reasonDefinitionKey)
        {
            this.OriginationSourceProductReasonDefinitionKey = originationSourceProductReasonDefinitionKey;
            this.OriginationSourceProductKey = originationSourceProductKey;
            this.ReasonDefinitionKey = reasonDefinitionKey;
		
        }		

        public int OriginationSourceProductReasonDefinitionKey { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public int ReasonDefinitionKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceProductReasonDefinitionKey =  key;
        }
    }
}