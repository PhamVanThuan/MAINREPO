using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OrganisationStructureOriginationSourceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OrganisationStructureOriginationSourceDataModel(int organisationStructureKey, int originationSourceKey)
        {
            this.OrganisationStructureKey = organisationStructureKey;
            this.OriginationSourceKey = originationSourceKey;
		
        }
		[JsonConstructor]
        public OrganisationStructureOriginationSourceDataModel(int organisationStructureOriginationSourceKey, int organisationStructureKey, int originationSourceKey)
        {
            this.OrganisationStructureOriginationSourceKey = organisationStructureOriginationSourceKey;
            this.OrganisationStructureKey = organisationStructureKey;
            this.OriginationSourceKey = originationSourceKey;
		
        }		

        public int OrganisationStructureOriginationSourceKey { get; set; }

        public int OrganisationStructureKey { get; set; }

        public int OriginationSourceKey { get; set; }

        public void SetKey(int key)
        {
            this.OrganisationStructureOriginationSourceKey =  key;
        }
    }
}