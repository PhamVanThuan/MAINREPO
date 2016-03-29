using SAHL.Core.IoC;
using SAHL.Core.Services;

namespace SAHL.Core.Web.Services
{
    public class WebService : HostedService, IStartableService, IStoppableService, IWebService
    {
        public WebService()
            : base()
        {
        }
    }
}