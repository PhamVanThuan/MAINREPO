using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Models.Lookup;
using SAHL.Services.Query.Resources.Attorney;

namespace SAHL.Services.Query.Models.Attorney
{
    public class AttorneyDataModel : IQueryDataModel
    {

        public AttorneyDataModel()
        {
            SetupRelationships();
        }

        public Dictionary<string, string> AliaseLookup { get; set; }

        public List<IRelationshipDefinition> Relationships { get; set; }

        public Guid Id { get; set; }
        public int? AttorneyKey { get; set; }
        public int? LegalEntityKey { get; set; }
        public string Name { get; set; }
        public bool? IsLitigationAttorney { get; set; }
        public bool? IsRegistrationAttorney { get; set; }
        public bool? IsPanelAttorney { get; set; }
        public string AttorneyContact { get; set; }
        public int? GeneralStatusKey { get; set; }
        public string GeneralStatus { get; set; }
        public int? DeedsOfficeKey { get; set; }
        public string DeedsOffice { get; set; }
        public decimal? Mandate { get; set; }
        public bool? WorkflowEnabled { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "GeneralStatus",
                DataModelType = typeof (LookupDataModel),
                RelationshipType = RelationshipType.OneToOneLookup,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "GeneralStatusKey", RelatedKey = "GeneralStatusKey", Value = ""}
                    }
            });
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "DeedsOffice",
                DataModelType = typeof (LookupDataModel),
                RelationshipType = RelationshipType.OneToOneLookup,
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "DeedsOfficeKey", RelatedKey = "DeedsOfficeKey", Value = ""}
                    }
            });
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "ContactInformation",
                DataModelType = typeof (AttorneyContactInformationDataModel),
                RelationshipType = RelationshipType.OneToOne,
                RelatedFields =
                    new List<IRelatedField>() { new RelatedField() { LocalKey = "Id", RelatedKey = "AttorneyId", Value = "" } }
            });
            Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "Contacts",
                DataModelType = typeof (IEnumerable<AttorneyContactDataModel>),
                RelationshipType = RelationshipType.OneToManyWhere,
                RelatedFields =
                    new List<IRelatedField>() {new RelatedField() {LocalKey = "Id", RelatedKey = "AttorneyId", Value = ""}}
            });
        }

    }

}