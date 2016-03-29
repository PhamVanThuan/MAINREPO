using SAHL.Core.TextSearch;

namespace SAHL.Services.CapitecSearch.Services
{
    public class FullTextSearchPagedQueryWrapper<T> : IQueryModel<T> where T : class, new()
    {
        public T QueryModel { get; protected set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public string FreeText { get; set; }

        public FullTextSearchPagedQueryWrapper(T queryModel, int currentPage, int pageSize)
        {
            this.QueryModel = queryModel;
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
        }
    }
}