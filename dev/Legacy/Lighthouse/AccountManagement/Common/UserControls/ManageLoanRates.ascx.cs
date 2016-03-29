using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHLLib;
using System.Data.SqlClient;

public partial class LoanRates_ascx : System.Web.UI.UserControl
{
    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ucToolbar.initializeToolbar(SAHLLib.Security.etFunction.LoanRates);
            hideToolbarIfClosed();
        }

        setDataSourceConnectionStr();
        ucToolbar.btnSave.Click += new ImageClickEventHandler(save);
        bindData();

    }


    void hideToolbarIfClosed()
    {
        Account oAcc = new Account();
        int iAccountKey = Convert.ToInt32(Request.QueryString["param0"]);
        if (!oAcc.Editable(iAccountKey))
        {
            ucToolbar.Visible = false;
        }

    }

    /// <summary>
    /// Set all SQLDataSource objects connection string properties.
    /// </summary>
    private void setDataSourceConnectionStr()
    {
        //Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
        string sCon = DBConnection.ConnectionString();

        oRateData.ConnectionString = sCon.Replace("SAHLDB", "2am");
        oMargin.ConnectionString = sCon;
    }


    /// <summary>
    /// Updates the database with the bank account changes specified by the user.
    /// </summary>
    private void save(object sender, ImageClickEventArgs e)
    {
        updateRate();
        ucToolbar.enableEdit(false);

    }


    private void updateRate()
    {
        string sAccountKey = Request.QueryString["param0"];
        int iVarRateConfigKey = Convert.ToInt32(ddMargin.SelectedValue.ToString());
        int iFixRateConfigKey = VariFix.getNewFixedRateConfigKey(iVarRateConfigKey);
        int iFixKey = 0;
        int iVarKey = 0;

        VariFix.getFinancialServiceKeys(sAccountKey,ref iVarKey,ref iFixKey);

        SqlCommand oCmd = new SqlCommand();
        oCmd.Connection = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        oCmd.CommandType = CommandType.StoredProcedure;
        oCmd.CommandText = "pLoanUpdateRate";
        
        oCmd.Parameters.Add("@FinancialServiceKey",SqlDbType.Int).Value = iVarKey;
        oCmd.Parameters.Add("@RateConfigurationKey",SqlDbType.Int).Value = iVarRateConfigKey;
        oCmd.Parameters.Add("@UserId", SqlDbType.VarChar,25).Value = sahl.User.userID();

        oCmd.Connection.Open();
        oCmd.ExecuteNonQuery();
//Yuck!!
        SqlParameterCollection oSPC = oCmd.Parameters;
        oSPC["@FinancialServiceKey"].Value = iFixKey;
        oSPC["@RateConfigurationKey"].Value = iFixRateConfigKey;

        oCmd.ExecuteNonQuery();
        oCmd.Connection.Close();

    }

    /// <summary>
    /// Sets the form controls with their database values.
    /// </summary>
    private void bindData()
    {
        

        if (pnlEdit.Visible == false)
        {
            DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
            DataView oDV = (DataView)oRateData.Select(oDSSA);

            if (oDV.Count > 1)
            {

                lblMarketRateVar.Text = oDV.Table.Rows[0]["MarketRateDesc"].ToString();
                lblMarketRateFlexi.Text = oDV.Table.Rows[1]["MarketRateDesc"].ToString();
                ddMargin.SelectedValue = oDV.Table.Rows[0]["rateconfigurationkey"].ToString();
                lblLoanRateVar.Text = Convert.ToDouble(oDV.Table.Rows[0]["LoanRate"].ToString()).ToString("P");
                lblLoanRateFlexi.Text = Convert.ToDouble(oDV.Table.Rows[1]["LoanRate"].ToString()).ToString("P");
                hfVarFinService.Value = oDV.Table.Rows[0]["financialservicekey"].ToString();
                hfFlexiFinService.Value = oDV.Table.Rows[1]["financialservicekey"].ToString();
            }
            else
            {
                lblMarketRateVar.Text = oDV.Table.Rows[0]["MarketRateDesc"].ToString();
                ddMargin.SelectedValue = oDV.Table.Rows[0]["rateconfigurationkey"].ToString();
                lblLoanRateVar.Text = Convert.ToDouble(oDV.Table.Rows[0]["LoanRate"].ToString()).ToString("P");
                hfVarFinService.Value = oDV.Table.Rows[0]["financialservicekey"].ToString();
            }

        }

       
    }


    public string getMargin() {
        DataSourceSelectArguments oDSSA = new DataSourceSelectArguments();
        DataView oDV = (DataView)oRateData.Select(oDSSA);
        return oDV.Table.Rows[0]["MarginDesc"].ToString();
    }
}
