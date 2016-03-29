using System;
using System.Collections.Generic;
using System.Threading;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Messaging;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Node;
using SAHL.X2Engine2.Providers;
using StructureMap;
using SAHL.Core.Logging;

namespace SAHL.X2Engine2.Console
{
    internal class Program
    {
        private static IX2Engine engine = null;
        private static IX2EngineNode node = null;
        private static ManualResetEvent mre = new ManualResetEvent(false);

        private static void DoNode()
        {
            node = ObjectFactory.GetInstance<IX2EngineNode>();
            node.Initialise();
            System.Console.WriteLine("Node up");
            //Thread engineThread = new Thread(new ThreadStart(DoEngine));
            //engineThread.Name = "Engine Thread";
            //engineThread.Start();

            while (!mre.WaitOne(1, false))
            {
            }
            node.Teardown();
        }

        private static void DoEngine()
        {
            engine = ObjectFactory.GetInstance<IX2Engine>();
            engine.Initialise();
        }

        private static void Main(string[] args)
        {
            IoC.Initialize();
            ISystemMessageCollection messages = new SystemMessageCollection();
            messages.AddMessage(new SystemMessage("error", SystemMessageSeverityEnum.Error));
            DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
            var statementProvider = ObjectFactory.GetInstance<AssemblyUIStatementProvider>();
            var testLoggerAppSource = new LoggerAppSourceFromImplicitSource("TestLoggerAppSource");
            DbContextConfiguration.Instance.RepositoryFactory = new DapperSqlRepositoryFactory(statementProvider,
                new Logger(new NullRawLogger(), new NullMetricsRawLogger(), testLoggerAppSource, new MetricTimerFactory()),
                new LoggerSource("StatementTests", LogLevel.Error, false));

            //Thread nodeThread = new Thread(new ThreadStart(DoNode));
            //nodeThread.Name = "Node Thread";
            //nodeThread.Start();

            System.Console.WriteLine("Key To send");
            System.Console.ReadLine();

            while (true)
            {
                string userName = @"sahl\PaulC", processName = "CreateInstance", workflowName = "CreateInstance", activityForCreate = "Create Instance";
                //string userName = @"sahl\PaulC", processName = "MultipleDecisions", workflowName = "MultipleDecisions", activityForCreate = "Create Instance";
                var requestPublisher = ObjectFactory.GetInstance<IX2RequestPublisher>();
                var requestSubscriber = ObjectFactory.GetInstance<IX2RequestSubscriber>();
                var workflowDataProvider = ObjectFactory.GetInstance<IWorkflowDataProvider>();
                //var numberOfMessagesToSend = 500;
                var messageBus = ObjectFactory.GetInstance<IMessageBusAdvanced>();
                List<long> instanceIds = new List<long>();
                messageBus.Subscribe<X2Response>((response) =>
                {
                    System.Console.WriteLine(response.InstanceId);
                    var request = new X2RequestForExistingInstance(Guid.NewGuid(), response.InstanceId, X2RequestType.CreateComplete, null, activityForCreate, false);
                    if (!response.IsErrorResponse &&
                         request != null &&
                         !instanceIds.Contains(request.InstanceId))
                    {
                        instanceIds.Add(request.InstanceId);
                        requestPublisher.Publish<X2RequestForExistingInstance>(new X2RouteEndpoint("EDNode", new QueueNameConstructor().GenerateQueueName(processName, workflowName)), request);
                    }
                });
                //Parallel.For(0, 1000, (numberInQueue) =>
                //{
                //    Thread.Sleep(new Random().Next(0, 1000));
                requestPublisher.Publish<X2CreateInstanceRequest>(new X2RouteEndpoint("EDNode", new QueueNameConstructor().GenerateQueueName(processName, workflowName)),
                    new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, workflowName, null, false));
                //});
                System.Console.WriteLine("Key To send more");
                System.Console.ReadLine();
            }

            System.Console.ReadLine();

            mre.Set();
        }

        private static readonly Random getrandom = new Random();
        private static readonly object syncLock = new object();

        private static long GetRandom()
        {
            lock (syncLock)
            { // synchronize
                return getrandom.Next();
            }
        }
    }
}