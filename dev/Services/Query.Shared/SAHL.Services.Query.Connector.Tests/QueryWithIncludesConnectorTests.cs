using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SAHL.Services.Query.Connector.Tests
{
    [TestFixture]
    public class QueryWithIncludesConnectorTests
    {

        private QueryWithIncludes query;

        [SetUp]
        public void SetupTests()
        {
            query = new QueryWithIncludes(null, "");
        }

        [Test]
        public void QueryFind_GivenQueryWithIncludeFields_ShouldCreateJsonFields()
        {

            //arrange
            query.IncludeField("Name");

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={fields: {'Name':'true'}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithTwoIncludeFields_ShouldCreateJsonFields()
        {

            //arrange
            query.IncludeField("Name")
                .IncludeField("Id");

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={fields: {'Name':'true','Id':'true'}}", json);

        }
        [Test]
        public void QueryFind_GivenQueryWithRelatedIncludes_ShouldCreateJsonIncludes()
        {
            //arrange
            query.IncludeRelationship("Name");

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={include: 'Name'}", json);
        }

        [Test]
        public void QueryFind_GivenQueryWithTwoRelatedIncludes_ShouldCreateJsonIncludes()
        {
            //arrange
            query.IncludeRelationship("Name")
                .IncludeRelationship("Id");

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={include: 'Name,Id'}", json);
        }

        [Test]
        public void QueryFind_GivenQueryWithTwoIncludeFieldsAsBulk_ShouldCreateJsonFields()
        {

            //arrange
            query.IncludeFields(new string[] { "Name", "Id" });

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={fields: {'Name':'true','Id':'true'}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithTwoRelatedIncludesAsBulk_ShouldCreateJsonIncludes()
        {
            //arrange
            query.IncludeRelationships(new string[] { "Name", "Id" });

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={include: 'Name,Id'}", json);
        }

    }

}
