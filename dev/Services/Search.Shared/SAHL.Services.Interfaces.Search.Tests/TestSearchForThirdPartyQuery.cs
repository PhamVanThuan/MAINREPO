using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;

namespace SAHL.Services.Interfaces.Search.Tests
{
    [TestFixture]
    public class TestSearchForThirdPartyQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var searchFilters = new List<SearchFilter> { new SearchFilter("test", "name") };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new SearchForThirdPartyQuery("queryText", searchFilters, "indexName");
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var queryText     = "queryText";
            var searchFilters = new List<SearchFilter> { new SearchFilter("test", "name") };
            var indexName     = "indexName";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new SearchForThirdPartyQuery(queryText, searchFilters, indexName);
            //---------------Test Result -----------------------
            Assert.AreEqual(queryText, query.QueryText);
            CollectionAssert.AreEquivalent(searchFilters, query.Filters);
            Assert.AreEqual(indexName, query.IndexName);
        }
    }
}
