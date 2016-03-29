using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HeaderIconDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HeaderIconDataModel(int coreBusinessObjectKey, int headerIconTypeKey)
        {
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.HeaderIconTypeKey = headerIconTypeKey;
		
        }
		[JsonConstructor]
        public HeaderIconDataModel(int headerIconKey, int coreBusinessObjectKey, int headerIconTypeKey)
        {
            this.HeaderIconKey = headerIconKey;
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.HeaderIconTypeKey = headerIconTypeKey;
		
        }		

        public int HeaderIconKey { get; set; }

        public int CoreBusinessObjectKey { get; set; }

        public int HeaderIconTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.HeaderIconKey =  key;
        }
    }
}