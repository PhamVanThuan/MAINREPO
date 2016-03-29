using System.Collections.Generic;

namespace SAHL.Core.TextSearch
{
    public class QueryResult<T> : IQueryResultCollection<T>
        where T : class, IQueryResultModel, new()
    {
        public QueryResult()
        {
            Paging = new PagingInfo();
            CurrentPageResults = new List<T>();
        }

        /// <summary>
        /// The matching results on the selected page number
        /// </summary>
        public IEnumerable<T> CurrentPageResults { get; set; }

        /// <summary>
        /// Container for information to do with paging
        /// </summary>
        public PagingInfo Paging { get; set; }
    }
}