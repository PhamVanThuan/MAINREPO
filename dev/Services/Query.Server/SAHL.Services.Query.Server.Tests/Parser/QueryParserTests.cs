using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Builders.Core;
using SAHL.Services.Query.Parsers;
using SAHL.Services.Query.Parsers.Elemets;

namespace SAHL.Services.Query.Server.Tests.Parser
{
    
    [TestFixture]
    public class QueryParserTests
    {

        private QueryStringQueryParser stringQueryParser;

        [SetUp]
        public void SetupTests()
        {
            stringQueryParser = new QueryStringQueryParser();
        }

        [Test]
        public void GetWhere_GivenEmptyFilterCollection_ShouldReturnEmptyWhereItemList()
        {
            
            //arrange
            NameValueCollection filter = new NameValueCollection();

            //action
            var whereItems = stringQueryParser.GetWhereParts(filter);

            //assert
            Assert.AreEqual(0, whereItems.Count);

        }

        [Test]
        public void GetWhere_GivenSingleFilterItemCollection_ShouldReturnSingleWhereItemList()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[where][value]", "200");

            //action
            var whereItems = stringQueryParser.GetWhereParts(filter);

            //assert
            Assert.AreEqual(1, whereItems.Count);
            Assert.AreEqual("value", whereItems[0].Field);
            Assert.AreEqual("200", whereItems[0].Value);
            
        }

        [Test]
        public void GetWhere_GivenTwoFilterItemCollection_ShouldReturnTwoWhereItemList()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[where][value]", "200");
            filter.Add("filter[where][value2]", "100");

            //action
            var whereItems = stringQueryParser.GetWhereParts(filter);

            //assert
            Assert.AreEqual(2, whereItems.Count);
            Assert.AreEqual("value", whereItems[0].Field);
            Assert.AreEqual("200", whereItems[0].Value);

            Assert.AreEqual("value2", whereItems[1].Field);
            Assert.AreEqual("100", whereItems[1].Value);

        }

        [Test]
        public void GetColumnName_GivenValidSimpleWhereFilter_ShouldReturnColumnName()
        {

            //arrange
            string filter = "filter[where][column]";

            //action
            string column = stringQueryParser.GetColumnName(filter);

            //assert
            Assert.AreEqual("column", column);

        }

        [Test]
        public void GetLimit_GivenEmptyFilterCollection_ShouldReturnEmptyLimitPart()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();

            //action
            var limitPart = stringQueryParser.GetLimitPart(filter);

            //assert
            Assert.IsNull(limitPart);
            
        }
        
        [Test]
        public void GetLimit_GivenSingleLimitInFilter_ShouldReturnLimitItem()
        {
            
            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[limit]", "10");

            //action
            var limitPart = stringQueryParser.GetLimitPart(filter);
            
            //assert
            Assert.AreEqual(10, limitPart.Count);

        }

        [TestCase("filter[where][column][eq]", "=")]
        [TestCase("filter[where][column][lt]", "<")]
        [TestCase("filter[where][column][lte]", "<=")]
        [TestCase("filter[where][column][gt]", ">")]
        [TestCase("filter[where][column][gte]", ">=")]
        [TestCase("filter[where][column][like]", "like")]
        [TestCase("filter[where][column][in]", "in")]
        [TestCase("filter[where][column][inq]", "not in")]
        public void GetOperator_GivenFilterWithOperator_ShouldReturnOperator(string key, string validOp)
        {
            //arrange

            //action
            string op = stringQueryParser.GetOperator(key);

            //assert
            Assert.AreEqual(validOp, op);

        }

        [Test]
        public void GetOperator_GivenFilterWithNoOperator_ShouldReturnEqOperator()
        {
            //arrange
            string key = "filter[where][column]";

            //action
            string op = stringQueryParser.GetOperator(key);

            //assert
            Assert.AreEqual("=", op);

        }

        [Test]
        public void GetOperator_GivenFilterWithOperationNotSupported_ShouldReturnDefaultEqOperator()
        {
            //arrange
            string key = "filter[where][column][nop]";

            //action
            string op = stringQueryParser.GetOperator(key);

            //assert
            Assert.AreEqual("=", op);
        }

        [Test]
        public void GetColumnName_GivenValidFilerWithColumnAndOperator_ShouldReturnColumnName()
        {

            //arrange
            string filter = "filter[where][column][eq]";

            //action
            string column = stringQueryParser.GetColumnName(filter);

            //assert
            Assert.AreEqual("column", column);

        }

        [Test]
        public void GetWhere_GivenSingleFilterItemCollectionWithOperator_ShouldReturnSingleWhereItemList()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[where][value][eq]", "200");

            //action
            var whereItems = stringQueryParser.GetWhereParts(filter);

            //assert
            Assert.AreEqual(1, whereItems.Count);
            Assert.AreEqual("value", whereItems[0].Field);
            Assert.AreEqual("200", whereItems[0].Value);
            Assert.AreEqual("=", whereItems[0].Operator);

        }

        [TestCase("filter[where][and][column][op]", "and")]
        [TestCase("filter[where][or][column][op]", "or")]
        public void GetClauseOperation_GivenWhereFilterWithClauseOperation_ShouldReturnClauseOperation(string key, string validClauseOp)
        {

            //arrange

            //action
            var clauseOp = stringQueryParser.GetClauseOperation(key);

            //assert
            Assert.AreEqual(validClauseOp, clauseOp);

        }

        [Test]
        public void GetClauseOperation_GivenFilterWithNoClauseOperation_ShouldReturnDefaultClauseOperation_And()
        {
            
            //arrange
            string filter = "filter[where][column][op]";

            //action
            string clauseOperation = stringQueryParser.GetClauseOperation(filter);

            //assert
            Assert.AreEqual("and", clauseOperation);

        }

        [TestCase("filter[where][column]", "200", "and", "column", "=")]
        [TestCase("filter[where][column][eq]", "200", "and", "column", "=")]
        [TestCase("filter[where][column][lt]", "200", "and", "column", "<")]
        [TestCase("filter[where][column][gt]", "200", "and", "column", ">")]
        [TestCase("filter[where][and][column][gt]", "200", "and", "column", ">")]
        [TestCase("filter[where][or][column][gt]", "200", "or", "column", ">")]
        [TestCase("filter[where][or][column]", "200", "or", "column", "=")]
        [TestCase("filter[where][and][column][like]", "%attorney%", "and", "column", "like")]
        [TestCase("filter[where][or][column][like]", "%attorney%", "or", "column", "like")]
        public void GetWhere_GivenVariousWhereFilterOptions_ShouldReturnValidFieldValueAndOperations(string whereFilter, string value
            , string validClauseOperartor, string validField, string validOperation)
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add(whereFilter, value);

            //action
            var whereItems = stringQueryParser.GetWhereParts(filter);

            //assert
            Assert.AreEqual(1, whereItems.Count);
            Assert.AreEqual(validClauseOperartor, whereItems[0].ClauseOperator);
            Assert.AreEqual(validField, whereItems[0].Field);
            Assert.AreEqual(validOperation, whereItems[0].Operator);
            Assert.AreEqual(value, whereItems[0].Value);

        }

        [TestCase("filter[where][column]", "200", "and", "column", "=", "filter[limit]", 5)]
        [TestCase("filter[where][or][column][gt]", "200", "or", "column", ">", "filter[limit]", 6)]
        public void GetWhere_GivenVariousWhereAndLimitFilterOptions_ShouldReturnValidFieldValueAndOperations(string whereFilter
            , string value, string validClauseOperartor, string validField, string validOperation, string limitFilter, int limit)
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add(whereFilter, value);
            filter.Add(limitFilter, limit.ToString());

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);
            
            //assert
            Assert.AreEqual(1, findManyQuery.Where.Count);
            Assert.AreEqual(validClauseOperartor, findManyQuery.Where[0].ClauseOperator);
            Assert.AreEqual(validField, findManyQuery.Where[0].Field);
            Assert.AreEqual(validOperation, findManyQuery.Where[0].Operator);
            Assert.AreEqual(value, findManyQuery.Where[0].Value);
            Assert.NotNull(findManyQuery.Limit);
            Assert.AreEqual(limit, findManyQuery.Limit.Count);

        }

        [Test]
        public void GetIncludeField_GivenFilterFieldInput_ShouldReturnTheField()
        {
            
            //arrange

            //action
            string field = stringQueryParser.GetIncludeField("filter[fields][column]");

            //assert
            Assert.AreEqual("column", field);

        }

        [TestCase("filter[where][column]", "200")]
        [TestCase("filter[limit]", "10")]
        [TestCase("filter[order]", "column desc")]
        [TestCase("filter[include]", "include")]
        [TestCase("filter[skip]", "20")]
        public void FindManyQuery_GivenFilterWithNoFieldsOption_ShouldReturnEmptyList(string filterInput, string value)
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add(filterInput, value);

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(0, findManyQuery.Fields.Count);

        }

        [Test]
        public void FindManyQuery_GivenFilterWithSingleFieldToIncludeOption_ShouldReturnSingleFieldItem()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[fields][column]", "true");

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Fields.Count);
            Assert.AreEqual("column", findManyQuery.Fields[0]);

        }
        
        [Test]
        public void FindManyQuery_GivenFilterWithSingleFieldToNotIncludeOption_ShouldReturnNoFieldItems()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[fields][column]", "false");

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(0, findManyQuery.Fields.Count);

        }
        
        [Test]
        public void FindManyQuery_GivenFilterWithTwoFieldsToIncludeOption_ShouldReturnTwoFieldItems()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[fields][column1]", "true");
            filter.Add("filter[fields][column2]", "true");

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(2, findManyQuery.Fields.Count);
            Assert.AreEqual("column1", findManyQuery.Fields[0]);
            Assert.AreEqual("column2", findManyQuery.Fields[1]);

        }
        
        [Test]
        public void FindManyQuery_GivenFilterWithTwoFieldsOneToIncludeOneToExludeOption_ShouldReturnOneFieldItem()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[fields][column1]", "true");
            filter.Add("filter[fields][column2]", "false");

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.Fields.Count);
            Assert.AreEqual("column1", findManyQuery.Fields[0]);
            
        }

        [TestCase("filter[order]", "")]
        [TestCase("filter[order][0]", "0")]
        public void GetOrderField_GivenFilterOrderInput_ShouldReturnTheOrder(string filterInput, string field)
        {

            //arrange

            //action
            string returnedField = stringQueryParser.GetOrderField(filterInput);

            //assert
            Assert.AreEqual(field, returnedField);

        }

        [TestCase("filter[order]", "Colunm Desc")]
        [TestCase("filter[order][0]", "Colunm Desc")]
        public void FindManyQuery_GivenFilterWithSingleOrder_ShouldReturnOneOrderByItem(string filterInput, string value)
        {
            
            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add(filterInput, value);

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(1, findManyQuery.OrderBy.Count);
            Assert.AreEqual(value, findManyQuery.OrderBy[0].Field);
            Assert.AreEqual(0, findManyQuery.OrderBy[0].Sequence);

        }

        [Test]
        public void FindManyQuery_GivenFilterWithTwoOrders_ShouldReturnTwoOrderByItems()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add("filter[order][0]", "Colunm Desc");
            filter.Add("filter[order][1]", "Colunm1 Desc");

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.AreEqual(2, findManyQuery.OrderBy.Count);
            Assert.AreEqual("Colunm Desc", findManyQuery.OrderBy[0].Field);
            Assert.AreEqual(0, findManyQuery.OrderBy[0].Sequence);
            Assert.AreEqual("Colunm1 Desc", findManyQuery.OrderBy[1].Field);
            Assert.AreEqual(1, findManyQuery.OrderBy[1].Sequence);

        }

        [Test]
        public void GetSkipPart_GivenNoSkipFilter_ShouldReturnNull()
        {

            //arrange
            string filterInput = "filter[order]";
            NameValueCollection filter = new NameValueCollection();
            filter.Add(filterInput, "20");

            //action
            var skipPart = stringQueryParser.GetSkipPart(filter, null);

            //assert
            Assert.IsNull(skipPart);

        }

        [Test]
        public void GetSkipPart_GivenVaildSkipFilterWithNoLimit_ShouldReturnSkipPart()
        {
            
            //arrange
            string filterInput = "filter[skip]";
            NameValueCollection filter = new NameValueCollection();
            filter.Add(filterInput, "20");

            //action
            var skipPart = stringQueryParser.GetSkipPart(filter, null);

            //assert
            Assert.IsNotNull(skipPart);
            Assert.AreEqual(100, skipPart.Take);
            Assert.AreEqual(20, skipPart.Skip);

        }
        
        [Test]
        public void GetSkipPart_GivenVaildSkipFilterWithLimit_ShouldReturnSkipPart()
        {

            //arrange
            string filterInput = "filter[skip]";
            NameValueCollection filter = new NameValueCollection();
            filter.Add(filterInput, "20");

            ILimitPart limitPart = new LimitPart()
            {
                Count = 10
            };

            //action
            var skipPart = stringQueryParser.GetSkipPart(filter, limitPart);

            //assert
            Assert.IsNotNull(skipPart);
            Assert.AreEqual(10, skipPart.Take);
            Assert.AreEqual(20, skipPart.Skip);

        }

        [TestCase("filter[skip]", 20, "", 100)]
        [TestCase("filter[skip]", 20, "filter[limit]", 20)]
        public void FindManyQuery_GivenFilterWithSkip_ShouldReturnSkip(string filterInput, int value, string filterLimit, int take)
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            if (filterLimit.Length > 0)
            {
                filter.Add(filterLimit, take.ToString());
            }
            filter.Add(filterInput, value.ToString());
            

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.NotNull(findManyQuery.Skip);
            Assert.AreEqual(value, findManyQuery.Skip.Skip);
            Assert.AreEqual(take, findManyQuery.Skip.Take);

        }

        [Test]
        public void FindManyQuery_GivenFilterWithNoIncludes_ShouldReturnEmptyIncludesCollection()
        {
            
            //arrange
            NameValueCollection filter = new NameValueCollection();

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findManyQuery.Includes);
            Assert.AreEqual(0, findManyQuery.Includes.Count);

        }

        [TestCase("filter[include]", "rel1", 1)]
        [TestCase("filter[include]", "rel1,rel2", 2)]
        [TestCase("filter[include]", "rel1,rel2,rel3", 3)]
        public void FindManyQuery_GivenFilterWithInclude_ShouldReturnTheCorrectIncludedFieldsCount(string filterHeader, string fields, int count)
        {

            //arrange
            NameValueCollection filter = new NameValueCollection();
            filter.Add(filterHeader, fields);

            //action
            var findManyQuery = stringQueryParser.FindManyQuery(filter);

            //assert
            Assert.IsNotNull(findManyQuery.Includes);
            Assert.AreEqual(count, findManyQuery.Includes.Count);
        }


    }

}