using SAHL.Common.CacheData;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanSubmitToCredit : SAHL.Web.Views.Common.Presenters.WorkflowYesNoPresenter
    {
        public PersonalLoanSubmitToCredit(IWorkFlowConfirm view, SAHLCommonBaseController controller)
            : base(view, controller) { }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            _view.TitleText = "Submit to Credit";
        }
    }
}