using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LegalEntityRelationshipDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public LegalEntityRelationshipDataModel(int legalEntityKey, int relatedLegalEntityKey, int relationshipTypeKey)
        {
            this.LegalEntityKey = legalEntityKey;
            this.RelatedLegalEntityKey = relatedLegalEntityKey;
            this.RelationshipTypeKey = relationshipTypeKey;
		
        }
		[JsonConstructor]
        public LegalEntityRelationshipDataModel(int legalEntityRelationshipKey, int legalEntityKey, int relatedLegalEntityKey, int relationshipTypeKey)
        {
            this.LegalEntityRelationshipKey = legalEntityRelationshipKey;
            this.LegalEntityKey = legalEntityKey;
            this.RelatedLegalEntityKey = relatedLegalEntityKey;
            this.RelationshipTypeKey = relationshipTypeKey;
		
        }		

        public int LegalEntityRelationshipKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int RelatedLegalEntityKey { get; set; }

        public int RelationshipTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.LegalEntityRelationshipKey =  key;
        }
    }
}