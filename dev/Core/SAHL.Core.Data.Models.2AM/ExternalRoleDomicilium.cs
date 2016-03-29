using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExternalRoleDomiciliumDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExternalRoleDomiciliumDataModel(int legalEntityDomiciliumKey, int externalRoleKey, DateTime changeDate, int aDUserKey)
        {
            this.LegalEntityDomiciliumKey = legalEntityDomiciliumKey;
            this.ExternalRoleKey = externalRoleKey;
            this.ChangeDate = changeDate;
            this.ADUserKey = aDUserKey;
		
        }
		[JsonConstructor]
        public ExternalRoleDomiciliumDataModel(int externalRoleDomiciliumKey, int legalEntityDomiciliumKey, int externalRoleKey, DateTime changeDate, int aDUserKey)
        {
            this.ExternalRoleDomiciliumKey = externalRoleDomiciliumKey;
            this.LegalEntityDomiciliumKey = legalEntityDomiciliumKey;
            this.ExternalRoleKey = externalRoleKey;
            this.ChangeDate = changeDate;
            this.ADUserKey = aDUserKey;
		
        }		

        public int ExternalRoleDomiciliumKey { get; set; }

        public int LegalEntityDomiciliumKey { get; set; }

        public int ExternalRoleKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public int ADUserKey { get; set; }

        public void SetKey(int key)
        {
            this.ExternalRoleDomiciliumKey =  key;
        }
    }
}