using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SubsidyProviderTypeDataModel :  IDataModel
    {
        public SubsidyProviderTypeDataModel(int subsidyProviderTypeKey, string description)
        {
            this.SubsidyProviderTypeKey = subsidyProviderTypeKey;
            this.Description = description;
		
        }		

        public int SubsidyProviderTypeKey { get; set; }

        public string Description { get; set; }
    }
}