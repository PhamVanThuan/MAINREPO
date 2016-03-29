using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using Castle.ActiveRecord;
using DomainService2;
using Ninject;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Config;
using SAHL.Common.BusinessModel.Configuration;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using SAHL.X2.Common;
using SAHL.X2.Common.DataAccess;
using SAHL.X2.Framework.DataAccess;

using X2DomainService.Interface.Origination;

namespace X2DomainService.ServiceTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
        }

        private static void DoShit()
        {
        }

        internal class DBMan
        {
            //string connstr = "Data Source=DEVB03;Initial Catalog=Warehouse;Persist Security Info=True;User ID=eworkadmin2;Password=W0rdpass;";

            //internal void ExecuteNonQuery(string SQL)
            //{
            //    try
            //    {
            //        using (SqlConnection conn = new SqlConnection(connstr))
            //        {
            //            conn.Open();
            //            using (SqlCommand cmd = new SqlCommand(SQL))
            //            {
            //                cmd.Connection = conn;
            //                int nRecords = cmd.ExecuteNonQuery();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
            //}
        }

        private static void Test()
        {
            //IX2ReturnData ret = null;
            //string AssignedUser = string.Empty;
            //int ApplicationKey = 3;
            //for (int i = 0; i < 100; i++)
            //{
            //    try
            //    {
            //        using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            //        using (IActiveDataTransaction Tran = IActiveDataTransactionFactory.BeginTransaction("asd", TranCallerType.ExternalEngine))
            //        {
            //            try
            //            {
            //                Valuations val = new Valuations();
            //                val.RecalcHOC(50000354, 1015891, false);
            //            }
            //            catch (Exception ex)
            //            {
            //                //IActiveDataTransactionFactory.RollBackTransaction(Tran, false);
            //            }
            //        }
            //        Console.WriteLine("{1}-{0}", i, Thread.CurrentThread.Name);
            //    }
            //    catch (Exception ex)
            //    {
            //        string s = ex.ToString();
            //    }
            //}
        }
    }
}