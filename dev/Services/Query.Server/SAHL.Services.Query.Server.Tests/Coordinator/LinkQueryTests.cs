using NUnit.Framework;
using SAHL.Services.Query.Coordinators;

namespace SAHL.Services.Query.Server.Tests.Coordinator
{
    [TestFixture]
    public class LinkQueryTests
    {

        private LinkQuery linkQuery;
        
        [Test]
        public void IsTemplated_GivenUrlWithInsertTemplates_ShouldReturnTrue()
        {
            //arrange
            string url = @"~/test/{id}";
            string relativeUrl = CreateRelativeUrl(url);
            string absoluteUrl = CreateAbsoluteUrl(url);
            
            //action
            linkQuery = new LinkQuery("test", relativeUrl, absoluteUrl, "");

            //assert
            Assert.IsTrue(linkQuery.IsTemplatedUrl());

        }
        
        [Test]
        public void IsTemplated_GivenSimpleUrl_ShouldReturnFalse()
        {
            //arrange
            string url = "/test";
            string relativeUrl = CreateRelativeUrl(url);
            string absoluteUrl = CreateAbsoluteUrl(url);
            
            //action
            linkQuery = new LinkQuery("test", relativeUrl, absoluteUrl, "");

            //assert
            Assert.IsFalse(linkQuery.IsTemplatedUrl());

        }

        [TestCase("/test/?filter={where: {id: 1}}")]
        [TestCase("/test/test?paging={Paging: {currentPage: 1, pageSize: 10}}")]
        public void IsTemplated_GivenUrlWithNoInsertTemplatesButWithJson_ShouldReturnFalse(string url)
        {
            //arrange
            string relativeUrl = CreateRelativeUrl(url);
            string absoluteUrl = CreateAbsoluteUrl(url);
            
            //action
            linkQuery = new LinkQuery("test", relativeUrl, absoluteUrl, "");

            //assert
            Assert.IsFalse(linkQuery.IsTemplatedUrl());

        }
        
        private static string CreateAbsoluteUrl(string url)
        {
            return @"http://servername" + url;
        }

        private static string CreateRelativeUrl(string url)
        {
            return @"~" + url;
        }

    }
}