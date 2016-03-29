using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityAddressDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityAddressDataModel(int legalEntityKey, int addressKey, int addressTypeKey, DateTime effectiveDate, int generalStatusKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AddressKey = addressKey;
            this.AddressTypeKey = addressTypeKey;
            this.EffectiveDate = effectiveDate;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public LegalEntityAddressDataModel(int legalEntityAddressKey, int legalEntityKey, int addressKey, int addressTypeKey, DateTime effectiveDate, int generalStatusKey)
        {
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.LegalEntityKey = legalEntityKey;
            this.AddressKey = addressKey;
            this.AddressTypeKey = addressTypeKey;
            this.EffectiveDate = effectiveDate;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int LegalEntityAddressKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AddressKey { get; set; }

        public int AddressTypeKey { get; set; }

        public DateTime EffectiveDate { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityAddressKey =  key;
        }
    }
}