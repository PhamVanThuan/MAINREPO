using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SAHL.Services.EventProjectionStreamer.EventProjectionStreamerStartup))]

namespace SAHL.Services.EventProjectionStreamer
{
    public class EventProjectionStreamerStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", builder =>
                {
                    builder.UseCors(CorsOptions.AllowAll);

                    var hubConfiguration = new HubConfiguration
                        {
                            EnableDetailedErrors    = true,
                            //EnableJavaScriptProxies = false,
                            EnableJSONP             = true,
                        };
                    builder.RunSignalR(hubConfiguration);
                });
        }
    }
}
