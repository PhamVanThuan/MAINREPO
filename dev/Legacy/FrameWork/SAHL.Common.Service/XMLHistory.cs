using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;
using System.IO;

namespace SAHL.Common.Service
{
    public static class XMLHistory
    {
        public static DataTable ConvertParametersToDataTable(ParameterCollection parameters, string tableName)
        {
            DataTable DT = new DataTable();

			if (tableName != null)
				DT.TableName = tableName;

			if (parameters == null)
				return DT;

            object[] items = new object[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                string type = "";

                switch (parameters[i].SqlDbType)
                {
                    case SqlDbType.BigInt:
                        type = "System.Int64";
                        break;

                    case SqlDbType.Bit:
                        type = "System.Boolean";
                        break;

                    case SqlDbType.Char:
                        type = "System.Char";
                        break;

                    case SqlDbType.DateTime:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.Timestamp:
                        type = "System.DateTime";
                        break;

                    case SqlDbType.Decimal:
                    case SqlDbType.Money:
                    case SqlDbType.SmallMoney:
                        type = "System.Decimal";
                        break;

                    case SqlDbType.Float:
                        type = "System.Double";
                        break;

                    case SqlDbType.Int:
                        type = "System.Int32";
                        break;

                    case SqlDbType.NChar:
                    case SqlDbType.NText:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Text:
                    case SqlDbType.VarChar:
                    case SqlDbType.Xml:
                    case SqlDbType.UniqueIdentifier:
                        type = "System.String";
                        break;

                    case SqlDbType.Real:
                        type = "System.Single";
                        break;

                    case SqlDbType.SmallInt:
                        type = "System.Int16";
                        break;


                    case SqlDbType.TinyInt:
                        type = "System.Byte";
                        break;

                    default:
                        type = "System.Object";
                        break;
                }

                DT.Columns.Add(parameters[i].ParameterName, Type.GetType(type));
                items[i] = parameters[i].Value;
            }

            DT.LoadDataRow(items, true);
            return DT;
        }

		public static DataTable ConvertParametersToDataTable(ParameterCollection parameters)
		{
			return ConvertParametersToDataTable(parameters, null);
		}

        public static void AddParametersToDataTable(DataTable table, ParameterCollection parameters)
        {
            object[] items = new object[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                items[i] = parameters[i].Value;
            }

            table.LoadDataRow(items, true);
        }

        public static DataSet CreateRequestDataSet(string ProviderName, string MethodName, ParameterCollection parameters)
        {
			if (parameters == null)
				return null;

			DataSet DS = new DataSet(String.Format("{0}_{1}_Request", ProviderName, MethodName));
			DataTable DT = ConvertParametersToDataTable(parameters);
            DT.TableName = "Parameters";
            DS.Tables.Add(DT);
            return DS;
        }

		//public static DataSet CreateResponseDataSet(string ProviderName, string MethodName, string ResultName, object Result)
		//{
		//    DataSet DS = new DataSet(String.Format("{0}_{1}_Response", ProviderName, MethodName));
		//    DataTable DT = new DataTable("Result");
		//    DT.Columns.Add(ResultName);
		//    //DT.Columns.Add("Type", typeof(String));
		//    DT.Rows.Add(new object[] { Result });
		//    DS.Tables.Add(DT);
		//    return DS;
		//}

		public static DataSet CreateResponseDataSet(string ProviderName, string MethodName, ParameterCollection parameters)
		{
			if (parameters == null)
				return null;

			DataSet DS = new DataSet(String.Format("{0}_{1}_Response", ProviderName, MethodName));
			DataTable DT = ConvertParametersToDataTable(parameters);
			DT.TableName = "Response";
			DS.Tables.Add(DT);
			return DS;
		}

		//public static void NameResponseDataSet(string ProviderName, string MethodName, ref DataSet DS)
		//{
		//    DS.DataSetName = String.Format("{0}_{1}_Response", ProviderName, MethodName);
		//}

        public static int InsertXMLHistory(string xml, int GenericKey, int GenericKeyTypeKey)
        {
            IDbConnection con = Helper.GetSQLDBConnection();
			string query = "INSERT INTO [2AM].[dbo].[XMLHistory] ([GenericKeyTypeKey],[GenericKey],[XMLData],[InsertDate]) VALUES ( @GenericKeyTypeKey, @GenericKey, @XMLData, @InsertDate ) set @XMLHistoryKey = SCOPE_IDENTITY()";
			ParameterCollection parameters = new ParameterCollection();
			Helper.AddParameter(parameters, "@XMLHistoryKey", SqlDbType.Int, ParameterDirection.Output, 0);
			Helper.AddIntParameter(parameters, "@GenericKeyTypeKey", GenericKeyTypeKey);
            Helper.AddIntParameter(parameters, "@GenericKey", GenericKey);
            Helper.AddVarcharParameter(parameters, "@XMLData", String.IsNullOrEmpty(xml) ? "" : xml );
            Helper.AddDateParameter(parameters, "@InsertDate", DateTime.Now);
			Helper.ExecuteNonQuery(con, query, parameters);

			if (parameters[0].SqlValue != DBNull.Value)
				return Convert.ToInt32(parameters[0].Value);
			else
				return -1;
        }

        public static int InsertXMLHistory(DataSet DS, int GenericKey, int GenericKeyTypeKey)
        {
            //writexml
            //XmlDocument xdoc = new XmlDocument();
            //xdoc.LoadXml(DS.GetXml());

            //for reference, back to DS:
            //DataSet ds = new DataSet();
            //ds.ReadXml(new XmlNodeReader(xdoc));
			if (DS == null)
				return InsertXMLHistory("", GenericKey, GenericKeyTypeKey);
			else
				return InsertXMLHistory(DS.GetXml(), GenericKey, GenericKeyTypeKey);
        }

		public static void UpdateXMLHistory(int XmlHistoryKey, string xml)
		{
			IDbConnection con = Helper.GetSQLDBConnection();
			string query = "UPDATE [2AM].[dbo].[XMLHistory] SET [XMLData] = @XMLData, [InsertDate] = @InsertDate WHERE XMLHistoryKey = @XMLHistoryKey SELECT SCOPE_IDENTITY()";
			ParameterCollection parameters = new ParameterCollection();
			Helper.AddIntParameter(parameters, "@XMLHistoryKey", XmlHistoryKey);
			Helper.AddVarcharParameter(parameters, "@XMLData", xml);
			Helper.AddDateParameter(parameters, "@InsertDate", DateTime.Now);
			Helper.ExecuteNonQuery(con, query, parameters);
		}

		public static string CreateXmlString(DataSet RequestDS, DataSet ResponseDS, string ProviderName, string MethodName)
		{
            if (RequestDS == null)
                RequestDS = new DataSet(String.Format("{0}_{1}_Request", ProviderName, MethodName));
            else
                RequestDS.DataSetName = String.Format("{0}_{1}_Request", ProviderName, MethodName);

			if (ResponseDS == null)
				ResponseDS = new DataSet(String.Format("{0}_{1}_Response_FAILED", ProviderName, MethodName));
			else
				ResponseDS.DataSetName = String.Format("{0}_{1}_Response", ProviderName, MethodName);

			//MemoryStream requestStream = new MemoryStream();
			//MemoryStream responseStream = new MemoryStream();

			//RequestDS.InferXmlSchema.WriteXml(requestStream, XmlWriteMode.WriteSchema);
			//ResponseDS.WriteXml(responseStream, XmlWriteMode.WriteSchema);

			//string request = System.Text.Encoding.Default.GetString(requestStream.ToArray());
			//string response = System.Text.Encoding.Default.GetString(responseStream.ToArray());

			string header = String.Format("{0}.{1}", ProviderName, MethodName);
			string xml = String.Format("<{0}>\n{1}\n{2}\n</{3}>", header, RequestDS.GetXml(), ResponseDS.GetXml(), header);

			return xml;
		}

		public static void CreateXMLHistory(string ProviderName, string MethodName, ParameterCollection RequestParameters, int GenericKey, int GenericKeyTypeKey, DataSet ResponseDataSet)
        {
			DataSet RequestDS = CreateRequestDataSet(ProviderName, MethodName, RequestParameters);
			CreateXMLHistory(ProviderName, MethodName, RequestDS, GenericKey, GenericKeyTypeKey, ResponseDataSet);
		}

		public static void CreateXMLHistory(string ProviderName, string MethodName, ParameterCollection RequestParameters, int GenericKey, int GenericKeyTypeKey, ParameterCollection ResponseParameters)
		{
			DataSet RequestDS = CreateRequestDataSet(ProviderName, MethodName, RequestParameters);
			DataSet ResponseDS = null;

			if (ResponseParameters != null)
				ResponseDS = CreateResponseDataSet(ProviderName, MethodName, ResponseParameters);
			
			CreateXMLHistory(ProviderName, MethodName, RequestDS, GenericKey, GenericKeyTypeKey, ResponseDS);
		}

		public static void CreateXMLHistory(string ProviderName, string MethodName, DataSet RequestDataSet, int GenericKey, int GenericKeyTypeKey, DataSet ResponseDataSet)
		{
			string xml = CreateXmlString(RequestDataSet, ResponseDataSet, ProviderName, MethodName);
			InsertXMLHistory(xml, GenericKey, GenericKeyTypeKey);
		}

    }
}
