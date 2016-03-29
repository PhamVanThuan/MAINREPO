using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MailingAddressDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public MailingAddressDataModel(int accountKey, int addressKey, bool onlineStatement, int onlineStatementFormatKey, int languageKey, int? legalEntityKey, int? correspondenceMediumKey)
        {
            this.AccountKey = accountKey;
            this.AddressKey = addressKey;
            this.OnlineStatement = onlineStatement;
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.LanguageKey = languageKey;
            this.LegalEntityKey = legalEntityKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }
		[JsonConstructor]
        public MailingAddressDataModel(int mailingAddressAccountKey, int accountKey, int addressKey, bool onlineStatement, int onlineStatementFormatKey, int languageKey, int? legalEntityKey, int? correspondenceMediumKey)
        {
            this.MailingAddressAccountKey = mailingAddressAccountKey;
            this.AccountKey = accountKey;
            this.AddressKey = addressKey;
            this.OnlineStatement = onlineStatement;
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.LanguageKey = languageKey;
            this.LegalEntityKey = legalEntityKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }		

        public int MailingAddressAccountKey { get; set; }

        public int AccountKey { get; set; }

        public int AddressKey { get; set; }

        public bool OnlineStatement { get; set; }

        public int OnlineStatementFormatKey { get; set; }

        public int LanguageKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public int? CorrespondenceMediumKey { get; set; }

        public void SetKey(int key)
        {
            this.MailingAddressAccountKey =  key;
        }
    }
}