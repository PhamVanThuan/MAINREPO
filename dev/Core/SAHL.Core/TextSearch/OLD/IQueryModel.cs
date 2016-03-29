namespace SAHL.Core.TextSearch
{
    public interface IQueryModel<T>
        where T : class, new()
    {
        /// <summary>
        /// The strongly typed model whose properties values (search terms) should be matched
        /// </summary>
        T QueryModel { get; }

        /// <summary>
        /// Free hand text for searching across all the properties of a model
        /// </summary>
        string FreeText { get; set; }

        /// <summary>
        /// The requested zero based page index;
        /// </summary>
        int CurrentPage { get; set; }

        /// <summary>
        /// The number of results to place on a single page
        /// </summary>
        int PageSize { get; set; }
    }
}