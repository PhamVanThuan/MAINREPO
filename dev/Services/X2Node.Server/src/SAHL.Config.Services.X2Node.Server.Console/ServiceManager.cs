using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using SAHL.Core.IoC;

namespace SAHL.Config.Services.X2.NodeServer.Console
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
            var startables = ObjectFactory.GetAllInstances<IStartable>();
            foreach (var startable in startables)
            {
                try
                {
                    startable.Start();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

            var startableServices = container.GetAllInstances<IStartableService>();
            foreach (var services in startableServices)
            {
                try
                {
                    System.Console.WriteLine("Starting Startables");
                    services.Start();

                }
                catch (Exception)
                {
                    
                    throw;
                }
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
