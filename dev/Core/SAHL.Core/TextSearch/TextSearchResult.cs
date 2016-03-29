using System;
using System.Collections.Generic;

namespace SAHL.Core.TextSearch
{
    public class TextSearchResult<T>
    {
        public TextSearchResult(int numberOfResults, int resultPageSize, int currentResultPage, int queryTimeInMilliseconds, IEnumerable<T> results)
        {
            this.NumberOfResults = numberOfResults;
            this.QueryTimeInMilliseconds = queryTimeInMilliseconds;
            this.ResultPageSize = resultPageSize;
            this.CurrentResultPage = currentResultPage;
            this.Results = new List<T>(results);
        }

        public IEnumerable<T> Results { get; protected set; }

        public int QueryTimeInMilliseconds { get; protected set; }

        public int NumberOfResults { get; protected set; }

        public int ResultPageSize { get; protected set; }

        public int CurrentResultPage { get; protected set; }

        public int NumberOfPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.NumberOfResults / this.ResultPageSize);
            }
        }
    }
}