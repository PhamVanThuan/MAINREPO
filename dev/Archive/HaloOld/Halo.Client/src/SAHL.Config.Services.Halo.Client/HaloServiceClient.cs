using System.Threading;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.Halo;

namespace SAHL.Config.Services.Halo.Client
{
    public class HaloServiceClient : ServiceClient, IHaloService
    {
        public HaloServiceClient(IServiceUrlConfiguration serviceConfiguration)
            : base(serviceConfiguration)
        {
        }

        public ISystemMessageCollection PerformCommand<T>(T command) where T : IHaloServiceCommand
        {
            return base.PerformCommandInternal<T>(command);
        }
    }
}