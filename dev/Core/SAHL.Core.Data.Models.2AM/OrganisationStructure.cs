using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OrganisationStructureDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OrganisationStructureDataModel(int? parentKey, string description, int organisationTypeKey, int generalStatusKey)
        {
            this.ParentKey = parentKey;
            this.Description = description;
            this.OrganisationTypeKey = organisationTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }
		[JsonConstructor]
        public OrganisationStructureDataModel(int organisationStructureKey, int? parentKey, string description, int organisationTypeKey, int generalStatusKey)
        {
            this.OrganisationStructureKey = organisationStructureKey;
            this.ParentKey = parentKey;
            this.Description = description;
            this.OrganisationTypeKey = organisationTypeKey;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int OrganisationStructureKey { get; set; }

        public int? ParentKey { get; set; }

        public string Description { get; set; }

        public int OrganisationTypeKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public void SetKey(int key)
        {
            this.OrganisationStructureKey =  key;
        }
    }
}