using System.Collections.Generic;

namespace SAHL.Core.TextSearch
{
    public interface IFreeTextSearchProvider<T>
    {
        TextSearchResult<T> Search(string searchIndexName, string freeTextTerms, Dictionary<string, string> filters, int pageSize, int currentPage);
    }
}