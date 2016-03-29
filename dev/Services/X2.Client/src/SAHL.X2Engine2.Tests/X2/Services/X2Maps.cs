using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using SAHL.X2Engine2.Providers;
using StructureMap;
using SAHL.X2Engine2.Tests.X2;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;
using SAHL.X2Engine2.Tests.X2.Models;
using SAHL.X2Engine2.Tests.X2.SqlStatements;
using SAHL.Core.Data;
namespace SAHL.X2Engine2.Tests.X2.Services
{
    public sealed class X2Maps : IX2Maps
    {
        public IEnumerable<X2ProcessWorkflow> GetMapNames()
        {
            return new[]
            {
               new X2ProcessWorkflow("CreateInstance","CreateInstance",0),
               new X2ProcessWorkflow("SetDataFieldsTest","SetDataFieldsTest",0),
               new X2ProcessWorkflow("SimpleTimer","SimpleTimer",0),
               new X2ProcessWorkflow("SourceDestinationReturn","Source",0),
               new X2ProcessWorkflow("UserCloneCreated","UserCloneCreated",0),
               new X2ProcessWorkflow("MultipleDecisions","MultipleDecisions",0),
               new X2ProcessWorkflow("AutoForwardToState","AutoForwardToState",0),
               new X2ProcessWorkflow("CloneWakeUpParent","CloneWakeUpParent",0),
               new X2ProcessWorkflow("CommonFlagOnMainInstance","CommonFlagOnMainInstance",0)
            };
        }

        public IEnumerable<X2StateActivity> GetMapInfo()
        {
            var activities = default(IEnumerable<X2StateActivity>);
            using (var db = new Db().InWorkflowContext())
            {
                var workflowsStatement = new X2WorkflowsSqlStatement();
                var processNames = this.GetMapNames().Select(x=>x.Process);
                var workflows = db.Select<X2ProcessWorkflow>(workflowsStatement).Where(x => processNames.Contains(x.Process));

                if (workflows.Count() == 0)
                    throw new Exception("Scenario Maps have not been deployed to configured database.");
                var lastPublishedMap = workflows.LastOrDefault();
                var activitiesSqlStatement = new X2ActivitiesSqlStatement(lastPublishedMap.WorkflowId);
                activities = db.Select<X2StateActivity>(activitiesSqlStatement);
                db.Complete();
            }
            return activities;
        }

        public void Dispose()
        {
        }
    }
}