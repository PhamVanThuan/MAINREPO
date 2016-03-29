using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.DecisionTreeDesign;

namespace SAHL.Services.DecisionTreeDesign.Services
{
    public class DecisionTreeDesignService : IDecisionTreeDesignService
    {
        private IServiceCommandRouter serviceCommandRouter;

        public DecisionTreeDesignService(IServiceCommandRouter serviceCommandRouter)
        {
            this.serviceCommandRouter = serviceCommandRouter;
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : IDecisionTreeServiceCommand
        {
            return this.serviceCommandRouter.HandleCommand<T>(command, metadata);
        }
    }
}