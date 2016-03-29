using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDFDocumentWriter.Data;
using System.Data;
using System.Data.SqlClient;

namespace PDFDocumentWriter.DataAccess
{
	public class DataAccess
	{
		public static void GetLookupData(Data.Lookup ds)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("select * from [2am].dbo.UIStatement with (nolock);");
			sb.AppendLine("select * from [2am].dbo.ReportStatement with (nolock);");
			sb.AppendLine("select * from [2am].dbo.DocumentTemplate with (nolock);");
			sb.AppendLine("select * from [2am].dbo.ReportStatementDocumentTemplate with (nolock);");
			sb.AppendLine("select * from [2am].dbo.Control with (nolock) where ControlDescription in ('Legal Agreements Template Path', 'Legal Agreements Output Path');");
			List<string> TableMappings = new List<string>();
			TableMappings.Add("UIStatement");
			TableMappings.Add("ReportStatement");
			TableMappings.Add("DocumentTemplate");
			TableMappings.Add("ReportStatementDocumentTemplate");
			TableMappings.Add("Control");
			DBMan.FillMultiTable(ds, TableMappings, sb.ToString(), null);
		}

		public static void ExecuteUIStatement(System.Data.DataSet ds, string Query, Dictionary<string, object> Params)
		{
			List<string> TableMapping = new List<string>();
			TableMapping.Add("DATA");
			DBMan.FillMultiTable(ds, TableMapping, Query, Params);
		}

		public static void GetAccountRoles(DataTable dt, int AccountKey)
		{
		}

		public static void GetDocumentsToProcess(DataTable dt, string ServiceName)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("update [2am].dbo.DocumentGenerationQ set ServicedBy='{0}' where ServicedBy is NULL", ServiceName);
			DBMan.ExecuteNonQuery(sb.ToString());
			sb = new StringBuilder();
			sb.AppendFormat("select * from [2am].dbo.DocumentGenerationQ where ServicedBy='{0}'", ServiceName);
			DBMan.FillFromQuery(dt, sb.ToString(), null);
		}

		public static void RemoveProcessedDocument(int ID)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("delete from [2am].dbo.DocumentGenerationQ where ID={0}", ID);
			DBMan.ExecuteNonQuery(sb.ToString());
		}

		public static int GetStatementKey(string applicationName, string statementName, string procedure, string connectionString)
		{
			int statementKey = -1;

			DBHelper db = new DBHelper(procedure, System.Data.CommandType.Text, connectionString);
			try
			{
				db.AddVarcharParameter("@Application", applicationName);
				db.AddVarcharParameter("@StatementName", statementName);
				db.Fill();
				DataRow R = db.FirstRow();
				if (R == null)
					throw new Exception();

				if (!connectionString.ToLower().Contains("sahls15"))
				{
					// update the LastAccessedDate of the uiStatement
					statementKey = Convert.ToInt32(R["StatementKey"]);
					string sql = Properties.Settings.Default.RepositoryUpdateLastAccessedDate;
					db = new DBHelper(sql, connectionString);
					db.AddDateParameter("@LastAccessedDate", DateTime.Now);
					db.AddIntParameter("@StatementKey", statementKey);
					int result = db.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				db = null;
			}
			return statementKey;
		}

	}
}
