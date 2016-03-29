using System;
using System.Collections.Generic;
using System.Text;
using DomainService.Workflow;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataAccess;
using SAHL.X2.Common;
using System.Threading;
using System.Data.SqlClient;

namespace X2DomainService.ServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DomainService.X2DomainService svc = new DomainService.X2DomainService();
            try
            {
                //DomainService.X2DomainService.InitialiseActiveRecord();
                svc.OnStart(null);
                //DoShit();
                Console.WriteLine("Started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
            //svc.OnStop();
        }

        static void DoShit()
        {
            int nThreads = 10;
            for (int i = 0; i < nThreads; i++)
            {
                Thread t = new Thread(new ThreadStart(Test));
                t.Name = i.ToString();
                t.Start();
            }
        }

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

        static void Test()
        {
            IX2ReturnData ret = null;
            string AssignedUser = string.Empty;
            int ApplicationKey = 3;
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                    //using (IActiveDataTransaction Tran = IActiveDataTransactionFactory.BeginTransaction("asd", TranCallerType.ExternalEngine))
                    {
                        try
                        {
                            string query = @"INSERT INTO [Warehouse].[dbo].[Audits]
           ([AuditDate],[ApplicationName],[HostName],[WorkStationID],[WindowsLogon]
           ,[FormName],[TableName],[PrimaryKeyName],[PrimaryKeyValue],[AuditData])
     VALUES(getdate(), 'DomainService', 'sahls118', '1', 'sahl\bla', 'form', 'OfferRole', 
            'OfferRoleKey', '1111', '<XML>')";
//                            string query = @"INSERT INTO [x2skinny]..[Audits]
//           ([AuditDate],[ApplicationName],[HostName],[WorkStationID],[WindowsLogon]
//           ,[FormName],[TableName],[PrimaryKeyName],[PrimaryKeyValue],[AuditData])
//     VALUES(getdate(), 'DomainService', 'sahls118', '1', 'sahl\bla', 'form', 'OfferRole', 
//            'OfferRoleKey', '1111', '<XML>')";

                            //query = "insert into x2..test values ('test', getdate())";
                            DBMan d = new DBMan();
                            d.ExecuteNonQuery(query);
                            //SAHL.X2.Framework.DataAccess.WorkerHelper.ExecuteNonQuery(Tran.Context, query, new ParameterCollection());

                            if (i % 2 == 0)
                            {
                                //IActiveDataTransactionFactory.CommitTransaction(Tran);
                                ts.Complete();
                            }
                            else
                            {
                               // IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
                                //ts.Complete();
                            }
                        }
                        catch (Exception ex)
                        {
                            //IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
                        }
                    }
                    Console.WriteLine("{1}-{0}", i, Thread.CurrentThread.Name);
                }
                catch (Exception ex)
                {
                    string s = ex.ToString();
                }
            }
        }
    }
}
