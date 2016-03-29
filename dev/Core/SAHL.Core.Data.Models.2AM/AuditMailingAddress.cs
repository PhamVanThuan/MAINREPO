using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditMailingAddressDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditMailingAddressDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int mailingAddressAccountKey, int accountKey, int addressKey, bool onlineStatement, int onlineStatementFormatKey, int? languageKey, int? legalEntityKey, int? correspondenceMediumKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MailingAddressAccountKey = mailingAddressAccountKey;
            this.AccountKey = accountKey;
            this.AddressKey = addressKey;
            this.OnlineStatement = onlineStatement;
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.LanguageKey = languageKey;
            this.LegalEntityKey = legalEntityKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }
		[JsonConstructor]
        public AuditMailingAddressDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int mailingAddressAccountKey, int accountKey, int addressKey, bool onlineStatement, int onlineStatementFormatKey, int? languageKey, int? legalEntityKey, int? correspondenceMediumKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.MailingAddressAccountKey = mailingAddressAccountKey;
            this.AccountKey = accountKey;
            this.AddressKey = addressKey;
            this.OnlineStatement = onlineStatement;
            this.OnlineStatementFormatKey = onlineStatementFormatKey;
            this.LanguageKey = languageKey;
            this.LegalEntityKey = legalEntityKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int MailingAddressAccountKey { get; set; }

        public int AccountKey { get; set; }

        public int AddressKey { get; set; }

        public bool OnlineStatement { get; set; }

        public int OnlineStatementFormatKey { get; set; }

        public int? LanguageKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public int? CorrespondenceMediumKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}