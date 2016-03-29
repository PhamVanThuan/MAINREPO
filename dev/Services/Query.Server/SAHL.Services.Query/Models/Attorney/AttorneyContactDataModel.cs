using System;
using System.Collections.Generic;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Core;

namespace SAHL.Services.Query.Models.Attorney
{
    public class AttorneyContactDataModel : IQueryDataModel
    {

        public AttorneyContactDataModel()
        {
            SetupRelationships();
        }

        public int Id { get; set; }
        public int? AttorneyKey { get; set; }
        public Guid? AttorneyId { get; set; }
        public string CellPhoneNumber { get; set; }
        public string HomePhoneCode { get; set; }
        public string HomePhoneNumber { get; set; }
        public string WorkPhoneCode { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string FaxCode { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }

        public Dictionary<string, string> AliaseLookup { get; set; }
        public List<IRelationshipDefinition> Relationships { get; set; }

        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
            Relationships.Add(new RelationshipDefinition
            {
                RelatedEntity = "Attorney",
                DataModelType = typeof(AttorneyDataModel),
                RelationshipType = RelationshipType.ManyToOne,
                RelatedFields = new List<IRelatedField>
                {
                    new RelatedField { LocalKey = "AttorneyId", RelatedKey = "Id", Value = "" }
                },
            });
        }

    }

}