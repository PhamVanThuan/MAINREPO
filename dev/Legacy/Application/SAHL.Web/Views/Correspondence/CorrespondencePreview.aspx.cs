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
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Correspondence.Interfaces;
using System.Drawing;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Globalization;
using SAHL.Common.Collections.Interfaces;
using AjaxControlToolkit;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Correspondence
{
    public partial class CorrespondencePreview : SAHLCommonBaseView, ICorrespondencePreview
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
        
            // this must be set here for the report control to work
            EnableViewState = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        #region ICorrespondencePreview Members

        public void AddReportsToPage(AjaxControlToolkit.TabContainer tabContainer)
        {
            if (tabContainer != null && tabContainer.Tabs.Count > 0)
                pnTabContainer.Controls.Add(tabContainer);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        #endregion

    }
}