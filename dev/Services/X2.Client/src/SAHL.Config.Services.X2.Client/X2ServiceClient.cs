using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.Services.Interfaces.X2;
using System.Net.Http;

namespace SAHL.Config.Services.X2.Client
{
    public class X2ServiceClient : SAHL.Core.Web.Services.ServiceClient, IX2Service
    {
        public X2ServiceClient(IServiceUrlConfiguration serviceConfiguration)
            : base(serviceConfiguration)
        {
            
            base.UseWindowsAuth();
        }


        public ISystemMessageCollection PerformCommand<T>(T command) where T : IX2Message
        {
            return base.PerformCommandInternal<T>(command, null);
        }
    }
}