using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using NHibernate;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class X2RepositoryTest : TestBase
    {
        private static bool HasSetup = false;

        [SetUp()]
        public void Setup()
        {
            if (!HasSetup)
            {
                base.DoSetup();
                HasSetup = true;
            }
        }

        [Test]
        public void GetInstanceForGenericKey()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            using (new SessionScope())
            {
                ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                IWorkFlow workflow = x2Repo.GetWorkFlowByName(SAHL.Common.Constants.WorkFlowName.ApplicationCapture, SAHL.Common.Constants.WorkFlowProcessName.Origination);

                // get first instance from application capture workflow that has an offerkey
                string sql = "SELECT i.* FROM [X2].[X2DATA]." + workflow.StorageTable + "  xd (nolock) JOIN x2.x2.Instance i (nolock) on i.id = xd.InstanceID JOIN [2am]..Offer o on o.OfferKey = xd." + workflow.StorageKey;

                ISession session = sessionHolder.CreateSession(typeof(Instance_DAO));
                IQuery sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(Instance_DAO));
                sqlQuery.SetMaxResults(1);
                IList<Instance_DAO> instances_DAO = sqlQuery.List<Instance_DAO>();

                if (instances_DAO.Count == 0)
                    Assert.Ignore("No instances found");

                IEventList<IInstance> instances = new DAOEventList<Instance_DAO, IInstance, Instance>(instances_DAO);

                Assert.IsNotNull(instances);
                Assert.IsTrue(instances.Count > 0);

                IInstance instance = instances[0];

                // get the application from the instance
                IApplication application = appRepo.GetApplicationFromInstance(instance);
                Assert.IsNotNull(application);

                IInstance newInstance = x2Repo.GetInstanceForGenericKey(application.Key, "Application Capture", "Origination");
                Assert.IsNotNull(newInstance);
            }
        }

        [Test]
        public void GetInstancesForGenericKey()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            using (new SessionScope())
            {
                ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

                IWorkFlow workflow = x2Repo.GetWorkFlowByName(SAHL.Common.Constants.WorkFlowName.ApplicationCapture, SAHL.Common.Constants.WorkFlowProcessName.Origination);

                // get first instance from application capture workflow that has an offerkey
                string sql = "SELECT i.* FROM [X2].[X2DATA]." + workflow.StorageTable + "  xd (nolock) JOIN x2.x2.Instance i (nolock) on i.id = xd.InstanceID JOIN [2am]..Offer o on o.OfferKey = xd." + workflow.StorageKey;

                ISession session = sessionHolder.CreateSession(typeof(Instance_DAO));
                IQuery sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(Instance_DAO));
                sqlQuery.SetMaxResults(1);
                IList<Instance_DAO> instances_DAO = sqlQuery.List<Instance_DAO>();

                if (instances_DAO.Count == 0)
                    Assert.Ignore("No instances found");

                IEventList<IInstance> instances = new DAOEventList<Instance_DAO, IInstance, Instance>(instances_DAO);

                Assert.IsNotNull(instances);
                Assert.IsTrue(instances.Count > 0);

                IInstance instance = instances[0];

                // get the application from the instance
                IApplication application = appRepo.GetApplicationFromInstance(instance);
                Assert.IsNotNull(application);

                // lets use the repo method to return a list of instances
                IList<IInstance> instanceList = x2Repo.GetInstancesForGenericKey(application.Key, SAHL.Common.Constants.WorkFlowProcessName.Origination);
                Assert.IsNotNull(instanceList);
                Assert.IsTrue(instanceList.Count > 0);

                // check that all the instances have the same applicationkey
                foreach (IInstance i in instanceList)
                {
                    IApplication app = appRepo.GetApplicationFromInstance(i);
                    Assert.IsNotNull(app);
                }
            }
        }

        [Test]
        public void CanPerformWorkflowSuperSearch()
        {
            using (new SessionScope())
            {
                string sql = @"select max(ID) as ID from [x2].[x2].workflow (nolock) where name='Application Management'";
                string statesSQL = @"select max(ID) as ID from [x2].[x2].State (nolock) where name='QA'";
                DataTable dt = base.GetQueryResults(sql);
                int workflowID = Convert.ToInt32(dt.Rows[0][0]);
                dt.Clear();
                dt = base.GetQueryResults(statesSQL);
                int stateID = Convert.ToInt32(dt.Rows[0][0]);
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                IWorkflowSearchCriteria searchCriteria = new WorkflowSearchCriteria() as IWorkflowSearchCriteria;
                IWorkflowSearchCriteriaWorkflowFilter workflowFilter = new WorkflowSearchCriteriaWorkflowFilter(workflowID, new[] { stateID }) as IWorkflowSearchCriteriaWorkflowFilter;
                searchCriteria.WorkflowFilter.Add(workflowFilter);
                searchCriteria.IncludeHistoricUsers = true;
                IList<IInstance> Instances = x2Repo.SuperSearchWorkflow(searchCriteria);
                int count = Instances.Count;
                Assert.That(Instances.Count > 1);
            }
        }

        [Test]
        public void CanReassignToUserTest()
        {
            IX2Repository X2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            IADUser ADUser = bmtm.GetMappedType<IADUser>(ADUser_DAO.FindFirst());
            IInstance Instance = bmtm.GetMappedType<IInstance>(Instance_DAO.FindFirst());
            bool success = X2Repo.CanInstanceBeAssignedToUser(Instance, ADUser);
            Assert.That(success == false);
        }

        [Test]
        public void CreateActiveExternalActivity()
        {
            IX2Repository X2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            IActiveExternalActivity iAEA = X2Repo.CreateActiveExternalActivity();

            Assert.IsNotNull(iAEA);
        }

        [Test]
        public void GetExternalActivityByName()
        {
            string sql = "select max(WorkFlowId) as ID "
                        + "from [x2].[x2].externalactivity (nolock) "
                        + "where name='EXTCreateApplication' "
                        + "and "
                        + "workflowid=(select max(id) from [x2].[x2].workflow (nolock) "
                        + "where name='Application Capture')";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            int ExternalActivityID = Convert.ToInt32(dt.Rows[0][0]);
            dt.Dispose();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IExternalActivity ea = x2Repo.GetExternalActivityByName("EXTCreateApplication", ExternalActivityID);
            Assert.IsNotNull(ea);
        }

        [Test]
        public void GetLatestExternalActivityIDFromWorkFlow()
        {
            string sql = "select max(id) as ID "
                        + "from [x2].[x2].externalactivity (nolock) "
                        + "where name='EXTCreateApplication' "
                        + "and "
                        + "workflowid=(select max(id) from [x2].[x2].workflow (nolock) "
                        + "where name='Application Capture')";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            int ExternalActivityID = Convert.ToInt32(dt.Rows[0][0]);
            dt.Dispose();

            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            int eaID = x2Repo.GetLatestExternalActivityIDFromWorkFlow("Application Capture", "EXTCreateApplication");
            Assert.AreEqual(ExternalActivityID, eaID);
        }

        [Test]
        public void GetChildInstances()
        {
            string sql = @"	SELECT     TOP (1) insParent.ParentInstanceID, COUNT(insChild.ID) AS ChildCount
							FROM  x2.X2.Instance AS insParent (nolock)
                            INNER JOIN x2.X2.Instance AS insChild (nolock) ON insParent.ParentInstanceID = insChild.ID
							WHERE     (insParent.ParentInstanceID IS NOT NULL)
							GROUP BY insParent.ParentInstanceID, insChild.ID
							HAVING      (COUNT(insChild.ID) > 1)
							ORDER BY 1 DESC	";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            int InstanceID = Convert.ToInt32(dt.Rows[0][0]);
            int count = Convert.ToInt32(dt.Rows[0][1]);
            dt.Dispose();

            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            IEventList<IInstance> Instances = x2Repo.GetChildInstances(InstanceID);
            Assert.IsTrue(Instances.Count == count);
        }

        [Test]
        public void GetActivitiesForName()
        {
            string sql = @"select top 1  Name , count(*) from [x2].X2.Activity group by name
						order by count(*) ";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            string activityname = Convert.ToString(dt.Rows[0][0]);
            int count = Convert.ToInt32(dt.Rows[0][1]);
            dt.Dispose();

            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IEventList<IActivity> activies = x2Repo.GetActivitiesForName(activityname);
            Assert.AreEqual(count, activies.Count);
        }

        [Test]
        public void GetActivityForName()
        {
            string sql = @"select top 1  Name , count(*) from [x2].X2.Activity group by name
						order by count(*) ";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            string activityname = Convert.ToString(dt.Rows[0][0]);
            int count = Convert.ToInt32(dt.Rows[0][1]);
            dt.Dispose();

            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IActivity activies = x2Repo.GetActivityForName(activityname);
            Assert.IsNotNull(activies);
        }

        [Test]
        public void GetArchiveSearchWorkflowsTest()
        {
            string sql = string.Format(@"SELECT  top 1   wf.Name
										FROM    x2.X2.WorkFlow  wf INNER JOIN
										x2.X2.State  st ON wf.ID = st.ID AND wf.id = St.ID
										where  st.Type = {0}
										group by   wf.Name,  st.Type
										order by count(*) desc
										", (int)X2StateTypes.Archive, 2);

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            string activityname = "'" + Convert.ToString(dt.Rows[0][0]) + "'";
            dt.Dispose();

            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IEventList<IWorkFlow> workflows = x2Repo.GetArchiveSearchWorkflows(activityname);
            Assert.Greater(workflows.Count, 0);
        }

        [Test]
        public void GetAllUIStatements()
        {
            IX2Repository X2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            IEventList<IUIStatement> uiStatements = X2Repo.GetAllUIStatement();

            Assert.IsTrue(uiStatements.Count > 0);
        }

        [Test]
        public void GetWorkflowRoleForGenericKey()
        {
            using (new SessionScope())
            {
                IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");

                // get the fist workflowrole record
                string sql = @"SELECT top 1 * from [2am]..WorkflowRole (nolock)";
                DataTable dt = base.GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data found");
                int genericKey = Convert.ToInt32(dt.Rows[0]["GenericKey"]);
                int workflowRoleTypeKey = Convert.ToInt32(dt.Rows[0]["WorkflowRoleTypeKey"]);
                int generalStatusKey = Convert.ToInt32(dt.Rows[0]["GeneralStatusKey"]);
                dt.Dispose();

                // get the number of workflowrole records
                sql = String.Format(@"SELECT count(*) from [2am]..WorkflowRole (nolock) where GenericKey={0}", genericKey);
                int recordCount1 = Convert.ToInt32(base.ExecuteScalar(con, sql));
                sql = String.Format(@"SELECT count(*) from [2am]..WorkflowRole (nolock) where GenericKey={0} and WorkflowRoleTypeKey={1}", genericKey, workflowRoleTypeKey);
                int recordCount2 = Convert.ToInt32(base.ExecuteScalar(con, sql));
                sql = String.Format(@"SELECT count(*) from [2am]..WorkflowRole (nolock) where GenericKey={0} and WorkflowRoleTypeKey={1} and GeneralStatusKey={2}", genericKey, workflowRoleTypeKey, generalStatusKey);
                int recordCount3 = Convert.ToInt32(base.ExecuteScalar(con, sql));

                // use repo method to test
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

                IList<IWorkflowRole> workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(genericKey);
                Assert.IsTrue(workflowRoles.Count == recordCount1);

                workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(genericKey, workflowRoleTypeKey);
                Assert.IsTrue(workflowRoles.Count == recordCount2);

                workflowRoles = x2Repo.GetWorkflowRoleForGenericKey(genericKey, workflowRoleTypeKey, generalStatusKey);
                Assert.IsTrue(workflowRoles.Count == recordCount3);
            }
        }

        [Test]
        public void GetWorkflowRoleForLegalEntityKey()
        {
            using (new SessionScope())
            {
                IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");

                // get the fist workflowrole record
                string sql = @"SELECT top 1 * from [2am]..WorkflowRole (nolock)";
                DataTable dt = base.GetQueryResults(sql);
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data found");
                int legalEntityKey = Convert.ToInt32(dt.Rows[0]["LegalEntityKey"]);
                int workflowRoleTypeKey = Convert.ToInt32(dt.Rows[0]["WorkflowRoleTypeKey"]);
                int generalStatusKey = Convert.ToInt32(dt.Rows[0]["GeneralStatusKey"]);
                dt.Dispose();

                // get the number of workflowrole records
                sql = String.Format(@"SELECT count(*) from [2am]..WorkflowRole (nolock) where LegalEntityKey={0} and WorkflowRoleTypeKey={1} and GeneralStatusKey={2}", legalEntityKey, workflowRoleTypeKey, generalStatusKey);
                int recordCount = Convert.ToInt32(base.ExecuteScalar(con, sql));

                // use repo method to test
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

                IList<IWorkflowRole> workflowRoles = x2Repo.GetWorkflowRoleForLegalEntityKey(legalEntityKey, workflowRoleTypeKey, generalStatusKey);
                Assert.IsTrue(workflowRoles.Count == recordCount);
            }
        }

        [Test]
        public void GetScheduledActivities()
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            //get one instance id to one scheduled task
            string sql = @"select top 10
                sa.InstanceID, count(sa.InstanceID)
                from x2.X2.ScheduledActivity sa (nolock)
                join x2.x2.Activity a (nolock) on sa.ActivityID = a.ID
                group by sa.InstanceID
                having count(sa.InstanceID) = 1
                ";

            DataTable dtA = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null).Tables[0];
            DataTable dt;

            if (dtA.Rows.Count > 0)
            {
                dt = x2Repo.GetScheduledActivities(dtA);

                Assert.AreEqual(dtA.Rows.Count, dt.Rows.Count);
            }

            //get one instance id to many scheduled tasks
            sql = @"select top 5
                sa.InstanceID, count(sa.InstanceID) Occurs
                from x2.X2.ScheduledActivity sa (nolock)
                join x2.x2.Activity a (nolock) on sa.ActivityID = a.ID
                group by sa.InstanceID
                having count(sa.InstanceID) = 2";

            dtA = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), null).Tables[0];

            if (dtA.Rows.Count == 0)
            {
                Assert.Ignore("Insufficient Data to perform test");
            }

            dt = x2Repo.GetScheduledActivities(dtA);
            Assert.AreEqual(dt.Rows.Count, dtA.Rows.Count * 2);
        }

        [Test]
        public void GetRelatedInstances()
        {
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();

            //use a prarent id
            string sql = @"select top 1 i.ID
                            from x2.x2.Instance i (nolock)
                            join x2.X2.[State] s (nolock) on s.ID = i.StateID
	                            and s.[Type] = 1
                            where i.ParentInstanceID is null
                            order by i.ID desc";

            Int64 iID = Convert.ToInt64(CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null));
            DataTable dt = x2Repo.GetRelatedInstances(iID);

            Assert.GreaterOrEqual(dt.Rows.Count, 1);

            //use a child id
            sql = @"select top 1 i.ID
                    from x2.x2.Instance i (nolock)
                    join x2.X2.[State] s (nolock) on s.ID = i.StateID
	                    and s.[Type] = 1
                    where i.ParentInstanceID is not null
                    order by i.ID desc";
            iID = Convert.ToInt64(CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null));
            dt = x2Repo.GetRelatedInstances(iID);

            Assert.GreaterOrEqual(dt.Rows.Count, 2);
        }

        [Test]
        public void GetWorkFlowByNameTest()
        {
            using (new SessionScope())
            {
                WorkFlow_DAO wf = WorkFlow_DAO.FindFirst();
                string WorkFlowName = wf.Name;
                string ProcessName = wf.Process.Name;
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                IWorkFlow workflow = x2Repo.GetWorkFlowByName(WorkFlowName, ProcessName);
                Assert.AreEqual(workflow.Name, WorkFlowName);
                Assert.AreEqual(workflow.Process.Name, ProcessName);
            }
        }

        [Test]
        public void GetDebtCousellingInstanceByAccountKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 dc.AccountKey
                from [2am].debtcounselling.DebtCounselling dc (nolock)
                join x2.x2data.Debt_Counselling xdc (nolock)
	                on xdc.DebtCounsellingKey = dc.DebtCounsellingKey
                join x2.x2.Instance xi (nolock)
	                on xi.ID = xdc.InstanceID
                where
	                xi.ParentInstanceID is null
		                and
	                dc.DebtCounsellingStatusKey = 1";

                int accountKey = (int)CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
                IInstance instance = x2Repo.GetDebtCousellingInstanceByAccountKey(accountKey);
                Assert.IsNotNull(instance);
            }
        }

        [Test]
        public void GetProcessesByUserActivity()
        {
            string sql = @";with Activity_CTE (ProcessID)
                            as
                            (
	                            select max(w.ProcessID)
	                            from [x2].[x2].Activity a (nolock)
	                            join [x2].[x2].Workflow w (nolock) on w.ID =  a.WorkFlowID
	                            join [x2].[x2].Process p (nolock) on p.ID =  w.ProcessID
	                            where a.[Name] = 'Clear Cache'
	                            group by p.[Name]
                            )
                            select
	                            p.*
                            from
	                            Activity_CTE
                            join
	                            [x2].[x2].Process p (nolock) on p.ID = Activity_CTE.ProcessID";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            //int ExternalActivityID = Convert.ToInt32(dt.Rows[0][0]);
            dt.Dispose();
            IX2Repository x2Repo = RepositoryFactory.GetRepository<IX2Repository>();
            IList<IProcess> processes = x2Repo.GetProcessesByUserActivity("Clear Cache");
            Assert.IsTrue(processes.Count == dt.Rows.Count);
        }
    }
}