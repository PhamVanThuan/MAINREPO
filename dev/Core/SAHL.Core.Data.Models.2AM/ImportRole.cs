using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportRoleDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportRoleDataModel(int legalEntityKey, string roleTypeKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.RoleTypeKey = roleTypeKey;
		
        }
		[JsonConstructor]
        public ImportRoleDataModel(int roleKey, int legalEntityKey, string roleTypeKey)
        {
            this.RoleKey = roleKey;
            this.LegalEntityKey = legalEntityKey;
            this.RoleTypeKey = roleTypeKey;
		
        }		

        public int RoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string RoleTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.RoleKey =  key;
        }
    }
}