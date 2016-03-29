using System;
using System.Data;
using System.Configuration;

/// <summary>
/// Summary description for uiStatement
/// </summary>
public class uiStatement
{
	public uiStatement()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string Get(string p_sApplicationName, string p_sStatementName)
    {
        DBHelper db = new DBHelper(ConfigurationManager.AppSettings["uiStatementSelect"], CommandType.Text);
        try
        {
            DataTable Dt = new DataTable();
            db.AddVarcharParameter("@Application", p_sApplicationName);
            db.AddVarcharParameter("@StatementName", p_sStatementName);
            db.Fill();
            DataRow R = db.FirstRow();
            return R["Statement"].ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            db = null;
        }
    }
}
