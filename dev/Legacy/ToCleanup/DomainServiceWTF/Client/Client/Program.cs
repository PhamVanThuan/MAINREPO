using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using SAHL.X2.Common;
//using OriginationClient;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataAccess;
using System.Reflection;
using System.Threading;
using System.Data.SqlClient;

namespace Client
{
    public class Work
    {
        internal class DBMan
        {
            string connstr = "Data Source=DEVA03;Initial Catalog=Warehouse;Persist Security Info=True;User ID=eworkadmin2;Password=W0rdpass;";
            internal void ExecuteNonQuery(string SQL)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(SQL))
                        {
                            cmd.Connection = conn;
                            int nRecords = cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static void DoWork(int ApplicationKey, Int64 InstanceID)
        {
            IX2ReturnData ret = null;
            string AssignedUser = string.Empty;
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    using (IActiveDataTransaction Tran = IActiveDataTransactionFactory.BeginTransaction("asd", TranCallerType.ExternalEngine))
                    {
                        try
                        {
                            if (null == Tran || null == Tran.CurrentTransaction)
                            {
                                Console.WriteLine("*** bad bad bad null tran! ***");
                                continue;
                            }
                            //ret = ApplicationCapture.Test(Tran);
                            //ret = ApplicationCapture.MarkUsersAsInactive(Tran, 
                            string SQL = string.Format("insert into x2..test values ('{0}', getdate())", i);
                            //new DBMan().ExecuteNonQuery(SQL);
                            //WorkerHelper.ExecuteNonQuery(Tran.Context, SQL, new ParameterCollection());
                            if (null != ret && ret.MC.ErrorMessages.Count > 0)
                            {
                                Console.WriteLine("MC's for {0}", ApplicationKey);
                                IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
                            }
                            else
                            {
                                if (i % 2 == 0)
                                    IActiveDataTransactionFactory.CommitTransaction(Tran);
                                else
                                    IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
                            }
                        }
                        catch (Exception ex)
                        {
                            IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
                        }
                    }
                    Console.WriteLine("{1}-{0}", i, Thread.CurrentThread.Name);
                }
            }
            catch (RemotingException re)
            {
                Console.WriteLine(re.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class StartObj
    {
        public int ApplicationKey;
        public Int64 InstanceID;
        public StartObj(int A, Int64 I)
        {
            ApplicationKey = A;
            InstanceID = I;
        }
    }
    class Program
    {
        public static void SeedLog()
        {
            LogSettingsClass lsl = new LogSettingsClass();
            lsl.AppName = "Origination Client Proxy";
            lsl.ConsoleLevel = Convert.ToInt32(3);
            lsl.ConsoleLevelLock = Convert.ToBoolean(false);
            lsl.FileLevel = Convert.ToInt32(3);
            lsl.FileLevelLock = Convert.ToBoolean(false);
            lsl.FilePath = @"c:\logs\";
            lsl.NumDaysToStore = 14;
            lsl.RollOverSizeInKB = 2048;
            LogPlugin.SeedLogSettings(lsl);
        }

        static void Main(string[] args)
        {
            SeedLog();
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            StartWork();

            Console.WriteLine("Workers Created");
            Console.ReadLine();
        }

        static void StartWork()
        {
            StartObj s = new StartObj(612591, 946323);
            Thread t = new Thread(new ParameterizedThreadStart(DoShit));
            t.Name = "612591";
            t.Start(s);
            return;
            s = new StartObj(559740, 946610);
            t = new Thread(new ParameterizedThreadStart(DoShit));
            t.Name = "559740";
            t.Start(s);

            s = new StartObj(623053, 946358);
            t = new Thread(new ParameterizedThreadStart(DoShit));
            t.Name = "623053";
            t.Start(s);

            s = new StartObj(594397, 946297);
            t = new Thread(new ParameterizedThreadStart(DoShit));
            t.Name = "594397";
            t.Start(s);

            s = new StartObj(624642, 945267);
            t = new Thread(new ParameterizedThreadStart(DoShit));
            t.Name = "624642";
            t.Start(s);


        }

        static void DoShit(object o)
        {
            StartObj s = o as StartObj;
            Work.DoWork(s.ApplicationKey, s.InstanceID);
        }

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string AssemblyName = args.Name;
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
            // Use FullName Type Resolve
            foreach (Assembly asm in asms)
            {
                if (asm.FullName == AssemblyName)
                    return asm;
            }
            // Use simple name resolution. This may cause the TypeFactoryCreate to throw if the asm is the wrong one (ie wrong metadata)
            foreach (Assembly asm in asms)
            {
                int pos = asm.FullName.IndexOf(',');
                string tn = asm.FullName.Substring(0, pos);
                if (tn == AssemblyName)
                    return asm;
            }
            LogPlugin.LogWarning("Unable to RESOLVE assembly:{0}", args.Name);
            return null;
        }

    }
}
