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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Utils;
using SAHL.Common;

namespace SAHL.Web.Views.Common
{
    public partial class WorkFlowConfirm : SAHLCommonBaseView, IWorkFlowConfirm
    {
        public event EventHandler OnYesButtonClicked;
        public event EventHandler OnNoButtonClicked;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void ShowControls(bool visible)
        {
            divOuter.Visible = visible;
        }

		public string TitleText
		{
            get
            {
                return lblTitleText.Text;
            }
            set
            {
                lblTitleText.Text = value;
            }
		}

        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (OnYesButtonClicked != null)
                OnYesButtonClicked(sender, e);
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            if (OnNoButtonClicked != null)                
                OnNoButtonClicked(sender, e);
        }
    }
}
