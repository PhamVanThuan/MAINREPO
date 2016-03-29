using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityBankAccountDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityBankAccountDataModel(int legalEntityKey, int bankAccountKey, int generalStatusKey, string userID, DateTime? changeDate)
        {
            this.LegalEntityKey = legalEntityKey;
            this.BankAccountKey = bankAccountKey;
            this.GeneralStatusKey = generalStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public LegalEntityBankAccountDataModel(int legalEntityBankAccountKey, int legalEntityKey, int bankAccountKey, int generalStatusKey, string userID, DateTime? changeDate)
        {
            this.LegalEntityBankAccountKey = legalEntityBankAccountKey;
            this.LegalEntityKey = legalEntityKey;
            this.BankAccountKey = bankAccountKey;
            this.GeneralStatusKey = generalStatusKey;
            this.UserID = userID;
            this.ChangeDate = changeDate;
		
        }		

        public int LegalEntityBankAccountKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int BankAccountKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityBankAccountKey =  key;
        }
    }
}