using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class RoleDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public RoleDataModel(int legalEntityKey, int accountKey, int roleTypeKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.RoleTypeKey = roleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }
		[JsonConstructor]
        public RoleDataModel(int accountRoleKey, int legalEntityKey, int accountKey, int roleTypeKey, int generalStatusKey, DateTime statusChangeDate)
        {
            this.AccountRoleKey = accountRoleKey;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.RoleTypeKey = roleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.StatusChangeDate = statusChangeDate;
		
        }		

        public int AccountRoleKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AccountKey { get; set; }

        public int RoleTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.AccountRoleKey =  key;
        }
    }
}