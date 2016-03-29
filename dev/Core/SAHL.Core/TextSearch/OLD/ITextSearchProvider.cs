namespace SAHL.Core.TextSearch
{
    public interface ITextSearchProvider
    {
        QueryResult<U> FreeTextSearch<T, U>(string searchTerm, int currentPage, int pageSize)
            where T : class, new()
            where U : class, IQueryResultModel, new();

        string[] GetAutoCompleteSuggestions(string fieldName, string partialSearchTerm);

        string[] GetMultiFieldAwareAutoCompleteSuggestions<T>(string propertyRequestingSuggestions, T queryModel)
            where T : class, new();

        QueryResult<U> MultiFieldSearchAndAcrossWildcardWithin<T, U>(IQueryModel<T> model)
            where T : class, new()
            where U : class, IQueryResultModel, new();

        QueryResult<U> MultiFieldSearchAndAcrossExactMatchWithin<T, U>(IQueryModel<T> model)
            where T : class, new()
            where U : class, IQueryResultModel, new();

        void RefreshIndex();

        void RefreshIndexOnCapitecFailoverWebServer();

        void RefreshIndexAndClearStagingData();
    }
}