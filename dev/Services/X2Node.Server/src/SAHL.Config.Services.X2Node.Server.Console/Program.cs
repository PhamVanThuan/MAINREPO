using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Topshelf;
using SAHL.Core.Messaging.EasyNetQ;
using EasyNetQ;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace SAHL.Config.Services.X2.NodeServer.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

                var container = IoC.Initialize();

                HostFactory.Run(x =>
                {
                    x.Service<ServiceManager>(serviceConfigurator =>
                    {
                        serviceConfigurator.ConstructUsing(() => new ServiceManager());
                        serviceConfigurator.WhenStarted(serviceManager => serviceManager.StartServices());
                        serviceConfigurator.WhenStopped(serviceManager => serviceManager.StopServices());
                    });

                    x.RunAsLocalSystem();

                    x.SetDisplayName(ConfigurationManager.AppSettings["DisplayName"]);
                    x.SetDescription(ConfigurationManager.AppSettings["Description"]);
                    x.SetServiceName(ConfigurationManager.AppSettings["ServiceName"]);
                });

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            try
            {
                var source = string.Format("{0} - {1}", "SAHL.Config.Services.X2.NodeServer.Console", "FirstChanceException");
                var log = "Application";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(e.Exception.ToString());
                sb.AppendLine(System.Environment.NewLine);
                sb.AppendLine(new StackTrace(true).ToString());
                string eventInfo = sb.ToString();

                if (!System.Diagnostics.EventLog.SourceExists(source))
                    System.Diagnostics.EventLog.CreateEventSource(source, log);

                System.Diagnostics.EventLog.WriteEntry(source, eventInfo, EventLogEntryType.Error);
            }
            catch (Exception)
            {
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var source = string.Format("{0} - {1}", "SAHL.Config.Services.X2.NodeServer.Console", "UnhandledException");
                var log = "Application";

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(e.ExceptionObject.ToString());
                sb.AppendLine(System.Environment.NewLine);
                sb.AppendLine(new StackTrace(true).ToString());
                string eventInfo = sb.ToString();

                if (!System.Diagnostics.EventLog.SourceExists(source))
                    System.Diagnostics.EventLog.CreateEventSource(source, log);

                System.Diagnostics.EventLog.WriteEntry(source, eventInfo, EventLogEntryType.Error);
            }
            catch (Exception)
            {
            }
        }


    }


}
