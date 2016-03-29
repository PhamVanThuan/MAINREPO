using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Runtime.Remoting;
using Machine.Fakes.Sdk;
using Microsoft.SqlServer.Server;
using NSubstitute.Core.SequenceChecking;
using NUnit.Framework;
using Newtonsoft.Json;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Parsers;
using SAHL.Services.Query.Parsers.Elemets;
using StructureMap.Pipeline;

namespace SAHL.Services.Query.Server.Tests.Parser
{
    [TestFixture]
    public class JsonParserTests
    {

        private JsonQueryParser QueryParser { get; set; }

        [SetUp]
        public void StartUp()
        {
            QueryParser = new JsonQueryParser();
        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithLimit_ShouldReturnManyQueryWithLimitSet()
        {
            
            //arrange
            string filter = "{limit:10}";

            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findManyQuery.Limit);

        }


        [Test]
        public void FindManyQuery_GivenJsonStringWithNoSkip_ShouldReturnNullForSkip()
        {
            //arrange
            string filter = "";

            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNull(findManyQuery.Skip);

        }

        [TestCase("{skip:10}", 10, 100)]
        [TestCase("{limit:10, skip:10}", 10, 10)]
        public void FindManyQuery_GivenJsonStringWithSkip_ShouldReturnManyQueryWithSkipSet(string filter, int skip, int take)
        {

            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findManyQuery.Skip);
            Assert.AreEqual(skip, findManyQuery.Skip.Skip);
            Assert.AreEqual(take, findManyQuery.Skip.Take);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithNoExcludeFields_ShouldReturnManyQueryWithFieldsSetToEmptyList()
        {
            //arrange
            string filter = "";

            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(0, findManyQuery.Fields.Count);

        }

        [TestCase("{fields: {id: true}}", 1)]
        [TestCase("{fields: {id: true, make: true}}", 2)]
        [TestCase("{fields: {id: true, make: true, model: true}}", 3)]
        [TestCase("{fields: {id: true, make: true, model: false}}", 2)]
        [TestCase("{fields: {}}", 0)]
        public void FindManyQuery_GivenJsonStringWithExcludeFields_ShouldReturnManyQueryWithFieldsSet(string filter, int itemCount)
        {
            //arrange
            
            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(itemCount, findManyQuery.Fields.Count);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithExcludeFields_ShouldReturnManyQueryWithFieldsSetCorrectly()
        {
            //arrange
            string filter = "{fields: {id: true, make: true, model: false}}";
            
            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(2, findManyQuery.Fields.Count);
            Assert.AreEqual("id", findManyQuery.Fields[0]);
            Assert.AreEqual("make", findManyQuery.Fields[1]);
        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithNoOrderBy_ShouldReturnManyQueryWithOrderListSetToEmpty()
        {
            
            //arrange
            string filter = "";

            //action
            IFindQuery query = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(0, query.OrderBy.Count);

        }

        [TestCase(@"{order: {0: 'id desc'}}", 1, 0, "id desc")]
        [TestCase(@"{order: {0: 'id'}}", 1, 0, "id")]
        public void FindManyQuery_GivenJsonStringWithOrderBy_ShouldReturnManyQueryWithOrderListSet(string filter, int count, int sequence, string value)
        {

            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);
            
            //assert
            Assert.AreEqual(count, findManyQuery.OrderBy.Count);
            Assert.AreEqual(sequence, findManyQuery.OrderBy[0].Sequence);
            Assert.AreEqual(value, findManyQuery.OrderBy[0].Field);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithTwoOrderBy_ShouldReturnManyQueryWithOrderListSet()
        {

            //arrange
            string filter = @"{order: {0: 'id desc', 1: 'name asc'}}";

            //action
            IFindQuery findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(2, findManyQuery.OrderBy.Count);
            Assert.AreEqual(0, findManyQuery.OrderBy[0].Sequence);
            Assert.AreEqual("id desc", findManyQuery.OrderBy[0].Field);
            Assert.AreEqual(1, findManyQuery.OrderBy[1].Sequence);
            Assert.AreEqual("name asc", findManyQuery.OrderBy[1].Field);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithEmptyWhere_ShouldReturnWhereListSetToEmpty()
        {
            
            //arrange
            string filter = "";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(0, findManyQuery.Where.Count);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithSimpleWhere_ShouldReturnWhereListWithOneItem()
        {
            //arrange
            string filter = "{where: {id: 1}}";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("1", findManyQuery.Where[0].Where[0].Value);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithSimpleWhere_ShouldReturnWhereListWithOneItemAndDefaultsSet()
        {
            //arrange
            string filter = "{where: {id: 1}}";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("1", findManyQuery.Where[0].Where[0].Value);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("=", findManyQuery.Where[0].Where[0].Operator);

        }

        [TestCase("{where: {id: 1}}", "id", "1", "and", "=")]
        [TestCase("{where: {and: {id: 1}}}", "id", "1", "and", "=")]
        [TestCase("{where: {and: {eq: {id: 1}}}}", "id", "1", "and", "=")]
        [TestCase("{where: {and: {gt: {id: 1}}}}", "id", "1", "and", ">")]
        [TestCase("{where: {and: {lt: {count: 100}}}}", "count", "100", "and", "<")]
        public void FindManyQuery_GivenJsonStringWithClauseOperationWhere_ShouldReturnWhereListWithOneItemSet
                    (string filter, string key, string value, string clauseOperation, string operation)
        {
            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(key, findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual(value, findManyQuery.Where[0].Where[0].Value);
            Assert.AreEqual(clauseOperation, findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual(operation, findManyQuery.Where[0].Where[0].Operator);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithAndMultipleWhereClauses_ShouldReturnWhereListWithValuesSet()
        {

            //arrange 
            string filter = "{where: {and: {id: 1, name: 2}}}";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(2, findManyQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findManyQuery.Where[0].Where[1].Field);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[0].ClauseOperator);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithOrMultipleWhereClauses_ShouldReturnWhereListWithValuesSet()
        {

            //arrange 
            string filter = "{where: {or: {id: 1, name: 2}}}";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(2, findManyQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("or", findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findManyQuery.Where[0].Where[1].Field);
            Assert.AreEqual("or", findManyQuery.Where[0].Where[0].ClauseOperator);

        }

        [TestCase("{where: {and: {id: 1, name: 2}}}", 2, "and")]
        [TestCase("{where: {and: {id: 1, name: 2, isactive: true}}}", 3, "and")]
        [TestCase("{where: {or: {id: 1, name: 2}}}", 2, "or")]
        [TestCase("{where: {or: {id: 1, name: 2, isactive: true}}}", 3, "or")]
        public void FindManyQuery_GivenJsonStringWithMultipleWhereClauses_ShouldReturnWhereListWithCorrectCount(string filter, int count, string clauseOperator)
        {

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(count, findManyQuery.Where[0].Where.Count);

            foreach (var where in findManyQuery.Where[0].Where)
            {
                Assert.AreEqual(clauseOperator, where.ClauseOperator);
                Assert.AreEqual("=", where.Operator);
            }
            
        }

        [TestCase("{where: {and: {like: {name: '%test%'}}}}", "name", "%test%", "and", "like")]
        public void FindManyQuery_GivenJsonStringWithClauseOperationWhereThatContainSpecialCharacters_ShouldReturnWhereListWithOneItemAndDefaultsSet
                    (string filter, string key, string value, string clauseOperation, string operation)
        {
            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(1, findManyQuery.Where[0].Where.Count);
            Assert.AreEqual(key, findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual(value, findManyQuery.Where[0].Where[0].Value);
            Assert.AreEqual(clauseOperation, findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual(operation, findManyQuery.Where[0].Where[0].Operator);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithNoInclude_ShouldReturnNoIncludeFields()
        {
            //arrange
            string filter = "";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findQuery.Includes);
            Assert.AreEqual(0, findQuery.Includes.Count);

        }

        [TestCase("{include: 'id'}", 1)]
        [TestCase("{include: 'id, name'}", 2)]
        [TestCase("{include: 'id, name, status, lookup'}", 4)]
        public void FindManyQuery_GivenJsonStringWithIncludeRelationships_ShouldReturnListOfIncludeItemsWithCorrectCount(string filter, int count)
        {
            //arrange
            
            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findQuery.Includes);
            Assert.AreEqual(count, findQuery.Includes.Count);
        }
        
        [TestCase("{where: {like: {id: 1}}}", "like")]
        [TestCase("{where: {gt: {id: 1}}}", ">")]
        public void FindManyQuery_GivenJsonStringWithOperatorWhereClauseAndNoOperatorCaluse_ShouldIncludeWherePartsWhereAndIsDefault(string filter, string operatoin)
        {

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(1, findManyQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual(operatoin, findManyQuery.Where[0].Where[0].Operator);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithMultipleFieldsUsingSameOperator_ShouldIncludeMulitleWherePartsWithSameOperator()
        {

            //arrange
            string filter = "{where: {gt: {id: 1, count: 2 }}}";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(2, findManyQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual(">", findManyQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("count", findManyQuery.Where[0].Where[1].Field);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[1].ClauseOperator);
            Assert.AreEqual(">", findManyQuery.Where[0].Where[1].Operator);

        }
        
        [Test]
        public void FindManyQuery_GivenJsonStringWithIncludRelationships_ShouldReturnCorrectListOfInclude()
        {
            //arrange
            string filter = "{include: 'id, name'}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("id", findQuery.Includes[0]);
            Assert.AreEqual("name", findQuery.Includes[1]);

        }

        [TestCase(@"{""Order"":{""0"":""name asc""},""Limit"":2,""where"":{""like"":{""name"":""Salomie Stephen*""}}}", "Salomie Stephen%")]
        [TestCase(@"{""Order"":{""0"":""name asc""},""Limit"":2,""where"":{""like"":{""name"":""*Salomie Stephen*""}}}", "%Salomie Stephen%")]
        [TestCase(@"{""Order"":{""0"":""name asc""},""Limit"":2,""where"":{""like"":{""name"":""*Salomie*en*""}}}", "%Salomie%en%")]
        public void findManyQuery_GivenJsonWithComplexQueryParts_ShouldSetFindQueryObectCorrectly(string filter, string likeValue)
        {

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findQuery.Limit);
            Assert.AreEqual("like", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("and", findQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findQuery.Where[0].Where[0].Field);
            Assert.AreEqual(likeValue, findQuery.Where[0].Where[0].Value);
            Assert.AreEqual(1, findQuery.OrderBy.Count);
            
        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithSimpleeWhereClausesAndElementsInQuotes_ShouldReturnWhereListWithValuesSet()
        {

            //arrange 
            string filter = @"{""where"":{""id"":""1""}}";

            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(1, findManyQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual("and", findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("1", findManyQuery.Where[0].Where[0].Value);

        }

        [TestCase(@"{""Order"":{""0"":""name asc""},""Limit"":2,""where"":{""like"":{""name"":""Salomie Stephen%""}}}")]
        [TestCase(@"{""order"":{""0"":""name asc""},""limit"":2,""Where"":{""like"":{""name"":""Salomie Stephen%""}}}")]
        [TestCase(@"{""oRder"":{""0"":""name asc""},""lImit"":2,""Where"":{""like"":{""name"":""Salomie Stephen%""}}}")]
        [TestCase(@"{""oRder"":{""0"":""name asc""},""lImit"":2,""Where"":{""Like"":{""name"":""Salomie Stephen%""}}}")]
        public void findManyQuery_GivenJsonWithComplexQueryPartsInMixedCase_ShouldSetFindQueryObectCorrectly(string filter)
        {
            //arrange
            
            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(2, findQuery.Limit.Count);
            Assert.AreEqual("like", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("and", findQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findQuery.Where[0].Where[0].Field);
            Assert.AreEqual("Salomie Stephen%", findQuery.Where[0].Where[0].Value);
            Assert.AreEqual(1, findQuery.OrderBy.Count);

        }

        [Test]
        public void findManyQuery_GivenJsonWithStartsWithOperator_ShouldSetFindQueryObectToWhereWithLikeAndBeginningWithcard()
        {
            //arrange
            string filter = @"{where:{startswith:{name:""Salomie""}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("like", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("and", findQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findQuery.Where[0].Where[0].Field);
            Assert.AreEqual("Salomie%", findQuery.Where[0].Where[0].Value);
        }

        [Test]
        public void findManyQuery_GivenJsonWithEndsWithOperator_ShouldSetFindQueryObectToWhereWithLikeAndEndingWildcard()
        {
            //arrange
            string filter = @"{where:{endswith:{name:""Salomie""}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("like", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("and", findQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findQuery.Where[0].Where[0].Field);
            Assert.AreEqual("%Salomie", findQuery.Where[0].Where[0].Value);
        }
        
        [Test]
        public void findManyQuery_GivenJsonWithContainsOperator_ShouldSetFindQueryObectToWhereWithLikeAndTwoWildcards()
        {
            //arrange
            string filter = @"{where:{contains:{name:""Salomie""}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("like", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("and", findQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual("name", findQuery.Where[0].Where[0].Field);
            Assert.AreEqual("%Salomie%", findQuery.Where[0].Where[0].Value);
        }

        [TestCase("{where: {in: {id: '1,2,3'}}}")]
        [TestCase("{where: {In: {id: '1,2,3'}}}")]
        public void FindManyQuery_GivenJsonWithWhereThatContainsIn_ShouldSetWherePartCorrectly(string filter)
        {

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("in", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("1,2,3", findQuery.Where[0].Where[0].Value);
            Assert.AreEqual("id", findQuery.Where[0].Where[0].Field);

        }

        [TestCase("{where: {between: {id: '1,3'}}}")]
        [TestCase("{where: {Between: {id: '1,3'}}}")]
        public void FindManyQuery_GivenJsonWithWhereThatContainsBetween_ShouldSetWherePartCorrectly(string filter)
        {

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("between", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("1,3", findQuery.Where[0].Where[0].Value);
            Assert.AreEqual("id", findQuery.Where[0].Where[0].Field);

        }

        [TestCase("{where: {inq: {id: '1,2,3'}}}")]
        [TestCase("{where: {Inq: {id: '1,2,3'}}}")]
        public void FindManyQuery_GivenJsonWithWhereThatContainsNotIn_ShouldSetWherePartCorrectly(string filter)
        {

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual("not in", findQuery.Where[0].Where[0].Operator);
            Assert.AreEqual("1,2,3", findQuery.Where[0].Where[0].Value);
            Assert.AreEqual("id", findQuery.Where[0].Where[0].Field);

        }

        [Test]
        public void FindManyQuery_GivenJsonWithNoLimit_ShouldHaveLimitSetToOneHundred()
        {
            
            //arrange
            string filter = @"{where:{contains:{name:""Salomie""}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findQuery.Limit);
            Assert.AreEqual(100, findQuery.Limit.Count);

        }

        [Test]
        public void FindManyQuery_GivenJsonWithLimitSetAndLimitGreaterThanOneHundred_ShouldHaveLimitSetToOneHundred()
        {

            //arrange
            string filter = @"{limit: 120, where:{contains:{name:""Salomie""}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findQuery.Limit);
            Assert.AreEqual(100, findQuery.Limit.Count);

        }

        [Ignore]
        [TestCase("{where: {and: {count: {eq: 100}}}}", "count", "100", "and", "=")]
        public void FindManyQuery_GivenJsonStringWhereForConnector_ShouldReturnWhereListWithOneItemSet
                    (string filter, string key, string value, string clauseOperation, string operation)
        {
            //action
            var findManyQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(key, findManyQuery.Where[0].Where[0].Field);
            Assert.AreEqual(value, findManyQuery.Where[0].Where[0].Value);
            Assert.AreEqual(clauseOperation, findManyQuery.Where[0].Where[0].ClauseOperator);
            Assert.AreEqual(operation, findManyQuery.Where[0].Where[0].Operator);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithTwoWhereClauseItemsOnTheSameField_ShouldHaveWhereClausePartsWithTheDuplicateFieldRenamedSoThatParamaterNameHasNumber()
        {

            //arrange
            string filter = @"{where:{and:{lt:{id:'1'}, gt:{id:'2'}}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(2, findQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findQuery.Where[0].Where[0].ParameterName);
            Assert.AreEqual("id2", findQuery.Where[0].Where[1].ParameterName);

        }
        
        [Test]
        public void FindManyQuery_GivenJsonStringWithTwoWhereClauseItemsOnTheSameFieldAndOneOnAnotherField_ShouldHaveWhereClausePartsWithTheDuplicateFieldRenamedSoThatParamaterNameHasNumber()
        {

            //arrange
            string filter = @"{where:{and:{lt:{id:'1'}, gt:{id:'2', key:'3'}}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(3, findQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findQuery.Where[0].Where[0].ParameterName);
            Assert.AreEqual("id2", findQuery.Where[0].Where[1].ParameterName);
        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithThreeWhereClauseItemsOnTheSameField_ShouldHaveWhereClausePartsWithTheDuplicateFieldRenamedSoThatParamaterNameHasNumber()
        {

            //arrange
            string filter = @"{where:{and:{lt:{id:'1'}, gt:{id:'2'}, inq:{id: '5,6,7'}}}}";

            //action
            IFindQuery findQuery = QueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(3, findQuery.Where[0].Where.Count);
            Assert.AreEqual("id", findQuery.Where[0].Where[0].ParameterName);
            Assert.AreEqual("id2", findQuery.Where[0].Where[1].ParameterName);
            Assert.AreEqual("id3", findQuery.Where[0].Where[2].ParameterName);
        }
        
    }

}