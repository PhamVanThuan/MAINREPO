using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using NSubstitute;
using NUnit.Framework.Constraints;
using SAHL.Core.Data;
using SAHL.Services.Query.Mappers;
using SAHL.Services.Query.Server.Tests.Models;
using SAHL.Services.Query.Server.Tests.Representations;

namespace SAHL.Services.Query.Server.Tests.Mapper
{
    [TestFixture]
    public class DataModelToRepresentationMapperTests
    {

        private ILinkResolver linkResolver;
        private DataModelToRepresentationMapper<TestDataModel, TestRepresentation, TestListRepresentation> mapper;

        [SetUp]
        public void SetupTests()
        {
            linkResolver = Substitute.For<ILinkResolver>();
            mapper = new DataModelToRepresentationMapper<TestDataModel, TestRepresentation, TestListRepresentation>(linkResolver);
        }

        [Test]
        public void MapModelsToRepresentation_GivenTestDataModel_ShouldMapToTestRepresentation()
        {

            //action
            TestDataModel dataModel = CreateTestModel();
            TestRepresentation testRepresentation = (TestRepresentation) mapper.MapModelToRepresentation(dataModel);
            
            //assert
            Assert.AreEqual(dataModel.Id, testRepresentation.Id);
            Assert.AreEqual(dataModel.Count, testRepresentation.Count);
            Assert.AreEqual(dataModel.Description, testRepresentation.Description);
            Assert.AreEqual(dataModel.Name, testRepresentation.Name);

        }

        [Test]
        public void MapModelTpRepresentation_GivenTestDataModelWithItemsSetToNull_ShouldReturnMappedTestRepresentationWithSamFieldsSetToNull()
        {

            //action
            TestDataModel dataModel = CreateNulledTestModel();
            
            TestRepresentation testRepresentation = (TestRepresentation) mapper.MapModelToRepresentation(dataModel);
            
            //assert
            Assert.AreEqual(dataModel.Id, testRepresentation.Id);
            Assert.IsNull(testRepresentation.Count);
            Assert.IsNull(testRepresentation.Description);
            Assert.AreEqual(dataModel.Name, testRepresentation.Name);

        }

        [Test]
        public void MapModelsToRepresentation_GivenListOfDataModel_ShouldReturnRepresentationListWithCorrectListCount()
        {
            
            //arrange
            List<TestDataModel> dataList = new List<TestDataModel>();
            dataList.Add(CreateTestModel());
            dataList.Add(CreateTestModel());

            //action
            TestListRepresentation listRepresentation = (TestListRepresentation) mapper.MapModelsToRepresentation(dataList);

            //assert
            Assert.AreEqual(2, listRepresentation.List.Count());

        }

        [Test]
        public void MapModelsToRepresentation_GivenListOfDataModel_ShouldReturnRepresentationListWithItemsSetCorrectly()
        {

            //arrange
            TestDataModel dataModel = CreateTestModel();
            List<TestDataModel> dataList = new List<TestDataModel>();
            dataList.Add(dataModel);
            dataList.Add(dataModel);

            //action
            TestListRepresentation listRepresentation = (TestListRepresentation) mapper.MapModelsToRepresentation(dataList);

            //assert
            Assert.AreEqual(dataModel.Id, ((TestRepresentation)listRepresentation.List[0]).Id);
            Assert.AreEqual(dataModel.Id, ((TestRepresentation)listRepresentation.List[1]).Id);
            Assert.AreEqual(dataModel.Name, ((TestRepresentation)listRepresentation.List[0]).Name);
            Assert.AreEqual(dataModel.Name, ((TestRepresentation)listRepresentation.List[1]).Name);
            Assert.AreEqual(dataModel.Description, ((TestRepresentation)listRepresentation.List[0]).Description);
            Assert.AreEqual(dataModel.Description, ((TestRepresentation)listRepresentation.List[1]).Description);
            Assert.AreEqual(dataModel.Count, ((TestRepresentation)listRepresentation.List[0]).Count);
            Assert.AreEqual(dataModel.Count, ((TestRepresentation)listRepresentation.List[1]).Count);

        }
        
        [Test]
        public void MapModelsToRepresentation_GivenListOfDataModelsWithNulledFields_ShouldReturnRepresentationListWithNullItemsSetToNull()
        {

            //arrange
            TestDataModel dataModel = CreateTestModel();
            List<TestDataModel> dataList = new List<TestDataModel>();
            dataList.Add(dataModel);
            dataList.Add(dataModel);

            //action
            TestListRepresentation listRepresentation = (TestListRepresentation) mapper.MapModelsToRepresentation(dataList);

            //assert
            Assert.AreEqual(dataModel.Id, ((TestRepresentation)listRepresentation.List[0]).Id);
            Assert.AreEqual(dataModel.Id, ((TestRepresentation)listRepresentation.List[1]).Id);
            Assert.AreEqual(dataModel.Name, ((TestRepresentation)listRepresentation.List[0]).Name);
            Assert.AreEqual(dataModel.Name, ((TestRepresentation)listRepresentation.List[1]).Name);
            Assert.AreEqual(dataModel.Description, ((TestRepresentation)listRepresentation.List[0]).Description);
            Assert.AreEqual(dataModel.Description, ((TestRepresentation)listRepresentation.List[1]).Description);
            Assert.AreEqual(dataModel.Count, ((TestRepresentation)listRepresentation.List[0]).Count);
            Assert.AreEqual(dataModel.Count, ((TestRepresentation)listRepresentation.List[1]).Count);

        }
        
        private TestDataModel CreateTestModel()
        {
            TestDataModel testDataModel = new TestDataModel();
            testDataModel.Id = 1;
            testDataModel.Count = 100;
            testDataModel.Name = "A Name";
            testDataModel.Description = "A Test Data Model";
            return testDataModel;
        }

        private TestDataModel CreateNulledTestModel()
        {
            TestDataModel testDataModel = CreateTestModel();
            testDataModel.Count = null;
            testDataModel.Description = null;
            return testDataModel;
        }

    }
}