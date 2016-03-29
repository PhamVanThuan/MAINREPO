using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using System.Net;

[assembly: OwinStartup(typeof(SAHL.Services.DecisionTreeDesign.Startup))]

namespace SAHL.Services.DecisionTreeDesign
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //var listener = (HttpListener)app.Properties[typeof(HttpListener).FullName];
            //listener.AuthenticationSchemes = AuthenticationSchemes.Ntlm;
            GlobalHost.HubPipeline.AddModule(new CatchAllHubErrorHandlingPipelineModule()); 

            var idProvider = new PrincipalUserIdProvider();
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
            
            app.MapSignalR();
        }
    }
}