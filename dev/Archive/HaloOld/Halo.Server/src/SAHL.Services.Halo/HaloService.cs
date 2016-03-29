using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Halo
{
    public class HaloService : IHaloService
    {
        private IServiceCommandRouter serviceCommandRouter;

        public HaloService(IServiceCommandRouter serviceCommandRouter)
        {
            this.serviceCommandRouter = serviceCommandRouter;
        }

        public ISystemMessageCollection PerformCommand<T>(T command) where T : IHaloServiceCommand
        {
            return this.serviceCommandRouter.HandleCommand<T>(command);
        }
    }
}