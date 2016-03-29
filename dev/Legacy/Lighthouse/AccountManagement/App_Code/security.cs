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

/// <summary>
/// Summary description for security
/// </summary>
public class Security
{
	public Security()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static bool editAllowed(SAHLLib.Security.etFunction a_etFunction)
    {
        SAHLLib.Security oSec = new SAHLLib.Security();

        bool bAllowed = oSec.functionAccessAllowed(a_etFunction, HttpContext.Current.User.Identity.Name.ToLower().Replace("sahl\\", ""), DBConnection.ConnectionString());

        return bAllowed;

    }
}
