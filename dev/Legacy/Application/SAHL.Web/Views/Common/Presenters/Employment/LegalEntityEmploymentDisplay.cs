using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    public class LegalEntityEmploymentDisplay : LegalEntityEmploymentBasic
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentDisplay(IEmploymentView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.SubsidyDetailsClicked += new EventHandler(_view_SubsidyDetailsClicked);
            _view.ExtendedDetailsClicked += new EventHandler(_view_ExtendedDetailsClicked);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                //_view.ExtendedDetailsButtonVisible = emp.RequiresExtended;
                // Extended Details should always be visible
                _view.ExtendedDetailsButtonVisible = true;
            }
            else
            {
                _view.EmployerDetails.Visible = false;
                _view.EmploymentDetails.Visible = false;
            }
        }

        protected void _view_SubsidyDetailsClicked(object sender, EventArgs e)
        {
            CachedEmployment = _view.SelectedEmployment;
            PreviousView = View.ViewName;
            View.ShouldRunPage = false;
            View.Navigator.Navigate("SubsidyDetails");
        }

        protected void _view_ExtendedDetailsClicked(object sender, EventArgs e)
        {
            CachedEmployment = _view.SelectedEmployment;
            View.ShouldRunPage = false;
            View.Navigator.Navigate("EmploymentExtended");
        }


    }
}
