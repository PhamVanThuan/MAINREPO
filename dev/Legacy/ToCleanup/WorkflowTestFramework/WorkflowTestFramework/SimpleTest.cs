using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.Security;
using System.Threading;
using System.Security.Principal;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Common;
//remove from
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.X2.Framework.Interfaces;
//remove to
using SAHL.Common.DataAccess;
using System.Data;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Interfaces;

namespace WorkflowTestFramework
{
    [TestFixture]
    public class SimpleTest : BaseHelper
    {
        UserPool _userPool = new UserPool();

        [Test]
        public void ThreadTest()
        {
            int CreateCount = Properties.Settings.Default.CreateInstances;

            ThreadPool.SetMaxThreads(Properties.Settings.Default.ThreadCount, Properties.Settings.Default.ThreadCountCP);

            //Setup listeners that will mark thread processing as complete
            ManualResetEvents mreC = new ManualResetEvents(CreateCount);

            for (int i = 0; i < CreateCount; i++)
            {
                ManualResetEvent mre = mreC.GetNextResetEvent();
                ThreadPool.QueueUserWorkItem(CreateHelper, mre);
            }

            ProcessUserStatesThread();

            for (int i = 0; i < mreC.MREventList.Count; i++)
            {
                ManualResetEvent[] mrArr;
                mreC.MREventList.TryGetValue(i, out mrArr);
                if (mrArr != null)
                    WaitHandle.WaitAll(mrArr);
            }

            ProcessUserStatesThread();
        }

        [Test]
        public void CreateApplicationTest()
        {
            
            IApplication_WTF app = CreateApplication(6, 1);
            Assert.IsNotNull(app);

        }

        protected void ProcessUserStatesThread()
        {
            //Get all the user states that have user activities
            IDbConnection con = Helper.GetSQLDBConnection("X2");

            DataTable dt = new DataTable();
            try
            {
                //Get all the user states that have user activities
                string strSQL = String.Format(@"select distinct p.Name, p.ID, w.Name, w.ID, s.Name, s.ID --, * 
                            from X2_WTF.X2.State s
                            join X2_WTF.X2.Activity a on a.StateID = s.ID and a.Type = 1
                            Join X2_WTF.X2.Workflow w on s.WorkflowID = w.ID
                            join X2_WTF.X2.Process p on w.ProcessID = p.ID
                            where s.Type = 1
                            and s.WorkFlowID in (
	                            select max(ID) as WorkFlowID--, max(ProcessID) as ProcessID, w.Name
	                            --*
	                            from X2_WTF.X2.WorkFlow w where w.Name like 'X2EngineTest%'
	                            Group by w.Name
                            )");

                con.Open();

                Helper.FillFromQuery(dt, strSQL, con, null);

                ManualResetEvent[] doneEvents = new ManualResetEvent[dt.Rows.Count];
                int count = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    Thread t = new Thread(new ParameterizedThreadStart(CreateUserStateHelper));
                    doneEvents[count] = new ManualResetEvent(false);
                    UserStateObj usobj = new UserStateObj(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()), dr[2].ToString(), Convert.ToInt32(dr[3].ToString()), dr[4].ToString(), Convert.ToInt32(dr[5].ToString()), _userPool, doneEvents[count]);
                    t.Start(usobj);
                    count += 1;
                }

                WaitHandle.WaitAll(doneEvents);
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();

                con.Dispose();
            }
        }

        private void CreateUserStateHelper(object obj)
        {
            UserStateObj usobj = obj as UserStateObj;

            UserStateHelper usHelper = new UserStateHelper(usobj.ProcessName, usobj.ProcessID, usobj.WorkflowName, usobj.WorkflowID, usobj.StateName, usobj.StateID, usobj.UserPool, usobj.DoneEvent);

            usHelper.Execute();
        }

        public class UserStateObj
        {
            public string ProcessName;
            public int ProcessID;
            public string WorkflowName;
            public int WorkflowID;
            public string StateName;
            public int StateID;
            public UserPool UserPool;
            public ManualResetEvent DoneEvent;

            public UserStateObj(string ProcessName, int ProcessID, string WorkflowName, int WorkflowID, string StateName, int StateID, UserPool UserPool, ManualResetEvent DoneEvent)//
            {
                this.ProcessName = ProcessName;
                this.ProcessID = ProcessID;
                this.WorkflowName = WorkflowName;
                this.WorkflowID = WorkflowID;
                this.StateName = StateName;
                this.StateID = StateID;
                this.UserPool = UserPool;
                this.DoneEvent = DoneEvent;
            }
        }

        protected void CreateHelper(object state)
        {
            ManualResetEvent _doneEvent = state as ManualResetEvent;

            #region Inputs

            //Create a real offer
            IApplication_WTF app = CreateApplication(6, 1);

            Dictionary<string, string> Inputs = new Dictionary<string, string>();
            Inputs.Add("ApplicationKey", app.Key.ToString());

            string ProcessName = Constants.WorkFlowProcessName.X2EngineTest;
            string ProcessVersion = (-1).ToString();
            string WorkFlowName = Constants.WorkFlowName.X2EngineTest;
            string ActivityName = Constants.WorkFlowActivityName.CreateInstanceAction;

            #endregion

            //Get a user, this method will prevent other processing from using the same user
            SAHLPrincipal p = _userPool.GetSAHLPrincipalForProcessing();

            Thread.CurrentPrincipal = p;

            //Create the case
            CreateCompleteInstance(p, ProcessName, ProcessVersion, WorkFlowName, ActivityName, Inputs, false);

            //release the user so other processing can use it
            _userPool.ReleaseSAHLPrincipal(p);

            _doneEvent.Set();
        }
    }
}
