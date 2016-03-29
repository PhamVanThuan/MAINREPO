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

    public static string ConnectionString()
    {
        return (new SAHLLib.ConnectionString(HttpContext.Current.Request.ServerVariables["server_NAME"], HttpContext.Current.User.Identity.Name, "").ToString());
    }

    //______________________________________________________________________________________________________________________________
    public static string ConnectionString(string sUserName)
    {
        string sUser = "";
        if (sUserName.Length > 5)
        {
            if (sUserName.Substring(0, 5) != "SAHL\\") sUser = "SAHL\\" + sUserName;
        }
        else
        {
            sUser = "SAHL\\" + sUserName;
        }
        return (new SAHLLib.ConnectionString(HttpContext.Current.Request.ServerVariables["server_NAME"], sUser, "").ToString());
    }

    public static string ConnectionString(string sUserName, string pwd)
    {
        string sUser = "";
        if (sUserName.Length > 5)
        {
            if (sUserName.Substring(0, 5) != "SAHL\\") sUser = "SAHL\\" + sUserName;
        }
        else
        {
            sUser = "SAHL\\" + sUserName;
        }
        return (new SAHLLib.ConnectionString(HttpContext.Current.Request.ServerVariables["server_NAME"], sUser, pwd).ToString());
    }
}