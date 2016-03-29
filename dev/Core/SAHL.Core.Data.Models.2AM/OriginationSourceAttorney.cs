using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceAttorneyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceAttorneyDataModel(int originationSourceKey, int attorneyKey)
        {
            this.OriginationSourceKey = originationSourceKey;
            this.AttorneyKey = attorneyKey;
		
        }
		[JsonConstructor]
        public OriginationSourceAttorneyDataModel(int originationSourceAttorneyKey, int originationSourceKey, int attorneyKey)
        {
            this.OriginationSourceAttorneyKey = originationSourceAttorneyKey;
            this.OriginationSourceKey = originationSourceKey;
            this.AttorneyKey = attorneyKey;
		
        }		

        public int OriginationSourceAttorneyKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public int AttorneyKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceAttorneyKey =  key;
        }
    }
}