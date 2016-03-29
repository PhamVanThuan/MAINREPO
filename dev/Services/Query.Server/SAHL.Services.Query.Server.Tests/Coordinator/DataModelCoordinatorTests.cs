using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.LinkCoordinator;
using SAHL.Services.Query.Models;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Models.Core;

namespace SAHL.Services.Query.Server.Tests.Coordinator
{
    [TestFixture]
    public class DataModelCoordinatorTests
    {

        private DataModelCoordinator DataModelCoordinator;

        [SetUp]
        public void SetupTest()
        {
            DataModelCoordinator = new DataModelCoordinator();
        }

        [Test]
        public void ResolveDataModelRelationships_GivenDataModelWithSingleRelationship_ShouldResolveTheValueForTheRelationship()
        {
            
            //arrange
            var attorneyDataModel = CreateAttorneyDataModel();
            AddGeneralStatusRelationship(attorneyDataModel);
            
            //action
            var dataModelWithRelationships = (AttorneyDataModel) DataModelCoordinator.ResolveDataModelRelationships(attorneyDataModel);

            //assert
            Assert.AreEqual("1", dataModelWithRelationships.Relationships[0].RelatedFields.First().Value);

        }

        [Test]
        public void ResolveDataModelRelationships_GivenDataModelWithTwoRelationship_ShouldResolveTheValueForTheRelationships()
        {

            //arrange
            var attorneyDataModel = CreateAttorneyDataModel();
            AddGeneralStatusRelationship(attorneyDataModel);
            AddDeedsOfficeRelationship(attorneyDataModel);

            //action
            var dataModelWithRelationships = (AttorneyDataModel)DataModelCoordinator.ResolveDataModelRelationships(attorneyDataModel);

            //assert
            Assert.AreEqual("1", dataModelWithRelationships.Relationships[0].RelatedFields.First().Value);
            Assert.AreEqual("2", dataModelWithRelationships.Relationships[1].RelatedFields.First().Value);

        }

        [Test]
        public void ResolveDataModelRelationships_GivenDataModelWithNoRelationships_ShouldDoNothing()
        {

            //arrange
            var attorneyDataModel = CreateAttorneyDataModel();

            //action
            var dataModelWithRelationships = (AttorneyDataModel)DataModelCoordinator.ResolveDataModelRelationships(attorneyDataModel);
            
            //assert
            Assert.AreEqual(0, dataModelWithRelationships.Relationships.Count);

        }

        [Test]
        public void ResolveDataModelRelationships_GivenDataModelsWithTwoRelationship_ShouldResolveAllModelsAndTheValuesForTheRelationships()
        {

            //arrange
            var attorneyDataModel = CreateAttorneyDataModel();
            AddGeneralStatusRelationship(attorneyDataModel);
            AddDeedsOfficeRelationship(attorneyDataModel);

            List<AttorneyDataModel> attorneyDataModels = new List<AttorneyDataModel>();
            attorneyDataModels.Add(attorneyDataModel);
            attorneyDataModels.Add(attorneyDataModel);
            
            //action
            var dataModelWithRelationships = (List<AttorneyDataModel>)DataModelCoordinator.ResolveDataModelRelationships(attorneyDataModels);

            //assert
            Assert.AreEqual("1", dataModelWithRelationships[0].Relationships[0].RelatedFields.First().Value);
            Assert.AreEqual("2", dataModelWithRelationships[0].Relationships[1].RelatedFields.First().Value);
            Assert.AreEqual("1", dataModelWithRelationships[1].Relationships[0].RelatedFields.First().Value);
            Assert.AreEqual("2", dataModelWithRelationships[1].Relationships[1].RelatedFields.First().Value);
            Assert.AreEqual(2, dataModelWithRelationships.Count);

        }

        [Test]
        public void ResolveDataModleRelationships_GivenDataModelRelationshipThatHasNullValueOnModel_ShouldRemoveTheRelationShip()
        {

            //arrange
            var attorneyDataModel = CreateAttorneyDataModel();
            AddGeneralStatusRelationship(attorneyDataModel);
            attorneyDataModel.GeneralStatusKey = null;

            //action
            var dataModelWithRelationships = (AttorneyDataModel)DataModelCoordinator.ResolveDataModelRelationships(attorneyDataModel);

            //assert
            Assert.AreEqual(0, dataModelWithRelationships.Relationships.Count);

        }

        private void AddGeneralStatusRelationship(AttorneyDataModel attorneyDataModel)
        {
            attorneyDataModel.Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "GeneralStatus",
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "GeneralStatusKey", RelatedKey = "GeneralStatusKey", Value = ""}
                    },
                RelationshipType = RelationshipType.OneToOne
            });
        }

        private void AddDeedsOfficeRelationship(AttorneyDataModel attorneyDataModel)
        {
            attorneyDataModel.Relationships.Add(new RelationshipDefinition()
            {
                RelatedEntity = "DeedsOffice",
                RelatedFields =
                    new List<IRelatedField>()
                    {
                        new RelatedField() {LocalKey = "DeedsOfficeKey", RelatedKey = "DeedsOfficeKey", Value = ""}
                    },
                RelationshipType = RelationshipType.OneToOne
            });
        }

        private AttorneyDataModel CreateAttorneyDataModel()
        {
            AttorneyDataModel attorneyDataModel = new AttorneyDataModel()
            {
                AttorneyContact = "AttorneyContact",
                DeedsOffice = "DeedsOffice",
                DeedsOfficeKey = 2,
                GeneralStatus = "Active",
                GeneralStatusKey = 1,
                Id = new Guid(),
                IsLitigationAttorney = true,
                IsRegistrationAttorney = false,
                IsPanelAttorney = true
            };
            attorneyDataModel.Relationships = new List<IRelationshipDefinition>();
            return attorneyDataModel;
        }

    }

}
