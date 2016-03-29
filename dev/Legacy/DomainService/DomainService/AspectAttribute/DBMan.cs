using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AspectAttribute
{
    public class DBMan
    {
        // go make a app.Config section for this stuff.
        public static string strConn = "Data Source=DEVB03;Initial Catalog=X2_WTF;Persist Security Info=True;User ID=EWorkadmin2;Password=W0rdpass";//"Data Source=localhost;Initial Catalog=X2;Persist Security Info=True;User ID=sa;Password=sa";

        static DBMan()
        {
            Configuration c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringSettings cs = c.ConnectionStrings.ConnectionStrings["SAHL.X2.Framework.Properties.Settings.DBConnectionString"];
            strConn = cs.ConnectionString;
        }

        protected static SqlConnection CreateAndOpen()
        {
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            return conn;
        }

        public static DataSet ExecuteSQL(string SQL)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataSet ds = new DataSet();
            try
            {
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "Activity");
                da.Dispose();
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
            return ds;
        }

        public static void ExecuteNonQuery(string SQL)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            DataSet ds = new DataSet();
            try
            {
                conn = CreateAndOpen();
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