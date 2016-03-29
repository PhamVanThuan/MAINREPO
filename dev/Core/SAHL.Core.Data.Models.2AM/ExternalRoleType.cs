using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExternalRoleTypeDataModel :  IDataModel
    {
        public ExternalRoleTypeDataModel(int externalRoleTypeKey, string description, int? externalRoleTypeGroupKey)
        {
            this.ExternalRoleTypeKey = externalRoleTypeKey;
            this.Description = description;
            this.ExternalRoleTypeGroupKey = externalRoleTypeGroupKey;
		
        }		

        public int ExternalRoleTypeKey { get; set; }

        public string Description { get; set; }

        public int? ExternalRoleTypeGroupKey { get; set; }
    }
}