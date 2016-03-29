using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.FurtherLending.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.FurtherLending.Presenters
{
    public class DeclineWithOffer : CalculatorBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DeclineWithOffer(IFurtherLendingCalculator view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        
        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            _view.ApprovalMode = SAHL.Common.Globals.ApprovalTypes.DeclineWithOffer;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.BtnSubmitText = "Decline with Offer";
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _view.CanUpdate = true;

            CheckEmploymentTypeForCredit();
        }

        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!_view.ShouldRunPage)
                return;

            DoITCCheck();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            CreditStageComplete(true);
        }
    }
}
