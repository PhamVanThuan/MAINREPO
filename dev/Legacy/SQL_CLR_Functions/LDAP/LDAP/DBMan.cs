using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace LDAP
{
    public class UpdateInfo
    {
        string _InsertText;
        public string InsertText
        {
            get { return _InsertText; }
            set { _InsertText = value; }
        }
        string _UpdateText;
        public string UpdateText
        {
            get { return _UpdateText; }
            set { _UpdateText = value; }
        }
        string _DeleteText;
        public string DeleteText
        {
            get { return _DeleteText; }
            set { _DeleteText = value; }
        }
        public UpdateInfo(string Insert, string Update, string Delete)
        {
            _InsertText = Insert;
            _UpdateText = Update;
            _DeleteText = Delete;
        }

        // how do I create one of there?
        // Upload new image once you do this
        // then test update and then we have a good framework for strong typed datasets.
        List<SqlParameter> _InsertParams = new List<SqlParameter>();

        public List<SqlParameter> InsertParams
        {
            get { return _InsertParams; }
            set { _InsertParams = value; }
        }
        List<SqlParameter> _UpdateParams = new List<SqlParameter>();

        public List<SqlParameter> UpdateParams
        {
            get { return _UpdateParams; }
            set { _UpdateParams = value; }
        }
        List<SqlParameter> _DeleteParams = new List<SqlParameter>();

        public List<SqlParameter> DeleteParams
        {
            get { return _DeleteParams; }
            set { _DeleteParams = value; }
        }

    }

    public class DBMan
    {
        public static string strConn;
        static DBMan()
        {
            strConn = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
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

        public static void Update(DataTable dt, UpdateInfo info)
        {
            try
            {
                using (SqlConnection conn = CreateAndOpen())
                {
                    using (SqlDataAdapter da = new SqlDataAdapter())
                    {
                        if (!string.IsNullOrEmpty(info.InsertText))
                        {
                            da.InsertCommand = new SqlCommand(info.InsertText, conn);
                            da.InsertCommand.UpdatedRowSource = UpdateRowSource.Both;
                            foreach (SqlParameter prm in info.InsertParams)
                                da.InsertCommand.Parameters.Add(prm);
                        }
                        if (!string.IsNullOrEmpty(info.UpdateText))
                        {
                            da.UpdateCommand = new SqlCommand(info.UpdateText, conn);
                            da.UpdateCommand.UpdatedRowSource = UpdateRowSource.Both;
                            foreach (SqlParameter prm in info.UpdateParams)
                                da.UpdateCommand.Parameters.Add(prm);
                        }
                        if (!string.IsNullOrEmpty(info.DeleteText))
                        {
                            da.DeleteCommand = new SqlCommand(info.DeleteText, conn);
                            da.DeleteCommand.UpdatedRowSource = UpdateRowSource.Both;
                            foreach (SqlParameter prm in info.DeleteParams)
                                da.DeleteCommand.Parameters.Add(prm);
                        }
                        da.Update(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                throw;
            }
        }
        #endregion
    }
}
