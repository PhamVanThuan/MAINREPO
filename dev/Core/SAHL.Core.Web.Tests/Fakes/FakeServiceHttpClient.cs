using System.Net.Http;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Core.Web.Tests
{
    public class FakeServiceHttpClient : ServiceHttpClient
    {
        public FakeServiceHttpClient(IServiceUrlConfiguration serviceConfiguration, IJsonActivator jsonActivator)
            : base(serviceConfiguration, jsonActivator)
        {
        }

        public FakeServiceHttpClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public IWebHttpClient WebHttpClient { get; set; }

        public string ResultMessage { get; set; }

        public new HttpClientHandler HttpClientHandler
        {
            get { return base.HttpClientHandler; }
        }

        public new PaginationQueryParameter PaginationQueryParameter
        {
            get { return base.PaginationQueryParameter; }
        }

        public new FilterQueryParameter FilterQueryParameter
        {
            get { return base.FilterQueryParameter; }
        }

        public new SortQueryParameter SortQueryParameter
        {
            get { return base.SortQueryParameter; }
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFakeTestCommand
        {
            var messageResult = base.PerformCommandInternal<T>(command, metadata);
            command.ResultMessage = this.ResultMessage;
            return messageResult;
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IFakeTestQuery
        {
            var messageResult = base.PerformQueryInternal<T>(query);
            return messageResult;
        }

        public IWebHttpClient RetrieveWebHttpClient()
        {
            return this.GetConfiguredClient();
        }

        protected override IWebHttpClient GetConfiguredClient()
        {
            this.WebHttpClient = this.WebHttpClient ?? base.GetConfiguredClient();
            return this.WebHttpClient;
        }
    }
}