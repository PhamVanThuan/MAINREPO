using NUnit.Framework;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using StructureMap;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using SAHL.X2Engine2.Tests.X2;
using System.Collections.Generic;
using SAHL.X2Engine2.Tests.X2.Services;
using SAHL.X2Engine2.Tests.X2.Models;
using SAHL.X2Engine2.Tests.X2.SqlStatements;
using SAHL.Core.X2.AppDomain;

namespace SAHL.X2Engine2.Tests.X2
{
    [TestFixture]
    public class MapTests
    {
        [Ignore]
        [Test, TestCaseSource("ScenarioMaps")]
        public void Test(X2ProcessWorkflow process)
        {
            try
            {
                DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();


                Console.WriteLine("Checking Activities...");
                Console.WriteLine("");

                var mapInfo = ObjectFactory.GetInstance<IX2Maps>().GetMapInfo();
                Assert.Greater(mapInfo.Count(), 0);
                foreach (var stateActivity in mapInfo)
                    Console.WriteLine("    State: {0}, Activity: {1}", stateActivity.State, stateActivity.Activity);
                Console.WriteLine("");


                var x2service = ObjectFactory.GetInstance<IX2Service>();
                var response = x2service.CreateWorkflowInstance(process, @"SAHL\HaloUser", "Create Instance");

                Thread.Sleep(15000);

                if (process.Workflow != "CommonFlagOnMainInstance")
                {
                    this.AssertExpectedEndState(response.InstanceId);
                }
                else
                {
                    var externalActivity = default(X2ExternalActivity);
                    using (var db = new Db().InWorkflowContext())
                    {
                        var x2ProcessWorkflowStatement = new X2WorkflowsSqlStatement();
                        var workflows = db.Select<X2ProcessWorkflow>(x2ProcessWorkflowStatement);
                        process = workflows.Where(x => x.Workflow == process.Workflow).FirstOrDefault();
                        var x2ExternalActivitiesSqlStatement = new X2ExternalActivitiesSqlStatement(process.WorkflowId);
                        externalActivity = db.SelectOne<X2ExternalActivity>(x2ExternalActivitiesSqlStatement);
                        db.Complete();
                    }
                    ObjectFactory.GetInstance<IX2Service>().RaiseExternalFlag(process, response.InstanceId, externalActivity);
                    Thread.Sleep(15000);
                    this.AssertExpectedEndState(response.InstanceId);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void AssertExpectedEndState(long instanceId)
        {
            using (var db = new Db().InWorkflowContext())
            {
                var sqlStatement = new X2CaseSqlStatement(instanceId);
                var x2Case = db.SelectOne<X2Case>(sqlStatement);

                Assert.NotNull(x2Case, "No X2 instance found for instance '{0}'", instanceId);
                Assert.NotNull(x2Case.Process, "No process for instance '{0}'", instanceId);
                Assert.NotNull(x2Case.State, "No state for instance '{0}'", instanceId);

                Assert.That(x2Case.State.Contains("Expected"), "Scenariomap : 'Process:{0} Workflow:{1}' failed, workflow case did not end up at the expected state. Instance:{2} is at workflow state:{3}"
                    , x2Case.Process, x2Case.Workflow, x2Case.InstanceId, x2Case.State);
                db.Complete();
            }
        }

        private IEnumerable<X2ProcessWorkflow> ScenarioMaps()
        {
            SpecificationIoCBootstrapper.Initialize();
            return new X2Maps().GetMapNames();
        }
    }
}