using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;

namespace SAHL.Services.Interfaces.DecisionTree
{
    public class DecisionTreeServiceClient : ServiceHttpClientWindowsAuthenticated, IDecisionTreeServiceClient
    {
        public DecisionTreeServiceClient(IServiceUrlConfigurationProvider serviceUrlConfigurationProvider, IJsonActivator jsonActivator)
            : base(serviceUrlConfigurationProvider, jsonActivator)
        {
        }

        public ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery
        {
            return base.PerformQueryInternal<T>(query);
        }
    }
}