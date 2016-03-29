using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.ComponentModel;
using SAHL.Web.Services.LDAP;
using System.Diagnostics;

namespace SAHL.Web.Services.Internal
{
    /// <summary>
    /// Summary description for LDAPQueryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class LDAPQueryService : System.Web.Services.WebService
    {

        [WebMethod]
        public List<string> GetFeatureAccessForUser(string SAHLUserName)
        {
            ADLookup ad = new ADLookup("SAHL");
            try
            {
                string UserName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                return ad.ListGroupsForUser(SAHLUserName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return new List<string>();
        }
    }
}
