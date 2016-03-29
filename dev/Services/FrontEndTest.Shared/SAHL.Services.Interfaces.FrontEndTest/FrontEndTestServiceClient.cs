using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using System;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest
{
    public class FrontEndTestServiceClient : ServiceHttpClientWindowsAuthenticated, IFrontEndTestServiceClient
    {
        public FrontEndTestServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator) { }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IFrontEndTestQuery
        {
            return base.PerformQueryInternal<T>(query);
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IFrontEndTestCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }
    }
}