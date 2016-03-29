using System;

using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.Halo
{
    public class HaloServiceClient : ServiceHttpClientWindowsAuthenticated, IHaloServiceClient
    {
        private readonly IWebHttpClient webHttpClient;

        public HaloServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator, IWebHttpClient webHttpClient)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
            if (webHttpClient == null) { throw new ArgumentNullException("webHttpClient"); }
            this.webHttpClient = webHttpClient;
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IHaloServiceQuery
        {
            var messageCollection = SystemMessageCollection.Empty();
            messageCollection.Aggregate(this.PerformQueryInternal<T>(query));
            return messageCollection;
        }

        protected override IWebHttpClient GetConfiguredClient()
        {
            return this.webHttpClient != null ? webHttpClient : base.GetConfiguredClient();
        }
    }
}
