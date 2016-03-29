using SAHL.Config.Services;
using System;

namespace SAHL.WCFServices.ComcorpConnector.Server
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // *** Removed Autofac. Implemented StructureMap and a custom service host factory.
            // *** ref: http://lostechies.com/jimmybogard/2008/07/30/integrating-structuremap-with-wcf/

            ServiceBootstrapper bootstrapper = new ServiceBootstrapper();
            bootstrapper.Initialise();
        }
    }
}
