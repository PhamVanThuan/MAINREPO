using NUnit.Framework;
using SAHL.Config.Services;
using SAHL.Core;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest;
using SAHL.Services.Interfaces.Search;
using SAHL.Services.Interfaces.Search.Models;
using SAHL.Services.Interfaces.Search.Queries;
using SAHL.Testing.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SAHL.Testing.SolrIndex.Tests
{
    [TestFixture]
    public class SolrIndexTest
    {
        private IIocContainer _container;
        protected ISearchServiceClient _searchService;
        protected IFrontEndTestServiceClient _feTestClient;
        public List<SearchFilter> searchFilters;
        public IServiceRequestMetadata metadata;
        public ILinkedKeyManager _linkedKeyManager;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ServiceBootstrapper bootStrapper = new ServiceBootstrapper();
            _container = bootStrapper.Initialise();
            _searchService = _container.GetInstance<ISearchServiceClient>();
            _feTestClient = _container.GetInstance<IFrontEndTestServiceClient>();
            _linkedKeyManager = _container.GetInstance<ILinkedKeyManager>();
        }

        [SetUp]
        public void TestSetUp()
        {
            _searchService.SetPaginationQueryParameters(10, 1);
            metadata = new ServiceRequestMetadata();
            searchFilters = new List<SearchFilter>();
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public void AssertLegalEntityReturnedInThirdPartySearch(SearchForThirdPartyQuery query, int legalEntityKey, int expectedRecordCount, int timeoutSeconds = 30)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int actualRecordCount = 0;
            int page = 1;
            while (sw.Elapsed < TimeSpan.FromSeconds(timeoutSeconds))
            {
                _searchService.SetPaginationQueryParameters(10, page);
                _searchService.PerformQuery(query).WithoutMessages();
                actualRecordCount = query.Result.Results.Where(x => x.LegalEntityKey == legalEntityKey).Count();
                if (actualRecordCount == expectedRecordCount)
                {
                    break;
                }                
                //Get next page.  Once the last page has been read start at page 1 again
                if(++page > query.Result.NumberOfPages)
                {
                    page = 1;
                }
            }
            sw.Stop();
            Assert.AreEqual(expectedRecordCount, actualRecordCount, string.Format("Expected Third Party with LegalEntityKey: {0} to be searchable by query text: {1}", legalEntityKey, query.QueryText));
        }

        public void AssertLegalEntityReturnedInClientSearch(SearchForClientQuery query, int legalEntityKey, int expectedRecordCount, int timeoutSeconds = 30)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int actualRecordCount = 0;
            int page = 1;
            while (sw.Elapsed < TimeSpan.FromSeconds(timeoutSeconds))
            {
                _searchService.SetPaginationQueryParameters(10, page);
                _searchService.PerformQuery(query).WithoutMessages();
                actualRecordCount = query.Result.Results.Where(x => x.LegalEntityKey == legalEntityKey).Count();
                if (actualRecordCount == expectedRecordCount)
                {
                    break;
                }
                if (++page > query.Result.NumberOfPages)
                {
                    page = 1;
                }
            }
            sw.Stop();
            Assert.AreEqual(expectedRecordCount, actualRecordCount, string.Format("Expected Client with LegalEntityKey: {0} to be searchable by query text: {1}", legalEntityKey, query.QueryText));
        }

        public IEnumerable<SearchForTaskQueryResult> SearchTaskIndexByInstanceId(long instanceId, string stateName, int timeoutSeconds)
        {
            return SearchTaskIndexByInstanceId(instanceId, stateName, null, timeoutSeconds);
        }

        public IEnumerable<SearchForTaskQueryResult> SearchTaskIndexByInstanceId(long instanceId, string stateName, IEnumerable<SearchFilter> additionalFilters = null, int timeoutSeconds = 30)
        {
            var searchFilters = new List<SearchFilter>();
            searchFilters.Add(new SearchFilter("InstanceId", instanceId.ToString()));
            if (additionalFilters != null)
            {
                searchFilters.AddRange(additionalFilters);
            }
            var query = new SearchForTaskQuery("*", searchFilters, "Task");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            int page = 1;
            IEnumerable<SearchForTaskQueryResult> records = new List<SearchForTaskQueryResult>();
            while (sw.Elapsed < TimeSpan.FromSeconds(timeoutSeconds))
            {
                _searchService.SetPaginationQueryParameters(10, page);
                _searchService.PerformQuery(query).WithoutMessages();
                records = query.Result.Results.Where(x=>x.State == stateName);
                if (records.Count() > 0)
                {
                    break;
                }
                if (++page > query.Result.NumberOfPages)
                {
                    page = 1;
                }
            }
            sw.Stop();
            Assert.Greater(records.Count(), 0, string.Format("Expected Instance: {0} to be returned in the task index", instanceId));
            return records;
        }

        public void AssertThirdPartyInvoiceReturnedInThirdPartyInvoiceSearch(SearchForThirdPartyInvoicesQuery query, int thirdPartyInvoiceKey, int expectedRecordCount, int timeoutSeconds = 30)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int actualRecordCount = 0;
            int page = 1;
            while (sw.Elapsed < TimeSpan.FromSeconds(timeoutSeconds))
            {
                _searchService.SetPaginationQueryParameters(10, page);
                _searchService.PerformQuery(query).WithoutMessages();
                actualRecordCount = query.Result.Results.Where(x => x.ThirdPartyInvoiceKey == thirdPartyInvoiceKey).Count();
                if (actualRecordCount == expectedRecordCount)
                {
                    sw.Stop();
                    break;
                }
                if (++page > query.Result.NumberOfPages)
                {
                    page = 1;
                }
            }
            Assert.AreEqual(expectedRecordCount, actualRecordCount, string.Format("Expected ThirdPartyInvoice with ThirdPartyInvoiceKey: {0} to be searchable by query text: {1}", thirdPartyInvoiceKey, query.QueryText));
        }

    }
}