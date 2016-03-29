using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using Machine.Fakes;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Parsers;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Server.Tests.Parser;
using SAHL.Services.Query.Validators;

namespace SAHL.Services.Query.Server.Tests.Factory
{
    [TestFixture]
    public class QueryFactoryTests
    {

        private QueryFactory QueryFactory { get; set; }
        private IQueryStringQueryParser QueryStringQueryParser { get; set; }
        private IJsonQueryParser JsonQueryParser { get; set; }
        private IPagingParser PagingParser { get; set; }
        private IFindQueryValidator FindQueryValidator;

        [SetUp]
        public void Setup()
        {
            QueryStringQueryParser = Substitute.For<IQueryStringQueryParser>();
            JsonQueryParser = Substitute.For<IJsonQueryParser>();
            PagingParser = Substitute.For<IPagingParser>();
            FindQueryValidator = new FindQueryValidator();

            QueryFactory = new QueryFactory(FindQueryValidator, QueryStringQueryParser, JsonQueryParser, PagingParser);
        }

        [Test]
        public void CreateFindManyQuery_GivenEmptyFilterNamedValueCollection_ShouldReturnDefaultEmptyQuery()
        {
            
            //arrange
            NameValueCollection filter = new NameValueCollection();

            //action
            var query = QueryFactory.CreateFindManyQuery(filter);

            //assert
            Assert.IsNotNull(query);
            Assert.AreEqual(0, query.Fields.Count);
            Assert.AreEqual(0, query.Where.Count);
            Assert.AreEqual(0, query.OrderBy.Count);
            Assert.IsNotNull(query.Limit);
            Assert.AreEqual(100, query.Limit.Count);
            Assert.IsNull(query.Skip);

        }

        [Test]
        public void CreateFindManyQuery_GivenSimpleJsonFilter_ShouldCallJsonParserFindManyQuery()
        {
            
            //arrange
            NameValueCollection filter = new NameValueCollection()
            {
                {"filter", "{where: {id: 0}}"}
            };
            
            //action
            QueryFactory.CreateFindManyQuery(filter);

            //assert
            JsonQueryParser.Received(1).FindManyQuery("{where: {id: 0}}");
            QueryStringQueryParser.DidNotReceive().FindManyQuery(filter);

        }

        [Test]
        public void CreateFindManyQuery_GivenSimpleQueryStringFilter_ShouldCallQueryStringParser()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection()
            {
                { "filter[where][column]", "200" }
            };

            //action
            QueryFactory.CreateFindManyQuery(filter);

            //assert
            JsonQueryParser.DidNotReceive().FindManyQuery(Arg.Any<string>());
            QueryStringQueryParser.Received(1).FindManyQuery(filter);

        }

        [Test]
        public void CreateFindManyQuery_GivenPagingElemet_ShouldCallFindPaging()
        {
            //arrange
            NameValueCollection filter = new NameValueCollection()
            {
                {"paging", "{paging: {currentPage: 1, pageSize: 100}}"}
            };

            //action
            var queryStringQueryParser = new QueryStringQueryParser();
            var queryFactory = new QueryFactory(FindQueryValidator, queryStringQueryParser, JsonQueryParser, PagingParser);
            queryFactory.CreateFindManyQuery(filter);

            //assert
            JsonQueryParser.DidNotReceive().FindManyQuery(Arg.Any<string>());
            PagingParser.Received(1).FindPaging(Arg.Any<string>());

        }

        [Test]
        public void CreateFindManyQuery_GivenPagingElemet_ShouldCreatePagePartElement()
        {
            //arrange
            NameValueCollection filter = new NameValueCollection()
            {
                {"paging", "{paging: {currentPage: 1, pageSize: 100}}"}
            };
            
            PagingParser = new PagingParser();
            var queryStringQueryParser = new QueryStringQueryParser();
            var queryFactory = new QueryFactory(FindQueryValidator, queryStringQueryParser, JsonQueryParser, PagingParser);
            
            //action
            var query = queryFactory.CreateFindManyQuery(filter);
           
            //assert
            Assert.IsNotNull(query.PagedPart);
            Assert.AreEqual(1, query.PagedPart.CurrentPage);
            Assert.AreEqual(100, query.PagedPart.PageSize);

        }

        [Test]
        public void FindManyQuery_GivenJsonWithPagingAndNoOrderFilter_ShouldAutomaticallyAddIdAsOrderItemOnQuery()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection()
            {
                {"paging", "{paging: {currentPage: 1, pageSize: 100}}"}
            };

            PagingParser = new PagingParser();
            var queryStringQueryParser = new QueryStringQueryParser();
            var queryFactory = new QueryFactory(FindQueryValidator, queryStringQueryParser, JsonQueryParser, PagingParser);

            //action
            var query = queryFactory.CreateFindManyQuery(filter);

            //assert
            Assert.AreEqual("Id", query.OrderBy[0].Field);
            Assert.AreEqual(0, query.OrderBy[0].Sequence);

        }

        [Test]
        public void FindManyQuery_GivenJsonWithPagingAndCurrentPageLessThanOne_ShouldRaiseReturnAnError()
        {

            //arrange
            NameValueCollection filter = new NameValueCollection()
            {
                {"paging", "{paging: {currentPage: 0, pageSize: 100}}"}
            };

            PagingParser = new PagingParser();
            var queryStringQueryParser = new QueryStringQueryParser();
            var queryFactory = new QueryFactory(FindQueryValidator, queryStringQueryParser, JsonQueryParser, PagingParser);

            //action
            var query = queryFactory.CreateFindManyQuery(filter);

            //assert
            Assert.AreEqual(1, query.Errors.Count);
            Assert.AreEqual("Page size can't be 0 or negative.", query.Errors[0]);
            
        }


    }

}