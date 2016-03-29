using SAHL.Common.Web.UI;
using SAHL.Web.Views.PersonalLoan.Interfaces;
using System;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class PersonalLoanApplicationDeclarationUpdate : PersonalLoanApplicationDeclarationDisplay
    {
        public PersonalLoanApplicationDeclarationUpdate(IPersonalLoanApplicationDeclaration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // set update mode
            _view.UpdateMode = true;
            _view.UpdateButtonText = "Update";

            // call the base initialise
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            // setup the buttons
            _view.ShowUpdateButton = true;
            _view.ShowCancelButton = true;
            _view.ShowBackButton = false;
        }
    }
}