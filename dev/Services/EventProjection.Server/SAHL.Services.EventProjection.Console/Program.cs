using EasyNetQ;
using SAHL.Core.IoC;
using SAHL.Core.Logging;
using SAHL.Core.Messaging.EasyNetQ;
using SAHL.Core.Services;
using SAHL.Services.EventProjection.Services;
using SAHL.Services.Interfaces.Communications;
using StructureMap;
using StructureMap.Pipeline;

namespace SAHL.Services.EventProjection.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ObjectFactory.Configure(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.WithDefaultConventions();
                    scan.LookForRegistries();
                });
                x.For<IRawLogger>().Use<NullRawLogger>();
                x.For<IRawMetricsLogger>().Use<NullMetricsRawLogger>();
            });

            ObjectFactory.Configure(x=>{
                x.For<IEasyNetQMessageBusSettings>().Singleton().Use<ShortNameEasyNetQMessageBusSettings>();
            });
            var startables = ObjectFactory.GetAllInstances<IStartable>();
            foreach (var startable in startables)
            {
                startable.Start();
            }

            var eventProjectionService = ObjectFactory.GetInstance<IEventProjectionService>();
            eventProjectionService.Start();
            System.Console.ReadLine();
        }
    }
}