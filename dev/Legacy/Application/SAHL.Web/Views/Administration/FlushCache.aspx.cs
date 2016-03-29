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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Web.AJAX;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Administration
{
    /// <summary>
    /// View used to flush various items from the cache.
    /// </summary>
    public partial class FlushCache : SAHLCommonBaseView, SAHL.Web.Views.Administration.Interfaces.IFlushCache
    {

        /// <summary>
        /// Raised when the lookups button is clicked.
        /// </summary>
        public event KeyChangedEventHandler LookupButtonClicked;

        /// <summary>
        /// Raised when the 'Clear All' button is clicked.
        /// </summary>
        public event EventHandler ClearAllButtonClicked;

        /// <summary>
        /// Raised when the 'Clear All Lookups' button is clicked.
        /// </summary>
        public event EventHandler LookupAllButtonClicked;

        /// <summary>
        /// Raised when the user access button is clicked.
        /// </summary>
        public event KeyChangedEventHandler UserAccessButtonClicked;

        /// <summary>
        /// Raised when the 'Clear All' UIStatements button is clicked.
        /// </summary>
        public event EventHandler UIStatementButtonClicked;

		/// <summary>
		/// Raised when the 'Clear All' UIStatements button is clicked.
		/// </summary>
		public event EventHandler OrgStructureButtonClicked;

        #region Page Life Cycle Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage) return;

            ddlLookup.DataSource = Enum.GetNames(typeof(LookupKeys));
            ddlLookup.DataBind();
            ddlLookup.SelectedIndex = 0;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!ShouldRunPage) return;

            RegisterWebService(ServiceConstants.AdUser);
            lblMsg.Visible = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            // this is done here as setting it to Enabled=false results in the client side script call not 
            // being added
            btnLookup.Attributes.Add("disabled", "true");
        }

        #endregion


        /// <summary>
        /// Displays an info message on the screen to the user.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="isError">Whether the message is an error or not.</param>
        public void SetMessage(string text, bool isError)
        {
            lblMsg.InnerText = text;
            lblMsg.Visible = true;

            // update the css class depending on whether it's an error
            string cssClass = lblMsg.Attributes["class"];
            cssClass = cssClass + (isError ? " backgroundError borderError" : " backgroundLight borderAll");
            lblMsg.Attributes["class"] = cssClass;
        }

        protected void btnClearAll_Click(object sender, EventArgs e)
        {
            if (ClearAllButtonClicked != null)
                ClearAllButtonClicked(sender, new EventArgs());
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            if (ddlLookup.SelectedValue == SAHLDropDownList.PleaseSelectValue)
                return;

            if (LookupButtonClicked != null)
            {
                LookupButtonClicked(sender, new KeyChangedEventArgs(ddlLookup.SelectedValue));
                ddlLookup.SelectedIndex = 0;
            }

        }

        protected void btnLookupAll_Click(object sender, EventArgs e)
        {
            if (LookupAllButtonClicked != null)
            {
                LookupAllButtonClicked(sender, new EventArgs());
                ddlLookup.SelectedIndex = 0;
            }

        }

        protected void btnUserAccess_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUserAccess.Text))
                return;

            if (UserAccessButtonClicked != null)
                UserAccessButtonClicked(sender, new KeyChangedEventArgs(txtUserAccess.Text));

        }

        protected void btnUIStatement_Click(object sender, EventArgs e)
        {
            if (UIStatementButtonClicked != null)
                UIStatementButtonClicked(sender, e);

        }

		protected void btnOrgStructure_Click(object sender, EventArgs e)
		{
			if (OrgStructureButtonClicked != null)
				OrgStructureButtonClicked(sender, e);

		}


        public event EventHandler RuleItemButtonClicked;

        protected void btnRuleItem_Click(object sender, EventArgs e)
        {
            if (RuleItemButtonClicked != null)
                RuleItemButtonClicked(sender, e);
        }   
    }
}
