using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using SAHL.Core.Web;
using SAHL.Core.Services;
using SAHL.Core.TextSearch.Solr.Models;

namespace SAHL.Core.TextSearch.Solr
{
    public class SolrFreeTextSearchProvider<T> : IFreeTextSearchProvider<T>
    {
        private readonly IServiceUrlConfigurationProvider serviceUrlConfigurationProvider;
        private readonly ISolrFreeTextSearchUrlBuilder urlBuilder;
        private readonly IWebHttpClientBuilder webClientBuilder;

        public SolrFreeTextSearchProvider(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, 
                                          ISolrFreeTextSearchUrlBuilder urlBuilder, IWebHttpClientBuilder webClientBuilder)
        {
            this.serviceUrlConfigurationProvider = serviceUrlConfigurationProvider;
            this.urlBuilder                      = urlBuilder;
            this.webClientBuilder                = webClientBuilder;
        }

        public TextSearchResult<T> Search(string searchIndexName, string freeTextTerms, Dictionary<string, string> filters, int pageSize, int currentPage)
        {
            var baseUrl             = urlBuilder.BuildBaseUrl(serviceUrlConfigurationProvider.ServiceHostName, serviceUrlConfigurationProvider.ServiceName);
            var url                 = urlBuilder.BuildUrl(baseUrl, searchIndexName, freeTextTerms, pageSize, currentPage, filters);
            var client              = webClientBuilder.GetConfiguredClient(baseUrl);
            var httpResponseMessage = client.Get(url).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json               = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var serviceQueryResult = JsonConvert.DeserializeObject<SolrResponse<T>>(json);
                var textSearchResult   = new TextSearchResult<T>(serviceQueryResult.response.numFound, pageSize, currentPage, 
                                                                 serviceQueryResult.responseHeader.QTime, 
                                                                 serviceQueryResult.response.docs);
                return textSearchResult;
            }

            throw new Exception(httpResponseMessage.ReasonPhrase);
        }
    }
}
