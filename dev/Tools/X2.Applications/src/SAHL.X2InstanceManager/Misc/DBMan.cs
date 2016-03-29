using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SAHL.X2.Framework.DataSets;
using System.Configuration;
using System.Windows.Forms;

namespace SAHL.X2InstanceManager.Misc
{
    public class DBMan
    {
        // go make a app.Config section for this stuff.
        static string strConn = "Data Source=sahls103;Initial Catalog=X2;Persist Security Info=True;User ID=ServiceArchitect;Password=Service1Architect";//"Data Source=localhost;Initial Catalog=X2;Persist Security Info=True;User ID=sa;Password=sa";
        public static void  SetConnString(string X2DB)
        {
            strConn = X2DB;
        }

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

        public static DataRow GetX2WorkflowRow(string WorkflowName, string ProcessName)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select top 1 wf.* ");
                sb.Append("from [x2].[x2].Workflow wf (nolock) ");
                sb.Append("join [x2].[x2].Process p (nolock) on p.ID = wf.ProcessID ");
                sb.AppendFormat("where wf.Name = '{0}' and p.Name = '{1}'", WorkflowName, ProcessName);
                sb.Append("order by wf.ID desc");

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

        public static DataTable GetOfferRoleTypeList(string WorkflowName)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                DataSet ds = new DataSet();
                StringBuilder sb = new StringBuilder();
                // line below changed by cf to include offerroletypekey = 1 (for life consultant)
                if (WorkflowName == "LifeOrigination")
                    sb.AppendFormat("select * from [2am]..offerroletype where offerroletypegroupkey=1 and offerroletypekey = 1");
                else
                    sb.AppendFormat("select * from [2am]..offerroletype where offerroletypegroupkey=1 and offerroletypekey >100");

                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "ORT");
                da.Dispose();
                return ds.Tables["ORT"];
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
            return null;
        }

        public static void GetPotentialStatesForInstance(SAHL.X2.Framework.DataSets.X2 ds, Int64 InstanceID)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select s.* ");
                sb.AppendFormat(" from x2.state s join x2.workflow w on s.workflowid=w.id");
                sb.AppendFormat(" join x2.instance i on i.workflowid=w.id");
                sb.AppendFormat(" where i.id={0}", InstanceID);
                sb.AppendFormat(" order by s.name");
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "State");
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

        public static void GetX2Child(SAHL.X2.Framework.DataSets.X2 ds, Int64 InstanceID)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("declare @PID int ");
                sb.Append("declare @WID int ");
                sb.Append("declare @IID int ");
                sb.Append("set @PID = (select max (id) from x2.process where name='X2Bench') ");
                sb.Append("set @WID = (select max(id) from x2.workflow where name='X2Bench' and ProcessID=@PID) ");
                sb.Append("select * from x2.activity where workflowid=@WID ");
                sb.Append("select * from x2.State where workflowid=@WID ");
                sb.AppendFormat("set @IID = (select ID from x2.Instance where ParentInstanceID={0}) ", InstanceID);
                sb.AppendFormat("select * from x2.Instance where ParentInstanceID={0} ", InstanceID);
                sb.AppendFormat("select * from x2.worklist where InstanceID=@IID ");
                sb.AppendFormat("select * from x2.instanceactivitysecurity where InstanceID=@IID ");
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

        public static void GetActivityList(SAHL.X2.Framework.DataSets.X2 ds, int WID)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from x2.activity where workflowid={0} order by name", WID);
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "Activity");
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
        
        public static void GetProcessList(SAHL.X2.Framework.DataSets.X2 ds)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"select a.ID, p.name, p.createdate, p.version from x2.process p
join 
(select max(id) as id, Name from x2.process group by name) a
on p.id=a.id");
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "Process");
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
        
        public static void GetWorkflowList(SAHL.X2.Framework.DataSets.X2 ds, int PID)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select * from x2.workflow where ProcessID={0}", PID);
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.SelectCommand = cmd;
                da.Fill(ds, "Workflow");
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
        
        public static void UpdateInstanceState(int StateID, Int64 InstanceID)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update x2.instance set stateid={0} where id={1}", StateID, InstanceID);
                conn = CreateAndOpen();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.CommandText = sb.ToString();
                cmd.Connection = conn;
                da.UpdateCommand = cmd;
                da.UpdateCommand.ExecuteScalar();
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
                MessageBox.Show(ex.ToString());
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
