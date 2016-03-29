using NUnit.Framework;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using SAHL.Testing.SolrIndex.Tests;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Testing.Services.Tests.Search
{
    [TestFixture]
    public class ClientSearchTests : SolrIndexTest
    {
        [Test]
        public void ClientFullLegalNameSearchTest()
        {
            var record = this.GetRandomClients(1, multipleRoles: false);
            var results = this.SearchForClient(record.LegalName);
            Assert.Greater(results.Count(), 0, "No search results for {0}", record.LegalName);
            Assert.AreEqual(results.FirstOrDefault().LegalName, record.LegalName);
        }

        [Test]
        public void ClientFullEmailSearchTest()
        {
            var record = this.GetRandomClients(1, multipleRoles: false);
            var results = this.SearchForClient(record.Email);
            Assert.Greater(results.Count(), 0, "No search results for {0}", record.Email);
            Assert.AreEqual(results.FirstOrDefault().EmailAddress, record.Email);
            Assert.AreEqual(results.FirstOrDefault().LegalName, record.LegalName);
        }

        [Test]
        public void ClienIdNumberSearchTest()
        {
            var record = this.GetRandomClients(1, multipleRoles: false);
            var results = this.SearchForClient(record.IdNumber);
            Assert.Greater(results.Count(), 0, "No search results for {0}", record.IdNumber);
            Assert.AreEqual(results.FirstOrDefault().EmailAddress, record.Email);
            Assert.AreEqual(results.FirstOrDefault().LegalName, record.LegalName);
            Assert.AreEqual(results.FirstOrDefault().LegalIdentity, record.IdNumber);
        }

        [Test]
        public void ClientWithMultipleRolesTest()
        {
            var record = this.GetRandomClients(1, multipleRoles: true);
            var results = this.SearchForClient(record.IdNumber);
            Assert.AreEqual(results.Count(), 1, "Expected a single search result");
        }

        private ClientSearch GetRandomClients(int count, bool multipleRoles)
        {
            if (multipleRoles)
            {
                return TestApiClient.Get<ClientSearch>(new { hasmultipleroles = true }).First();
            }
            return TestApiClient.Get<ClientSearch>(new { }).First();
        }

        private IEnumerable<SearchForClientQueryResult> SearchForClient(string text)
        {
            var searchFilters = new SearchFilter[] { };
            var query = new SearchForClientQuery(text, searchFilters, "Client");

            base._searchService.PerformQuery<SearchForClientQuery>(query).WithoutMessages();
            return query.Result.Results;
        }
    }

    internal class ClientSearch
    {
        public string IdNumber { get; set; }

        public string LegalName { get; set; }

        public string Email { get; set; }

        public bool HasMultipleRoles { get; set; }
    }
}