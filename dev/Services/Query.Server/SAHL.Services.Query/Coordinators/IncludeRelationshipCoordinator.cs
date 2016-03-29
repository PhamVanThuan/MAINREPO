using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SAHL.Core;
using SAHL.Services.Query.Factories;

namespace SAHL.Services.Query.Coordinators
{
    public class IncludeRelationshipCoordinator : IIncludeRelationshipCoordinator
    {
        private readonly IWebRequestFactory webRequestFactory;
        private readonly IStreamResultReaderFactory streamResultReaderFactory;

        public IncludeRelationshipCoordinator(IWebRequestFactory webRequestFactory, IStreamResultReaderFactory streamResultReaderFactory)
        {
            this.webRequestFactory = webRequestFactory;
            this.streamResultReaderFactory = streamResultReaderFactory;
        }

        public void Fetch(IEnumerable<LinkQuery> urls)
        {
            Parallel.ForEach(urls, CoreGlobals.DefaultParallelOptions, PerformRequest());
        }

        private Action<LinkQuery> PerformRequest()
        {
            return query =>
            {
                var request = this.webRequestFactory.Create(query.AbsoluteUrl);
                request.UseDefaultCredentials = true;
                try
                {
                    var result = this.streamResultReaderFactory.Process(request, a => a.ReadToEnd());
                    query.JsonResult = result;
                }
                catch (Exception ex)
                {
                    //TODO: log?
                    query.JsonResult = string.Format(@"{0}{1}{2}{3}{4}",
                        @"{ ""error"" : ""An error occured attempting to retrieve this embedded resource: ",
                        ex.Message,
                        @" "", ""url"": """,
                        query.AbsoluteUrl,
                        @""" }"
                        );
                }
            };
        }
    }
}
