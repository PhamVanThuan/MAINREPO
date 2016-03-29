using SAHL.Batch.Common;
using StructureMap;
using Topshelf;

namespace SAHL.Batch.Service
{   
    internal class Program
    {
        private static void Main(string[] args)
        {
            SAHL.Batch.Service.DependencyResolution.BootStrapper.Initialize();

            HostFactory.Run(hostConfigurator =>
                {
                    hostConfigurator.Service<IBatchServiceManager>(serviceConfigurator =>
                        {
                            serviceConfigurator.ConstructUsing(() => ObjectFactory.GetInstance<IBatchServiceManager>());
                            serviceConfigurator.WhenStarted(batchService => batchService.StartQueueHandlers());
                            serviceConfigurator.WhenStopped(batchService => batchService.StopQueueHandlers());
                        });

                    hostConfigurator.RunAsLocalSystem();

                    hostConfigurator.SetDisplayName("SAHL.Batch.Service");
                    hostConfigurator.SetDescription("Batch processing by consuming messages of a queue");
                    hostConfigurator.SetServiceName("SAHL.Batch.Service");
                });
        }
    }
}