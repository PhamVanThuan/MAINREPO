using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Security;
using SAHL.Common;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.CacheData;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;

public partial class TimeoutError : System.Web.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        // set the stylesheet
        if (User.Identity.Name.ToUpper().StartsWith("SAHL"))
            styles.Href = "CSS/SAHL.css";
        else
            styles.Href = "CSS/RCS.css";


        // if we get here, we've timed out, so destroy everything from the session
        Session.Clear();
        SAHLPrincipalCache cache = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
        cache.GetGlobalData().Clear();
        cache = null;
    }

    /*
    private void LoadSessionSettings()
    {
        SAHL.Common.Web.UI.Configuration.WebAppSection WebAppsSection = ConfigurationManager.GetSection("WebApps") as SAHL.Common.Web.UI.Configuration.WebAppSection;

        if (WebAppsSection != null)
        {
            string key = WebAppsSection.SelectedApplication;

            string AppName = "", CSSFile = "", Title = "";

            string Domain = Request.Headers["Host"];

            foreach (SAHL.Common.Web.UI.Configuration.WebAppSectionElement section in WebAppsSection.WebApplications)
            {
                if (section.ApplicationName == key)
                {
                    AppName = section.ApplicationName;
                    CSSFile = section.CSSFile;
                    Title = section.Title;
                }

                string forDomains = section.ForDomains;

                if (forDomains.Length > 0)
                {
                    string[] Domains = forDomains.Split(',');
                    bool bBreak = false;
                    for (int i=0; i<Domains.Length; i++)
                    {
                        if (Domain.StartsWith(forDomains))
                        {
                            AppName = section.ApplicationName;
                            CSSFile = section.CSSFile;
                            Title = section.Title;
                            bBreak = true;
                            break;
                        }
                    }
                    if (bBreak)
                        break;
                }
            }

            Session.Add("CSS", CSSFile);
            Session.Add("Master", AppName + ".master");
            Session.Add("Application", AppName);
            Header.Title = Title;

            HtmlLink cssHtmlLink = new HtmlLink();
            cssHtmlLink.Href = "CSS/" + Session["CSS"];
            cssHtmlLink.Attributes.Add("rel", "stylesheet");
            cssHtmlLink.Attributes.Add("type", "text/css");

            // Add the HtmlLink to the Head section of the page.
            this.Header.Controls.Add(cssHtmlLink);

        }
    }
     * */

    protected void HomePageButton_Click(object sender, EventArgs e)
    {
        // clear out the CBO 
        SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
        //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();
        CBOManager CBOManager = new CBOManager();
        CBOManager.SetCurrentCBONode(principal, null, CBOManager.GetCurrentNodeSetName(principal));
        //CBOManager.SetCurrentContextNode(principal, null, CBOManager.GetCurrentNodeSetName(principal));
        UIPManager.StartNavigationTask("SAHL");
    }
}
