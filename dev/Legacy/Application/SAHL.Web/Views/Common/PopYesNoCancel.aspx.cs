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

public partial class Views_PopYesNoCancel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlLink cssHtmlLink = new HtmlLink();
        cssHtmlLink.Href = "../CSS/" + Session["CSS"];
        cssHtmlLink.Attributes.Add("rel", "stylesheet");
        cssHtmlLink.Attributes.Add("type", "text/css");

        // Add the HtmlLink to the Head section of the page.
        this.Header.Controls.Add(cssHtmlLink);

        // Add the common javascript file.
        ClientScript.RegisterClientScriptInclude("SAHLScripts", Page.ResolveClientUrl("~/Scripts/SAHLScripts.js"));
    }
}
