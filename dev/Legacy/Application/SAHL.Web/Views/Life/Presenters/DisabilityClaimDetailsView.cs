using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsView : DisabilityClaimDetailsBase
    {
        public DisabilityClaimDetailsView(IDisabilityClaimDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
        }
    }
}