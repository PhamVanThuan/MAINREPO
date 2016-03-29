using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ManageLoanStatus : System.Web.UI.UserControl
{
    private bool bEditable = true;

    protected void Page_Load(object sender, EventArgs e)
    {
        getAccountStatus();
        if (!bEditable)
        {
            pnlStatus.Visible = true;
        }
        else
        {
            pnlStatus.Visible = false;
        }


        
    }

    void getAccountStatus()
    {
        Account oAcc = new Account();
        int iAccountKey = Convert.ToInt32(Request.QueryString["param0"]);
        if (!oAcc.Editable(iAccountKey))
        {
            bEditable = false;
            lStatusMsg.Text = oAcc.StatusMessage;
            tStatusBar.BgColor = System.Drawing.ColorTranslator.ToHtml(oAcc.StatusColor);
        }

    }
}
