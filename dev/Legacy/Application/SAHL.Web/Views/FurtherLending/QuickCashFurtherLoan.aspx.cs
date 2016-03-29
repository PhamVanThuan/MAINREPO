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
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.FurtherLending
{
    public partial class QuickCashFurtherLoan : SAHLCommonBaseView, IQuickCashFurtherLoan
    {
        private bool _hasQuickCashDeclineReasons;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void QuickCashDetails_QCDeclineReasons(object sender, EventArgs e)
        {
            if (OnQCDeclineReasons != null)
                OnQCDeclineReasons(sender, e);
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

            QuickCashDetails.QCDeclineReasons += new EventHandler(QuickCashDetails_QCDeclineReasons);
        }

        public void BindQuickCashDetails(IApplication Application)
        {
            #region Binding the QuickCash Details Control
            ISupportsQuickCashApplicationInformation supportsQuickCashApplicationInformation = Application as ISupportsQuickCashApplicationInformation;

            IApplicationMortgageLoanWithCashOut applicationMortgageLoanWithCashOut = Application as IApplicationMortgageLoanWithCashOut;
            double cashOut = 0.0;
            if (applicationMortgageLoanWithCashOut != null
                && applicationMortgageLoanWithCashOut.RequestedCashAmount.HasValue
                && supportsQuickCashApplicationInformation != null)
            {
                cashOut = applicationMortgageLoanWithCashOut.RequestedCashAmount.Value;
                QuickCashDetails.BindQuickCash(supportsQuickCashApplicationInformation.ApplicationInformationQuickCash, cashOut, HasQuickCashDeclineReasons);
            }

            #endregion
        }

        public void GetQuickCashDetails(IApplicationInformationQuickCash appInfoQC)
        {
            // Get the QC data
            if (appInfoQC != null)
                QuickCashDetails.GetQuickCashDetails(appInfoQC);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            OnSaveButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnQCDeclineReasons;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSaveButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool HasQuickCashDeclineReasons
        {
            get { return _hasQuickCashDeclineReasons; }
            set { _hasQuickCashDeclineReasons = value; }
        }

    }
}
