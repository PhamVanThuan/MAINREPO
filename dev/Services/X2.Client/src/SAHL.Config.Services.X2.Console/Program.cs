using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SAHL.Config.Services.X2.Client;
using SAHL.Core.Services;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using EasyNetQ;
using EasyNetQ.Loggers;
using RabbitMQ.Client;
using SAHL.Core.X2.Messages.Management;
using SAHL.Core.SystemMessages;
using SAHL.X2Engine2.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using System.Linq;

namespace SAHL.Config.Services.X2.Console
{
    internal class Program
    {
        public enum CacheTypes
        {
            Lookups,
            LookupItem,
            UIStatement,
            DomainService
        }

        static ServiceRequestMetadata metadata = null;

        private static void CreateHelpDeskInstance()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            var fieldInputList = new Dictionary<string, string>();
            fieldInputList.Add("HelpDeskQueryKey", "0");
            fieldInputList.Add("CurrentConsultant", "SAHL\\BCUser");
            fieldInputList.Add("LegalEntityKey", "1130850");

            var activityName = "Create Request";
            var processName = "Help Desk";
            var workFlowName = "Help Desk";
            var ignoreWarnings = false;
            var data = new Object();

            var request = new SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), activityName, processName, workFlowName, metadata, ignoreWarnings, null, null, fieldInputList, null);
            var x2Response = client.PerformCommand(request);
            var response = request.Result;
            
            if (response.InstanceId > 0)
            {
                System.Console.WriteLine(response.InstanceId);
            }
            System.Console.ReadLine();
        }

        private static void CreateWorkflowInstance()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            var fieldInputList = new Dictionary<string, string>();
            fieldInputList.Add("ApplicationKey", "998877");
            fieldInputList.Add("isEstateAgentApplication", "False");

            var activityName = "Create Instance";
            var processName = "Origination";
            var workFlowName = "Application Capture";
            var ignoreWarnings = false;
            var data = new Object();

            SAHL.Core.X2.Messages.X2CreateInstanceRequest request = new SAHL.Core.X2.Messages.X2CreateInstanceRequest(Guid.NewGuid(), activityName, processName, workFlowName, metadata, ignoreWarnings, null, null, fieldInputList, null);
            var x2Response = client.PerformCommand(request);

            System.Console.WriteLine("Got Response");
            var response = request.Result;
            if (response.InstanceId > 0)
            {
                System.Console.WriteLine(response.InstanceId);
                var instanceId = response.InstanceId;
                var mapVariables = new Dictionary<string, string>();
                var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityName, false, mapVariables);
                client.PerformCommand(completeRequest);
                if (completeRequest.Result.IsErrorResponse)
                {
                    var errorResponse = request.Result as X2ErrorResponse;
                    string BP = "Err";
                }
            }

            System.Console.WriteLine("complete....");

        }

        private static void CreateWithComplete_WorkflowInstance()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            var fieldInputList = new Dictionary<string, string>();
            fieldInputList.Add("ApplicationKey", "1768271");
            fieldInputList.Add("isEstateAgentApplication", "False");
            fieldInputList.Add("LeadType", "1");
            fieldInputList.Add("CaseOwnerName", "SAHL\\BCUser2");

            var activityName = "Create Instance";
            var processName = "Origination";
            var workFlowName = "Application Capture";
            var ignoreWarnings = false;
            var data = new Object();

            SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest request = new SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), activityName, processName, workFlowName, metadata, ignoreWarnings, null, null, fieldInputList, null);
            var x2Response = client.PerformCommand(request);

            System.Console.WriteLine("Got Response");
            var response = request.Result;
            System.Console.WriteLine("complete.... {0}", response);
        }

        private static void CreateWorkflowInstanceForV3()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            var fieldInputList = new Dictionary<string, string>();
            fieldInputList.Add("ApplicationKey", "0");
            fieldInputList.Add("Reference", "Some test case");

            var activityName = "Create Disability Claim";
            var processName = "LifeClaims";
            var workFlowName = "Disability Claim";
            var ignoreWarnings = false;
            var data = new Object();

            var request = new SAHL.Core.X2.Messages.X2CreateInstanceWithCompleteRequest(Guid.NewGuid(), activityName, processName, workFlowName, metadata, ignoreWarnings, null, null, fieldInputList, null);
            var x2Response = client.PerformCommand(request);

            var response = request.Result;
            if (response.InstanceId > 0)
            {
                System.Console.WriteLine(response.InstanceId);
            }


           System.Console.ReadLine();
        }


        private static void CreateWorkflowInstanceForTestMap()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            var fieldInputList = new Dictionary<string, string>();
            fieldInputList.Add("ApplicationKey", "111999");
            fieldInputList.Add("Reference", "Demo test case");

            var activityName = "Create Case";
            var processName = "TestMap";
            var workFlowName = "TestMap";
            var ignoreWarnings = false;
            var data = new Object();

            SAHL.Core.X2.Messages.X2CreateInstanceRequest request = new SAHL.Core.X2.Messages.X2CreateInstanceRequest(Guid.NewGuid(), activityName, processName, workFlowName, metadata, ignoreWarnings, null, null, fieldInputList, null);
            var x2Response = client.PerformCommand(request);

            System.Console.WriteLine("Got Response");
            var response = request.Result;
            if (response.InstanceId > 0)
            {
                System.Console.WriteLine(response.InstanceId);
                var instanceId = response.InstanceId;
                var mapVariables = new Dictionary<string, string>();
                var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityName, false, mapVariables);
                client.PerformCommand(completeRequest);
                if (completeRequest.Result.IsErrorResponse)
                {
                    var errorResponse = request.Result as X2ErrorResponse;
                    System.Console.WriteLine("something bad has happened....!!!!");
                }
            }

            System.Console.ReadLine();
        }

        private static void UserStartActivity()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            long instanceId = 8536295;

            var fieldInputList = new Dictionary<string, string>();

            var activityName = "Submit Application";
            var ignoreWarnings = false;
            var data = new Object();

            SAHL.Core.X2.Messages.X2RequestForExistingInstance request = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.UserStart, metadata, activityName, ignoreWarnings, fieldInputList, null);
            var x2Response = client.PerformCommand(request);

            System.Console.WriteLine("Got Response");
            var response = request.Result;
            if (response.InstanceId > 0)
            {
                System.Console.WriteLine(response.InstanceId);
                var mapVariables = new Dictionary<string, string>();
                var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityName, false, mapVariables);
                client.PerformCommand(completeRequest);
                if (completeRequest.Result.IsErrorResponse)
                {
                    var errorResponse = request.Result as X2ErrorResponse;
                    System.Console.WriteLine("something bad has happened....!!!!");
                }
            }

            System.Console.ReadLine();
        }

        private static void PublishUserStartActivity()
        {
            long instanceId = 8536295;

            var fieldInputList = new Dictionary<string, string>();

            var activityName = "Submit Application";
            var aDUserName = "SAHL\\BCUser2";
            var ignoreWarnings = false;
            var data = new Object();

            SAHL.Core.X2.Messages.X2RequestForExistingInstance request = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.UserStart, metadata, activityName, ignoreWarnings, fieldInputList, null);

            var connectionString = String.Format("host={0};username={1};password={2};requestedHeartbeat=0", "localhost", "admin", "W0rdpass");
            var logger = new ConsoleLogger();
            var bus = RabbitHutch.CreateBus(connectionString, x => x.Register<IEasyNetQLogger>(_ => logger));
            var advancedServiceBus = bus.Advanced;
            var exchange = advancedServiceBus.ExchangeDeclare("x2.origination.application_capture.user", ExchangeType.Direct);

            Thread.Sleep(2000);

            advancedServiceBus.Publish(exchange, "#", false, false, new Message<X2WrappedRequest>(new X2WrappedRequest(request)));

            System.Console.ReadLine();
        }


        private static void RefreshCache()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

            IX2NodeManagementMessage message = new X2NodeManagementMessage(X2ManagementType.RefreshCache, CacheTypes.DomainService.ToString());
            var x2Response = client.PerformCommand(message);
            System.Console.ReadLine();
        }

        private static void Main(string[] args)
        {
            try
            {
                TestErrorMessages();
                //RefreshCache();
                //CreateWithComplete_WorkflowInstance();
                //PublishUserStartActivity();
                //UserStartActivity();
                //CreateWorkflowInstanceForTestMap();
                //CreateWorkflowInstance();
                //CreateWorkflowInstanceForV3();
                //Task.Run(() =>
                //{
                //});

                //IServiceUrlConfigurationProvider serviceConfigurationProvider = new ServiceUrlConfigurationProvider("localhost/x2service/");
                //IJsonActivator jsonActivator = new JsonActivator();

                //X2ServiceClient client = new X2ServiceClient(serviceConfigurationProvider, jsonActivator);

                //string json = "{\"$type\":\"SAHL.Core.X2.Messages.X2SystemRequestGroup, SAHL.Core.X2\",\"ActivityNames\":{\"$type\":\"System.Collections.Generic.List`1[[System.String, mscorlib]], mscorlib\",\"$values\":[\"90 day timer\"]},\"ActivityTime\":\"2014-01-27T08:58:52.813\",\"Id\":\"00000000-0000-0000-0000-000000000000\",\"RequestType\":16,\"CorrelationId\":\"10fc530a-f7ee-40cc-a250-2fadc47b6a4a\",\"UserName\":\"X2\",\"MapVariables\":null,\"InstanceId\":6635425,\"IgnoreWarnings\":true,\"Data\":null,\"Result\":null}\"";
                //X2SystemRequestGroup srg = new X2SystemRequestGroup(Guid.NewGuid(), "X2", X2RequestType.SystemRequestGroup, 6635425, new List<string> { "90 day timer" }, DateTime.Parse("2014-01-27T08:58:52.813"));
                ////var sysrequest = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                //var resp = client.PerformCommand(srg, metadata);

                //while (true)
                //{
                //    Thread.Sleep(100);
                //    try
                //    {
                //        string mapName = "Application Capture";
                //        string processName = "Origination";
                //        string activityName = "Submit Application";
                //        string user = @"SAHL\BCUser";
                //        var request = new X2RequestForExistingInstance(Guid.NewGuid(), 89260, X2RequestType.UserStart, user, activityName, false);

                //        System.Console.WriteLine("Send Create - push enter");
                //        System.Console.ReadLine();
                //        var messaages = client.PerformCommand<X2RequestForExistingInstance>(request, metadata);
                //        System.Console.WriteLine("Got Response");
                //        var response = request.Result;
                //        if (response.InstanceId > 0)
                //        {
                //            System.Console.WriteLine(response.InstanceId);
                //            var instanceId = response.InstanceId;
                //            var mapVariables = new Dictionary<string, string>();
                //            //mapVariables.Add("ApplicationKey", "1435634");
                //            var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.UserComplete, user, activityName, false, mapVariables);
                //            client.PerformCommand(completeRequest, metadata);
                //            if (completeRequest.Result.IsErrorResponse)
                //            {
                //                string BP = "Err";
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        System.Console.WriteLine(ex.ToString());
                //    }
                //}
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            //WorkflowScenario();
            //CloneWakeupParent();
            //CommonFlagOnMainInstance();
            //UserCloneCreated();
            //AutoForwardToState();
            //ClearCache();
        }

        private static void WorkflowScenario()
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
                IJsonActivator jsonActivator = new JsonActivator();
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

                while (true)
                {
                    try
                    {
                        string mapName = "CommonFlagOnMainInstance";
                        string processName = "CommonFlagOnMainInstance";
                        string activityForCreate = "Create Instance";
                        string user = @"SAHL\PaulC";
                        var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, mapName, metadata, false);
                        System.Console.WriteLine("Send Create");
                        var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);
                        System.Console.WriteLine("Got Response");
                        var response = request.Result;
                        if (response.InstanceId > 0)
                        {
                            System.Console.WriteLine(response.InstanceId);
                            var instanceId = response.InstanceId;
                            var mapVariables = new Dictionary<string, string>();
                            //mapVariables.Add("ApplicationKey", "1435634");
                            var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables);
                            client.PerformCommand(completeRequest);
                            if (completeRequest.Result.IsErrorResponse)
                            {
                                string BP = "Err";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    System.Console.WriteLine("push to send again");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static void CloneWakeupParent()
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
                IJsonActivator jsonActivator = new JsonActivator();
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
                while (true)
                {
                    try
                    {
                        string mapName = "CloneWakeUpParent";
                        string processName = "CloneWakeUpParent";
                        string activityForCreate = "Create Instance";
                        string user = @"SAHL\PaulC";
                        var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, mapName, metadata, false);

                        System.Console.WriteLine("Send Create");
                        var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);
                        System.Console.WriteLine("Got Response");
                        var response = request.Result;
                        if (response.InstanceId > 0)
                        {
                            System.Console.WriteLine(response.InstanceId);
                            var instanceId = response.InstanceId;
                            var mapVariables = new Dictionary<string, string>();
                            //mapVariables.Add("ApplicationKey", "1435634");
                            var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables);
                            client.PerformCommand(completeRequest);
                            if (completeRequest.Result.IsErrorResponse)
                            {
                                string BP = "Err";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    System.Console.WriteLine("push to send again");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static void UserCloneCreated()
        {
            IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
            IJsonActivator jsonActivator = new JsonActivator();
            X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
            try
            {
                string mapName = "UserCloneCreated";
                string processName = "UserCloneCreated";
                string activityForCreate = "Create Instance";
                string user = @"SAHL\PaulC";
                var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, mapName, metadata, false);

                System.Console.WriteLine("Send Create");
                var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);
                System.Console.WriteLine("Got Response");
                var response = request.Result;
                if (response.InstanceId > 0)
                {
                    System.Console.WriteLine(response.InstanceId);
                    var instanceId = response.InstanceId;
                    var mapVariables = new Dictionary<string, string>();
                    //mapVariables.Add("ApplicationKey", "1435634");
                    var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables);
                    client.PerformCommand(completeRequest);
                    if (completeRequest.Result.IsErrorResponse)
                    {
                        string BP = "Err";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static void CommonFlagOnMainInstance()
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
                IJsonActivator jsonActivator = new JsonActivator();
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
                while (true)
                {
                    try
                    {
                        string mapName = "CommonFlagOnMainInstance";
                        string processName = "CommonFlagOnMainInstance";
                        string activityForCreate = "Create Instance";
                        string user = @"SAHL\PaulC";
                        var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, mapName, metadata, false);

                        System.Console.WriteLine("Send Create");
                        var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);
                        System.Console.WriteLine("Got Response");
                        var response = request.Result;
                        if (response.InstanceId > 0)
                        {
                            System.Console.WriteLine(response.InstanceId);
                            var instanceId = response.InstanceId;
                            var mapVariables = new Dictionary<string, string>();
                            //mapVariables.Add("ApplicationKey", "1435634");
                            var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables);
                            client.PerformCommand(completeRequest);
                            if (completeRequest.Result.IsErrorResponse)
                            {
                                string BP = "Err";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    System.Console.WriteLine("push to send again");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static void AutoForwardToState()
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
                IJsonActivator jsonActivator = new JsonActivator();
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);

                while (true)
                {
                    try
                    {
                        string mapName = "AutoForwardToState";
                        string processName = "AutoForwardToState";
                        string activityForCreate = "Create Instance";
                        string user = @"SAHL\PaulC";
                        var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, mapName, metadata, false);

                        System.Console.WriteLine("Send Create");
                        var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);
                        System.Console.WriteLine("Got Response");
                        var response = request.Result;
                        if (response.InstanceId > 0)
                        {
                            System.Console.WriteLine(response.InstanceId);
                            var instanceId = response.InstanceId;
                            var mapVariables = new Dictionary<string, string>();
                            //mapVariables.Add("ApplicationKey", "1435634");
                            var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables);
                            client.PerformCommand(completeRequest);
                            if (completeRequest.Result.IsErrorResponse)
                            {
                                string BP = "Err";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    System.Console.WriteLine("push to send again");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static void PeronalLoan()
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
                IJsonActivator jsonActivator = new JsonActivator();
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
                while (true)
                {
                    try
                    {
                        string mapName = "Personal Loans";
                        string processName = "Personal Loan";
                        string activityForCreate = "Create Personal Loan Lead";
                        string user = @"SAHL\PaulC";
                        var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, mapName, metadata, false);
                        System.Console.WriteLine("Send Create");
                        var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);
                        System.Console.WriteLine("Got Response");
                        var response = request.Result;
                        if (response.InstanceId > 0)
                        {
                            System.Console.WriteLine(response.InstanceId);
                            var instanceId = response.InstanceId;
                            var mapVariables = new Dictionary<string, string>();
                            mapVariables.Add("ApplicationKey", "1435634");
                            var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables);
                            client.PerformCommand(completeRequest);
                            if (completeRequest.Result.IsErrorResponse)
                            {
                                string BP = "Err";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    System.Console.WriteLine("push to send again");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private static void ClearCache()
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service");
                IJsonActivator jsonActivator = new JsonActivator();
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
                while (true)
                {
                    try
                    {
                        IDictionary<string, string> processesToClear = new Dictionary<string, string>();

                        processesToClear.Add("Life", "LifeOrigination"); // worked
                        processesToClear.Add("Personal Loan", "Personal Loans"); // falls over
                        processesToClear.Add("Debt Counselling", "Debt Counselling"); // falls over
                        processesToClear.Add("Loan Adjustments", "Loan Adjustments"); // falls over
                        processesToClear.Add("CAP2 Offers", "CAP2 Offers");
                        processesToClear.Add("Help Desk", "Help Desk");
                        processesToClear.Add("Origination", "Application Capture"); // worked

                        foreach (var item in processesToClear)
                        {
                            string processName = item.Key;
                            string workflowName = item.Value;
                            string activityForCreate = "Clear Cache";
                            string user = @"SAHL\CraigF";

                            // setup the data to pass thru
                            CacheTypes cacheType = CacheTypes.Lookups;
                            string lookUpTableName = "";

                            string data = cacheType.ToString();
                            if (!String.IsNullOrEmpty(lookUpTableName))
                                data += "," + lookUpTableName;

                            var request = new X2CreateInstanceRequest(Guid.NewGuid(), activityForCreate, processName, workflowName, metadata, false, null, null, null, data);
                            System.Console.WriteLine("Send X2CreateInstanceRequest : {0}", workflowName);
                            var messaages = client.PerformCommand<X2CreateInstanceRequest>(request);

                            var response = request.Result;
                            System.Console.WriteLine("Got Response. InstanceID = {0}", response.InstanceId);

                            if (response.InstanceId > 0)
                            {
                                var instanceId = response.InstanceId;
                                var mapVariables = new Dictionary<string, string>();
                                var completeRequest = new X2RequestForExistingInstance(Guid.NewGuid(), instanceId, X2RequestType.CreateComplete, metadata, activityForCreate, false, mapVariables, data);
                                System.Console.WriteLine("Send X2RequestForExistingInstance");
                                client.PerformCommand(completeRequest);
                                System.Console.WriteLine("Got Response");

                                if (completeRequest.Result.IsErrorResponse)
                                {
                                    string BP = "Err";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine(ex.ToString());
                    }
                    System.Console.WriteLine("push to send again");
                    System.Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }


        private static void TestErrorMessages()
        {
            ISystemMessageCollection messages = new MessageCollectionFactory().CreateEmptyCollection();
            DoWork((IDomainMessageCollection)messages);
            if (messages.AllMessages.Count() == 0)
            {
                System.Console.WriteLine("no errors");
            }

        }

        private static void DoWork(IDomainMessageCollection messages)
        {
            messages.Add(new Error("test","test"));
        }
    }
}