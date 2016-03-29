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
using System.Reflection;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.UI;
using AjaxControlToolkit;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;

using SAHL.Common;

namespace SAHL.Web
{
    public partial class SAHLMaster : SAHLMasterBase
    {

        private DateTime _dtStart = DateTime.Now;
        //ICBOService CBOService = ServiceFactory.GetService<ICBOService>();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.mnuCBO.Navigate += new SAHL.Common.Web.UI.Events.NavigateEventHandler(mnuCBO_Navigate);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // javascript includes
            Page.ClientScript.RegisterClientScriptInclude("JavaScriptMaster", String.Format(Page.ResolveClientUrl("~/Scripts/Master.js?v={0}"), Version));
            Page.ClientScript.RegisterClientScriptInclude("JavaScriptDefault", String.Format(Page.ResolveClientUrl("~/Scripts/Default.js?v={0}"), Version));

            // no way to get a handle on the body part of the tabs control but we want to add a border, so unfortunately we need to do it here
            string styles = @"<style type=""text/css"">
                #{0}_body {{ border-right: solid 1px #999999 !important; }}
                </style>";
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "MasterStyles", String.Format(styles, this.tabsMenu.ClientID), false);

            // add javascript event handlers for the tabs
            tabsMenu.OnClientActiveTabChanged = "masterMenuActiveTabChanged";

            // set extra status info
            lblInfoUser.Text = Page.User.Identity.Name;
            lblInfoVersion.Text = Version;

            TimeSpan ts = DateTime.Now - _dtStart;
            string totalSeconds = ts.TotalSeconds.ToString();
            if (totalSeconds.Length >= 5)
                totalSeconds = totalSeconds.Substring(0, 5);
            lblInfoPageLoadTime.Text = totalSeconds + " seconds";

            IViewBase view = Page as IViewBase;
            if (view != null)
            {
                int idx = view.CurrentPresenter.LastIndexOf(".");
                if (idx > -1)
                    lblCurrentPresenter.Text = view.CurrentPresenter.Substring(idx + 1);
                else
                    lblCurrentPresenter.Text = view.CurrentPresenter;
                lblCurrentPresenter.ToolTip = view.CurrentPresenter;

                lblCurrentView.ToolTip = view.GetType().FullName;
                lblCurrentView.Text = view.ViewName;
            }

            // set the active tab for display
            CBONodeSetType nodeSetName = view.CBOManager.GetCurrentNodeSetName(Context.User as SAHLPrincipal);
            if (nodeSetName == CBONodeSetType.CBO)
                this.tabsMenu.ActiveTab = this.tabsMenu.Tabs[0];
            else
                this.tabsMenu.ActiveTab = this.tabsMenu.Tabs[1];

            // set the attributes of the controls that require values for the client
            favIcon.Attributes.Add("href", ResolveClientUrl("~/Images/favicon.ico"));

            // add the hidden values that are used on the client - they need to be done this way as we don't 
            // want .NET-generated ids - we need static names to work with because we can't put <%= %> tags in the 
            // master as we need to insert dynamic controls from the view base
            AddClientHiddenValue("hidTimeoutUrl", ResolveClientUrl("~/TimeoutError.aspx"));
            AddClientHiddenValue("hidPageTimeout", (Page.Session.Timeout - 1).ToString());
            AddClientHiddenValue("hidMenuWidth", MenuWidth.ToString());
            AddClientHiddenValue("hidVirtualRoot", ResolveClientUrl("~/"));
            AddClientHiddenValue("navigate", "");
            AddClientHiddenValue("hidNodeSet", "");

            SAHLPrincipal sp = Context.User as SAHLPrincipal;
            if (sp.IsInRole(System.Configuration.ConfigurationManager.AppSettings.Get("HaloNoWorkFlowAccess")))
                pnlTasks.Visible = false;

        }
                private void AddClientHiddenValue(string id, string value)
        {
            HtmlGenericControl c = new HtmlGenericControl("input");
            c.Attributes.Add("type", "hidden");
            c.Attributes.Add("id", id);
            c.Attributes.Add("name", id);
            c.Attributes.Add("value", value);
            Page.Form.Controls.Add(c);
        }

        void mnuCBO_Navigate(object sender, SAHL.Common.Web.UI.Events.NavigateEventArgs e)
        {
            throw new Exception("The navigation method or operation is not implemented on the CBO Menu [NavigateValue=" + e.NavigateValue + "].");
        }

        /// <summary>
        /// Gets the width of the menu to display.
        /// </summary>
        // TODO: This needs to be set by the user
        protected int MenuWidth
        {
            get
            {
                if (Request.Form["hidMenuWidth"] != null && Request.Form["hidMenuWidth"].Length > 0)
                    return Int32.Parse(Request.Form["hidMenuWidth"]);
                else
                    return 250;
            }
        }
        /// <summary>
        /// Gets the current version number of the application.  This is used to append to script files so new releases 
        /// don't use old cached versions.
        /// </summary>
        protected static string Version
        {
            get
            {
                AssemblyName aName = Assembly.GetExecutingAssembly().GetName();
                return aName.Version.ToString(4);
            }
        }

        #region SAHLMasterBase Members

        /// <summary>
        /// Implements <see cref="SAHLMasterBase.ValidationSummary"/>.
        /// </summary>
        public override SAHL.Common.Web.UI.Controls.SAHLValidationSummary ValidationSummary
        {
            get
            {
                return valSummary;
            }
        }

        #endregion
    }
}