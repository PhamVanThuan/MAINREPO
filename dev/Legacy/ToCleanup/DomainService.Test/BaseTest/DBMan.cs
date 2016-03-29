using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BaseTest
{
    public class DBMan
    {
        // go make a app.Config section for this stuff.
        static string strConn = "Data Source=DEVA03;Initial Catalog=X2;Persist Security Info=True;User ID=ServiceArchitect;Password=Service1Architect";//"Data Source=localhost;Initial Catalog=X2;Persist Security Info=True;User ID=sa;Password=sa";
        static DBMan()
        {
            strConn = ConfigurationManager.ConnectionStrings[1].ConnectionString;
        }
        //public static void SetConnString(string X2DB)
        //{
        //    strConn = ConfigurationManager.ConnectionStrings[1].ConnectionString;
        //    strConn = X2DB;
        //}

        protected static SqlConnection CreateAndOpen()
        {
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            return conn;
        }

        public static DataRow GetX2DataRow(Int64 InsatnceID, string DataTableName)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from x2data.{0} where Instanceid={1}", DataTableName, InsatnceID);

                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                DataSet ds = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataRow dr = ds.Tables[0].Rows[0];
                da.Dispose();
                return dr;
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
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

        public static void GetX2Data(SAHL.X2.Framework.DataSets.X2 ds, Int64 InstanceID)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("declare @PID int ");
                sb.Append("declare @WID int ");
                sb.Append("set @PID = (select max (id) from x2.process where name='X2Bench') ");
                sb.Append("set @WID = (select max(id) from x2.workflow where name='X2Bench' and ProcessID=@PID) ");
                sb.Append("select * from x2.activity where workflowid=@WID ");
                sb.Append("select * from x2.State where workflowid=@WID ");
                sb.AppendFormat("select * from x2.Instance where ID={0}", InstanceID);
                sb.AppendFormat("select * from x2.worklist where InstanceID={0}", InstanceID);
                sb.AppendFormat("select * from x2.instanceactivitysecurity where InstanceID={0}", InstanceID);

                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.TableMappings.Add("Table", "Activity");
                da.TableMappings.Add("Table1", "State");
                da.TableMappings.Add("Table2", "Instance");
                da.TableMappings.Add("Table3", "WorkList");
                da.TableMappings.Add("Table4", "InstanceActivitySecurity");

                da.Fill(ds);
                da.Dispose();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
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

        public static void RunNonQuery(string SQL)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = SQL;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
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
                Console.WriteLine(ex.ToString());
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
    }
}
