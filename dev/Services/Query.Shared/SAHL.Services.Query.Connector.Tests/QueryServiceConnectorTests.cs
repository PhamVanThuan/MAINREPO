using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector.Tests
{
    [TestFixture]
    public class QueryServiceConnectorTests
    {

        private IQuery query;
        [SetUp]
        public void SetupTests()
        {
            query = new Query(null, "");
        }

        [Test]
        public void QueryFind_GivenQueryWithWhere_ShouldCreateJsonWhere()
        {
            
            //arrange
            query.Where().And().Equals().Field("Name").Value("Value");
            
            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={where: {and: {eq: {'Name': 'Value'}}}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithLimit_ShouldShouldCreateJsonLimit()
        {
            
            //arrange
            query.Limit(5); 

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={limit: 5}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithSkip_ShouldCreateJsonSkip()
        {
            //arrange
            query.Skip(5);

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={skip: 5}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithOrder_ShouldCreateJsonOrder()
        {
            //arrange
            query.OrderBy("Name").Asc();

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={order: {0: 'Name asc'}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithTwoOrderItems_ShouldCreateJsonOrder()
        {
            //arrange
            query.OrderBy("Name").Asc()
                .OrderBy("Id").Desc();

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={order: {0: 'Name asc',1: 'Id desc'}}", json);

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
        public void QueryFind_GivenQueryWithTwoRelatedIncludesAsBulk_ShouldCreateJsonIncludes()
        {
            //arrange
            query.IncludeRelationships(new string[] {"Name", "Id"});

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={include: 'Name,Id'}", json);
        }

        [Test]
        public void QueryFind_GivenQueryWithPaging_ShouldCreateJsonWithPaging()
        {
            //arrange
            query.IncludePaging().SetCurrentPageTo(1).WithPageSize(10);

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("paging={Paging: {currentPage: 1, pageSize: 10}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithOrderAndPaging_ShouldCreateFilterAndPagingJson()
        {
            //arrange
            query.OrderBy("Name")
                .Asc()
                .IncludePaging()
                .SetCurrentPageTo(1)
                .WithPageSize(10);

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={order: {0: 'Name asc'}}&paging={Paging: {currentPage: 1, pageSize: 10}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithWhereAndOrderBy_ShouldCreateCorrectJson()
        {
            //arrange
            query
                .Where().And().Equals().Field("Name").Value("SomeValue")
                .OrderBy("Name").Asc();

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={where: {and: {eq: {'Name': 'SomeValue'}}},order: {0: 'Name asc'}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithIncludeFieldsAndOrderBy_ShouldCreateCorrectJson()
        {

            //arrange
            query.IncludeField("Name")
                .OrderBy("Name").Asc();

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={order: {0: 'Name asc'},fields: {'Name':'true'}}", json);

        }

        [Test]
        public void QueryFind_GivenQueryWithIncludeTwoFieldsAndTwoOrderBy_ShouldCreateCorrectJson()
        {

            //arrange
            query.IncludeField("Name")
                .IncludeField("Id")
                .OrderBy("Name").Asc()
                .OrderBy("Id").Desc();

            //action
            string json = query.AsJson();

            //assert
            Assert.AreEqual("filter={order: {0: 'Name asc',1: 'Id desc'},fields: {'Name':'true','Id':'true'}}", json);

        }

        [Test]
        public void QueryFind_GivenFullQuery_ShouldCreateCorrectJson()
        {
            //arrange
            query
                .Where().And().Equals().Field("Name").Value("Value")
                .IncludeField("Name")
                .IncludeField("Id")
                .IncludeRelationship("DeedsOffice")
                .IncludeRelationship("GeneralStatus")
                .Limit(5)
                .Skip(10)
                .OrderBy("Name").Asc()
                .OrderBy("Id").Desc();

            //action
            string json = query.AsJson();

            //assert
            var expectedJson = "filter={where: {and: {eq: {'Name': 'Value'}}},limit: 5,skip: 10,order: {0: 'Name asc',1: 'Id desc'}" +
                ",fields: {'Name':'true','Id':'true'},include: 'DeedsOffice,GeneralStatus'}";
            Assert.AreEqual(expectedJson, json);

        }


    }

}