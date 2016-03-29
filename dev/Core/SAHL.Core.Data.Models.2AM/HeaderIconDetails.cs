using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HeaderIconDetailsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HeaderIconDetailsDataModel(int genericKeyTypeKey, int genericKey, int headerIconTypeKey, string description)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.HeaderIconTypeKey = headerIconTypeKey;
            this.Description = description;
		
        }
		[JsonConstructor]
        public HeaderIconDetailsDataModel(int headerIconDetailsKey, int genericKeyTypeKey, int genericKey, int headerIconTypeKey, string description)
        {
            this.HeaderIconDetailsKey = headerIconDetailsKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.HeaderIconTypeKey = headerIconTypeKey;
            this.Description = description;
		
        }		

        public int HeaderIconDetailsKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public int HeaderIconTypeKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.HeaderIconDetailsKey =  key;
        }
    }
}