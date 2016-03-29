using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class title_ascx : System.Web.UI.UserControl
{
    public string Title = "";

    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                lblTitle.Text = Parent.Page.Title;
                
                Session.Add("Opener", Request.UrlReferrer.ToString());
                Session.Add("Closer", "<script language='Javascript'>parent.location.href='" + Session["Opener"].ToString() + "';</script>");

            }
            catch { } ;
        }
    }

}
