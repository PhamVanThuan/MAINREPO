using System.Collections.Generic;
using NUnit.Framework;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Helper;
using SAHL.Services.Query.Server.Tests.Models;

namespace SAHL.Services.Query.Server.Tests.Coordinator
{
    
    [TestFixture]
    public class FieldExclusionCoordinatorTests
    {

        [Test]
        public void MarkFieldsAsNull_GivenObjectWithEmptyFieldsList_ShouldNotMarkAnyFieldsAsNull()
        {
            
            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            var model = CreateTestDataModel();

            //action
            coordinator.MarkFieldsAsNull(model, new List<string>());

            //assert
            Assert.IsNotNull(model.Description);
            Assert.IsNotNull(model.Id);
            Assert.IsNotNull(model.AliaseLookup);

        }

        [Test]
        public void MarkFieldsAsNull_GivenModletWithSingleFieldsList_ShouldSetFieldsInSuppliedListAndAliasLookupsAsNotNull()
        {

            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            var model = CreateTestDataModel();
            List<string> fields = new List<string>() {"Description"};
            
            //action
            coordinator.MarkFieldsAsNull(model, fields);

            //assert
            Assert.IsNotNull(model.Description);
            Assert.IsNotNull(model.Id);
            Assert.IsNotNull(model.AliaseLookup);
            Assert.IsNotNull(model.Relationships);

        }

        [Test]
        public void MarkFieldsAsNull_GivenModeltWithTwoFieldsList_ShouldSetFieldsInSuppliedListAndAliasLookupsAsNotNull()
        {

            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            var model = CreateTestDataModel();
            List<string> fields = new List<string>() { "Description", "Id" };

            //action
            coordinator.MarkFieldsAsNull(model, fields);

            //assert
            Assert.IsNotNull(model.Description);
            Assert.IsNotNull(model.Id);
            Assert.IsNotNull(model.AliaseLookup);
            Assert.IsNotNull(model.Relationships);

        }

        [Test]
        public void MarkListItemsWithNull_GivenEmptyListOfItemsToNotMarkWithNull_ShouldReturnAnEmptyList()
        {
            
            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            List<TestDataModel> testDataModels = new List<TestDataModel>();
            
            //action
            List<string> fields = new List<string>();
            coordinator.MarkListItemsWithNull(testDataModels, fields);

            //assert
            Assert.AreEqual(0, testDataModels.Count);

        }

        [Test]
        public void MarkListItemsWithNull_GivenEmptyListOfFieldsItemsToNotMarkWithNullAndListWithSingleItem_ShouldReturnUntouchedList()
        {
            
            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            List<TestDataModel> testDataModels = new List<TestDataModel>();
            testDataModels.Add(CreateTestDataModel());
            List<string> fields = new List<string>();

            //action
            coordinator.MarkListItemsWithNull(testDataModels, fields);

            //assert
            Assert.AreEqual(1, testDataModels.Count);
            Assert.IsNotNull(testDataModels[0].Description);
            Assert.IsNotNull(testDataModels[0].Id);
            Assert.IsNotNull(testDataModels[0].AliaseLookup);
            Assert.IsNotNull(testDataModels[0].Relationships);

        }

        [Test]
        public void MarkListItemsWithNull_GivenListOfFieldsItemsToNotMarkWithNullAndListWithSingleItem_ShouldReturnListWithFieldsSetToNull()
        {
            
            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            List<TestDataModel> testDataModels = new List<TestDataModel>();
            testDataModels.Add(CreateTestDataModel());
            List<string> fields = new List<string>() { "Description" };
            
            //action
            coordinator.MarkListItemsWithNull(testDataModels, fields);

            //assert
            Assert.AreEqual(1, testDataModels.Count);
            Assert.IsNotNull(testDataModels[0].Description);
            Assert.IsNotNull(testDataModels[0].Id);
            Assert.IsNotNull(testDataModels[0].AliaseLookup);
            Assert.IsNotNull(testDataModels[0].Relationships);

        }

        [Test]
        public void MarkFieldsAsNull_GivenListOfFieldsItemsToNotMarkNull_ShouldNotSetIdFieldToNull()
        {
            //arrange
            FieldExclusionCoordinator<TestDataModel> coordinator = new FieldExclusionCoordinator<TestDataModel>();
            TestDataModel testDataModel = CreateTestDataModel();
            List<string> fields = new List<string>() { "Description" };

            //action
            coordinator.MarkFieldsAsNull(testDataModel, fields);

            //assert
            Assert.AreEqual(1, testDataModel.Id);

        }

        private static TestDataModel CreateTestDataModel()
        {
            TestDataModel model = new TestDataModel();
            model.Description = "Some Value";
            model.Id = 1;
            return model;
        }


    }

}