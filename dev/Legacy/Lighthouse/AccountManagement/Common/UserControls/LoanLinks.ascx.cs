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
using System.Collections.Specialized;

public partial class LoanLinks : System.Web.UI.UserControl
{
    private int iAccountKey = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Account oAcc = new Account();
        iAccountKey = Convert.ToInt32(Request.QueryString["param0"]);
        int ProductKey = oAcc.ProductKey(iAccountKey);

        NameValueCollection config = (NameValueCollection)ConfigurationSettings.GetConfig("Links/LoanLinks");

        for (int i=0;i < config.Count;i++) 
        {
            
            string[] sURLArray = config[i].Split(',');
            string sDescription = config.Keys[i]; ;
            string sHref = sURLArray[1];
            if (sDescription == "[ ITC Check ]")  
              
              sHref = sURLArray[1] + "?refType=L" + "&RefNumber=" + Request.QueryString["param0"] +
                                     "&Referrer=" + "http://sahlnet/base/plugins/accountmanagement/mortgageloan/ManageLoan.aspx" +
                                     "?param0=" + Request.QueryString["param0"] +
                                     "&param1=" + pageName() +
                                     "&param2=" + sahl.User.userID() +
                                     "&param3=" + HttpUtility.UrlEncode(Request.Url.ToString()) ;

            else
            
              sHref = sURLArray[1] + "?param0=" + Request.QueryString["param0"] +
                                     "&param1=" + pageName() +
                                     "&param2=" + sahl.User.userID() +
                                     "&param3=\"" + HttpUtility.UrlEncode(Request.Url.ToString()) + "\"" +
                                     "&param4=" + sURLArray[2];
            
            LiteralControl oLC = new LiteralControl(createAnchorTag(sHref,sDescription) + "<br>");

            if (sURLArray[0].Length > 1)
            {
                if (Convert.ToInt16(sURLArray[0].Substring(0,1)) == 0 || Convert.ToInt16(sURLArray[0].Substring(0, 1)) == ProductKey || Convert.ToInt16(sURLArray[0].Substring(2, 1)) == ProductKey)
                    pnlLinkContainer.Controls.Add(oLC);
            }
            else
            if (Convert.ToInt16(sURLArray[0].Substring(0,1)) != 0 && Convert.ToInt16(sURLArray[0].Substring(0,1)) != ProductKey)
            {
                //do nothing
            }
            else
                pnlLinkContainer.Controls.Add(oLC);
               
        }


    }

    private string createAnchorTag(string a_sHref, string a_sDesc)
    {
        return string.Format( "<a href='{0}'>{1}</a>",a_sHref,a_sDesc);
    }

    private string pageName(){
        string sSriptName = Request.ServerVariables["SCRIPT_NAME"];
        return sSriptName.Substring(sSriptName.LastIndexOf('/')+1).Replace(".aspx", "");

    }

    void hideLinksIfClosed()
    {
        Account oAcc = new Account();
        iAccountKey = Convert.ToInt32(Request.QueryString["param0"]);
        if (!oAcc.Editable(iAccountKey))
        {
            pnlLinkContainer.Visible = false;
        }

    }
}


