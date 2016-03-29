using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.DecisionTree
{
    public interface IDecisionTreeServiceClient
    {
        ISystemMessageCollection PerformQuery<T>(T query) where T : IServiceQuery;
    }
}