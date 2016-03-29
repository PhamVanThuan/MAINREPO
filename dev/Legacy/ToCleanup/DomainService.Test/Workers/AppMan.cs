using System;
using System.Collections.Generic;
using System.Text;
using BaseTest;
using System.Threading;
using System.Data;

namespace Workers
{
    public class AppMan : BaseWorker
    {
        public override void Work()
        {
            int Count = 0;
            while (!mre.WaitOne(0, false) && Started)
            {
                try
                {
                    bool b = DoCaseLifecycle();
                    Thread.Sleep(SleepTime);
                    Count++;
                    if (NumberOfIterations != -1)
                    {
                        if (Count >= NumberOfIterations)
                            Started = false;
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.ToString();
                    Console.WriteLine(s);
                }
            }
        }

        public bool DoCaseLifecycle()
        {
            string Error = string.Empty;
            string SessionID = string.Empty;
            bool b = false;
            SessionID = engine.Login(ref Error);
            if (string.IsNullOrEmpty(Error))
            {
                Thread.Sleep(SleepTime);
                b = engine.PerformAction(SessionID, InstanceID, "Application Received", ref Error, new Dictionary<string, string>());
                if (!b)
                {
                    GetNewCase();
                    return false;
                }
                Console.WriteLine("Application Recieved: {0}", GenericKey);
                Thread.Sleep(SleepTime);
                b = CheckAndWaitForState("QA", InstanceID, 25);
                if (!b)
                {
                    GetNewCase();
                    return false;
                }
                b = engine.PerformAction(SessionID, InstanceID, "QA Complete", ref Error, new Dictionary<string, string>());
                if (!b)
                {
                    GetNewCase();
                    return false;
                }
                Console.WriteLine("QA Complete: {0}", GenericKey);
                Thread.Sleep(SleepTime);
                // wait for the thing to get to manage application. Of course it may end up at Application Hold
                b = CheckAndWaitForState("Manage Application", InstanceID, 25);
                if (!b)
                {
                    GetNewCase();
                    return false;
                }
                for (int i = 0; i < 25; i++)
                {
                    b = engine.PerformAction(SessionID, InstanceID, "Application in Order", ref Error, new Dictionary<string, string>());
                    if (!b)
                    {
                        if (Error == "Activity Application In Order does not exist.")
                        {
                            GetNewCase();
                            return false;
                        }
                        Console.WriteLine("{0} - {1}", GenericKey, Error);
                    }
                    else
                    {
                        Console.WriteLine("Application in Order: {0}", GenericKey);
                        // case got submitted (shouldnt unless there is a DTC error 2.0.2.34 will fix this but in 2.0.2.12 not so.
                        GetNewCase();
                        return false;
                    }
                }
            }
            return false;
        }

        void GetNewCase()
        {
            // go get new app key
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select top 1 ");
            sb.AppendFormat("d.applicationkey, d.InstanceID ");
            sb.AppendFormat("from x2data.application_management d ");
            sb.AppendFormat("join x2.instance i on d.instanceid=i.id ");
            sb.AppendFormat("join x2.state s on i.stateid=s.id ");
            sb.AppendFormat("where s.name='Awaiting Applictaion' order by i.id desc");
            DataSet ds = DBMan.ExecuteSQL(sb.ToString());
            GenericKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            InstanceID = Convert.ToInt64(ds.Tables[0].Rows[0][1]);
        }
    }
}
