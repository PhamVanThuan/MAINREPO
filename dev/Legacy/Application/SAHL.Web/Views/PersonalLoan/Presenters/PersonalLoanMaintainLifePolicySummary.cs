using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanMaintainLifePolicySummary : SAHLCommonBasePresenter<IPersonalLoanMaintainLifePolicy>
    {
        public PersonalLoanMaintainLifePolicySummary(IPersonalLoanMaintainLifePolicy view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
            {
                return;
            }

            this.View.SetupSummaryView();
        }
    }
}