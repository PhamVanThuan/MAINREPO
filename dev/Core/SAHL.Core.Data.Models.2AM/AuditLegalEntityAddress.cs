using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AuditLegalEntityAddressDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public AuditLegalEntityAddressDataModel(string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityAddressKey, int legalEntityKey, int addressKey, int addressTypeKey, DateTime effectiveDate, int generalStatusKey)
        {
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.LegalEntityKey = legalEntityKey;
            this.AddressKey = addressKey;
            this.AddressTypeKey = addressTypeKey;
            this.EffectiveDate = effectiveDate;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public AuditLegalEntityAddressDataModel(decimal auditNumber, string auditLogin, string auditHostName, string auditProgramName, DateTime auditDate, string auditAddUpdateDelete, int legalEntityAddressKey, int legalEntityKey, int addressKey, int addressTypeKey, DateTime effectiveDate, int generalStatusKey)
        {
            this.AuditNumber = auditNumber;
            this.AuditLogin = auditLogin;
            this.AuditHostName = auditHostName;
            this.AuditProgramName = auditProgramName;
            this.AuditDate = auditDate;
            this.AuditAddUpdateDelete = auditAddUpdateDelete;
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.LegalEntityKey = legalEntityKey;
            this.AddressKey = addressKey;
            this.AddressTypeKey = addressTypeKey;
            this.EffectiveDate = effectiveDate;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public decimal AuditNumber { get; set; }

        public string AuditLogin { get; set; }

        public string AuditHostName { get; set; }

        public string AuditProgramName { get; set; }

        public DateTime AuditDate { get; set; }

        public string AuditAddUpdateDelete { get; set; }

        public int LegalEntityAddressKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AddressKey { get; set; }

        public int AddressTypeKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(decimal key)
        {
            this.AuditNumber =  key;
        }
    }
}