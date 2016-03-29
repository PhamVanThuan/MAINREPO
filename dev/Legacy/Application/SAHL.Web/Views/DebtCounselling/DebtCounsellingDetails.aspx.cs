using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class DebtCounsellingDetails : SAHLCommonBaseView, IDebtCounsellingDetails
    {
        #region Properties
        public string ReferenceNumber 
        { 
            get
            {
                return txtReferenceNo.Text.Trim();
            }
            set
            {
                txtReferenceNo.Text = value;
            }
        }


        #endregion

        public event EventHandler OnCancelButtonClicked;
        public event EventHandler OnSubmitButtonClicked;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage)
                return;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClicked(sender, e);
        }
    }
}