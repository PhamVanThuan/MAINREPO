using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Query.Builders.Core;
using SAHL.Services.Query.Controllers.Test;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Parsers.Elemets;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Tests.Coordinator
{
    [TestFixture]
    public class PagingLinksCoordinatorTests
    {

        private PagingLinksCoordinator _pagingLinksLinksCoordinator;
        private ILinkResolver linkResolver;

        [SetUp]
        public void SetupTest()
        {
            linkResolver = Substitute.For<ILinkResolver>();
            _pagingLinksLinksCoordinator = new PagingLinksCoordinator(linkResolver);
        }

        [Test]
        public void CreatPagingLinks_GivenFindQueryWithPagedPartSetToCurrentPageOneAndPageSize10With10Count_ShouldReturnValidListOfLinks()
        {

            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            //action
            List<Link> links = _pagingLinksLinksCoordinator.CreatePagingLinks(findQuery, 10, typeof(TestRepresentation));

            //assert
            Assert.AreEqual(3, links.Count);
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "first"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "next"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "last"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "previous"));

            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "1"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "2"));

        }

        [Test]
        public void CreatPagingLinks_GivenFindQueryWithPagedPartSetToCurrentPageOneAndPageSize11With10Count_ShouldReturnValidListOfLinks()
        {

            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            //action
            List<Link> links = _pagingLinksLinksCoordinator.CreatePagingLinks(findQuery, 11, typeof(TestRepresentation));

            //assert
            Assert.AreEqual(5, links.Count);
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "first"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "next"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "last"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "previous"));

            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "1"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "2"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "3"));

        }

        [Test]
        public void CreatPagingLinks_GivenFindQueryWithPagedPartSetToCuurentPageOneAndPageSize10With100Count_ShouldReturnValidListOfLinks()
        {
            
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            //action
            List<Link> links = _pagingLinksLinksCoordinator.CreatePagingLinks(findQuery, 100, typeof (TestRepresentation));

            //assert
            Assert.AreEqual(13, links.Count);
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "first"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "next"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "last"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "previous"));

            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "1"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "2"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "3"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "4"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "5"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "6"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "7"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "8"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "9"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "10"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "11"));

        }
        
        [Test]
        public void CreatPagingLinks_GivenFindQueryWithPagedPartSetToCurrentPageTenAndPageSize10With100Count_ShouldReturnValidListOfLinks()
        {
            
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 10,
                PageSize = 10
            };

            //action
            List<Link> links = _pagingLinksLinksCoordinator.CreatePagingLinks(findQuery, 100, typeof (TestRepresentation));

            //assert
            Assert.AreEqual(13, links.Count);
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "first"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "next"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "last"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "previous"));

            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "1"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "2"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "3"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "4"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "5"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "6"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "7"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "8"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "9"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "10"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "11"));

        }
        
        [Test]
        public void CreatPagingLinks_GivenFindQueryWithPagedPartSetToCurrentPageTwoAndPageSize10With100Count_ShouldReturnValidListOfLinks()
        {
            
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 2,
                PageSize = 10
            };

            //action
            List<Link> links = _pagingLinksLinksCoordinator.CreatePagingLinks(findQuery, 100, typeof (TestRepresentation));

            //assert
            Assert.AreEqual(14, links.Count);
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "first"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "next"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "last"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "previous"));

            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "1"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "2"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "3"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "4"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "5"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "6"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "7"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "8"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "9"));
            Assert.IsNotNull(links.FirstOrDefault(x => x.Rel == "10"));
            Assert.IsNull(links.FirstOrDefault(x => x.Rel == "11"));

        }

        [TestCase("?paging={Paging: {currentPage: 1, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "first")]
        [TestCase("?paging={Paging: {currentPage: 2, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "previous")]
        [TestCase("?paging={Paging: {currentPage: 4, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "next")]
        [TestCase("?paging={Paging: {currentPage: 6, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "last")]
        [TestCase("?paging={Paging: {currentPage: 1, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "1")]
        [TestCase("?paging={Paging: {currentPage: 2, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "2")]
        [TestCase("?paging={Paging: {currentPage: 3, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "3")]
        [TestCase("?paging={Paging: {currentPage: 4, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "4")]
        [TestCase("?paging={Paging: {currentPage: 5, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "5")]
        [TestCase("?paging={Paging: {currentPage: 6, pageSize: 10}}&filter={order: {0: 'Name Desc'}}", "6")]
        public void CreatePagingLinks_GivenFindQueryWithPagedPartSetToCurrentPage2AndPageSize51WithCount_ShouldHaveValidLinkParts(string linkRel, string linkName)
        {
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 3,
                PageSize = 10
            };

            findQuery.FullFilterString = "{order: {0: 'Name Desc'}}";

            //action
            List<Link> links = _pagingLinksLinksCoordinator.CreatePagingLinks(findQuery, 51, typeof(TestRepresentation));

            //assert
            Link link = links.FirstOrDefault(x => x.Rel == linkName);
            Assert.AreEqual(linkRel, link.Href);

        }

    }
}