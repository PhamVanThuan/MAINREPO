using System;
using System.Collections.Generic;
using System.Text;
using BaseTest;
using System.Threading;

namespace Workers
{
    public class DemoWorker : BaseWorker
    {
        public override void Work()
        {
            DoMapLifeCycle();
        }

        public bool DoMapLifeCycle()
        {
            string Error = string.Empty;
            string SessionID = string.Empty;
            SessionID = engine.Login(ref Error);
            bool b = false;
            if (string.IsNullOrEmpty(Error))
            {
                Thread.Sleep(SleepTime);

                // Create case.
                Console.WriteLine("Create Case");
                InstanceID = engine.CreateCase(SessionID, WorkflowName, GenericKey, MapKeyName, ref Error, "Create Instance");
                Thread.Sleep(SleepTime);
                if (string.IsNullOrEmpty(Error))
                {
                    // Wait 5 seconds to ensure e-work case created. 
                    Thread.Sleep(5000);

                    // Check we are at the UserStateCallEWork State
                    if (!CheckAndWaitForState("UserStateCallEWork", InstanceID, 10))
                    {
                        // try Retry CreateEWorkCase
                        Console.WriteLine("Retry CreateEWorkCase {0}", InstanceID);
                        b = engine.PerformAction(SessionID, InstanceID, "Retry CreateEWorkCase", ref Error, null);
                        Thread.Sleep(SleepTime);
                        if (!b)
                        {
                            Console.WriteLine(Error);
                            return false;
                        }
                    }

                    // Perform the "PerformEWorkActivity" Action
                    Console.WriteLine("PerformEWorkActivity {0}", InstanceID);
                    b = engine.PerformAction(SessionID, InstanceID, "PerformEWorkActivity", ref Error, null);
                    Thread.Sleep(SleepTime);
                    if (!b)
                    {
                        Console.WriteLine(Error);
                        return false;
                    }

                    // Insert an ext activity to simulate e-work coming back.
                    InsertExternalActivity(InstanceID, "EXTEWORKDONE");
                    Console.WriteLine("Waiting for Pickup of EXT");
                    // Wait for it to be picked up by the engine and executed
                    Thread.Sleep(5000);
                    b = CheckAndWaitForState("EWorkComplete", InstanceID, 100);
                    if (!b)
                    {
                        Console.WriteLine("EXT flag not firing case not at EWorkComplete - Aborting");
                        return false;
                    }

                    // Call "CompleteCase"
                    Console.WriteLine("CompleteCase {0}", InstanceID);
                    b = engine.PerformAction(SessionID, InstanceID, "CompleteCase", ref Error, null);
                    Thread.Sleep(SleepTime);
                    if (!b)
                    {
                        Console.WriteLine(Error);
                        return false;
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine(Error);
                    return false;
                }
            }
            else
            {
                Console.WriteLine(Error);
                return false;
            }
        }

        private void InsertExternalActivity(Int64 InstnaceID, string EXTNAme)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("declare @IID bigint ");
            sb.AppendFormat("set @IID={0} ", InstnaceID);
            sb.AppendFormat("declare @EID int ");
            sb.AppendFormat("declare @WID int ");
            sb.AppendFormat("select top 1 @EID=ID,@WID=WorkflowID from x2.externalactivity where name='{0}' order by id desc ", EXTNAme);
            sb.AppendFormat("insert into x2.activeexternalactivity values (@EID, @WID, @IID, getdate(), null, null) ");
            DBMan.ExecuteSQL(sb.ToString());
        }
    }
}
