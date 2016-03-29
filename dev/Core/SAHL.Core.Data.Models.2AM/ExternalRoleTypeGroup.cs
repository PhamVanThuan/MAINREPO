using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExternalRoleTypeGroupDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExternalRoleTypeGroupDataModel(string description)
        {
            this.Description = description;
		
        }
		[JsonConstructor]
        public ExternalRoleTypeGroupDataModel(int externalRoleTypeGroupKey, string description)
        {
            this.ExternalRoleTypeGroupKey = externalRoleTypeGroupKey;
            this.Description = description;
		
        }		

        public int ExternalRoleTypeGroupKey { get; set; }

        public string Description { get; set; }

        public void SetKey(int key)
        {
            this.ExternalRoleTypeGroupKey =  key;
        }
    }
}