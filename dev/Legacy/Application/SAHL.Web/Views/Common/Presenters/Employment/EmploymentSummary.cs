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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;


namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// Displays employment information for all legal entities attached to an account.  
    /// </summary>
    public class EmploymentSummary : LegalEntityEmploymentBase<IEmploymentView>
    {
        private int _applicationKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public EmploymentSummary(IEmploymentView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            // get the account key from the CBO
            CBOMenuNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            _applicationKey = node.GenericKey;

            _view.SubsidyDetailsClicked += new EventHandler(_view_SubsidyDetailsClicked);
            _view.ExtendedDetailsClicked += new EventHandler(_view_ExtendedDetailsClicked);
            _view.EmploymentSelected += new EventHandler(_view_EmploymentSelected);

            View.GridColumnLegalEntityVisible = true;
            View.GridColumnStartDateVisible = false;
            BindEmployment();

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!View.ShouldRunPage) return;

            IEmployment emp = _view.SelectedEmployment;
            if (emp != null)
            {
                _view.EmployerDetails.Employer = emp.Employer;
                PopulateEmploymentDetails(emp, _view);
                _view.SubsidyDetailsButtonVisible = (emp is IEmploymentSubsidised);
                _view.ExtendedDetailsButtonVisible = emp.RequiresExtended;
            }
            else
            {
                _view.EmployerDetails.Visible = false;
                _view.EmploymentDetails.Visible = false;
            }

        }

        protected void _view_ExtendedDetailsClicked(object sender, EventArgs e)
        {
            CachedEmployment = _view.SelectedEmployment;
            PreviousView = View.ViewName;
            View.ShouldRunPage = false;
            View.Navigator.Navigate("EmploymentExtended");
        }

        protected void _view_SubsidyDetailsClicked(object sender, EventArgs e)
        {
            CachedEmployment = _view.SelectedEmployment;
            PreviousView = View.ViewName;
            View.ShouldRunPage = false;
            View.Navigator.Navigate("SubsidyDetails");
        }

        protected void _view_EmploymentSelected(object sender, EventArgs e)
        {
            // clear the cached data
            ClearCachedData();
            _view.EmployerDetails.ClearEmployer();

            IEmployment emp = View.SelectedEmployment;
            if (emp.Employer != null)
                CachedEmployerKey = emp.Employer.Key;
            PopulateEmploymentDetails(emp, _view);
            CachedEmployment = emp;

            BindEmployment();
        }

        private void BindEmployment()
        {
            IEventList<IEmployment> employment = EmploymentRepository.GetEmploymentByApplicationKey(_applicationKey,true);
            if (CachedEmployment != null)
                _view.SelectedEmployment = CachedEmployment;
            _view.BindEmploymentDetails(employment, false);

        }



    }
}
