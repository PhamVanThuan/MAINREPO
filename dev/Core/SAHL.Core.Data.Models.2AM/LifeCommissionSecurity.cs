using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeCommissionSecurityDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifeCommissionSecurityDataModel(string userID, bool? administrator)
        {
            this.UserID = userID;
            this.Administrator = administrator;
		
        }
		[JsonConstructor]
        public LifeCommissionSecurityDataModel(int securityKey, string userID, bool? administrator)
        {
            this.SecurityKey = securityKey;
            this.UserID = userID;
            this.Administrator = administrator;
		
        }		

        public int SecurityKey { get; set; }

        public string UserID { get; set; }

        public bool? Administrator { get; set; }

        public void SetKey(int key)
        {
            this.SecurityKey =  key;
        }
    }
}