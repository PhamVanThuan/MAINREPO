using System.Collections.Generic;

namespace SAHL.Core.Services
{
    public interface IServiceQueryResult
    {
        int NumberOfPages { get; set; }

        int ResultCountInAllPages { get; set; }

        int ResultCountInPage { get; set; }

        int QueryDurationInMilliseconds { get; set; }
    }

    public interface IServiceQueryResult<T> : IServiceQueryResult
    {
        IEnumerable<T> Results { get; }
    }
}