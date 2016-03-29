using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DetailType
/// </summary>
public class DetailType
{
	public DetailType()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    static public void delete(int a_iLoanNumber, int a_iDetailType)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString());
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = string.Format("delete from detail where loannumber = {0} and detailtypenumber = {1}", a_iLoanNumber.ToString(), a_iDetailType.ToString());

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();

        
    }
}
