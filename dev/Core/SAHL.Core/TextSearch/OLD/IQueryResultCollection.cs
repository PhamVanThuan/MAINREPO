using System.Collections.Generic;

namespace SAHL.Core.TextSearch
{
    public interface IQueryResultCollection<T>
        where T : class, IQueryResultModel, new()
    {
        IEnumerable<T> CurrentPageResults { get; set; }

        PagingInfo Paging { get; set; }
    }
}