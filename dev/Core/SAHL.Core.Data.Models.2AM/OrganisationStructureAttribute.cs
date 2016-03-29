using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OrganisationStructureAttributeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OrganisationStructureAttributeDataModel(int organisationStructureAttributeTypeKey, string attributeValue, int organisationStructureKey)
        {
            this.OrganisationStructureAttributeTypeKey = organisationStructureAttributeTypeKey;
            this.AttributeValue = attributeValue;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }
		[JsonConstructor]
        public OrganisationStructureAttributeDataModel(int organisationStructureAttributeKey, int organisationStructureAttributeTypeKey, string attributeValue, int organisationStructureKey)
        {
            this.OrganisationStructureAttributeKey = organisationStructureAttributeKey;
            this.OrganisationStructureAttributeTypeKey = organisationStructureAttributeTypeKey;
            this.AttributeValue = attributeValue;
            this.OrganisationStructureKey = organisationStructureKey;
		
        }		

        public int OrganisationStructureAttributeKey { get; set; }

        public int OrganisationStructureAttributeTypeKey { get; set; }

        public string AttributeValue { get; set; }

        public int OrganisationStructureKey { get; set; }

        public void SetKey(int key)
        {
            this.OrganisationStructureAttributeKey =  key;
        }
    }
}