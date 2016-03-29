using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserOrganisationStructureDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserOrganisationStructureDataModel(int aDUserKey, int organisationStructureKey, int? genericKey, int? genericKeyTypeKey, int generalStatusKey)
        {
            this.ADUserKey = aDUserKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public UserOrganisationStructureDataModel(int userOrganisationStructureKey, int aDUserKey, int organisationStructureKey, int? genericKey, int? genericKeyTypeKey, int generalStatusKey)
        {
            this.UserOrganisationStructureKey = userOrganisationStructureKey;
            this.ADUserKey = aDUserKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int UserOrganisationStructureKey { get; set; }

        public int ADUserKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int? GenericKey { get; set; }

        public int? GenericKeyTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.UserOrganisationStructureKey =  key;
        }
    }
}