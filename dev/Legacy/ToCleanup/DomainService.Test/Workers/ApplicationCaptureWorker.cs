using System;
using System.Collections.Generic;
using System.Text;
using BaseTest;
using System.Threading;
using System.Data;

namespace Workers
{
    public class ApplicationCaptureWorker : BaseWorker
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

                // create case with offerkey
                Console.WriteLine("Create Case");
                InstanceID = engine.CreateCase(SessionID, WorkflowName, GenericKey, MapKeyName, ref Error, "Create Instance");
                Thread.Sleep(SleepTime);
                //return false;
                if (string.IsNullOrEmpty(Error))
                {
                    // Check case is at the Application Capture State if not wait
                    if (!CheckAndWaitForState("Application Capture", InstanceID, 200))
                        return false;

                    // perform the "Add Memo Action" - just cause
                    //Console.WriteLine("Add Memo {0}", InstanceID);
                    //b = engine.PerformAction(SessionID, InstanceID, "Add Memo", ref Error, null);
                    //Thread.Sleep(SleepTime);

                    //// Create a followup for NOW - ccase will get moved back to app capture when timer fires.
                    //Console.WriteLine("Create Followup {0}", InstanceID);
                    //int MemoKey = CreateFollowup(GenericKey);
                    //Dictionary<string, string> dict = new Dictionary<string, string>();
                    //dict.Add("GenericKey", MemoKey.ToString());
                    //b = engine.PerformAction(SessionID, InstanceID, "Create Followup", ref Error, dict);
                    //Thread.Sleep(SleepTime);
                    //if (b)
                    //{
                    //    // Check case is back at Ready to Followup - wiat if not
                    //    Console.WriteLine("Ready To Followup {0}", InstanceID);
                    //    if (!CheckAndWaitForState("Ready to Followup", InstanceID, 200))
                    //    {
                    //        return false;
                    //    }
                    //    b = engine.PerformAction(SessionID, InstanceID, "Continue with Application", ref Error, null);
                    //    Thread.Sleep(SleepTime);
                    //    if (!b)
                    //    {
                    //        // poked we are
                    //        Console.WriteLine(string.Format("*** {0}", Error));
                    //        return false;
                    //    }
                    //}

                    // escalate to Manager
                    Console.WriteLine("Escalate to Manager {0}", InstanceID);
                    b = engine.PerformAction(SessionID, InstanceID, "Escalate to Manager", ref Error, null);
                    Thread.Sleep(SleepTime);
                    if (!b)
                    {
                        string why = "?";
                    }
                    // perform the Submit Application (will fail cause of rules)
                    Console.WriteLine("Manager Submit Application {0}", InstanceID);
                    b = engine.PerformAction(SessionID, InstanceID, "Manager Submit Application", ref Error, null);
                    Thread.Sleep(SleepTime);
                    if (!b)
                    {


                        // Decline case
                        Console.WriteLine("Decline {0}", InstanceID);
                        b = engine.PerformAction(SessionID, InstanceID, "Decline", ref Error, null);
                        Thread.Sleep(SleepTime);
                        if (!b)
                        {
                            Console.WriteLine(string.Format("*** {0}", Error));
                            return false;
                        }

                        // Decline Final the case
                        Console.WriteLine("Decline Final {0}", InstanceID);
                        b = engine.PerformAction(SessionID, InstanceID, "Decline Finalised", ref Error, null);
                        Thread.Sleep(SleepTime);
                        if (!b)
                        {
                            Console.WriteLine(string.Format("*** {0}", Error));
                            return false;
                        }
                        Thread.Sleep(SleepTime);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        protected int CreateFollowup(int OfferKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into [2am]..memo values (2, {0}, getdate(), 'Load Test', 1618, null, 1, getdate(), null); select @@identity", OfferKey);
            DataSet ds = DBMan.ExecuteSQL(sb.ToString());
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }
    }
}
