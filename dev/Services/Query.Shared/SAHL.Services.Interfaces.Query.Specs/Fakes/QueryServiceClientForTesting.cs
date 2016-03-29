using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.Query.Specs.Fakes
{
    public class QueryServiceClientForTesting : QueryServiceClient
    {
        public IWebHttpClient WebHttpClient { get; private set; }
        public object ObjectToSerialise { get; private set; }

        public QueryServiceClientForTesting(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator, IWebHttpClient webHttpClient)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
            this.WebHttpClient = webHttpClient;
        }

        protected override IWebHttpClient GetConfiguredClient()
        {
            return this.WebHttpClient;
        }

        protected override void SetQueryResult(IServiceQuery query, object result)
        {
            //intentionally empty, we set this value by substitution in testing
        }

        protected override HttpContent GetContentFromObject<T>(T objectToSerialize)
        {
            ObjectToSerialise = objectToSerialize;
            return base.GetContentFromObject(objectToSerialize);
        }
    }
}
