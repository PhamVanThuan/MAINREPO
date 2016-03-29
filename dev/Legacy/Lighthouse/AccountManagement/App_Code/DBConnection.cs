using System;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class DBConnection
{
    public DBConnection()
    {
		//
		// TODO: Add constructor logic here
		//
	}

    public static string ConnectionString() {

        return (new SAHLLib.ConnectionString(HttpContext.Current.Request.ServerVariables["server_NAME"], HttpContext.Current.User.Identity.Name, "").ToString());


    }
}
