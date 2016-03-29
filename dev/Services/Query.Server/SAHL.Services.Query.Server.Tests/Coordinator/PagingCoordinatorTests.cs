using System.Collections.Generic;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Parsers;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Builders.Core;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.Core;
using SAHL.Services.Query.Parsers.Elemets;
using SAHL.Services.Query.Server.Tests.Factory;
using SAHL.Services.Query.Server.Tests.Representations;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Tests.Coordinator
{
    [TestFixture]
    public class PagingCoordinatorTests
    {

        private IPagingCoordinator pagingCoordinator;
        private IPagingLinksCoordinator pagingLinksCoordinator;
        private ILinkResolver linkResolver;

        [SetUp]
        public void SetupTests()
        {
            linkResolver = Substitute.For<ILinkResolver>();
            pagingLinksCoordinator = new PagingLinksCoordinator(linkResolver);
            pagingCoordinator = new PagingCoordinator(pagingLinksCoordinator);
        }

        [Test]
        public void ApplyPaging_GivenNoPagingPartInQuery_ShouldNotApplyAnyLinks()
        {
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            List<Representation> list = new List<Representation>();
            list.Add(TestMetaDataFactory.CreateTestRepresentation());

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);
            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountOne(), typeof (TestRepresentation), list.Count);
            
            //assert
            Assert.IsNull(listRepresentation._paging);

        }

        [Test]
        public void ApplyPaging_GivenNoResultsFromQuery_ShouldReturnTotalCountZeroAndListCountEqualToTotalCount()
        {
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.Limit = new LimitPart(){
                                                Count = 100
                                              };

            List<Representation> list = new List<Representation>();
            
            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);
            
            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountZero(), typeof(TestRepresentation), list.Count);

            //assert
            Assert.AreEqual(0, listRepresentation.TotalCount);
            Assert.AreEqual(0, listRepresentation.ListCount);

        }

        [Test]
        public void ApplyPaging_GivenNoResultsGreaterThanQueryLimit_ShouldReturnTotalCountAndListCountEqualToQueryLimit()
        {
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.Limit = new LimitPart()
            {
                Count = 10
            };

            List<Representation> list = new List<Representation>();

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);

            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountEleven(), typeof(TestRepresentation), GetCountEleven());

            //assert
            Assert.AreEqual(11, listRepresentation.TotalCount);
            Assert.AreEqual(10, listRepresentation.ListCount);

        }

        [Test]
        public void ApplyPaging_GivenPagingPartWithOrderBYInQuery_ShouldReturnSixLinksAndSetPagingDetails()
        {
            //arrange
            IFindQuery findQuery = new FindManyQuery();

            findQuery.OrderBy = new List<IOrderPart>()
            {
                new OrderPart()
                {
                    Sequence = 0,
                    Field = "Name Desc"
                }
            };

            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            List<Representation> list = new List<Representation>();
            list.Add(TestMetaDataFactory.CreateTestRepresentation());

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);
            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountEleven(), typeof (TestRepresentation), list.Count);
            
            //assert
            Assert.IsNotNull(listRepresentation._paging);
            Assert.AreEqual(11, listRepresentation._paging.Count);
            Assert.AreEqual(1, listRepresentation._paging.CurrentPage);
            Assert.AreEqual(10, listRepresentation._paging.PageSize);
            Assert.AreEqual(5, listRepresentation._paging._links.Count);
            
        }

        [Test]
        public void ApplyPaging_GivenPagingWithPageSizeLargerThanTheTotalCountOfRecords_ShouldSetListCountToPagingSize()
        {
            
            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 20
            };

            List<Representation> list = new List<Representation>();
            list.Add(TestMetaDataFactory.CreateTestRepresentation());

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);

            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountEleven(), typeof(TestRepresentation), 11);
            
            //assert
            Assert.AreEqual(11, listRepresentation.ListCount);

        }

        [Test]
        //This test covers the case where we are paging and returning the paged result for the last page
        public void ApplyPaging_GivenPagingWhereTotalCountIsLargerThanPageSizeAndResultCountIsLessThanPageSize_ShouldSetListCountToResultCount()
        {

            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            List<Representation> list = new List<Representation>();
            list.Add(TestMetaDataFactory.CreateTestRepresentation());

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);

            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountEleven(), typeof(TestRepresentation), list.Count);

            //assert
            Assert.AreEqual(1, listRepresentation.ListCount);

        }

        [Test]
        public void ApplyPaging_GivenNoPagingButIncludeSkipAndTakeWhenListResultIsLessThanTheLimitSet_ShouldReturnListHeaderWithListCountSetToResultCountAndNotTheLimit()
        {

            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.Limit = new LimitPart() {Count = 10};
            findQuery.Skip = new SkipPart() {Skip = 10, Take = 10};
            findQuery.PagedPart = null;

            List<Representation> list = new List<Representation>();
            list.Add(TestMetaDataFactory.CreateTestRepresentation());

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);

            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountEleven(), typeof(TestRepresentation), list.Count);

            //assert
            Assert.AreEqual(1, listRepresentation.ListCount);

        }

        [Test]
        public void ApplyPaging_GivenPagingAndQueryResultCountIsEqualToPageCount_ShouldReturnListHeaderWithListCountSetToPageSize()
        {

            //arrange
            IFindQuery findQuery = new FindManyQuery();
            findQuery.PagedPart = new PagedPart()
            {
                CurrentPage = 1,
                PageSize = 10
            };

            List<Representation> list = new List<Representation>();
            list.Add(TestMetaDataFactory.CreateTestRepresentation());

            IListRepresentation listRepresentation = new TestListRepresentation(linkResolver, list);

            //action
            pagingCoordinator.ApplyPaging(listRepresentation, findQuery, () => GetCountEleven(), typeof(TestRepresentation), 10);

            //assert
            Assert.AreEqual(10, listRepresentation.ListCount);

        }

        public int GetCountOne()
        {
            return 1;
        }

        public int GetCountEleven()
        {
            return 11;
        }

        private int GetCountZero()
        {
            return 0;
        }
        
    }
}