using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ManageLoanDetail_ascx:System.Web.UI.UserControl
{
    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ibManageDetail.Visible = Security.editAllowed(SAHLLib.Security.etFunction.LoanDetail);
            ibManageDetail.PostBackUrl = ConfigurationSettings.AppSettings["DetailManagementPath"] + "?Source=PLUGIN&Action=Update&Number=" + Request.QueryString["param0"];
            hideToolbarIfClosed();
        }
        setDataSourceConnectionStr();
 
    }

    void hideToolbarIfClosed()
    {
        Account oAcc = new Account();
        int iAccountKey = Convert.ToInt32(Request.QueryString["param0"]);
        if (!oAcc.Editable(iAccountKey))
        {
            ibManageDetail.Visible = false;
        }

    }

    /// <summary>
    /// Set all SQLDataSource objects connection string properties.
    /// </summary>
    private void setDataSourceConnectionStr()
    {
        //Need to create a function in SAHLLib to allow DB specification. For now replace SAHLDB with 2am.
        string sCon = DBConnection.ConnectionString().Replace("SAHLDB", "2am");

        oLoanDetail.ConnectionString = sCon;
    
    }






}
