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
using System.Collections.Generic;
using SAHL.Common.CacheData;

namespace SAHL.Web.Views.QuickCash.Presenters.QuickCashDetails
{
    /// <summary>
    /// 
    /// </summary>
    public class QuickCashEscalateToCreditBase : QuickCashDetailsBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public QuickCashEscalateToCreditBase(IQuickCashDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            GlobalCacheData.Remove(ViewConstants.GenericKey);

            _view.ShowApprovedPanel = true;
            _view.QuickCashInformation = quickCashApplicationInformation;
            _view.ShowThirdPartyPaymentsGrid = false;
            _view.BindApprovedPanel();
            _view.BindQuickCashPaymentsGrid(false);
            _view.ShowButtons = true;
            _view.SetSubmitButtonText = "Next";

            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked+=new EventHandler(_view_OnSubmitButtonClicked);

        }

        protected void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
        }

        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            IList<ICacheObjectLifeTime> LifeTimes = new List<ICacheObjectLifeTime>();
            GlobalCacheData.Add(ViewConstants.GenericKey, _offerKey, LifeTimes);

            _view.Navigator.Navigate("MemoQuickCashEscalateToCredit");
        }
    }
}
