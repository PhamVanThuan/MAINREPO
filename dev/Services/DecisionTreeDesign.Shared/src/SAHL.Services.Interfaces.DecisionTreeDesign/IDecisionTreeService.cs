using SAHL.Core.Services;
using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.DecisionTreeDesign
{
    public interface IDecisionTreeDesignService
    {
        ISystemMessageCollection PerformCommand<T>(T Command, IServiceRequestMetadata metadata) where T : IDecisionTreeServiceCommand;
    }
}