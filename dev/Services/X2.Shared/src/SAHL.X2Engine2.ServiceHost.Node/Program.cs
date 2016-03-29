using EasyNetQ;
using SAHL.Config.Core.Conventions;
using SAHL.Core.Caching;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Messaging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages.Management;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Node;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Threading;
using Topshelf;
using SAHL.Core.Logging;
using System.Linq;
using System.Diagnostics;
using SAHL.Config.Services.Core.Conventions;

namespace SAHL.X2Engine2.ServiceHost.Node
{
    public class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            string serviceName = "X2EngineNode";
            StructureMap.IContainer container = null;

            container = IoC.Initialize();

            try
            {
                var node = container.GetInstance<IX2EngineNode>();
            }
            catch (Exception ex)
            {
                int a = 0;
            }
            var statementProvider = ObjectFactory.GetInstance<AssemblyUIStatementProvider>();
            var testLoggerAppSource = new LoggerAppSourceFromImplicitSource("TestLoggerAppSource");
            DbContextConfiguration.Instance.RepositoryFactory = new DapperSqlRepositoryFactory(statementProvider,
                new Logger(new NullRawLogger(), new NullMetricsRawLogger(), testLoggerAppSource, new MetricTimerFactory()),
                new LoggerSource("StatementTests", LogLevel.Error, false));

			var handler = ObjectFactory.GetInstance<IServiceCommandHandler<HandleMapReturnCommand>>();
			//var messages = handler.HandleCommand(new HandleMapReturnCommand(true, new SystemMessageCollection(), "", WorkflowMapCodeSectionEnum.OnStart, true));
            //Thread t = new Thread(new ThreadStart(Do));
            //t.Start();
            HostFactory.Run(x =>
            {
                x.Service<IX2EngineNode>(s =>
                {
                    s.ConstructUsing(name => container.GetInstance<IX2EngineNode>());
                    s.WhenStarted(tc => tc.Initialise());
                    s.WhenStopped(tc => tc.Teardown());
                });
                x.RunAsLocalSystem();

                x.SetDescription(serviceName);
                x.SetDisplayName(serviceName);
                x.SetServiceName(serviceName);
            });

            Console.ReadKey();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.ExceptionObject);
        }

        static void Do()
        {

            var bus = EasyNetQ.RabbitHutch.CreateBus("host=localhost");
            while (true)
            {
                Console.WriteLine("key to send metric");
                Console.ReadLine();
                try
                {
                    // no idea what this is meant to be displaying so I commented it out - ed (18/08/2014)
                    //ILogger metrics = ObjectFactory.GetInstance<ILogger>();
                    //var metricsSource = ObjectFactory.GetInstance<ILoggerSource>();

                    //metrics.GetThreadLocalStore()[SAHL.Core.Logging.Logger.CORRELATIONID] = Guid.NewGuid();
                    //metrics.GetThreadLocalStore()[SAHL.Core.Logging.Logger.THREADID] = Thread.CurrentThread.ManagedThreadId;
                    //metrics.LogLatencyMetric(metricsSource, "UserName", "", DateTime.Now, new TimeSpan(0, 0, 1));
                    //IDictionary<string, object> tls = metrics.GetThreadLocalStore();

                    //var message = new LatencyMetricMessage("Test app", "Source", DateTime.Now, new TimeSpan(0, 0, 1), "UserName", metrics.GetThreadLocalStore());
                    //messageBus.Publish<LatencyMetricMessage>(message);
                    //var logMessage = new SAHL.Core.Logging.Messages.ErrorLoggingMessage("X2NODE", "Error", "Method", "Source", "X2", metrics.GetThreadLocalStore());
                    //messageBus.Publish(logMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }
    }

    public static class IoC
    {
        public static StructureMap.IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                    scan.WithDefaultConventions();
                    scan.Convention<CommandHandlerDecoratorConvention>();
                    scan.ConnectImplementationsToTypesClosing(typeof(ICacheKeyGeneratorFactory<>));
                    scan.LookForRegistries();
                });
            });
            return ObjectFactory.Container;
        }
    }
}