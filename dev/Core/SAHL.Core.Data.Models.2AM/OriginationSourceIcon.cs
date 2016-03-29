using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OriginationSourceIconDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OriginationSourceIconDataModel(int originationSourceKey, string icon)
        {
            this.OriginationSourceKey = originationSourceKey;
            this.Icon = icon;
		
        }
		[JsonConstructor]
        public OriginationSourceIconDataModel(int originationSourceIconKey, int originationSourceKey, string icon)
        {
            this.OriginationSourceIconKey = originationSourceIconKey;
            this.OriginationSourceKey = originationSourceKey;
            this.Icon = icon;
		
        }		

        public int OriginationSourceIconKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public string Icon { get; set; }

        public void SetKey(int key)
        {
            this.OriginationSourceIconKey =  key;
        }
    }
}