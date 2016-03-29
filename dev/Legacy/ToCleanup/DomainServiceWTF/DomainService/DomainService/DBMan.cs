using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace DomainService
{
    public class DBMan
    {
        //static string strConn = "Data Source=localhost;Initial Catalog=DB;Persist Security Info=True;User ID=sa;Password=sa";
        static string X2Conn = "Data Source=DEVA03;Initial Catalog=x2skinny;Persist Security Info=True;User ID=EWorkadmin2;Password=W0rdpass";
        static string AM2Conn = "Data Source=DEVA03;Initial Catalog=2am;Persist Security Info=True;User ID=EWorkadmin2;Password=W0rdpass";
        static DBMan()
        {
            Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringSettings cs = c.ConnectionStrings.ConnectionStrings[2];
            X2Conn= cs.ConnectionString;
            cs = c.ConnectionStrings.ConnectionStrings[1];
            AM2Conn = cs.ConnectionString;
            
        }
        protected static SqlConnection CreateAndOpenX2()
        {
            SqlConnection conn = new SqlConnection(X2Conn);
            conn.Open();
            return conn;
        }

        protected static SqlConnection CreateAndOpen2am()
        {
            SqlConnection conn = new SqlConnection(AM2Conn);
            conn.Open();
            return conn;
        }

        public static void ClearConnPool()
        {
            SqlConnection.ClearAllPools();
        }

        public static void ClearThisPool(SqlConnection conn)
        {
            SqlConnection.ClearPool(conn);
        }

        public static DataSet X2ExecuteSQL(string SQL)
        {
            SqlConnection conn = null;
            try
            {
                conn = CreateAndOpenX2();
                return ExecuteSQL(conn, SQL, true);
            }
            finally
            {
                if (null != conn)
                    conn.Dispose();
            }
        }

        public static DataSet AM2ExecuteSQL(string SQL)
        {
            SqlConnection conn = null;
            try
            {
                conn = CreateAndOpenX2();
                return ExecuteSQL(conn, SQL, true);
            }
            finally
            {
                if (null != conn)
                    conn.Dispose();
            }
        }

        /// <summary>
        /// There is a bug in .NET 2.0 and distributed trans where the dis Tran can corrupt
        /// the whole connection pool. When this happens a IOE excep is thrown. Thinking of this
        /// is that we recycle the pool as per the MS suggestion and try again. If it fails after 
        /// a recycle the throw the error to the caller.
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="FirstPass"></param>
        /// <returns></returns>
        protected static DataSet ExecuteSQL(SqlConnection conn, string SQL, bool FirstPass)
        {
            SqlCommand cmd = null;
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "DATA");
                da.Dispose();
            }
            catch (InvalidOperationException ioe)
            {
                ClearThisPool(conn);
                if (FirstPass)
                    return ExecuteSQL(conn, SQL, false);
                else
                    throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (null != cmd)
                    cmd.Dispose();
            }
            return ds;
        }

        /// <summary>
        /// Future use perhaps. As this can potentially do updates outside of a tran bad things
        /// can happen.
        /// </summary>
        /// <param name="SQL"></param>
        protected static void ExecuteNonQuery(string SQL)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataSet ds = new DataSet();
            try
            {
                conn = CreateAndOpenX2();
                cmd = new SqlCommand(SQL, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (null != cmd)
                    cmd.Dispose();
                if (null != conn)
                    conn.Close();
            }
        }
    }
}
