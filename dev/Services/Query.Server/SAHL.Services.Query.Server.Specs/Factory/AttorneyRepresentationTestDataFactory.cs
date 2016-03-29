using System;
using System.Collections.Generic;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Models.Core;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.Attorney;
using SAHL.Services.Query.Server.Specs.Coordinators;
using SAHL.Services.Query.Server.Specs.Fakes;

namespace SAHL.Services.Query.Server.Specs.Factory
{

    public static class AttorneyRepresentationTestDataFactory
    {

        public static List<AttorneyDataModel> GetAttorneyDataModels()
        {
            return new List<AttorneyDataModel>()
            {
                GetAttorneyDetailDataModel(new Guid())
            };
        }
        
        public static AttorneyRepresentation GetAttorney(ILinkResolver linkResolver)
        {


            return new AttorneyRepresentation(linkResolver)
            {
                Id = new Guid(),
                Name = "Test Attorney",
                AttorneyContact = "Mr Contact",
                GeneralStatus = "Active",
                GeneralStatusKey = 1,
                DeedsOffice = "Cape Town",
                DeedsOfficeKey = 1,
                IsLitigationAttorney = true,
                IsRegistrationAttorney = true,
                IsPanelAttorney = true
            };
        }

        public static AttorneyDataModel GetAttorneyDetailDataModel(Guid id)
        {
            return new AttorneyDataModel()
            {
                Id = id,
                Name = "Test Attorney",
                AttorneyContact = "Mr Contact",
                GeneralStatus = "Active",
                DeedsOffice = "Cape Town",
                DeedsOfficeKey = 1,
                IsLitigationAttorney = true,
                IsRegistrationAttorney = true,
                IsPanelAttorney = true,
                GeneralStatusKey = 1,
                Relationships = new List<IRelationshipDefinition>
                {
                    new RelationshipDefinition
                    {
                        DataModelType = typeof(Test2DataModel),
                        RelatedEntity = "Test2",
                        RelatedFields = new List<IRelatedField>
                        {
                            new RelatedField
                            {
                                LocalKey = "Id",
                                RelatedKey = "TestId",
                                Value = "10",
                            }
                        },
                        RelationshipType = RelationshipType.OneToOne,
                    }
                }

            };
        }


    }

}