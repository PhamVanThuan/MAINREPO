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

namespace SAHL.Config.Services.X2.NodeServer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }


}
