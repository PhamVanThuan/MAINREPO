using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AdCheckValuationIDStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AdCheckValuationIDStatusDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public AdCheckValuationIDStatusDataModel(int adCheckValuationIDStatusKey, string description)
        {
            this.AdCheckValuationIDStatusKey = adCheckValuationIDStatusKey;
            this.Description = description;
		
        }		

        public int AdCheckValuationIDStatusKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.AdCheckValuationIDStatusKey =  key;
        }
    }
}