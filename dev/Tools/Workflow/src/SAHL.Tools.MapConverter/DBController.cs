using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SAHL.Tools.Workflow.Common.Database;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.MapConverter
{
    class DBController
    {
        #region variables
        private ISessionFactory sessionFactory = null;
        #endregion

        #region constructor
        internal DBController()
        {
            sessionFactory = SetupConnection();
        }
        #endregion

        #region methods
        internal IList<State> GetStatesByWorkflowID(int id)
        {
            IList<State> states = null;
            using (ISession session = this.sessionFactory.OpenSession())
            {
                states = (from x in session.Query<WorkFlow>()
                          where x.Id == id
                          select x).SingleOrDefault().States;
                foreach (State s in states)
                {
                    Guid guid = s.X2ID;
                    string name = s.WorkFlow.Process.Name;
                }
            }
            return states;
        }
        internal IList<Activity> GetActivitiesByWorkflowID(int id)
        {
            IList<Activity> activities = null;
            using (ISession session = this.sessionFactory.OpenSession())
            {
                activities = (from x in session.Query<WorkFlow>()
                          where x.Id == id
                          select x).SingleOrDefault().Activities;
                foreach (Activity a in activities)
                {
                    if (a.FromState != null)
                    {
                        int fromID = a.FromState.Id;
                        string fromStateName = a.FromState.Name;
                    }
                    if (a.ToState != null)
                    {
                        int toID = a.ToState.Id;
                        string toStateName = a.ToState.Name;
                    }
                    string workflow = a.WorkFlow.Name;
                    string proc = a.WorkFlow.Process.Name;
                    string name = a.Name;
                    Guid guid = a.X2ID;
                }
            }
            return activities;
        }
        internal Dictionary<string, int> GetMapWorkFlowIDs(string mapName)
        {
            Dictionary<string, int> workflowNameID = null;
            using (ISession session = sessionFactory.OpenSession())
            {
                Process workflow = (from x in session.Query<Process>().AsEnumerable().AsParallel()
                                    where x.Name == mapName
                                    select x).OrderByDescending(x => x.Id).First();

                workflowNameID = (from x in workflow.WorkFlows
                                  select new
                                  {
                                      name = x.Name,
                                      id = x.Id
                                  }).ToDictionary(k => k.name, v => v.id);
            }
            return workflowNameID;
        }
        #endregion methods

        #region private methods
        private ISessionFactory SetupConnection()
        {
            NHibernateInitialiser ARInit = new NHibernateInitialiser("Data Source=D101846\\DEV;Initial Catalog=X2;Integrated Security=SSPI;");
            return sessionFactory = ARInit.InitialiseNHibernate();
        }
        #endregion
    }
}
