using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using SAHL.X2.Common.DataAccess;

//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.X2.Framework.DataSets;

namespace SAHL.X2.Framework.DataAccess
{
    public class ParameterCollection : List<SqlParameter>
    {
        public void TransferParameters(SqlParameterCollection Destination)
        {
            foreach (SqlParameter P in this)
            {
                Destination.Add(P);
            }
        }

        public void TransferParameters(ParameterCollection Destination)
        {
            foreach (SqlParameter P in this)
            {
                Destination.Add(P);
            }
        }
    }

    /// <summary>
    /// The WorkerHelper assists the DataAccess Workers with Database interaction. It provides a number of methods
    /// that enable a calling class to either fill a table in a dataset, or update the database from the given datatable.
    /// </summary>
    public static class WorkerHelper
    {
        #region Fill

        public static void Fill(DataSet p_DataSet, StringCollection p_TableMappings, string p_sApplicationName, string p_sProcedureName, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();//new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
            Adaptor.SelectCommand.CommandText = Repository.Get(p_sApplicationName, p_sProcedureName, p_TransactionContext);

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            // Add the table mappings.
            if ((p_TableMappings != null) && (p_TableMappings.Count > 0))
            {
                Adaptor.TableMappings.Add("Table", p_TableMappings[0]);
                for (int i = 1; i < p_TableMappings.Count; i++)
                {
                    Adaptor.TableMappings.Add("Table" + i, p_TableMappings[i]);
                }
            }

            Adaptor.Fill(p_DataSet);
            Adaptor.SelectCommand.Parameters.Clear();
            Adaptor.Dispose();
            Adaptor = null;
        }

        public static void Fill(DataSet p_DataSet, string p_sTableName, string p_sApplicationName, string p_sProcedureName, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();//new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
            Adaptor.SelectCommand.CommandText = Repository.Get(p_sApplicationName, p_sProcedureName, p_TransactionContext);

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            Adaptor.Fill(p_DataSet, p_sTableName);

            Adaptor.SelectCommand.Parameters.Clear();
            Adaptor.Dispose();
            Adaptor = null;
        }

        public static void Fill(DataTable p_DataTable, string p_sApplicationName, string p_sProcedureName, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();//new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
            Adaptor.SelectCommand.CommandText = Repository.Get(p_sApplicationName, p_sProcedureName, p_TransactionContext);

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            Adaptor.Fill(p_DataTable);

            Adaptor.SelectCommand.Parameters.Clear();
            Adaptor.Dispose();
            Adaptor = null;
        }

        public static void FillFromQuery(DataSet p_DataSet, StringCollection p_TableMappings, string p_sQuery, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();//new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
            Adaptor.SelectCommand.CommandText = p_sQuery;

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            // Add the table mappings.
            if ((p_TableMappings != null) && (p_TableMappings.Count > 0))
            {
                Adaptor.TableMappings.Add("Table", p_TableMappings[0]);
                for (int i = 1; i < p_TableMappings.Count; i++)
                {
                    Adaptor.TableMappings.Add("Table" + i, p_TableMappings[i]);
                }
            }

            Adaptor.Fill(p_DataSet);

            Adaptor.SelectCommand.Parameters.Clear();

            Adaptor.Dispose();
            Adaptor = null;
        }

        public static void FillFromQuery(DataTable p_DataTable, string p_sQuery, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters, int reportCommandTimeout)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
            Adaptor.SelectCommand.CommandTimeout = reportCommandTimeout;
            Adaptor.SelectCommand.CommandText = p_sQuery;

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            Adaptor.Fill(p_DataTable);
            Adaptor.SelectCommand.Parameters.Clear();
            Adaptor.Dispose();
            Adaptor = null;
        }

        public static void FillFromQuery(DataTable p_DataTable, string p_sQuery, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
            Adaptor.SelectCommand.CommandText = p_sQuery;

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            Adaptor.Fill(p_DataTable);
            Adaptor.SelectCommand.Parameters.Clear();
            Adaptor.Dispose();
            Adaptor = null;
        }

        public static void FillFromQuery(DataSet p_DataSet, string p_sTableName, string p_sQuery, ITransactionContext p_TransactionContext, ParameterCollection p_Parameters)
        {
            SqlDataAdapter Adaptor = new SqlDataAdapter();
            Adaptor.SelectCommand = new SqlCommand(p_sQuery, (SqlConnection)p_TransactionContext.DataConnection);
            Adaptor.SelectCommand.CommandText = p_sQuery;

            if (p_Parameters != null)
                p_Parameters.TransferParameters(Adaptor.SelectCommand.Parameters);

            if (p_TransactionContext.DataTransaction != null)
                Adaptor.SelectCommand.Transaction = (SqlTransaction)p_TransactionContext.DataTransaction;

            Adaptor.Fill(p_DataSet, p_sTableName);
            Adaptor.SelectCommand.Parameters.Clear();
            Adaptor.Dispose();
            Adaptor = null;
        }

        #endregion Fill

        #region Update

        public static void Update(DataTable p_DataTable, ITransactionContext p_TransactionContext, UpdateInformation p_UpdateInfo)
        {
            SqlDataAdapter Adaptor = null;
            try
            {
                bool Transacting = false;
                if (p_TransactionContext.DataTransaction != null)
                    Transacting = true;

                // create our Adaptor
                Adaptor = new SqlDataAdapter();
                // initialize the Commands
                if (p_UpdateInfo.DeleteName != null)
                {
                    Adaptor.DeleteCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
                    DoCommandSetup(Adaptor.DeleteCommand, p_UpdateInfo.ApplicationName, p_UpdateInfo.DeleteName, p_TransactionContext.DataTransaction, p_UpdateInfo.DeleteParameters, Transacting, p_TransactionContext);
                }
                if (p_UpdateInfo.InsertName != null)
                {
                    Adaptor.InsertCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
                    DoCommandSetup(Adaptor.InsertCommand, p_UpdateInfo.ApplicationName, p_UpdateInfo.InsertName, p_TransactionContext.DataTransaction, p_UpdateInfo.InsertParameters, Transacting, p_TransactionContext);
                }
                if (p_UpdateInfo.UpdateName != null)
                {
                    Adaptor.UpdateCommand = (SqlCommand)p_TransactionContext.DataConnection.CreateCommand();
                    DoCommandSetup(Adaptor.UpdateCommand, p_UpdateInfo.ApplicationName, p_UpdateInfo.UpdateName, p_TransactionContext.DataTransaction, p_UpdateInfo.UpdateParameters, Transacting, p_TransactionContext);
                }

                Adaptor.Update(p_DataTable);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Adaptor != null)
                {
                    Adaptor.Dispose();
                    Adaptor = null;
                }
            }
        }

        private static void DoCommandSetup(SqlCommand p_Command, string p_sApplicationName, string p_sStatementName, IDbTransaction p_Transaction, ParameterCollection p_Parameters, bool p_bTransacting, ITransactionContext ctx)
        {
            p_Command.CommandText = Repository.Get(p_sApplicationName, p_sStatementName, ctx);
            if (p_bTransacting)
                p_Command.Transaction = (SqlTransaction)p_Transaction;
            //check the parameters
            if (p_Parameters != null)
            {
                p_Command.UpdatedRowSource = UpdateRowSource.OutputParameters;
                p_Parameters.TransferParameters(p_Command.Parameters);
            }
        }

        #endregion Update

        public static object ExecuteScalar(ITransactionContext p_Context, string p_CommandText, ParameterCollection p_Parameters)
        {
            object obj = null;
            bool WasOpened = false;
            IDbCommand cmd = null;
            try
            {
                if (p_Context.DataConnection.State != ConnectionState.Open)
                {
                    p_Context.DataConnection.Open();
                    WasOpened = true;
                }

                cmd = p_Context.DataConnection.CreateCommand();
                cmd.CommandText = p_CommandText;

                if (p_Context.DataTransaction != null)
                    cmd.Transaction = (SqlTransaction)p_Context.DataTransaction;

                for (int i = 0; i < p_Parameters.Count; i++)
                {
                    SqlParameter param = p_Parameters[i];
                    cmd.Parameters.Add(param);
                }

                obj = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }

                if (WasOpened)
                    if (p_Context.DataConnection != null)
                    {
                        p_Context.DataConnection.Close();
                    }
            }

            return obj;
        }

        public static int ExecuteNonQuery(ITransactionContext p_Context, string p_CommandText, ParameterCollection p_Parameters)
        {
            bool WasOpened = false;
            IDbCommand cmd = null;
            try
            {
                if (p_Context.DataConnection.State != ConnectionState.Open)
                {
                    p_Context.DataConnection.Open();
                    WasOpened = true;
                }

                cmd = p_Context.DataConnection.CreateCommand();
                cmd.CommandText = p_CommandText;

                if (p_Context.DataTransaction != null)
                    cmd.Transaction = (SqlTransaction)p_Context.DataTransaction;

                for (int i = 0; i < p_Parameters.Count; i++)
                {
                    SqlParameter param = p_Parameters[i];
                    cmd.Parameters.Add(param);
                }

                return cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }

                if (WasOpened)
                    if (p_Context.DataConnection != null)
                    {
                        p_Context.DataConnection.Close();
                    }
            }
        }

        #region Parameter Management Methods

        public static void AddXmlParameter(ParameterCollection p_Parameters, string p_sName, System.Xml.XmlDocument p_XmlDoc)
        {
            AddParameter(p_Parameters, p_sName, SqlDbType.Xml, ParameterDirection.Input, p_XmlDoc);
        }

        public static void AddDateParameter(ParameterCollection p_Parameters, string p_sName, DateTime p_Value)
        {
            AddParameter(p_Parameters, p_sName, SqlDbType.DateTime, ParameterDirection.Input, p_Value);
        }

        public static void AddDateParameter(ParameterCollection p_Parameters, string p_sName, DateTime? p_Value)
        {
            if (p_Value.HasValue)
                AddParameter(p_Parameters, p_sName, SqlDbType.DateTime, ParameterDirection.Input, p_Value);
            else
                AddParameter(p_Parameters, p_sName, SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
        }

        public static void AddBigIntParameter(ParameterCollection p_Parameters, string p_sName, Int64? p_Value)
        {
            if (null == p_Value)
            {
                SqlParameter prm = AddParameter(p_Parameters, p_sName, SqlDbType.BigInt, ParameterDirection.Input, DBNull.Value);
                prm.IsNullable = true;
            }
            else
                AddParameter(p_Parameters, p_sName, SqlDbType.BigInt, ParameterDirection.Input, p_Value);
            //AddParameter(p_Parameters, p_sName, SqlDbType.BigInt, ParameterDirection.Input, p_Value);
        }

        public static void AddIntParameter(ParameterCollection p_Parameters, string p_sName, Int32? p_Value)
        {
            if (null == p_Value)
            {
                SqlParameter prm = AddParameter(p_Parameters, p_sName, SqlDbType.Int, ParameterDirection.Input, DBNull.Value);
                prm.IsNullable = true;
            }
            else
                AddParameter(p_Parameters, p_sName, SqlDbType.Int, ParameterDirection.Input, p_Value);
            //AddParameter(p_Parameters, p_sName, SqlDbType.Int, ParameterDirection.Input, p_Value);
        }

        public static void AddFloatParameter(ParameterCollection p_Parameters, string p_sName, float p_Value)
        {
            AddParameter(p_Parameters, p_sName, SqlDbType.Float, ParameterDirection.Input, p_Value);
        }

        public static void AddDecimalParameter(ParameterCollection p_Parameters, string p_sName, decimal p_Value)
        {
            AddParameter(p_Parameters, p_sName, SqlDbType.Decimal, ParameterDirection.Input, p_Value);
        }

        public static void AddVarcharParameter(ParameterCollection p_Parameters, string p_sName, string p_Value)
        {
            AddParameter(p_Parameters, p_sName, SqlDbType.VarChar, ParameterDirection.Input, p_Value);
        }

        public static void AddBitParameter(ParameterCollection p_Parameters, string p_sName, bool p_Value)
        {
            AddParameter(p_Parameters, p_sName, SqlDbType.Bit, ParameterDirection.Input, p_Value);
        }

        public static SqlParameter AddParameter(ParameterCollection p_Parameters, string p_sName, SqlDbType p_dbType, ParameterDirection p_Direction, object p_Value)
        {
            try
            {
                SqlParameter param = new SqlParameter(p_sName, p_dbType);
                param.Direction = p_Direction;
                param.Value = p_Value;
                p_Parameters.Add(param);
                return param;
            }
            catch
            {
                throw;
            }
        }

        public static DbType ParseDbType(string SqlType)
        {
            switch (SqlType.ToLower())
            {
                case "bigint":
                    return DbType.Int64;

                case "binary":
                case "varbinary":
                case "image":
                    return DbType.Binary;

                case "bit":
                    return DbType.Boolean;

                case "char":
                case "text":
                    return DbType.AnsiStringFixedLength;

                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    return DbType.DateTime;

                case "decimal":
                    return DbType.Decimal;

                case "float":
                    return DbType.Single;

                case "int":
                    return DbType.Int32;

                case "money":
                case "smallmoney":
                    return DbType.Currency;

                case "nchar":
                case "ntext":
                    return DbType.StringFixedLength;

                case "nvarchar":
                case "varchar":
                    return DbType.String;

                case "real":
                    return DbType.Double;

                case "smallint":
                    return DbType.Int16;

                case "tinyint":
                    return DbType.Byte;

                case "uniqueidentifier":
                    return DbType.Guid;

                case "udt":
                case "variant":
                    return DbType.Object;

                case "xml":
                    return DbType.Xml;

                default:
                    return DbType.Object;
            }
        }

        public static SqlDbType ParseSqlDbType(string SqlType)
        {
            switch (SqlType.ToLower())
            {
                case "int":
                    return SqlDbType.Int;
                case "varchar":
                    return SqlDbType.VarChar;
                case "datetime":
                    return SqlDbType.DateTime;
                case "float":
                    return SqlDbType.Float;
                case "bit":
                    return SqlDbType.Bit;
                case "real":
                    return SqlDbType.Real;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "bigint":
                    return SqlDbType.BigInt;
                case "char":
                    return SqlDbType.Char;
                case "text":
                    return SqlDbType.Text;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "decimal":
                    return SqlDbType.Decimal;
                case "money":
                    return SqlDbType.Money;
                case "smallmoney":
                    return SqlDbType.SmallMoney;
                case "nchar":
                    return SqlDbType.NChar;
                case "ntext":
                    return SqlDbType.NText;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "udt":
                    return SqlDbType.Udt;
                case "variant":
                    return SqlDbType.Variant;
                case "xml":
                    return SqlDbType.Xml;
                case "binary":
                    return SqlDbType.Binary;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "image":
                    return SqlDbType.Image;
                default:
                    return SqlDbType.VarChar;
            }
        }

        public static string ParseQuotes(string parametervalue)
        {
            return (parametervalue.Replace("'", "''"));
        }

        #endregion Parameter Management Methods

        #region Linked Parameter Management Methods

        public static void AddLinkedDateParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.DateTime, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a interger typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Int, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a tiny integer (single byte) typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedTinyIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.TinyInt, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a small integer (two bytes) typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedSmallIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.SmallInt, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a big interger typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedBigIntParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.BigInt, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a xml typed linked parameter.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedXmlParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Xml, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter of type varchar (string).
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedVarcharParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.VarChar, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter of type bit.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedBitParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Bit, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter of type float.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedFloatParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Float, ParameterDirection.Input, p_sSourceName);
        }

        public static void AddLinkedDecimalParameter(ParameterCollection p_Parameters, string p_sName, string p_SourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Decimal, ParameterDirection.Input, p_SourceName);
        }

        /// <summary>
        /// Add a linked parameter of type money.
        /// </summary>
        /// <remarks > This method adds a input parameter. For output parameters <see cref="AddLinkedParameter"/></remarks>
        /// <param name="p_Parameters">The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedMoneyParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName)
        {
            AddLinkedParameter(p_Parameters, p_sName, SqlDbType.Money, ParameterDirection.Input, p_sSourceName);
        }

        /// <summary>
        /// Add a linked parameter to the collection. All properties can be passed in.
        /// </summary>
        /// <param name="p_Parameters"> The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_dbType">The type of the parameter to add.</param>
        /// <param name="p_Direction">The direction of the parameter.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        public static void AddLinkedParameter(ParameterCollection p_Parameters, string p_sName, SqlDbType p_dbType, ParameterDirection p_Direction, string p_sSourceName)
        {
            try
            {
                SqlParameter param = new SqlParameter(p_sName, p_dbType);
                param.Direction = p_Direction;
                param.SourceColumn = p_sSourceName;
                p_Parameters.Add(param);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Add a linked parameter to the collection. All properties can be passed in.
        /// </summary>
        /// <param name="p_Parameters"> The parameter collection, cannot be null.</param>
        /// <param name="p_sName">The name of the parameter to add.</param>
        /// <param name="p_sSourceName">The name of the column to link the parameter to.</param>
        /// <param name="p_iSize">The size (length) of the varchar column in the dataset.</param>
        public static void AddLinkedVarcharOutputParameter(ParameterCollection p_Parameters, string p_sName, string p_sSourceName, int p_iSize)
        {
            try
            {
                SqlParameter param = new SqlParameter(p_sName, SqlDbType.VarChar);
                param.Direction = ParameterDirection.Output;
                param.Size = p_iSize;
                param.SourceColumn = p_sSourceName;
                p_Parameters.Add(param);
            }
            catch
            {
                throw;
            }
        }

        #endregion Linked Parameter Management Methods

        public static void HandleRuleParam(Rules.RuleParameterRow drParam, ParameterCollection param, object Value)
        {
            Rules.RuleParameterTypeRow rptr = drParam.RuleParameterTypeRow;
            switch (rptr.Description.ToUpper())
            {
                case "INT32":
                    {
                        AddIntParameter(param, drParam.Name, Convert.ToInt32(Value));
                        break;
                    }
                case "INT64":
                    {
                        AddBigIntParameter(param, drParam.Name, Convert.ToInt64(Value));
                        break;
                    }
                case "BOOL":
                    {
                        AddBitParameter(param, drParam.Name, Convert.ToBoolean(Value));
                        break;
                    }
                case "FLOAT":
                    {
                        AddFloatParameter(param, drParam.Name, Convert.ToSingle(Value));
                        break;
                    }
                case "DOUBLE":
                    {
                        AddFloatParameter(param, drParam.Name, (float)Value);
                        break;
                    }
                case "STRING":
                    {
                        AddVarcharParameter(param, drParam.Name, Value.ToString());
                        break;
                    }
                case "DECIMAL":
                    {
                        AddDecimalParameter(param, drParam.Name, Convert.ToDecimal(Value));
                        break;
                    }
            }
        }
    }
}