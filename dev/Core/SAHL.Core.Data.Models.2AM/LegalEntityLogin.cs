using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityLoginDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityLoginDataModel(string username, string password, DateTime? lastLoginDate, int generalStatusKey, int legalEntityKey)
        {
            this.Username = username;
            this.Password = password;
            this.LastLoginDate = lastLoginDate;
            this.GeneralStatusKey = generalStatusKey;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public LegalEntityLoginDataModel(int legalEntityLoginKey, string username, string password, DateTime? lastLoginDate, int generalStatusKey, int legalEntityKey)
        {
            this.LegalEntityLoginKey = legalEntityLoginKey;
            this.Username = username;
            this.Password = password;
            this.LastLoginDate = lastLoginDate;
            this.GeneralStatusKey = generalStatusKey;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int LegalEntityLoginKey { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityLoginKey =  key;
        }
    }
}