using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SAHL.Core.TextSearch.Solr
{
    public class SolrFreeTextSearchUrlBuilder : ISolrFreeTextSearchUrlBuilder
    {
        public string BuildBaseUrl(string hostName, string serviceName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException("hostName");
            }

            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("serviceName");
            }

            return string.Format("http://{0}/{1}", hostName, serviceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="searchIndexName"></param>
        /// <param name="freeTextTerms"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage">Starts at page 1</param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public string BuildUrl(string baseUrl, string searchIndexName, string freeTextTerms, int pageSize, int currentPage, Dictionary<string, string> filters)
        {
            var ulrEncodedFreeTextTerms = HttpUtility.UrlEncode(freeTextTerms);

            var startFrom = (currentPage < 0 ? 0 : currentPage) * pageSize - pageSize;
            StringBuilder filterString = new StringBuilder();
            foreach (var filter in filters)
            {
                filterString.Append(string.Format("&fq={0}:{1}", filter.Key, filter.Value));
            }

            return string.Format("{0}/{1}/select?q={2}&start={3}&rows={4}&wt=json&defType=edismax{5}", 
                                 baseUrl, searchIndexName, ulrEncodedFreeTextTerms, startFrom, pageSize, filterString.ToString());
        }
    }
}