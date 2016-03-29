using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExternalLifePolicyDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExternalLifePolicyDataModel(int insurerKey, string policyNumber, DateTime commencementDate, int lifePolicyStatusKey, DateTime? closeDate, decimal sumInsured, bool policyCeded, int legalEntityKey)
        {
            this.InsurerKey = insurerKey;
            this.PolicyNumber = policyNumber;
            this.CommencementDate = commencementDate;
            this.LifePolicyStatusKey = lifePolicyStatusKey;
            this.CloseDate = closeDate;
            this.SumInsured = sumInsured;
            this.PolicyCeded = policyCeded;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public ExternalLifePolicyDataModel(int externalLifePolicyKey, int insurerKey, string policyNumber, DateTime commencementDate, int lifePolicyStatusKey, DateTime? closeDate, decimal sumInsured, bool policyCeded, int legalEntityKey)
        {
            this.ExternalLifePolicyKey = externalLifePolicyKey;
            this.InsurerKey = insurerKey;
            this.PolicyNumber = policyNumber;
            this.CommencementDate = commencementDate;
            this.LifePolicyStatusKey = lifePolicyStatusKey;
            this.CloseDate = closeDate;
            this.SumInsured = sumInsured;
            this.PolicyCeded = policyCeded;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int ExternalLifePolicyKey { get; set; }

        public int InsurerKey { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime CommencementDate { get; set; }

        public int LifePolicyStatusKey { get; set; }

        public DateTime? CloseDate { get; set; }

        public decimal SumInsured { get; set; }

        public bool PolicyCeded { get; set; }

        public int LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.ExternalLifePolicyKey =  key;
        }
    }
}