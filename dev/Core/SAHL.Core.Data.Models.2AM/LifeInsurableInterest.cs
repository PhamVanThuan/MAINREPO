using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeInsurableInterestDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LifeInsurableInterestDataModel(int legalEntityKey, int accountKey, int lifeInsurableInterestTypeKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.LifeInsurableInterestTypeKey = lifeInsurableInterestTypeKey;
		
        }
		[JsonConstructor]
        public LifeInsurableInterestDataModel(int lifeInsurableInterestKey, int legalEntityKey, int accountKey, int lifeInsurableInterestTypeKey)
        {
            this.LifeInsurableInterestKey = lifeInsurableInterestKey;
            this.LegalEntityKey = legalEntityKey;
            this.AccountKey = accountKey;
            this.LifeInsurableInterestTypeKey = lifeInsurableInterestTypeKey;
		
        }		

        public int LifeInsurableInterestKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AccountKey { get; set; }

        public int LifeInsurableInterestTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.LifeInsurableInterestKey =  key;
        }
    }
}