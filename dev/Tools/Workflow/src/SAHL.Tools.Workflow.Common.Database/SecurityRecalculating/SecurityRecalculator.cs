using NHibernate;
using SAHL.Config.Process;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Providers;
using SAHL.Tools.Workflow.Common.Database.Properties;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Providers;
using StructureMap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using SAHL.Core.Services;

namespace SAHL.Tools.Workflow.Common.Database.SecurityRecalculating
{
    public class SecurityRecalculation : IDataModel, IDataModelWithIdentitySeed
    {
        public int ID { get; set; }

        public Int64 InstanceID { get; set; }

        public SecurityRecalculation(int iD, Int64 instanceID)
        {
            this.ID = iD;
            this.InstanceID = instanceID;
        }
    }

    public class SecurityRecalculator
    {
        public static List<long> FailedInstances = new List<long>();
        public static ConcurrentDictionary<Guid, X2RequestForSecurityRecalc> requestsSentForRecalculation = new ConcurrentDictionary<Guid, X2RequestForSecurityRecalc>();
        public static int NumberOfRequestsToProcess;
        public static int NumberOfRequestsComplete;
        private IServiceRequestMetadata serviceRequestMetadata;

        public SecurityRecalculator()
        {
            this.serviceRequestMetadata = new ServiceRequestMetadata(
                new Dictionary<string, string>() { { SAHL.Core.Services.ServiceRequestMetadata.HEADER_USERNAME, "X2" } });
        }

        public int Recalculate(string processName, string connectionString)
        {
            return Recalculate(processName, 0, 0, connectionString);
        }

        public int Recalculate(string processName, int processID, int oldProcessID, string connectionString)
        {
            IList<Int64> instancesToRecalculate = new List<Int64>();

            ISessionFactory factory = InitialiseActiveRecord(connectionString);

            using (ISession session = factory.OpenSession())
            {
                // get the missing parameters
                if (processName != null && (processID <= 0 || oldProcessID <= 0))
                {
                    processID = GetLatestProcessID(session, processName);
                    oldProcessID = GetPreviousProcessID(session, processName, processID);
                }
                if (String.IsNullOrEmpty(processName) && processID > 0)
                    processName = GetProcessName(session, processID);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(string.Format("Recalculate Security for '{0}' - ProcessID {1} : Old ProcessID {2}", processName, processID, oldProcessID));

                // write the affected instance records to the staging table
                using (ITransaction trans = session.BeginTransaction())
                {
                    instancesToRecalculate = CheckAndInsertInstancesToRecalculate(session, processID, oldProcessID, 3600);
                    trans.Commit();
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0} Instances require recalculating", instancesToRecalculate.Count);

                // do the recalc if required
                if (instancesToRecalculate.Count > 0)
                {
                    int minutesUntilTimeout = ConfigurationManager.AppSettings["MinutesToWaitUntilTimeout"] != null ?
                        Convert.ToInt32(ConfigurationManager.AppSettings["MinutesToWaitUntilTimeout"]) : 60;
                    var container = new ProcessBootstrapper().Initialise();
                    var statementProvider = container.GetInstance<AssemblyUIStatementProvider>();

                    DbContextConfiguration.Instance.RepositoryFactory = container.GetInstance<ISqlRepositoryFactory>();

                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Starting X2 Node Service For Recalc");
                    Console.ForegroundColor = ConsoleColor.White;

                    IX2RequestPublisher publisher = ObjectFactory.GetInstance<IX2RequestPublisher>();
                    StartService();

                    var workflowDataProvider = ObjectFactory.GetInstance<IWorkflowDataProvider>();

                    var queueNameBuilder = ObjectFactory.GetInstance<IX2QueueNameBuilder>();

                    NumberOfRequestsToProcess = instancesToRecalculate.Count;
                    DateTime currentTime = DateTime.Now.AddMinutes(minutesUntilTimeout);

                    Console.WriteLine("timeout in {0} minutes : {1}", minutesUntilTimeout, currentTime.ToString());

                    foreach (var instanceID in instancesToRecalculate)
                    {
                        InstanceDataModel instance = workflowDataProvider.GetInstanceDataModel(instanceID);
                        WorkFlowDataModel workflow = workflowDataProvider.GetWorkflowById(instance.WorkFlowID);
                        ProcessDataModel process = workflowDataProvider.GetProcessById(workflow.ProcessID);

                        var x2workflow = new X2Workflow(process.Name, workflow.Name);
                        var route = queueNameBuilder.GetSystemQueue(x2workflow);
                        var request = BuildRequest(instanceID, serviceRequestMetadata);
                        requestsSentForRecalculation.TryAdd(request.CorrelationId, request);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Publish recalc request for InstanceID : {0}", instanceID);
                        Console.ForegroundColor = ConsoleColor.White;
                        publisher.Publish<X2RequestForSecurityRecalc>(route, request);
                    }

                    while (requestsSentForRecalculation.Count > 0)
                    {
                        Console.WriteLine("{0} instances remaining.", requestsSentForRecalculation.Count);
                        Thread.Sleep(1000);
                        if (DateTime.Now >= currentTime)
                        {
                            Console.WriteLine("Maximum timeout expired - terminating wait for responses.");
                            if (requestsSentForRecalculation.Count > 0)
                            {
                                Console.WriteLine("No response was received for the following requests:");
                                foreach (var requestSent in requestsSentForRecalculation)
                                {
                                    Console.WriteLine("InstanceID: {0}  CorrelationId: {1}", requestSent.Value.InstanceId, requestSent.Value.CorrelationId);
                                }
                            }
                            break;
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine("Stopping X2 Node Service For Recalc");
                    Console.ForegroundColor = ConsoleColor.White;

                    StopService();

                    Thread.Sleep(1000);

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            return instancesToRecalculate != null ? instancesToRecalculate.Count : 0;
        }

        private static void StartService()
        {
            var processProvider = ObjectFactory.GetInstance<IX2ProcessProvider>();
            processProvider.Initialise();

            var x2RequestSubscriber = ObjectFactory.GetInstance<IX2RequestSubscriber>();
            x2RequestSubscriber.Initialise();

            var subscriber = ObjectFactory.GetInstance<IX2ResponseSubscriber>();
            subscriber.Subscribe<X2Response>(HandleResponse);

            var x2ConsumerManager = ObjectFactory.GetInstance<IX2ConsumerManager>();
            x2ConsumerManager.Initialise();
        }

        private static void StopService()
        {
            var x2ConsumerManager = ObjectFactory.GetInstance<IX2ConsumerManager>();
            x2ConsumerManager.TearDown();
        }

        private static void HandleResponse(X2Response response)
        {
            if (!requestsSentForRecalculation.ContainsKey(response.RequestID))
            {
                return;
            }
            if (response.IsErrorResponse)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error response - InstanceID {0}", response.InstanceId);
                Console.ForegroundColor = ConsoleColor.White;
                lock (FailedInstances)
                {
                    FailedInstances.Add(response.InstanceId);
                }
            }
            else
            {
                // delete recalculated instance from the [x2].[staging].SecurityRecalculation table
                using (var db = new Db().InWorkflowContext())
                {
                    db.DeleteWhere<SecurityRecalculation>("InstanceID = @InstanceID", new { InstanceID = response.InstanceId });
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Succesfull response - delete InstanceID {0} from staging table", response.InstanceId);
                    Console.ForegroundColor = ConsoleColor.White;
                    db.Complete();
                }
            }
            X2RequestForSecurityRecalc requestRemoved = null;
            if (!requestsSentForRecalculation.TryRemove(response.RequestID, out requestRemoved))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to remove the instance from the requests sent to be recalculated list");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static X2RequestForSecurityRecalc BuildRequest(long instanceId, IServiceRequestMetadata serviceRequestMetadata)
        {
            return new X2RequestForSecurityRecalc(Guid.NewGuid(), instanceId, serviceRequestMetadata);
        }

        #region Helper Methods

        public int GetLatestProcessID(ISession session, string processName)
        {
            int processID = 0;

            ISQLQuery query = session.CreateSQLQuery(string.Format("select top 1 ID from [x2].[x2].Process where [Name] = '{0}' order by ID desc", processName));
            processID = Convert.ToInt32(query.UniqueResult());

            return processID;
        }

        public int GetPreviousProcessID(ISession session, string processName, int latestProcessID)
        {
            int previousProcessID = 0;

            ISQLQuery query = session.CreateSQLQuery(string.Format("select top 1 ID from [x2].[x2].Process where [Name] = '{0}' and ID <> {1} order by ID desc", processName, latestProcessID));
            previousProcessID = Convert.ToInt32(query.UniqueResult());

            return previousProcessID;
        }

        public string GetProcessName(ISession session, int processID)
        {
            string processName = String.Empty;

            ISQLQuery query = session.CreateSQLQuery(string.Format("select [Name] from [x2].[x2].Process where [ID] = {0}", processID));
            processName = Convert.ToString(query.UniqueResult());

            return processName;
        }

        public IList<Int64> CheckAndInsertInstancesToRecalculate(ISession session, int processID, int oldProcessID, int timeout)
        {
            IList<Int64> instances = new List<Int64>();

            ISQLQuery query = session.CreateSQLQuery(Resources.CompareSecurity);
            query.SetParameter("NewProcessID", processID);
            query.SetParameter("OldProcessID", oldProcessID);
            query.SetTimeout(timeout);
            instances = query.List<Int64>();

            return instances;
        }

        private static ISessionFactory InitialiseActiveRecord(string connectionString)
        {
            NHibernateInitialiser ARInit = new NHibernateInitialiser(connectionString);
            return ARInit.InitialiseNHibernate();
        }

        #endregion Helper Methods
    }
}