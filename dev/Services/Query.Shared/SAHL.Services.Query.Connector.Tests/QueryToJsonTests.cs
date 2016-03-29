using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Query.Connector.Core;


namespace SAHL.Services.Query.Connector.Tests
{
    public class QueryToJsonTests
    {

        private QueryToJson queryToJson;

        [SetUp]
        public void SetupTests()
        {
            queryToJson = new QueryToJson();
        }

        [Test]
        public void IncludeSkip_GivenEmptySkipPart_ShouldReturnEmptyString()
        {
            //arrange
            ISkipPart skipPart = null;

            //action
            var json = queryToJson.IncludeSkipJson(skipPart, false);

            //assert
            Assert.AreEqual("", json);

        }

        [Test]
        public void IncludeSkip_GivenSkipPartWithCount5_ShouldReturnJsonWithCount5()
        {
            //arrange
            ISkipPart skipPart = new SkipPart()
            {
                SkipCount = 5
            };
           
            //action
            var json = queryToJson.IncludeSkipJson(skipPart, false);

            //assert
            Assert.AreEqual("skip: 5",json);

        }

        [Test]
        public void IncludeLimit_GivenEmptyLimitPart_ShouldReturnEmptyString()
        {
            //arrange
            ILimitPart limitPart = null;

            //action
            var json = queryToJson.IncludeLimitJson(limitPart, false);

            //assert
            Assert.AreEqual("", json);

        }

        [Test]
        public void IncludeLimit_GivenLimitPartWithCount5_ShouldReturnJsonWithCount5()
        {
            //arrange
            ILimitPart limitPart = new LimitPart()
            {
                Count = 5
            };
           
            //action
            var json = queryToJson.IncludeLimitJson(limitPart, false);

            //assert
            Assert.AreEqual("limit: 5",json);

        }

        [Test]
        public void IncludeOrderBy_GivenEmptyOrderBy_ShouldReturnEmptyString()
        {
            //arrange
            List<IOrderPart> orderParts = new List<IOrderPart>();

            //action
            var json = queryToJson.IncludeOrderByJson(orderParts, false);

            //assert
            Assert.AreEqual("", json);

        }

        [Test]
        public void IncludeOrderBy_GivenSingleOrderByPart_ShouldReturnCorrectJson()
        {
            //arrange
            List<IOrderPart> orderParts = new List<IOrderPart>();
            orderParts.Add(CreateOrderPart("Name"));

            //action
            var json = queryToJson.IncludeOrderByJson(orderParts, false);

            //assert
            Assert.AreEqual("order: {0: 'Name asc'}", json);

        }

        [Test]
        public void IncludeOrderBy_GivenTwoOrderByParts_ShouldReturnCorrectJson()
        {
            //arrange
            List<IOrderPart> orderParts = new List<IOrderPart>();
            orderParts.Add(CreateOrderPart("Name"));
            orderParts.Add(CreateOrderPart("Id"));

            //action
            var json = queryToJson.IncludeOrderByJson(orderParts, false);

            //assert
            Assert.AreEqual("order: {0: 'Name asc',1: 'Id asc'}", json);

        }

        [Test]
        public void IncludeRelationships_GivenEmptyRelationship_ShouldReturnEmptyString()
        {
            //arrange
            List<string> relationships = new List<string>();

            //action
            var json = queryToJson.IncludeRelatedItemsJson(relationships, false);

            //assert
            Assert.AreEqual("", json);
        }

        [Test]
        public void IncludeRelatedItems_GivenSingleRelatedItem_ShouldReturnCorrectJson()
        {
            //arrange
            List<string> relationships = new List<string>() { "Name" };

            //action
            var json = queryToJson.IncludeRelatedItemsJson(relationships, false);

            //assert
            Assert.AreEqual("include: 'Name'", json);

        }

        [Test]
        public void IncludeRelatedItems_GivenTwoRelatedItem_ShouldReturnCorrectJson()
        {
            //arrange
            List<string> relationships = new List<string>() { "Name", "Id" };

            //action
            var json = queryToJson.IncludeRelatedItemsJson(relationships, false);

            //assert
            Assert.AreEqual("include: 'Name,Id'", json);

        }

        [Test]
        public void IncludeFields_GivenEmptyFields_ShouldReturnEmptyString()
        {
            //arrange
            List<string> fields = new List<string>();

            //action
            var json = queryToJson.IncludeFieldsJson(fields, false);

            //assert
            Assert.AreEqual("", json);

        }

        [Test]
        public void IncludeFields_GivenSingleFields_ShouldReturnCorrectJson()
        {
            //arrange
            List<string> fields = new List<string>() { "Name" };

            //action
            var json = queryToJson.IncludeFieldsJson(fields, false);

            //assert
            Assert.AreEqual("fields: {'Name':'true'}", json);

        }
       
        [Test]
        public void IncludeFields_GivenTwoFields_ShouldReturnCorrectJson()
        {
            //arrange
            List<string> fields = new List<string>() { "Name", "Id" };

            //action
            var json = queryToJson.IncludeFieldsJson(fields, false);

            //assert
            Assert.AreEqual("fields: {'Name':'true','Id':'true'}", json);

        }

        [Test]
        public void IncludeWhere_GivenEmptyWhere_ShouldReturnEmptyString()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();

            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("", json);

        }

        [Test]
        public void IncludeWhere_GivenSingleWhere_ShouldReturnCorrectJson()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();
            wheres.Add(CreateWherePartsWithAndEquals("Name", "SomeName"));
            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("where: {and: {eq: {'Name': 'SomeName'}}}", json);

        }

        [Test]
        public void IncludePagin_GivenEmptyPagingPart_ShouldReturnEmptyString()
        {
            
            //arrange
            IPagingPart pagingPart = null;

            //action
            var json = queryToJson.IncludePagingJson(pagingPart);

            //assert
            Assert.AreEqual("", json);

        }

        [Test]
        public void IncludePaging_GivenPagingPart_ShouldReturnPagingJson()
        {

            //arrange
            IPagingPart pagingPart = CreatePagingPart();

            //action
            var json = queryToJson.IncludePagingJson(pagingPart);

            //assert
            Assert.AreEqual("paging={Paging: {currentPage: 1, pageSize: 10}}", json);

        }

        [Test]
        public void WrapAsFilter_GivenValidJsonForFilter_ShouldReturnCorrectlyWrappedJson()
        {
            
            //arrange
            string validJson = "limit: 10";

            //action
            string json = queryToJson.WrapAsFilter(validJson);

            //assert
            Assert.AreEqual("filter={limit: 10}", json);

        }

        [Test]
        public void PrepareQuery_GivenValidFilterAndPaging_ShouldReturnValidQueryString()
        {
            //arrange
            string validJson = "filter={limit: 10}";
            string validPaging = "paging={Paging: {currentPage: 1, pageSize: 10}}";

            //action
            string json = queryToJson.PrepareQuery(validJson, validPaging);

            //assert
            Assert.AreEqual(@"filter={limit: 10}&paging={Paging: {currentPage: 1, pageSize: 10}}", json);

        }

        [Test]
        public void PrepareQuery_GivenValidFilterAndNoPaging_ShouldReturnValidQueryString()
        {
            //arrange
            string validJson = "filter={limit: 10}";
            string validPaging = "";

            //action
            string json = queryToJson.PrepareQuery(validJson, validPaging);

            //assert
            Assert.AreEqual(@"filter={limit: 10}", json);

        }

        [Test]
        public void PrepareQuery_GivenValidNoFilterAndPaging_ShouldReturnValidQueryString()
        {
            //arrange
            string validJson = "";
            string validPaging = "paging={Paging: {currentPage: 1, pageSize: 10}}";

            //action
            string json = queryToJson.PrepareQuery(validJson, validPaging);

            //assert
            Assert.AreEqual(@"paging={Paging: {currentPage: 1, pageSize: 10}}", json);

        }

        [Test]
        public void ShouldIncludeSeperator_GivenEmptyString_ShouldFalse()
        {
            //arrange
            string someJson = "";

            //action
            bool shouldInclude = QueryToJson.ShouldIncludeSeperator(someJson); 

            //assert
            Assert.IsFalse(shouldInclude);

        }

        [Test]
        public void ShouldIncludeSeperator_GivenNonEmptyString_ShouldTrue()
        {
            //arrange
            string someJson = "limit: 10";

            //action
            bool shouldInclude = QueryToJson.ShouldIncludeSeperator(someJson); 

            //assert
            Assert.IsTrue(shouldInclude);

        }

        [Test]
        public void IncludeLimitJson_GivenExistingJsonWhenCreatingLimitPart_ShouldJsonWithPreceedingComma()
        {
            //arrange
            string someJson = "skip: 10";
            ILimitPart limitPart = new LimitPart() {Count = 10};

            //action
            string json = queryToJson.IncludeLimitJson(limitPart, QueryToJson.ShouldIncludeSeperator(someJson));

            //assert
            Assert.AreEqual(",limit: 10", json);

        }

        [Test]
        public void IncludeWhere_GivenTwoWhereClauseItemsWithTheSameOperator_ShouldReturnCorrectJson()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();
            wheres.Add(CreateWherePartsWithAndEquals("Name", "SomeName"));
            wheres.Add(CreateWherePartsWithAndEquals("Description", "SomeDescription"));
            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("where: {and: {eq: {'Name': 'SomeName','Description': 'SomeDescription'}}}", json);

        }

        [Test]
        public void IncludeWhere_GivenTwoWhereClauseItemsWithTheDifferentOperator_ShouldReturnCorrectJson()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();
            wheres.Add(CreateWherePartsWithAndEquals("Name", "SomeName"));
            wheres.Add(CreateWherePartsWithAndLike("Description", "SomeDescription"));
            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("where: {and: {eq: {'Name': 'SomeName'},like: {'Description': 'SomeDescription'}}}", json);

        }
        
        [Test]
        public void IncludeWhere_GivenTwoWhereClauseOrItemsWithTheSameOperator_ShouldReturnCorrectJson()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();
            wheres.Add(CreateWherePartsWithOrEquals("Name", "SomeName"));
            wheres.Add(CreateWherePartsWithOrEquals("Description", "SomeDescription"));
            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("where: {or: {eq: {'Name': 'SomeName','Description': 'SomeDescription'}}}", json);
        }

        [Test]
        public void IncludeWhere_GivenTwoWhereClauseOrItemsWithTheDifferentOperator_ShouldReturnCorrectJson()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();
            wheres.Add(CreateWherePartsWithOrEquals("Name", "SomeName"));
            wheres.Add(CreateWherePartsWithOrLike("Description", "SomeDescription"));
            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("where: {or: {eq: {'Name': 'SomeName'},like: {'Description': 'SomeDescription'}}}", json);

        }

        [Test]
        public void IncludeWhere_GivenThreeWhereClauseItemsWithTheDifferentOperator_ShouldReturnCorrectJson()
        {
            //arrange
            List<IWhereClauseOperatorPart> wheres = new List<IWhereClauseOperatorPart>();
            wheres.Add(CreateWherePartsWithAndEquals("Name", "SomeName"));
            wheres.Add(CreateWherePartsWithAndEquals("Name", "AnotherName"));
            wheres.Add(CreateWherePartsWithAndLike("Description", "SomeDescription"));
            //action
            var json = queryToJson.IncludeWhereJson(wheres, false);

            //assert
            Assert.AreEqual("where: {and: {eq: {'Name': 'SomeName','Name': 'AnotherName'},like: {'Description': 'SomeDescription'}}}", json);

        }
        
        
        private IPagingPart CreatePagingPart()
        {
            IPagingPart pagingPart = new PagingPart(CreateQuery());
            pagingPart.SetCurrentPageTo(1).WithPageSize(10);
            return pagingPart;
        }

        private IWhereClauseOperatorPart CreateWherePartsWithAndEquals(string field, string value)
        {
            IWhereClauseOperatorPart whereClause = new WhereClauseOperatorPart(CreateQuery());
            whereClause.And().Equals().Field(field).Value(value);
            return whereClause;
        }
        
        private IWhereClauseOperatorPart CreateWherePartsWithAndLike(string field, string value)
        {
            IWhereClauseOperatorPart whereClause = new WhereClauseOperatorPart(CreateQuery());
            whereClause.And().Like().Field(field).Value(value);
            return whereClause;
        }

        private IWhereClauseOperatorPart CreateWherePartsWithOrEquals(string field, string value)
        {
            IWhereClauseOperatorPart whereClause = new WhereClauseOperatorPart(CreateQuery());
            whereClause.Or().Equals().Field(field).Value(value);
            return whereClause;
        }
        
        private IWhereClauseOperatorPart CreateWherePartsWithOrLike(string field, string value)
        {
            IWhereClauseOperatorPart whereClause = new WhereClauseOperatorPart(CreateQuery());
            whereClause.Or().Like().Field(field).Value(value);
            return whereClause;
        }
        
        private IOrderPart CreateOrderPart(string column)
        {
            IOrderPart orderPart = new OrderPart(CreateQuery(), column) {};
            orderPart.Asc();
            return orderPart;
        }

        private IQuery CreateQuery()
        {
            return Substitute.For<IQuery>();
        }

    }
}
