using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TranslatableItemDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TranslatableItemDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public TranslatableItemDataModel(int translatableItemKey, string description)
        {
            this.TranslatableItemKey = translatableItemKey;
            this.Description = description;
		
        }		

        public int TranslatableItemKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.TranslatableItemKey =  key;
        }
    }
}