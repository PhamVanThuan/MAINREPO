using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace PDFDocumentWriter.DataAccess
{
    public class DBMan
    {
        public static string strConn;
        static DBMan()
        {
            strConn = ConfigurationManager.ConnectionStrings["SAHL.Common.Web.Properties.Settings.DBConnectionString"].ConnectionString;
            
        }

        protected static SqlConnection CreateAndOpen()
        {

            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            return conn;
        }

        #region Standard OLEDBstuff for SQL
        public static void ExecuteNonQuery(string SQL)
        {
            try
            {
                using (SqlConnection conn = CreateAndOpen())
                {
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
              
        }

        public static DataTable ExecuteSQL(string SQL)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = CreateAndOpen())
                {
                    using (SqlCommand cmd = new SqlCommand(SQL, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            da.SelectCommand = cmd;
                            da.Fill(ds, "DATA");
                            da.Dispose();
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static object ExecuteScalar(string SQL)
        {
            object result = null;
            try
            {
                try
                {
                    using (SqlConnection conn = CreateAndOpen())
                    {
                        using (SqlCommand cmd = new SqlCommand(SQL, conn))
                        {
                            result = cmd.ExecuteScalar();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        
        public static void FillFromQuery(DataTable dt, string Query, SqlParameterCollection Params)
        {
            using (SqlConnection conn = CreateAndOpen())
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand(Query, conn))
                    {
                        da.SelectCommand = cmd;
                        if (null != Params)
                        {
                            foreach (SqlParameter prm in Params)
                            {
                                da.SelectCommand.Parameters.Add(prm);
                            }
                        }
                        da.Fill(dt);

                    }
                }
            }
        }

        public static void FillMultiTable(DataSet ds, List<string> TableMapping, string Query, Dictionary<string, object> Params)
        {
            using (SqlConnection conn = CreateAndOpen())
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = new SqlCommand(Query, conn))
                    {
                        da.SelectCommand = cmd;
                        if (null != Params)
                        {
                            string[] Keys = new string[Params.Keys.Count];
                            Params.Keys.CopyTo(Keys, 0);
                            foreach (string s in Keys)
                            {
                                da.SelectCommand.Parameters.Add(new SqlParameter(s, Params[s]));
                            }
                        }
                        if ((TableMapping != null) && (TableMapping.Count > 0))
                        {
                            da.TableMappings.Add("Table", TableMapping[0]);
                            for (int i = 1; i < TableMapping.Count; i++)
                            {
                                da.TableMappings.Add("Table" + i, TableMapping[i]);
                            }
                        }
                        //for (int i = 0; i < TableMapping.Count; i++)
                        //{
                        //    da.TableMappings.Add(TableMapping[i], TableMapping[i]);
                        //}
                        da.Fill(ds);
                    }
                }
            }
        }

        #endregion
    }
}
