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
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Factories;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.QuickCash
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WaiveCharges : SAHLCommonBaseView,IWaiveCharges
    {
        #region Private Members
        
        private bool _showManagerComments;
        private bool _enableWaiveChargesReasonControl;

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            txtWaiveChargesReason.Enabled = _enableWaiveChargesReasonControl;
            txtManagerComments.Visible = _showManagerComments;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {

        }

        #region IWaiveCharges Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;

        public bool EnableWaiveChargesReasonControl
        {
            set { _enableWaiveChargesReasonControl = value; }
        }

        public bool ShowManagerComments
        {
            set { _showManagerComments = value; }
        }

        #endregion
    }
}
