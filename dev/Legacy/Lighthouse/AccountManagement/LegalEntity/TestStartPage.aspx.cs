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

public partial class Main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

	protected void cmdFind_Click(object sender, EventArgs e)
	{
		//Go to Account details screen
		string strRedirect = "";
		strRedirect = "LegalEntityManagement.aspx?AccountKey=" + txtAccountKey.Text;
		Response.Redirect(strRedirect);

	}

}
