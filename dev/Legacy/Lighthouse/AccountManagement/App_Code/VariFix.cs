using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
/// Summary description for VariFix
/// </summary>
public class VariFix
{
    public VariFix()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a_sAccountKey"></param>
    /// <param name="a_iVarKey"></param>
    /// <param name="a_iFixKey"></param>
    static public void getFinancialServiceKeys(string a_sAccountKey, ref int a_iVarKey, ref int a_iFixKey)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = string.Format("select fs.FinancialServiceKey,FinancialServiceTypeKey " +
                           "from [2am].dbo.FinancialService fs (nolock) " +
                           "join [2am].fin.MortgageLoan ml (nolock) " +
                           "on ml.FinancialServiceKey = fs.FinancialServiceKey " +
                           "where fs.Accountkey = {0} " +
                           "and fs.CloseDate is null " +
                           "and fs.FinancialServiceTypeKey in (1,2)", a_sAccountKey);

        oCmd.Connection.Open();

        SqlDataReader oSDR = oCmd.ExecuteReader();

        while (oSDR.Read())
        {
            if (Convert.ToInt32(oSDR["FinancialServiceKey"].ToString()) == Constants.VARIABLE_LOAN)
            {
                a_iVarKey = Convert.ToInt32(oSDR["FinancialServiceKey"].ToString());
            }

            if (Convert.ToInt32(oSDR["FinancialServiceTypeKey"].ToString()) == Constants.FIXED_LOAN)
            {
                a_iFixKey = Convert.ToInt32(oSDR["FinancialServiceTypeKey"].ToString());
            }
        }
    }

    static public int getNewFixedRateConfigKey(int a_iRateConfigKey)
    {
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = string.Format("select rcf.rateconfigurationkey " +
                                         "from rateconfiguration rcf " +
                                         "inner join rateconfiguration rcv on rcv.marginkey = rcf.marginkey " +
                                         "where rcv.rateconfigurationkey = {0} " +
                                         "and rcv.marketratekey in (1,4) " +
                                         "and rcf.marketratekey = 3", a_iRateConfigKey);

        oCmd.Connection.Open();
        int iFixedRateConfigKey = Convert.ToInt32(oCmd.ExecuteScalar().ToString());
        oCmd.Connection.Close();

        return iFixedRateConfigKey;
    }
}