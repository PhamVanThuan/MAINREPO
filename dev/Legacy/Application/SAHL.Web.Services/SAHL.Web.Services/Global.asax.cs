using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Principal;
using System.Text;
using System.Web.SessionState;
using SAHL.Common.Interfaces;
using SAHL.Common.Web;
using SAHL.Common.Security;
using SAHL.Common.CacheData;
using System.Threading;
using System.Web;
using SAHL.Common.Web.UI.Exceptions;
using SAHL.X2.Common;
using SAHL.Common.Logging;
using SAHL.Communication;
using System.Reflection;

namespace SAHL.Web.Services
{
    public class Global : System.Web.HttpApplication
    {
        private bool impersonate;
        public bool Impersonate

        {
            set { impersonate = value; }
            get { return impersonate; }
        }

        public Global()
        {
            this.AuthenticateRequest += new EventHandler(Global_AuthenticateRequest);
        }

        static void Global_AuthenticateRequest(object sender, EventArgs e)
        {
            // Impersonate A windows User
            string Domain = Convert.ToString(ConfigurationManager.AppSettings["Domain"]);
            string DomainUser = Convert.ToString(ConfigurationManager.AppSettings["DomainUser"]);
            string DomainPassword = Convert.ToString(ConfigurationManager.AppSettings["DomainPassword"]);
            Impersonator i = new Impersonator(DomainUser, Domain, DomainPassword);
            i.Impersonate();

            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            HttpRequest request = context.Request;

            if (request.IsAuthenticated)
            {
                IIdentity identity = context.User.Identity;
                SAHLPrincipal principal = new SAHLPrincipal(identity);
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(principal);
                context.User = principal;

                // assign the aduser
                // principal.ADUser = spc.GetADUser(principal);
                HttpContext.Current.User = principal;
            }
            else
            {
                string user = String.Format(@"{0}:{1}", Thread.CurrentThread.ManagedThreadId.ToString(), DateTime.Now);
                SAHLPrincipal principal = new SAHLPrincipal(new GenericIdentity(user));
                // The reason for both entries being in here is to keep both in sync.
                // Due to this being done early in the process "Thread.CurrentPrincipal" will be 
                // synced to the value in "HttpContext.Current.User" once Global_AuthenticateRequest has been completed.
                // It is also good practise to keep both in sync all the time.
                HttpContext.Current.User = principal;
                Thread.CurrentPrincipal = principal;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            ActiveRecordHelper.InitialiseActiveRecord();
            
            // Initialise the Logging
            IMessageBus messageBus = new EasyNetQMessageBus(new EasyNetQMessageBusConfigurationProvider());
            LogPlugin.Logger = new MessageBusLogger(messageBus, new MessageBusLoggerConfiguration());
            
            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Starting SAHL Web Services");
        }

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
            LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, "UnhandledException", new Exception(e.ToString()),null);
		}


        protected void Application_End(object sender, EventArgs e)
        {
            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Ending SAHL Web Services");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }
    }
}