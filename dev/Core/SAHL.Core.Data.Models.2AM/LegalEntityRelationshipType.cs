using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityRelationshipTypeDataModel :  IDataModel
    {
        public LegalEntityRelationshipTypeDataModel(int relationshipTypeKey, string description)
        {
            this.RelationshipTypeKey = relationshipTypeKey;
            this.Description = description;
		
        }		

        public int RelationshipTypeKey { get; set; }

        public string Description { get; set; }
    }
}