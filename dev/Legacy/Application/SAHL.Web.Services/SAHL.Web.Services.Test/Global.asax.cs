using System;
using System.Reflection;
using SAHL.Common.Logging;
using SAHL.Common.Web;
using SAHL.Communication;

namespace SAHL.Web.Services.Test
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            ActiveRecordHelper.InitialiseActiveRecord();

            // Initialise the Logging
            IMessageBus messageBus = new EasyNetQMessageBus(new EasyNetQMessageBusConfigurationProvider());
            LogPlugin.Logger = new MessageBusLogger(messageBus, new MessageBusLoggerConfiguration());

            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Starting SAHL Web Services Test");
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogPlugin.Logger.LogErrorMessageWithException(MethodInfo.GetCurrentMethod().Name, "UnhandledException", new Exception(e.ToString()), null);
        }


        protected void Application_End(object sender, EventArgs e)
        {
            LogPlugin.Logger.LogInfoMessage(MethodInfo.GetCurrentMethod().Name, "Ending SAHL Web Services Test");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }
    }
}