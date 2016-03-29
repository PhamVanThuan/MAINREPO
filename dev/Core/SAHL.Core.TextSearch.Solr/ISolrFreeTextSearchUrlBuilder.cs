using System.Collections.Generic;
namespace SAHL.Core.TextSearch.Solr
{
    public interface ISolrFreeTextSearchUrlBuilder
    {
        string BuildBaseUrl(string hostName, string serviceName);

        string BuildUrl(string baseUrl, string searchIndexName, string freeTextTerms, int pageSize, int currentPage, Dictionary<string, string> filters);
    }
}