using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using SAHL.Core.SystemMessages;
using SAHL.DecisionTree.Shared;
using SAHL.DecisionTree.Shared.Core;
using SAHL.Services.DecisionTreeDesign.Managers.DecisionTree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace SAHL.Services.DecisionTreeDesign
{
    [HubName("decisionTreeDebugHub")]
    [Authorize]
    public class DecisionTreeDebugHub : Hub
    {
        IDecisionTreeManager internalTreeDataAccessManager;
        IDecisionTreeManager treeDataAccessManager
        {
            get
            {
                if (internalTreeDataAccessManager == null)
                {
                    internalTreeDataAccessManager = StructureMap.ObjectFactory.GetInstance<IDecisionTreeManager>();
                }
                return internalTreeDataAccessManager;
            }
        }

        private dynamic debugSessionId;



        private dynamic testSuiteSessionDetails;

        private SessionDebugStatus DebugStatus
        {
            get
            {
                var status = SessionDebugStatus.Indeterminate;
                if (debugSessionId != null)
                {
                    status = SessionDebugStatus.Debug;
                }
                else if (testSuiteSessionDetails.sessionId != null)
	            {
                    status = SessionDebugStatus.TestSuite;
	            }
                return status;
                
            }
        }

        public static ConcurrentDictionary<Guid, TreeProcessingContext> connectedSessions = new ConcurrentDictionary<Guid, TreeProcessingContext>();

        public void StartDebugSession(dynamic sessionData)
        {

            TreeProcessingContext treeDebugContext = GetNewDebugSessionContext(sessionData);
            
            try
            {
                treeDebugContext.PopulateTreeWithDebugValues(sessionData.input_variables);
                RunToNextBreakPoint(sessionData, treeDebugContext);
            }
            catch (Exception ex)
            {
                Clients.Caller.onDebugError(sessionData.debugSessionId, ex.Message);
                StopDebugSession(sessionData);
            }
        }

        public void StartDebugSessionAndStepOver(dynamic sessionData)
        {
            var debugSessionId = sessionData.debugSessionId;
            TreeProcessingContext treeDebugContext = GetNewDebugSessionContext(sessionData);

            try
            {
                treeDebugContext.PopulateTreeWithDebugValues(sessionData.input_variables);
                int? currentNodeId = treeDebugContext.StepOverNode(Guid.Parse(Convert.ToString(debugSessionId)));

                object[] output_variables = treeDebugContext.ExtractOutputsArray();
                object[] messages = treeDebugContext.ExtactMessagesArray();
                Clients.Caller.onExecutionStoppedAtNode(sessionData.debugSessionId, currentNodeId, output_variables, messages);
            }
            catch (Exception)
            {
                Clients.Caller.onDebugError(sessionData.debugSessionId, "Input variables have not been set!");
                StopDebugSession(sessionData);
            }
        }

        public void StepOverNode(dynamic debugSessionId)
        {
            int? previousNodeId;
            int? nextNodeId;
            Guid sessionGuid = Guid.Parse(Convert.ToString(debugSessionId));

            TreeProcessingContext treeDebugContext;
            if (connectedSessions.TryGetValue(sessionGuid, out treeDebugContext))
            {
                treeDebugContext.Paused = false;
                previousNodeId = treeDebugContext.CurrentNodeId;
                nextNodeId = treeDebugContext.StepOverNode(sessionGuid);
                object[] output_variables = treeDebugContext.ExtractOutputsArray();
                object[] messages = treeDebugContext.ExtactMessagesArray();

                if (treeDebugContext.ExecutionCompleted)
                {
                    treeDebugContext.ResetContext();

                    Clients.Caller.onExecutionCompleted(debugSessionId, output_variables, messages);
                    Clients.Caller.onDebugSessionEnded(debugSessionId);
                    StopDebugSession(new { debugSessionId = debugSessionId });
                }
                else
                {
                    Clients.Caller.onExecutionStoppedAtNode(debugSessionId, previousNodeId, output_variables, messages);
                }
            }
        }

        public void ContinueDebugSession(dynamic sessionData)
        {
            TreeProcessingContext treeDebugContext;
            var debugSessionId = sessionData.debugSessionId;

            if (connectedSessions.TryGetValue(Guid.Parse(Convert.ToString(debugSessionId)), out treeDebugContext))
            {
                treeDebugContext.Paused = false;
                RunToNextBreakPoint(sessionData, treeDebugContext);
            }
            else
            {
                Clients.Caller.onDebugError(sessionData.debugSessionId, "Decison Tree execution problem occured, please contact the service administrator.");
            }
        }

        public void Pause(dynamic debugSessionId)
        {
            Guid debugSessionGuid = Guid.Parse(Convert.ToString(debugSessionId));

            TreeProcessingContext treeDebugContext;
            if (connectedSessions.TryGetValue(debugSessionGuid, out treeDebugContext))
            {
                connectedSessions.AddOrUpdate(debugSessionGuid, treeDebugContext,
                    (key, oldTreeDebugContext) =>
                    {
                        if (!oldTreeDebugContext.Paused)
                        {
                            oldTreeDebugContext.Paused = true;
                        }
                        return oldTreeDebugContext;
                    });
            }
        }

        public void StopDebugSession(dynamic debugData)
        {
            Guid debugSessionGuid = Guid.Parse(Convert.ToString(debugData.debugSessionId));
            TreeProcessingContext treeDebugContext;
            if (!connectedSessions.TryRemove(debugSessionGuid, out treeDebugContext))
            {
                Clients.Caller.onDebugError(debugData.debugSessionId, "Decison Tree execution problem occured while stopping the debug session.");
            }
            treeDebugContext = null;
           
            Clients.Caller.onDebugSessionEnded(debugData.debugSessionId);
        }

        public void ExecuteTestSuite(dynamic testSuiteData)
        {
            var testSuiteSessionId = testSuiteData.testSuiteSessionId;

            TreeProcessingContext treeDebugContext = GetTestSuiteExecutionContext(testSuiteData);
            treeDebugContext.ResetContext();

            // tell the client the session has started
            Clients.Caller.onTestSuiteExecutionStarted(testSuiteSessionId);

            foreach (var story in testSuiteData.testCases)
            {
                Clients.Caller.onTestSuiteStoryExecutionStarted(story.id);
                var testCaseInputs = story.input_values;

                foreach (var scenario in story.scenarios)
                {
                    try
                    {
                        Clients.Caller.onTestSuiteStoryScenarioExecutionStarted(story.id, scenario.id);

                        testSuiteSessionDetails.storyId = story.id;
                        testSuiteSessionDetails.scenarioId = scenario.id;

                        var scenarioInputs = scenario.input_values;
                        treeDebugContext.PopulateTreeWithDebugValues(scenarioInputs);
                        treeDebugContext.Debug(Guid.Parse(Convert.ToString(testSuiteSessionId)));
                        
                        object[] output_variables = treeDebugContext.ExtractOutputsArray();
                        object[] messages = treeDebugContext.ExtactMessagesArray();
                        Clients.Caller.onTestSuiteStoryScenarioExecutionCompleted(story.id, scenario.id, output_variables, messages);
                        treeDebugContext.ResetContext();
                    }
                    catch (Exception ex)
                    {
                        treeDebugContext.ResetContext();
                        Clients.Caller.onTestSuiteStoryScenarioExecutionError(story.id, scenario.id, ex.Message);
                    }
                }
                Clients.Caller.onTestSuiteStoryExecutionCompleted(story.id);
            }
            Clients.Caller.onTestSuiteExecutionCompleted(testSuiteSessionId);
            CancelTestSuiteExecution(testSuiteSessionId);
        }

        public void CancelTestSuiteExecution(dynamic testSuiteExecutionId)
        {
            Guid testSuiteExecutionGuid = Guid.Parse(Convert.ToString(testSuiteExecutionId));
            TreeProcessingContext treeDebugContext;
            if (!connectedSessions.TryRemove(testSuiteExecutionGuid, out treeDebugContext))
            {
                Clients.Caller.onTestSuiteExecutionError(testSuiteExecutionId, "Decison Tree execution problem occured while stopping the debug session.");
            }
            treeDebugContext = null;
        }

        private TreeProcessingContext GetDebugSessionContext(dynamic sessionData)
        {
            debugSessionId = sessionData.debugSessionId;
            Clients.Caller.onDebugSessionStarted(debugSessionId);

            TreeProcessingContext treeDebugContext = GetCachedDebugSessionModel(debugSessionId);
            if (treeDebugContext == null)
            {
                treeDebugContext = GetNewDebugSessionContext(sessionData);
            }

            if ((treeDebugContext.BreakPoints == null) || (treeDebugContext.BreakPoints.Count != sessionData.breakpoints.Count))
            {
                treeDebugContext.DebugLocationChanged -= DebugLocationChanged;
                treeDebugContext.DebugLocationChanged += DebugLocationChanged;

                treeDebugContext.ExecutionExceptionRaised -= treeDebugContext_ExecutionExceptionRaised;
                treeDebugContext.ExecutionExceptionRaised += treeDebugContext_ExecutionExceptionRaised;

                treeDebugContext.SubtreeExecutionNotificationRaised -= treeDebugContext_SubtreeExecutionNotificationRaised;
                treeDebugContext.SubtreeExecutionNotificationRaised += treeDebugContext_SubtreeExecutionNotificationRaised;

                treeDebugContext.BreakPoints = sessionData.breakpoints;
            }

            return treeDebugContext;
        }

        private TreeProcessingContext GetNewDebugSessionContext(dynamic sessionData)
        {
            ISystemMessageCollection messages = new SystemMessageCollection();
            var treeName = Convert.ToString(sessionData.name);
            var treeVersion = Convert.ToInt32(Convert.ToString(sessionData.version));
            IDecisionTreeManager treeDataAccessManager = StructureMap.ObjectFactory.GetInstance<IDecisionTreeManager>();
            string treeJson = treeDataAccessManager.GetTreeJson(treeName, treeVersion);
            string globalsVersionsJson = Convert.ToString(sessionData.globalsVersions);
            debugSessionId = sessionData.debugSessionId;

            TreeProcessingContext treeDebugContext = new TreeProcessingContext(treeJson, globalsVersionsJson, messages);
            treeDebugContext.DebugLocationChanged += DebugLocationChanged;
            treeDebugContext.ExecutionExceptionRaised += treeDebugContext_ExecutionExceptionRaised;
            treeDebugContext.SubtreeExecutionNotificationRaised += treeDebugContext_SubtreeExecutionNotificationRaised;
            treeDebugContext.BreakPoints = sessionData.breakpoints;

            Clients.Caller.onDebugSessionStarted(debugSessionId);

            CacheDebugSessionModel(debugSessionId, treeDebugContext);

            return treeDebugContext;
        }

        private void treeDebugContext_SubtreeExecutionNotificationRaised(object sender, SubtreeExecutionStatusArgs e)
        {
            SendClientSubtreeStatusNotification(e);
        }

        private void SendClientSubtreeStatusNotification(SubtreeExecutionStatusArgs e)
        {
            var executionStartedArgs = e as SubtreeExecutionStartedArgs;
            var executionCompletedArgs = e as SubtreeExecutionCompletedArgs;

            switch (DebugStatus)
            {
                case SessionDebugStatus.Debug:
                    if (executionStartedArgs != null)
                    {
                        Clients.Caller.onDebugSubtreeExecutionStarted(debugSessionId, executionStartedArgs.SubtreeId.ToString(), executionStartedArgs.Inputs_array, new Object[] { });
                    }
                    else if (executionCompletedArgs != null)
                    {
                        Clients.Caller.onDebugSubtreeExecutionCompleted(debugSessionId, executionCompletedArgs.SubtreeId.ToString(), executionCompletedArgs.Outputs_array, executionCompletedArgs.Messages);
                    }
                    break;
                case SessionDebugStatus.TestSuite:
                    if (executionStartedArgs != null)
                    {
                        Clients.Caller.onTestSuiteSubtreeExecutionStarted(testSuiteSessionDetails.sessionId, testSuiteSessionDetails.storyId, testSuiteSessionDetails.scenarioId, executionStartedArgs.SubtreeId.ToString(), executionStartedArgs.SubtreeName, executionStartedArgs.Inputs_array);
                    }
                    else if (executionCompletedArgs != null)
                    {
                        Clients.Caller.onTestSuiteSubtreeExecutionCompleted(testSuiteSessionDetails.sessionId, testSuiteSessionDetails.storyId, testSuiteSessionDetails.scenarioId, executionCompletedArgs.SubtreeId.ToString(), executionCompletedArgs.Outputs_array, executionCompletedArgs.Messages);
                    }
                    break;
                default:
                    break;
            }
        }

        private TreeProcessingContext GetTestSuiteExecutionContext(dynamic testSuiteData)
        {
            testSuiteSessionDetails = new ExpandoObject();
            testSuiteSessionDetails.sessionId = testSuiteData.testSuiteSessionId;

            TreeProcessingContext treeDebugContext = GetCachedDebugSessionModel(testSuiteSessionDetails.sessionId);
            
            if (treeDebugContext == null)
            {
                ISystemMessageCollection messages = new SystemMessageCollection();
                var treeName = Convert.ToString(testSuiteData.name);
                var treeVersion = Convert.ToInt32(Convert.ToString(testSuiteData.version));
                
                string treeJson = treeDataAccessManager.GetTreeJson(treeName, treeVersion);
                string globalsVersionsJson = Convert.ToString(testSuiteData.globalsVersions);

                treeDebugContext = new TreeProcessingContext(treeJson, globalsVersionsJson, messages);
                treeDebugContext.BreakPoints = new JArray();
                CacheDebugSessionModel(testSuiteSessionDetails.sessionId, treeDebugContext);
            }
            
            treeDebugContext.DebugLocationChanged -= DebugLocationChanged;
            treeDebugContext.DebugLocationChanged += TestSuiteDebugLocationChanged;

            treeDebugContext.ExecutionExceptionRaised -= treeDebugContext_ExecutionExceptionRaised;
            treeDebugContext.ExecutionExceptionRaised += treeDebugContext_ExecutionExceptionRaised;

            treeDebugContext.SubtreeExecutionNotificationRaised -= treeDebugContext_SubtreeExecutionNotificationRaised;
            treeDebugContext.SubtreeExecutionNotificationRaised += treeDebugContext_SubtreeExecutionNotificationRaised;

            return treeDebugContext;
        }

        private void TestSuiteDebugLocationChanged(object sender, DebugEventsArgs e)
        {
            Clients.Caller.onTestSuiteExecutionLocationChanged(e.DebugSessionId.ToString(), e.PreviousNodeId, e.PreviousNodeResult, e.JustExecutedNodeId, e.NodeResult);
        }

        private TreeProcessingContext GetCachedDebugSessionModel(dynamic debugSessionId)
        {
            TreeProcessingContext treeDebugContext;
            if (!connectedSessions.TryGetValue(Guid.Parse(Convert.ToString(debugSessionId)), out treeDebugContext))
            {
                treeDebugContext = null;
            }
            else
            {
                treeDebugContext.ResetContext();
            }
            return treeDebugContext;
        }

        private void RunToNextBreakPoint(dynamic sessionData, TreeProcessingContext treeDebugContext)
        {
            Guid sessionGuid = Guid.Parse(Convert.ToString(sessionData.debugSessionId));

            int? currentNodeId = treeDebugContext.Debug(sessionGuid);

            object[] output_variables = treeDebugContext.ExtractOutputsArray();
            object[] messages = treeDebugContext.ExtactMessagesArray();
            Clients.Caller.onExecutionStoppedAtNode(sessionData.debugSessionId, currentNodeId.Value, output_variables, messages);

            if (treeDebugContext.ExecutionCompleted)
            {
                Clients.Caller.onExecutionCompleted(sessionData.debugSessionId, output_variables, messages);
                Clients.Caller.onDebugSessionEnded(sessionData.debugSessionId);
                StopDebugSession(sessionData);
            }
        }

        private void DebugLocationChanged(object sender, DebugEventsArgs e)
        {
            Clients.Caller.onDebugLocationChanged(e.DebugSessionId.ToString(), e.PreviousNodeId, e.PreviousNodeResult, e.JustExecutedNodeId, e.NodeResult);
        }

        private void treeDebugContext_ExecutionExceptionRaised(object sender, ExecutionExceptionRaisedArgs e)
        {
            var exceptionException = e.GetException();
            Clients.Caller.onDebugError(debugSessionId, new { message = exceptionException.Message, node = e.NodeId, errortype = e.ErrorType , line = e.LineNumber, source = e.Source});
        }

        private void CacheDebugSessionModel(dynamic debugSessionId, TreeProcessingContext treeDebugContext)
        {
            if (!connectedSessions.ContainsKey(Guid.Parse(Convert.ToString(debugSessionId))))
            {
                if (!connectedSessions.TryAdd(Guid.Parse(Convert.ToString(debugSessionId)), treeDebugContext))
                {
                    Clients.Caller.onDebugError(debugSessionId, "Decision Tree service error occured, please notify the administrator.");
                    Clients.Caller.onDebugSessionEnded(debugSessionId);
                }
            }
        }
    }
}