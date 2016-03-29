using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using System.Configuration;

namespace BaseTest
{
    public abstract class BaseWorker : IBaseWorker
    {
        protected Int64 InstanceID;
        protected int GenericKey;
        protected string MapKeyName;
        protected EngineConnector engine;
        protected bool Started = false;
        protected string WorkflowName;
        protected string ProcessName;
        protected ManualResetEvent mre = new ManualResetEvent(false);
        protected int NumberOfIterations = 1;
        protected object _Data = null;
        int Min=1,Max=1;
        public void SetSleepTimeRange(int Min, int Max)
        {
            this.Min = Min;
            this.Max = Max;
        }
        public void SetNumberIterations(int nInterations)
        {
            this.NumberOfIterations = nInterations;
        }
        protected int SleepTime 
        {
            get
            {
                return new Random().Next(Min, Max);
            }
        }

        public void Setup(Int64 InstanceID, int GenericKey, string MapKeyName, string WorkflowName, string ProcessName, string ADUserName, object Data)
        {
            this.GenericKey = GenericKey;
            this.MapKeyName = MapKeyName;
            this.WorkflowName = WorkflowName;
            this.ProcessName = ProcessName;
            string EngineURL = ConfigurationSettings.AppSettings["X2"];
            engine = new EngineConnector(WorkflowName, ProcessName, EngineURL);
            engine.SetADUser(ADUserName);
            this.InstanceID = InstanceID;
            _Data = Data;
        }

        public virtual bool Start()
        {
            try
            {
                mre.Reset();
                Thread t = new Thread(new ThreadStart(Work));
                t.Name = string.Format("Worker-{0}", GenericKey);
                t.Start();
                Started = true;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                throw;
            }
            return Started;
        }

        public virtual void Stop()
        {
            Started = false;
            mre.Set();
        }

        protected string GetCaseState(Int64 InstanceID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select s.name ");
            sb.AppendFormat("from x2.instance i join x2.state s on i.stateid=s.id ");
            sb.AppendFormat("where i.id={0}", InstanceID);
            DataSet ds = DBMan.ExecuteSQL(sb.ToString());
            string CaseStateName = ds.Tables[0].Rows[0][0].ToString();

            return CaseStateName;
        }
        protected bool CheckAndWaitForState(string StateName, Int64 InstanceID, int nTimesToCheck)
        {
            int Count = 0;
            string State = GetCaseState(InstanceID);
            while (!string.Equals(State.ToLower(), StateName.ToLower()))
            {
                Count++;
                if (Count >= nTimesToCheck || !Started)
                    return false;
                Thread.Sleep(new Random().Next(250,500));
                State = GetCaseState(InstanceID);
            }

            return true;
        }

        public abstract void Work();
    }
}
