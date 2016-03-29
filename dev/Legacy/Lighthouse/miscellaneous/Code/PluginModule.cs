using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for PluginModule
/// </summary>
public class PluginModule : UserControl
{
    public string ConnectionString
    {
        get { return DBConnection.ConnectionString(); }
    }
}
