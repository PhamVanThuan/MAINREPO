using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Diagnostics;
using SAHL.Reporting.WebServices.Common;
using System.Collections.Generic;

namespace SAHL.Reporting.WebServices.Services
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://SAHL.Reporting.WebServices/")]
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
