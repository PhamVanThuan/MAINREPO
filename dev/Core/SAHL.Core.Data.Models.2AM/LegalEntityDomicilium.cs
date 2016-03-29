using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityDomiciliumDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityDomiciliumDataModel(int legalEntityAddressKey, int generalStatusKey, DateTime changeDate, int aDUserKey)
        {
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ChangeDate = changeDate;
            this.ADUserKey = aDUserKey;
		
        }
		[JsonConstructor]
        public LegalEntityDomiciliumDataModel(int legalEntityDomiciliumKey, int legalEntityAddressKey, int generalStatusKey, DateTime changeDate, int aDUserKey)
        {
            this.LegalEntityDomiciliumKey = legalEntityDomiciliumKey;
            this.LegalEntityAddressKey = legalEntityAddressKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ChangeDate = changeDate;
            this.ADUserKey = aDUserKey;
		
        }		

        public int LegalEntityDomiciliumKey { get; set; }

        public int LegalEntityAddressKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public int ADUserKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityDomiciliumKey =  key;
        }
    }
}