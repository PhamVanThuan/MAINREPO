using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using System;

namespace SAHL.Services.Interfaces.PropertyDomain
{
    public class PropertyDomainServiceClient : ServiceHttpClientWindowsAuthenticated, IPropertyDomainServiceClient
    {
        public PropertyDomainServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IPropertyDomainCommand
        {
            return base.PerformCommandInternal<T>(command, metadata);
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IPropertyDomainQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}
