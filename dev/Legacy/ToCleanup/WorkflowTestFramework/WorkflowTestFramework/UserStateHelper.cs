using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Common.DataAccess;
using System.Threading;
using SAHL.Common.Security;

namespace WorkflowTestFramework
{
    public class UserStateHelper : BaseHelper
    {
        #region Locals
        private ManualResetEvent _doneEvent;
        private UserPool _userPool;
        private string _stateName;
        private int _stateID;
        private string _processName;
        private int _processID;
        private string _workflowName;
        private int _workflowID;
        private IDictionary<int, string> _activities;
        private IList<Int64> _instances;
        #endregion
        
        public UserStateHelper(string ProcessName, int ProcessID, string WorkflowName, int WorkflowID, string StateName, int StateID, UserPool UserPool, ManualResetEvent DoneEvent)//
        {
            _processName = ProcessName;
            _processID = ProcessID;
            _workflowName = WorkflowName;
            _workflowID = WorkflowID;
            _stateName = StateName;
            _stateID = StateID;
            _userPool = UserPool;
            _doneEvent = DoneEvent;
            Startup();
        }

        private void Startup()
        {
            IDbConnection con = Helper.GetSQLDBConnection("X2");
            IDataReader dr = null;
            _activities = new Dictionary<int, string>();

            try
            {
                //Get a list of activities for this state
                string strSQL = String.Format(@"select distinct a.Name, a.Priority--, * 
                                            from X2_WTF.X2.State s
                                            join X2_WTF.X2.Activity a on a.StateID = s.ID and a.Type = 1
                                            Join X2_WTF.X2.Workflow w on s.WorkflowID = w.ID
                                            where w.ID = {0} and s.ID = {1}", _workflowID, _stateID);

                con.Open();

                dr = Helper.ExecuteReader(con, strSQL);

                //create a thread to handle each state for processing
                while (dr.Read())
                {
                    KeyValuePair<int, string> kvp = new KeyValuePair<int, string>(dr.GetInt32(1), dr.GetString(0));
                    if (!_activities.Contains(kvp))
                        _activities.Add(kvp);
                }
            }
            finally
            {
                if (dr != null)
                    dr.Dispose();
                con.Dispose();
            }
        }

        public void Execute()
        {
            //Loop for a determined amount of time...
            DateTime dtStart = DateTime.Now;
            //make it one minute
            TimeSpan ts = Properties.Settings.Default.StateWaitTime; 

            //and loop
            while (true)
            {
                bool didWork = false;

                //Get any instances in this state for processing
                GetInstancesToProcess();

                //Process an activity
                //Queue in the Thread Pool
                foreach (int instanceID in _instances)
                {
                    didWork = true;

                    //still need an activity picker, use random for now
                    Random r = new Random();
                    int i = r.Next(1, _activities.Count);

                    ProcessActivity(instanceID, _activities[i]);
                }

                if (!didWork)
                {
                    TimeSpan ts1 = DateTime.Now.Subtract(dtStart);
                    if (ts1.CompareTo(ts) > 0) //we have been waiting longer than we set out to wait
                        break;
                }
                else
                {
                    Thread.Sleep(5000); //wait for 5 seconds, so some other threads can do some work
                    dtStart = DateTime.Now;//reset the start time
                }
            }

            _doneEvent.Set();

        }

        private void GetInstancesToProcess()
        {
            IDbConnection con = Helper.GetSQLDBConnection("X2");
            IDataReader dr = null;

            if (_instances == null)
                _instances = new List<Int64>();
            else
                _instances.Clear();

            try
            {
                //Get a list of instances in this state
                string strSQL = String.Format(@"select wtf.InstanceID  
                    from x2_wtf.X2DATA.{0} wtf (nolock)
                    join x2_wtf.x2.Instance i (nolock) on i.ID = wtf.InstanceID
                    where i.StateID = {1}", _workflowName, _stateID);

                con.Open();

                dr = Helper.ExecuteReader(con, strSQL);

                //loop and process
                while (dr.Read())
                {
                    Int64 instanceID = dr.GetInt64(0);

                    if (!_instances.Contains(instanceID))
                        _instances.Add(instanceID);
                }
            }
            finally
            {
                if (dr != null)
                    dr.Dispose();
                con.Dispose();
            }
        }

        private void ProcessActivity(int instanceID, string ActivityName)
        {
            //Get a user, this method will prevent other processing from using the same user
            SAHLPrincipal p = _userPool.GetSAHLPrincipalForProcessing();

            Thread.CurrentPrincipal = p;

            StartCompleteActivity(p, instanceID, ActivityName, null, false);

            //release the user so other processing can use it
            _userPool.ReleaseSAHLPrincipal(p);
        }
    }
}
