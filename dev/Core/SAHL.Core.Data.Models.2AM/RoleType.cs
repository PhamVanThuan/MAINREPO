using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RoleTypeDataModel :  IDataModel
    {
        public RoleTypeDataModel(int roleTypeKey, string description)
        {
            this.RoleTypeKey = roleTypeKey;
            this.Description = description;
		
        }		

        public int RoleTypeKey { get; set; }

        public string Description { get; set; }
    }
}