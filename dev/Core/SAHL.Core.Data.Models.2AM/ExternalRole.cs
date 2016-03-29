using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ExternalRoleDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ExternalRoleDataModel(int genericKey, int genericKeyTypeKey, int legalEntityKey, int externalRoleTypeKey, int generalStatusKey, DateTime changeDate)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.LegalEntityKey = legalEntityKey;
            this.ExternalRoleTypeKey = externalRoleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ChangeDate = changeDate;
		
        }
		[JsonConstructor]
        public ExternalRoleDataModel(int externalRoleKey, int genericKey, int genericKeyTypeKey, int legalEntityKey, int externalRoleTypeKey, int generalStatusKey, DateTime changeDate)
        {
            this.ExternalRoleKey = externalRoleKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.LegalEntityKey = legalEntityKey;
            this.ExternalRoleTypeKey = externalRoleTypeKey;
            this.GeneralStatusKey = generalStatusKey;
            this.ChangeDate = changeDate;
		
        }		

        public int ExternalRoleKey { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int ExternalRoleTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public void SetKey(int key)
        {
            this.ExternalRoleKey =  key;
        }
    }
}