using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Capitec;

namespace SAHL.Services.Capitec
{
    public class CapitecService : ICapitecService
    {
        private IServiceCommandRouter serviceCommandRouter;

        public CapitecService(IServiceCommandRouter serviceCommandRouter)
        {
            this.serviceCommandRouter = serviceCommandRouter;
        }

        public ISystemMessageCollection PerformCommand<T>(T command, IServiceRequestMetadata metadata) where T : ICapitecServiceCommand
        {
            return this.serviceCommandRouter.HandleCommand<T>(command,metadata);
        }
    }
}