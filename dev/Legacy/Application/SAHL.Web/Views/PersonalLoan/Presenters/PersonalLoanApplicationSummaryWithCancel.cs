using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanApplicationSummaryWithCancel : PersonalLoanApplicationSummary
    {
        public PersonalLoanApplicationSummaryWithCancel(IPersonalLoanApplicationSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.ShowCancelButton = true;
            _view.ShowHistoryButton = true;
        }
    }
}