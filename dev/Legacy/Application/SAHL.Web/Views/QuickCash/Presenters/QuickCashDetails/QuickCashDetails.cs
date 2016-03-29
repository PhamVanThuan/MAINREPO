using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.QuickCash.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class QuickCashDetails : QuickCashDetailsBase
    {
        int selectedGridIndex;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public QuickCashDetails(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            selectedGridIndex = 0;

            _view.onGridQuickCashPaymentSelectedIndexChanged+=new KeyChangedEventHandler(_view_onGridQuickCashPaymentSelectedIndexChanged);
            _view.ShowApprovedPanel = true;
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(true);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (appInfoQuickCash.ApplicationInformationQuickCashDetails != null && appInfoQuickCash.ApplicationInformationQuickCashDetails.Count > 0)
            {
                _view.BindThirdPartyPaymentsGrid(appInfoQuickCash.ApplicationInformationQuickCashDetails[selectedGridIndex].ApplicationExpenses, false);
                _view.ShowThirdPartyPaymentsGrid = true;
            }
            else
                _view.ShowThirdPartyPaymentsGrid = false;
        }

       
        protected void _view_onGridQuickCashPaymentSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            selectedGridIndex = Convert.ToInt32(e.Key);
        }        
    }
}
