using NUnit.Framework;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests
{
    [TestFixture]
    public sealed class ThirdPartySearchTests : SolrIndexTest
    {
        [Test]
        public void ThirdPartyFullLegalNameSearchTest()
        {
            var record = TestApiClient.Get<ThirdPartySearch>(new { }).First();

            var searchFilters = new SearchFilter[] { };
            var query = new SearchForThirdPartyQuery(record.LegalName, searchFilters, "ThirdParty");

            base._searchService.PerformQuery<SearchForThirdPartyQuery>(query).WithoutMessages();
            var results = query.Result.Results;

            Assert.Greater(results.Count(), 0, "No search results for {0}", record.LegalName);
            Assert.AreEqual(results.FirstOrDefault().LegalName, record.LegalName);
        }

        [Test]
        public void ThirdPartyEmailSearchTest()
        {
            var record = TestApiClient.Get<ThirdPartySearch>(new { }).First();

            var searchFilters = new SearchFilter[] { };
            var query = new SearchForThirdPartyQuery(record.LegalName, searchFilters, "ThirdParty");

            base._searchService.PerformQuery<SearchForThirdPartyQuery>(query).WithoutMessages();
            var results = query.Result.Results;

            Assert.Greater(results.Count(), 0, "No search results for {0}", record.LegalName);
            Assert.AreEqual(results.FirstOrDefault().EmailAddress, record.Email);
            Assert.AreEqual(results.FirstOrDefault().LegalName, record.LegalName);
        }

        public class ThirdPartySearch : SAHL.Core.Data.IDataModel
        {
            public string LegalName { get; set; }

            public string Email { get; set; }
        }
    }
}