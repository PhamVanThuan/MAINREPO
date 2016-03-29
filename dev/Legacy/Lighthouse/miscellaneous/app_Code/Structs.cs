using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

/// <summary>
/// Summary description for Structs
/// </summary>
public class Structs
{
	public Structs()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public struct structADUser
    {
        public string UserID;
        public string GivenName;
        public string Surname;
        public string FullName;
        public string Email;
        public ArrayList MemberOf;
    }
}
