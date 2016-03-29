using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ValuatorDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ValuatorDataModel(string valuatorContact, string valuatorPassword, byte? limitedUserGroup, int? generalStatusKey, int legalEntityKey)
        {
            this.ValuatorContact = valuatorContact;
            this.ValuatorPassword = valuatorPassword;
            this.LimitedUserGroup = limitedUserGroup;
            this.GeneralStatusKey = generalStatusKey;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public ValuatorDataModel(int valuatorKey, string valuatorContact, string valuatorPassword, byte? limitedUserGroup, int? generalStatusKey, int legalEntityKey)
        {
            this.ValuatorKey = valuatorKey;
            this.ValuatorContact = valuatorContact;
            this.ValuatorPassword = valuatorPassword;
            this.LimitedUserGroup = limitedUserGroup;
            this.GeneralStatusKey = generalStatusKey;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int ValuatorKey { get; set; }

        public string ValuatorContact { get; set; }

        public string ValuatorPassword { get; set; }

        public byte? LimitedUserGroup { get; set; }

        public int? GeneralStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.ValuatorKey =  key;
        }
    }
}