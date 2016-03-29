using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Web.Services;
using System;

namespace SAHL.Core.Web.Tests.Services
{
    [TestFixture]
    public class TestJsonActivator
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var jsonActivator = new JsonActivator();
            //---------------Test Result -----------------------
            Assert.IsNotNull(jsonActivator);
        }

        [Test]
        public void Serialize_GivenDataModel_ShouldSerializeDataModel()
        {
            //---------------Set up test pack-------------------
            var jsonActivator = new JsonActivator();
            var fakeTestDataModel = new FakeTestDataModel(Guid.NewGuid(), "test", DateTime.Now, null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var serializeData = jsonActivator.SerializeObject(fakeTestDataModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(serializeData);
            StringAssert.Contains("_name", serializeData);
        }

        [Test]
        public void Deserialize_GivenSerializedDataModelData_ShouldCreateAndReturnDataModel()
        {
            //---------------Set up test pack-------------------
            var jsonActivator = new JsonActivator();
            var fakeTestDataModel = new FakeTestDataModel(Guid.NewGuid(), "test", DateTime.Now, null);
            var serializeData = jsonActivator.SerializeObject(fakeTestDataModel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dataModel = jsonActivator.DeserializeObject<IDataModel>(serializeData) as FakeTestDataModel;
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataModel);
            Assert.AreEqual(fakeTestDataModel.Id, dataModel.Id);
            Assert.AreEqual(fakeTestDataModel.TestString, dataModel.TestString);
            Assert.AreEqual(fakeTestDataModel.Date, dataModel.Date);
            Assert.AreEqual(fakeTestDataModel.EmbeddedModel, dataModel.EmbeddedModel);
        }

        [Test]
        public void Deserialize_GivenSerializedDataModelWithEmbeddeDataModel_ShouldCreateAndReturnDataModelWithEmbeddedDataModel()
        {
            //---------------Set up test pack-------------------
            var jsonActivator = new JsonActivator();
            var secondFakeTestDataModel = new SecondFakeTestDataModel(Guid.NewGuid(), "second");
            var fakeTestDataModel = new FakeTestDataModel(Guid.NewGuid(), "test", DateTime.Now, secondFakeTestDataModel);
            var serializeData = jsonActivator.SerializeObject(fakeTestDataModel);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dataModel = jsonActivator.DeserializeObject<IDataModel>(serializeData) as FakeTestDataModel;
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataModel);
            Assert.IsNotNull(dataModel.EmbeddedModel);

            var embeddedDataModel = dataModel.EmbeddedModel as SecondFakeTestDataModel;
            Assert.IsNotNull(embeddedDataModel);

            Assert.AreEqual(embeddedDataModel.Id, secondFakeTestDataModel.Id);
            Assert.AreEqual(embeddedDataModel.StringData, secondFakeTestDataModel.StringData);
        }

        [Test]
        public void DeserializeObject_GivenSerializedDataModelAsString_ShouldCreateAndReturnDataModelAsObjectType()
        {
            //---------------Set up test pack-------------------
            var jsonActivator = new JsonActivator();
            var fakeTestDataModel = new SecondFakeTestDataModel(Guid.NewGuid(), "second");
            var serializeData = jsonActivator.SerializeObject(fakeTestDataModel);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dataModel = jsonActivator.DeserializeObject(serializeData);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataModel);

            Assert.AreEqual("SecondFakeTestDataModel", dataModel.GetType().Name);
        }
        
        private class FakeTestDataModel : IDataModel
        {
            public FakeTestDataModel(Guid id, string testString, DateTime date, IDataModel embeddedModel)
            {
                this.Id = id;
                this.TestString = testString;
                this.Date = date;
                this.EmbeddedModel = embeddedModel;
            }

            public Guid Id { get; set; }

            public string TestString { get; set; }

            public DateTime Date { get; set; }

            public IDataModel EmbeddedModel { get; set; }
        }

        private class SecondFakeTestDataModel : IDataModel
        {
            public SecondFakeTestDataModel(Guid id, string stringData)
            {
                this.Id = id;
                this.StringData = stringData;
            }

            public Guid Id { get; set; }

            public string StringData { get; set; }
        }
    }
}