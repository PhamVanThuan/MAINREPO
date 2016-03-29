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
/// Summary description for Account
/// </summary>
public class Account
{
    public enum etAccountStatus : int
    {
        Open = 1,
        Closed = 2
    }

    public enum etUneditableReason : int
    {
        AccountClosed,
        DebitOrderRun
    }

    private int AccountKey = 0;
    public string StatusMessage = "";
    public etUneditableReason UneditableReasonCode;
    public System.Drawing.Color StatusColor;

	public Account()
	{
		//
		// TODO: Add constructor logic here
		//
        
	}

    public int ProductKey(int a_iAccountKey)
    {
        AccountKey = a_iAccountKey;
        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = string.Format("SELECT osp.ProductKey FROM Account a " +
                    "INNER JOIN OriginationSourceProduct osp ON a.OriginationSourceProductKey = osp.OriginationSourceProductKey " +
                    "WHERE a.AccountKey = {0}", AccountKey.ToString());

        oCmd.Connection.Open();
        int iProductKey = Convert.ToInt32(oCmd.ExecuteScalar());
        oCmd.Connection.Close();

        return iProductKey;
    }
    public bool Editable(int a_iAccountKey)
    {
        bool bReturn = false;
        
        AccountKey = a_iAccountKey;
        
        if (!open())
        {
            StatusMessage = "THIS LOAN IS CLOSED AND LOCKED";
            bReturn = false;
            UneditableReasonCode = etUneditableReason.AccountClosed;
            StatusColor = System.Drawing.Color.Red;
        } else if (debitOrder(a_iAccountKey))
        {
            StatusMessage = "Debit order in process";
            bReturn = false;
            UneditableReasonCode = etUneditableReason.DebitOrderRun;
            StatusColor = System.Drawing.Color.Blue;
        } else {
            bReturn = true;
        }

        return bReturn;

    }

    private bool open()
    {

        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = string.Format("select accountstatuskey from account where accountkey = {0}",AccountKey.ToString());

        oCmd.Connection.Open();
        int iAccountStatusKey = Convert.ToInt32(oCmd.ExecuteScalar());
        oCmd.Connection.Close();

        if (iAccountStatusKey == Convert.ToInt32(etAccountStatus.Open))
            return true;
        else
            return false;


    }

    private bool debitOrder(int a_iAccountKey)
    {
        bool bReturn = false;
        // Sql to determine if a debit order run is in progress.
        string sSQL = "select count(*) from control " +
                      "where controldescription = 'Debit Order in Process' " +
                      "and controltext = 'Y'";

        // Sql to determine if an accounts debit order day is for the debit order run in progress.
        string sSQLInterimAudit = string.Format("select count(*) " +
                                  " from cdpaymentfileschedule (nolock) " +
                                  " where datepart(d,actiondate) = (select top 1 debitorderday from [2am]..financialservice fs (nolock) " +
                                  " inner join [2am]..FinancialServiceBankAccount fsba (nolock) on fs.FinancialServiceKey = fsba.FinancialServiceKey " +
                                  " where accountkey = {0}) " +
                                  " and status = 'Interim' " +
                                  " and ExportType ='D'",a_iAccountKey.ToString());

        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString());
        oCmd.CommandType = CommandType.Text;
        oCmd.CommandText = sSQL;

        oCmd.Connection.Open();
        int iDebitOrder = Convert.ToInt32(oCmd.ExecuteScalar());
        
        // if Debit order in progress
        if (iDebitOrder > 0)
        {
            oCmd.CommandText = sSQLInterimAudit;
            int iAccountDebitOrder = Convert.ToInt32(oCmd.ExecuteScalar());

            // If Accounts debit order day makes it part of the current debit order run in progress.
            if (iAccountDebitOrder > 0)
                bReturn = true; 
            else
	            bReturn = false;

        }
        else
            bReturn = false;

        oCmd.Connection.Close();
        return bReturn;

    }

}
