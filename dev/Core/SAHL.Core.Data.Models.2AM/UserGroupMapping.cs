using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserGroupMappingDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public UserGroupMappingDataModel(int organisationStructureKey, int functionalGroupDefinitionKey)
        {
            this.OrganisationStructureKey = organisationStructureKey;
            this.FunctionalGroupDefinitionKey = functionalGroupDefinitionKey;
		
        }
		[JsonConstructor]
        public UserGroupMappingDataModel(int userGroupMappingKey, int organisationStructureKey, int functionalGroupDefinitionKey)
        {
            this.UserGroupMappingKey = userGroupMappingKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.FunctionalGroupDefinitionKey = functionalGroupDefinitionKey;
		
        }		

        public int UserGroupMappingKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int FunctionalGroupDefinitionKey { get; set; }

        public void SetKey(int key)
        {
            this.UserGroupMappingKey =  key;
        }
    }
}