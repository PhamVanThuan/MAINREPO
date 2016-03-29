using System;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.ApplicationBlocks.UIProcess;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;

public partial class ApplicationError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // set the stylesheet
        if (User.Identity.Name.ToUpper().StartsWith("SAHL"))
            styles.Href = "CSS/SAHL.css";
        else
            styles.Href = "CSS/RCS.css";

//        LoadSessionSettings();
        if (Session["APP_EX"] != null)
        {
            Exception ex = Session["APP_EX"] as Exception;
            if (ex != null)
            {
#if (DEBUG)
                lblError.Text = "";
                if (ex is UIPException)
                {
                    lblError.Text = "The application has failed to initialize.";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();


                    sb.Append("<span class=\"code\">");

                    AddExceptionText(sb, ex);
                    //if ((ex is UIPException) && (ex.Data.Contains("HttpSessionStateValues")))
                    //{
                    //    AddText(sb, (string)ex.Data["HttpSessionStateValues"]);
                    //}

                    sb.Append("</span>");

                    lblStackTrace.Text = sb.ToString();
                    lblStackTrace.Visible = true;
                }
#else
                if (ex is UIPException)
                    lblError.Text = "The application has failed to initialize.";
                else
                    lblError.Text = ex.Message.Replace("\n", "<br />");
#endif
            }

            Session.Remove("APP_EX");

            //IViewBase view = this.Page as IViewBase;
            // make sure we remove CBO settings for the user otherwise we might get in an endless loop of 
            // going back to the same page, hitting the same error, ad nauseum
            SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
            //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();
            CBOManager CBOManager = new CBOManager();
            CBONodeSetType nodeSet = CBOManager.GetCurrentNodeSetName(principal);
            CBOManager.SetCurrentCBONode(principal, null, nodeSet);
            //CBOManager.SetCurrentContextNode(principal, null, nodeSet);
        }

    }

    private void AddExceptionText(StringBuilder sb, Exception ex)
    {
        sb.Append("<p>");
        sb.Append("<span style=\"font-weight:bold;font-size:bigger;\">");
        sb.Append(ex.Message.Replace("\n", "<br />"));
        sb.Append("</span><br />");
        if (!String.IsNullOrEmpty(ex.StackTrace))
            sb.Append(ex.StackTrace.Replace("\n", "<br />"));
        sb.Append("</p>");
        sb.Append("<br />");

        if (ex.InnerException != null)
            AddExceptionText(sb, ex.InnerException);
    }

    //private static void AddText(StringBuilder sb, String Text)
    //{
    //    sb.Append("<p>");
    //    sb.Append("<span style=\"font-weight:bold;font-size:bigger;\">");
    //    sb.Append(Text.Replace("\n", "<br />"));
    //    sb.Append("</span><br />");
    //    sb.Append("</p>");
    //    sb.Append("<br />");
    //}

    //private static void AddInitializationError(StringBuilder sb)
    //{
    //    sb.Append("<div style=\"padding-top:5px;\">");
    //    sb.Append("The application has failed to initialize.");
    //    sb.Append("</div>");        
    //}

    protected void HomePageButton_Click(object sender, EventArgs e)
    {
        Session.Clear();
        // LoadSessionSettings();
        UIPManager.StartNavigationTask("SAHL");
    }

    #region OLD
    /*
    private void SetHomePageButtonDisplay(Exception ex)
    {
        if (ex is SAHL.Common.DataAccess.Exceptions.SecurityConfigurationException)
        {
            HomePageButton.Visible = false;
        }
        else if (ex is System.Data.SqlClient.SqlException)
        {
            System.Data.SqlClient.SqlException sqlException = ex as System.Data.SqlClient.SqlException;
            if (sqlException != null && sqlException.Errors.Count > 0)
            {
                int errorNumber = sqlException.Errors[0].Number;
                if (errorNumber == 53)
                    HomePageButton.Visible = false;
            }
        }
    }

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
                    foreach (string TestDomain in Domains)
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

    
    */

    #endregion
}