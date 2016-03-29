using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LinkRateMapDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LinkRateMapDataModel(decimal existingLinkRate, decimal newLinkRate, int? existingCategory)
        {
            this.ExistingLinkRate = existingLinkRate;
            this.NewLinkRate = newLinkRate;
            this.ExistingCategory = existingCategory;
		
        }
		[JsonConstructor]
        public LinkRateMapDataModel(byte linkRateMapKey, decimal existingLinkRate, decimal newLinkRate, int? existingCategory)
        {
            this.LinkRateMapKey = linkRateMapKey;
            this.ExistingLinkRate = existingLinkRate;
            this.NewLinkRate = newLinkRate;
            this.ExistingCategory = existingCategory;
		
        }		

        public byte LinkRateMapKey { get; set; }

        public decimal ExistingLinkRate { get; set; }

        public decimal NewLinkRate { get; set; }

        public int? ExistingCategory { get; set; }

        public void SetKey(byte key)
        {
            this.LinkRateMapKey =  key;
        }
    }
}