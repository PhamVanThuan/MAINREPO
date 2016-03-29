using System;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.CreditProtectionPlan.Presenters
{
    public class LifePolicyClaimDisplay : LifePolicyClaimBase
    {
        public LifePolicyClaimDisplay(SAHL.Web.Views.CreditProtectionPlan.Interfaces.ILifePolicyClaim view, SAHLCommonBaseController controller)
			: base(view, controller)
		{
		}

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            _view.OnLifePolicyClaimGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnLifePolicyClaimGridSelectedIndexChanged);

            _view.LifePolicyClaimGrid_PostBackType = SAHL.Common.Web.UI.Controls.GridPostBackType.SingleClick;
            _view.ButtonRow_visability = false;
            _view.SetupControls(true);

            if (lifePolicyClaims != null && lifePolicyClaims.Count > 0)
            {
                _view.BindLifePolicyClaimGrid(lifePolicyClaims);
                _view.BindLifePolicyClaimFields(lifePolicyClaims[lifePolicyClaimGridIndexSelected], true, false);
            }
        }

        public void _view_OnLifePolicyClaimGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            lifePolicyClaimGridIndexSelected = (Convert.ToInt32(e.Key));
            if (lifePolicyClaims.Count > 0 && lifePolicyClaimGridIndexSelected >= 0)
                _view.BindLifePolicyClaimFields(lifePolicyClaims[lifePolicyClaimGridIndexSelected], true, false);
        }
    }
}