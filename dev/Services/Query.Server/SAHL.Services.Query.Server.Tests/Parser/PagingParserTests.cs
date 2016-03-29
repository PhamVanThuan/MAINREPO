using NUnit.Framework;
using SAHL.Services.Interfaces.Query.Parsers.Elements;
using SAHL.Services.Query.Parsers;

namespace SAHL.Services.Query.Server.Tests.Parser
{

    [TestFixture]
    public class PagingParserTests
    {

        private PagingParser pagingParser;

        [SetUp]
        public void SetupTests()
        {
            pagingParser = new PagingParser();
        }

        [Test]
        public void GetPaging_GivenJsonStringWithNoPaging_ShouldReturnNull()
        {
            //arrange
            string paging = "";

            //action
            IPagedPart pagingPart = pagingParser.FindPaging(paging);

            //assert
            Assert.IsNull(pagingPart);

        }

        [Test]
        public void FindManyQuery_GivenJsonStringWithLimit_ShouldReturnManyQueryWithLimitSet()
        {

            //arrange
            string paging = "{Paging: {currentPage: 1, pageSize: 100}}";

            //action
            IPagedPart pagingPart = pagingParser.FindPaging(paging);

            //assert
            Assert.IsNotNull(pagingPart);
            Assert.AreEqual(1, pagingPart.CurrentPage);
            Assert.AreEqual(100, pagingPart.PageSize);

        }

        [Test]
        public void FindManyQuery_GivenJsonWithPagingWherePageSizeExceedsMaxQueryLimit_ShouldSetThePageSizeToMaxLimit()
        {

            //arrange
            string paging = "{Paging: {currentPage: 1, pageSize: 101}}";

            //action
            IPagedPart pagingPart = pagingParser.FindPaging(paging);

            //assert
            Assert.IsNotNull(pagingPart);
            Assert.AreEqual(1, pagingPart.CurrentPage);
            Assert.AreEqual(100, pagingPart.PageSize);

        }
        
    }

}