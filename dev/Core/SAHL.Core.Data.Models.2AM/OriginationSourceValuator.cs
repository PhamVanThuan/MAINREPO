using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceValuatorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceValuatorDataModel(int originationSourceKey, int valuatorKey)
        {
            this.OriginationSourceKey = originationSourceKey;
            this.ValuatorKey = valuatorKey;
		
        }
		[JsonConstructor]
        public OriginationSourceValuatorDataModel(int originationSourceValuatorKey, int originationSourceKey, int valuatorKey)
        {
            this.OriginationSourceValuatorKey = originationSourceValuatorKey;
            this.OriginationSourceKey = originationSourceKey;
            this.ValuatorKey = valuatorKey;
		
        }		

        public int OriginationSourceValuatorKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public int ValuatorKey { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceValuatorKey =  key;
        }
    }
}