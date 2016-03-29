using SAHL.Config.Services;
using SAHL.Config.Services.Core;
using SAHL.Core;
using System;

namespace SAHL.Services.Cuttlefish.Console
{
    internal class Program
    {
        private static IServiceBootstrapper serviceBootstrapper;
        private static IIocContainer container;

        private static void Main(string[] args)
        {
            if (serviceBootstrapper == null)
            {
                serviceBootstrapper = new ServiceBootstrapper();
            }

            container = serviceBootstrapper.Initialise();

            var serviceManager = container.GetInstance<IServiceManager>();
            if (serviceManager == null)
            {
                throw new NullReferenceException("Windows Service Manager not found");
            }

            serviceManager.StartService();

            System.Console.ReadLine();
        }
    }
}