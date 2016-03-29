using Newtonsoft.Json;
using System.Collections.Generic;

namespace SAHL.Core.Services
{
    public class ServiceQueryResult<T> : IServiceQueryResult<T>
    {
        private IEnumerable<T> results;

        /// <summary>
        /// Use this for results that are not paged. i.e. no paging or post paging
        /// </summary>
        /// <param name="results"></param>
        [JsonConstructor]
        public ServiceQueryResult(IEnumerable<T> results)
        {
            this.results = results;
        }

        public int QueryDurationInMilliseconds { get; set; } 

        public int NumberOfPages { get; set; }

        public int ResultCountInAllPages { get; set; }

        public int ResultCountInPage { get; set; }

        [JsonProperty(TypeNameHandling = TypeNameHandling.None)]
        public IEnumerable<T> Results { get { return this.results; } }
    }
}