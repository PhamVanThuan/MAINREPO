using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.X2Engine2.Tests.X2.Models;
using SAHL.X2Engine2.Tests.X2.SqlStatements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Tests.X2.Managers
{
    public class X2TestDataManager : IX2TestDataManager
    {
        private IDbFactory dbFactory;

        public X2TestDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public List<X2ScenarioMapInfo> GetApplicationCaptureTestCases(string hostName, int workerId)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                X2ApplicationCaptureCasesStatement sqlStatement = new X2ApplicationCaptureCasesStatement(hostName, workerId);
                var cases = db.Select<X2ApplicationCaptureCase>(sqlStatement).ToList();
                List<X2ScenarioMapInfo> scnerarios = new List<X2ScenarioMapInfo>();

                foreach (var appCapCase in cases)
                {
                    var scenarioMapInfo = new X2ScenarioMapInfo("Create Instance", "Origination", "Application Capture", @"SAHL\BCUser2", false, 1,
                       new Dictionary<string, string>()
                   {
                       {"ApplicationKey", appCapCase.ApplicationKey.ToString()},
                       {"isEstateAgentApplication", "False"},
                       {"LeadType", "1"},
                        {"CaseOwnerName", "SAHL\\BCUser2"}
                   });
                    scnerarios.Add(scenarioMapInfo);
                }

                return scnerarios;
            }
        }

        public long GetRelatedInstanceIDFromParentInstance(long instanceID)
        {
            long relatedInstanceId = -1;

            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                X2GetInstanceFromParentInstanceSqlStatement sqlStatement = new X2GetInstanceFromParentInstanceSqlStatement(instanceID);
                relatedInstanceId = db.SelectOne<long>(sqlStatement);
            }

            return relatedInstanceId;
        }

        public X2Case GetX2Case(long instanceId)
        {
            using (IDbContext db = new Db().InWorkflowContext())
            {
                X2CaseSqlStatement sqlStatement = new X2CaseSqlStatement(instanceId);
                X2Case x2Case = db.SelectOne<X2Case>(sqlStatement);
                return x2Case;
            }
        }
    }
}
