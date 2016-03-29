using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Globals;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// Allows the capture of extended employment information for a legal entity.
    /// </summary>
    public class LegalEntityEmploymentExtendedDisplay : LegalEntityEmploymentBase<IEmploymentExtended>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentExtendedDisplay(IEmploymentExtended view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #region Methods

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            View.CancelButtonClicked += new EventHandler(View_CancelButtonClicked);

            
        }


        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!View.ShouldRunPage) return;

            IEmployment employment = CachedEmployment;
            _commRepo.AttachUnModifiedToCurrentNHibernateSession(employment);
            View.SetEmployment(employment);
            View.BackButtonVisible = false;
            View.SaveButtonVisible = false;

            View.ConfirmedIncomeReadOnly = true;
            View.ConfirmedIncomeEnabled = true;
            View.MonthlyIncomeReadOnly = true;
            View.MonthlyIncomeEnabled = true;

            _view.SetConfirmationDisplay(employment);
            _view.SetExtendedEmployment(employment);

            SetUpEmploymentExtendedView();
            DataTable dtVerificationProcess = EmploymentRepository.GetVerificationProcessDT(employment);
            _view.BindVerificationProcessList(dtVerificationProcess);
        }


        protected void View_CancelButtonClicked(object sender, EventArgs e)
        {
            string navigateTo = PreviousView;
            if (String.IsNullOrEmpty(PreviousView))
                navigateTo = "Cancel";
            ClearCachedData();
            View.ShouldRunPage = false;
            View.Navigator.Navigate(navigateTo);
        }

        protected void SetUpEmploymentExtendedView()
        {
            _view.ContactPersonReadOnly = true;
            _view.PhoneNumberReadOnly = true;
            _view.DepartmentReadOnly = true;
            _view.ConfirmationSourceReadOnly = true;
            _view.VerificationProcessReadOnly = true;
            _view.ConfirmedDetailsEnabled = true;
            _view.VerificationProcessPanelEnabled = true;
            _view.SalaryPayDayReadOnly = true;
            _view.UnionMemberReadOnly = true;
        }

        #endregion

    }
}
