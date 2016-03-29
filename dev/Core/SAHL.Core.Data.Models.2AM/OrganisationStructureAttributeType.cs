using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class OrganisationStructureAttributeTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public OrganisationStructureAttributeTypeDataModel(string description, int dataTypeKey, int? length)
        {
            this.Description = description;
            this.DataTypeKey = dataTypeKey;
            this.Length = length;
		
        }
		[JsonConstructor]
        public OrganisationStructureAttributeTypeDataModel(int organisationStructureAttributeTypeKey, string description, int dataTypeKey, int? length)
        {
            this.OrganisationStructureAttributeTypeKey = organisationStructureAttributeTypeKey;
            this.Description = description;
            this.DataTypeKey = dataTypeKey;
            this.Length = length;
		
        }		

        public int OrganisationStructureAttributeTypeKey { get; set; }

        public string Description { get; set; }

        public int DataTypeKey { get; set; }

        public int? Length { get; set; }

        public void SetKey(int key)
        {
            this.OrganisationStructureAttributeTypeKey =  key;
        }
    }
}