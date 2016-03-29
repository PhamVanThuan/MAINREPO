using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;



public partial class ManageLoan_aspx:System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        lbLoanCalculator.PostBackUrl = loanCalcURL();
    }

    protected void lbLoanDetail_Click(object sender, EventArgs e)
    {
        pnlMain.Visible = false;
        pnlHOC.Visible = false;
        pnlLoanDetail.Visible = true;
    }

    protected void lbHOC_Click(object sender, EventArgs e)
    {
        pnlMain.Visible = false;
        pnlHOC.Visible = true;
        pnlLoanDetail.Visible = false;
    }
    protected void lbLoanDetailMain_Click(object sender, EventArgs e)
    {
        pnlMain.Visible = true;
        pnlHOC.Visible = false;
        pnlLoanDetail.Visible = false;
    }
    protected void lbHOCMain_Click(object sender, EventArgs e)
    {
        pnlMain.Visible = true;
        pnlHOC.Visible = false;
        pnlLoanDetail.Visible = false;
    }


    private string loanCalcURL()
    {
        NameValueCollection config = (NameValueCollection)ConfigurationSettings.GetConfig("Links/LoanLinks");

        string[] sURLArray = config["[ Loan Calculator ]"].Split(',');
        string sHref = sURLArray[0] + "?param0=" + Request.QueryString["param0"] +
                                        "&param1=" + pageName() +
                                        "&param2=" + sahl.User.userID() +
                                        "&param3=\"" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\"" +
                                        "&param4=" + sURLArray[1];

        return sHref;
    

    }

    private string pageName()
    {
        string sSriptName = Request.ServerVariables["SCRIPT_NAME"];
        return sSriptName.Substring(sSriptName.LastIndexOf('/') + 1).Replace(".aspx", "");

    }
}
