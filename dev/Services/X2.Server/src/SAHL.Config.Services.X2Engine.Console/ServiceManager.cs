using SAHL.Core.IoC;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Config.Services.X2Engine.Console
{
    public class ServiceManager
    {
        private IContainer container;

        public ServiceManager()
        {
            container = ObjectFactory.Container;
        }

        public void StartServices()
        {
            System.Console.WriteLine("Starting Service");
            var startableServices = container.GetAllInstances<IStartableService>();
            foreach (var services in startableServices)
            {
                System.Console.WriteLine("Starting Startables");
                services.Start();
            }
        }

        public void StopServices()
        {
            System.Console.WriteLine("Stopping");
            var stoppableServices = container.GetAllInstances<IStoppableService>();
            foreach (var services in stoppableServices)
            {
                services.Stop();
            }

        }
    }
}
