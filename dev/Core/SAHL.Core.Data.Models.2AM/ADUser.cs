using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ADUserDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ADUserDataModel(string aDUserName, int generalStatusKey, string password, string passwordQuestion, string passwordAnswer, int? legalEntityKey)
        {
            this.ADUserName = aDUserName;
            this.GeneralStatusKey = generalStatusKey;
            this.Password = password;
            this.PasswordQuestion = passwordQuestion;
            this.PasswordAnswer = passwordAnswer;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public ADUserDataModel(int aDUserKey, string aDUserName, int generalStatusKey, string password, string passwordQuestion, string passwordAnswer, int? legalEntityKey)
        {
            this.ADUserKey = aDUserKey;
            this.ADUserName = aDUserName;
            this.GeneralStatusKey = generalStatusKey;
            this.Password = password;
            this.PasswordQuestion = passwordQuestion;
            this.PasswordAnswer = passwordAnswer;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int ADUserKey { get; set; }

        public string ADUserName { get; set; }

        public int GeneralStatusKey { get; set; }

        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public int? LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.ADUserKey =  key;
        }
    }
}