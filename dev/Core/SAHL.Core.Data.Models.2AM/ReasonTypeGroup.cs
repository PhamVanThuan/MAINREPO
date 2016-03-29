using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReasonTypeGroupDataModel :  IDataModel
    {
        public ReasonTypeGroupDataModel(int reasonTypeGroupKey, string description, int? parentKey)
        {
            this.ReasonTypeGroupKey = reasonTypeGroupKey;
            this.Description = description;
            this.ParentKey = parentKey;
		
        }		

        public int ReasonTypeGroupKey { get; set; }

        public string Description { get; set; }

        public int? ParentKey { get; set; }
    }
}