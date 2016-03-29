﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SAHL.Config.Services.X2.Client;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Tests.X2.Models;
using SAHL.X2Engine2.Tests.X2.Services;
using SAHL.X2Engine2.Tests.X2.SqlStatements;

namespace SAHL.X2Engine2.Tests.X2
{
    [TestFixture]
    public class ScenarioMapTests
    {
        [Ignore("we don't want this to run on the build server")]
        [Test, TestCaseSource("ScenarioMaps")]
        public void TestX2Service(X2ScenarioMapInfo scenarioMapInfo)
        {
            try
            {
                IServiceUrlConfiguration serviceConfiguration = new ServiceUrlConfiguration("localhost/x2service/");
                X2ServiceClient client = new X2ServiceClient(serviceConfiguration);
                ServiceRequestMetadata metadata = null;

                IX2CreateRequest request = new X2CreateInstanceRequest(CombGuid.Instance.Generate(), scenarioMapInfo.ActivityName, scenarioMapInfo.ProcessName, scenarioMapInfo.WorkFlowName, metadata, scenarioMapInfo.IgnoreWarnings, null, null, scenarioMapInfo.MapVariables, null);
                ISystemMessageCollection x2Response = client.PerformCommand(request);
                X2Response response = request.Result;

                if (response.InstanceId > 0)
                {
                    IX2RequestForExistingInstance completeRequest = new X2RequestForExistingInstance(CombGuid.Instance.Generate(), response.InstanceId, X2RequestType.CreateComplete, metadata, scenarioMapInfo.ActivityName, false, scenarioMapInfo.MapVariables);
                    x2Response = client.PerformCommand(completeRequest);
                    response = request.Result;

                    if (completeRequest.Result.IsErrorResponse)
                    {
                        X2ErrorResponse errorResponse = request.Result as X2ErrorResponse;
                        Assert.Fail("X2 Error Response: " + errorResponse.Message);
                    }
                }
                else
                {
                    Assert.Fail("InstanceId = 0");
                }

                if (response.InstanceId > 0)
                {
                    if (scenarioMapInfo.ProcessName != "CommonFlagOnMainInstance")
                    {
                        if (scenarioMapInfo.SleepSeconds > 0)
                        {
                            System.Threading.Thread.Sleep(scenarioMapInfo.SleepSeconds);
                        }
                        AssertExpectedEndState(response.InstanceId);
                    }

                    if (scenarioMapInfo.WorkFlowName == "SetDataFieldsTest")
                    {
                        AssertDataFields(response.InstanceId);
                    }
                    else if (scenarioMapInfo.ProcessName == "SourceDestinationReturn")
                    {
                        // now lets get the related instance and do assertions on that
                        long relatedInstanceID = GetRelatedInstanceIDFromSourceInstance(response.InstanceId);
                        AssertExpectedEndState(relatedInstanceID, "ExpectedEndStateRelated");
                    }
                    else if (scenarioMapInfo.ProcessName == "UserCloneCreated" ||
                        scenarioMapInfo.ProcessName == "CloneWakeUpParent")
                    {
                        // now lets get the related instance and do assertions on that
                        long relatedInstanceID = GetRelatedInstanceIDFromParentInstance(response.InstanceId);
                        AssertExpectedEndState(relatedInstanceID, scenarioMapInfo.ProcessName == "UserCloneCreated" ? "ExpectedEndStateClone" : "ExpectedEndStateChild");
                    }
                    else if (scenarioMapInfo.ProcessName == "CommonFlagOnMainInstance")
                    {
                        AssertExpectedEndState(response.InstanceId, "Created");

                        X2ExternalActivity externalActivity = null;
                        X2ProcessWorkflow processWorkflow = null;
                        using (var db = new Db().InWorkflowContext())
                        {
                            X2WorkflowsSqlStatement x2WorkflowsSqlStatement = new X2WorkflowsSqlStatement();
                            IEnumerable<X2ProcessWorkflow> workflows = db.Select<X2ProcessWorkflow>(x2WorkflowsSqlStatement);
                            processWorkflow = workflows.Where(x => x.Workflow == scenarioMapInfo.WorkFlowName).FirstOrDefault();
                            X2ExternalActivitiesSqlStatement x2ExternalActivitiesSqlStatement = new X2ExternalActivitiesSqlStatement(processWorkflow.WorkflowId);
                            externalActivity = db.SelectOne<X2ExternalActivity>(x2ExternalActivitiesSqlStatement);
                            db.Complete();
                        }

                        if (processWorkflow != null &&
                            processWorkflow.WorkflowId > 0 &&
                            externalActivity != null &&
                            externalActivity.ID > 0)
                        {
                            IX2ExternalActivityRequest externalActivityRequest = new X2ExternalActivityRequest(CombGuid.Instance.Generate(), response.InstanceId, 0, externalActivity.ID, response.InstanceId, processWorkflow.WorkflowId, metadata);
                            client.PerformCommand(externalActivityRequest);

                            if (externalActivityRequest.Result.IsErrorResponse)
                            {
                                X2ErrorResponse errorResponse = request.Result as X2ErrorResponse;
                                Assert.Fail("X2 Error Response: " + errorResponse.Message);
                            }

                            if (scenarioMapInfo.SleepSeconds > 0)
                            {
                                System.Threading.Thread.Sleep(scenarioMapInfo.SleepSeconds);
                            }

                            AssertExpectedEndState(response.InstanceId, "ExpectedEndState");
                        }
                    }
                }
                else
                {
                    Assert.Fail("InstanceId = 0");
                }
            }
            catch (Exception e)
            {
                Assert.Fail("shit went bad : " + e.Message);
            }
        }

        private void AssertExpectedEndState(long instanceId, string expectedState = "Expected")
        {
            using (IDbContext db = new Db().InWorkflowContext())
            {
                X2CaseSqlStatement sqlStatement = new X2CaseSqlStatement(instanceId);
                X2Case x2Case = db.SelectOne<X2Case>(sqlStatement);

                Assert.NotNull(x2Case, "No X2 instance found for instance '{0}'", instanceId);
                Assert.NotNull(x2Case.Process, "No process for instance '{0}'", instanceId);
                Assert.NotNull(x2Case.State, "No state for instance '{0}'", instanceId);

                Assert.That(x2Case.State.Contains(expectedState), "Scenariomap : 'Process:{0} Workflow:{1}' failed, workflow case did not end up at the expected state. Instance:{2} is at workflow state:{3}"
                    , x2Case.Process, x2Case.Workflow, x2Case.InstanceId, x2Case.State);
                db.Complete();
            }
        }

        private void AssertDataFields(long instanceId)
        {
            using (IDbContext db = new Db().InWorkflowContext())
            {
                X2SetDataFieldsTestSqlStatement sqlStatement = new X2SetDataFieldsTestSqlStatement(instanceId);
                X2SetDataFieldsTest x2SetDataFieldsTest = db.SelectOne<X2SetDataFieldsTest>(sqlStatement);

                Assert.AreEqual(x2SetDataFieldsTest.ApplicationKey, 123456);
                Assert.AreEqual(x2SetDataFieldsTest.TestBool, true);
                Assert.LessOrEqual(x2SetDataFieldsTest.TestDate, DateTime.Now);
                Assert.AreEqual(x2SetDataFieldsTest.TestString, "string value");
                Assert.AreEqual(x2SetDataFieldsTest.TestDecimal, 301);
                Assert.AreEqual(x2SetDataFieldsTest.TestDouble, 3.0);
                Assert.AreEqual(x2SetDataFieldsTest.TestSingle, 3.5F);
                Assert.AreEqual(x2SetDataFieldsTest.TestBigInt, 4294967296);

                db.Complete();
            }
        }

        private long GetRelatedInstanceIDFromSourceInstance(long instanceID)
        {
            long relatedInstanceId = -1;

            using (IDbContext db = new Db().InWorkflowContext())
            {
                X2GetInstanceFromSourceInstanceSqlStatement sqlStatement = new X2GetInstanceFromSourceInstanceSqlStatement(instanceID);
                relatedInstanceId = db.SelectOne<long>(sqlStatement);
                db.Complete();
            }

            return relatedInstanceId;
        }

        private long GetRelatedInstanceIDFromParentInstance(long instanceID)
        {
            long relatedInstanceId = -1;

            using (IDbContext db = new Db().InWorkflowContext())
            {
                X2GetInstanceFromParentInstanceSqlStatement sqlStatement = new X2GetInstanceFromParentInstanceSqlStatement(instanceID);
                relatedInstanceId = db.SelectOne<long>(sqlStatement);
                db.Complete();
            }

            return relatedInstanceId;
        }

        private IEnumerable<X2ScenarioMapInfo> ScenarioMaps()
        {
            SpecificationIoCBootstrapper.Initialize();
            return new X2ScenarioMaps().GetMapNames();
        }
    }
}