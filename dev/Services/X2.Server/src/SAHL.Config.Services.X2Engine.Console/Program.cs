using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using SAHL.X2Engine2;
using SAHL.X2Engine2.Node;
using System.Configuration;
using StructureMap;
using SAHL.Core.IoC;

namespace SAHL.Config.Services.X2Engine.Console
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
