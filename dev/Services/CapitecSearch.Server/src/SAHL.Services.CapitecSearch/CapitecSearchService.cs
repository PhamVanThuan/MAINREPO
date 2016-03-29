using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.CapitecSearch;

namespace SAHL.Services.CapitecSearch
{
    public class CapitecSearchService : ICapitecSearchService
    {
        private IServiceCommandRouter serviceCommandRouter;

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ICapitecSearchServiceCommand
        {
            return this.serviceCommandRouter.HandleCommand<T>(command,metadata);
        }

        public CapitecSearchService(IServiceCommandRouter serviceCommandRouter)
        {
            this.serviceCommandRouter = serviceCommandRouter;
        }
    }
}