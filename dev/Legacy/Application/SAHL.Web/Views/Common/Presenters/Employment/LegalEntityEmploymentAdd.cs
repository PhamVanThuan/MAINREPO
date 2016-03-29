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
using SAHL.Web.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using System.Collections.Generic;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    public class LegalEntityEmploymentAdd : LegalEntityEmploymentBasic
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentAdd(IEmploymentView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            // this happens here because it MUST go before the data bind due to the implementation of the grid
            _view.GridPostBack = false;

            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.EmployerDetails.EditMode = EmployerDetailsEditMode.EditName;
            _view.EmployerDetails.EmployerSelected += new KeyChangedEventHandler(EmployerDetails_EmployerSelected);
            _view.SaveButtonClicked += new EventHandler(_view_SaveButtonClicked);
        }

        void _view_SaveButtonClicked(object sender, EventArgs e)
        {
            IEmployment employment = View.GetCapturedEmployment();
            if (employment == null)
            {
                // no employment type has been selected - we need to duck out here as we cannot create
                // an employment object
                View.Messages.Add(new Error("Employment type is a mandatory field.", "An employment object cannot be created without a valid employment type."));
                return;
            }
            if (CachedEmployerKey > 0)
                employment.Employer = EmploymentRepository.GetEmployerByKey(CachedEmployerKey);
            employment.LegalEntity = GetLegalEntity(View.CurrentPrincipal);

            ValidateEmployment(employment);

            // if the employment is valid, first check to see if we need to go to the next screen
            if (View.IsValid)
            {
                string nextView = GetNavigateTarget(View.ViewName, employment);
                if (String.IsNullOrEmpty(nextView))
                {
                    // no navigation required, save the entity and navigate back to the display page
                    SaveEmployment(View, employment, "LegalEntityEmploymentDisplay");
                }
                else
                {
                    CachedEmployment = employment;
                    PreviousView = View.ViewName;
                    View.Navigator.Navigate(nextView);
                }
            }
        }

        void EmployerDetails_EmployerSelected(object sender, KeyChangedEventArgs e)
        {
            CachedEmployerKey = Convert.ToInt32(e.Key);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!View.ShouldRunPage) return;

            if (CachedEmployment != null)
                PopulateEmploymentDetails(CachedEmployment, _view);

            if (CachedEmployerKey > 0)
                // if there's an employer key in the cache, then use that to set the displayed employer
                _view.EmployerDetails.Employer = EmploymentRepository.GetEmployerByKey(CachedEmployerKey);

            // set control display properties
            _view.SaveButtonVisible = true;
            _view.CancelButtonVisible = true;
            _view.EmploymentDetails.EmploymentTypeReadOnly = false;
            _view.EmploymentDetails.EmploymentStatusReadOnly = false;
            _view.EmploymentDetails.RemunerationTypeReadOnly = false;
            _view.EmploymentDetails.StartDateReadOnly = false;
            _view.EmploymentDetails.EndDateReadOnly = false;
            _view.EmploymentDetails.BasicIncomeReadOnly = false;

            // we need to examine the values to work out if we need to change the text on the button to next
            string saveButtonText = "Add";
            IEmployment employment = View.GetCapturedEmployment();
            if (employment != null && (employment.RequiresExtended || (employment is IEmploymentSubsidised)))
            {
                saveButtonText = "Next";
                _view.EmploymentDetails.BasicIncomeEnabled = false;
            }

            _view.SaveButtonText = saveButtonText;
            
            // Controls View
            AddLogic();
        }

        #region Helper Methods

        private void AddLogic()
        {
            if ((_view.EmploymentDetails.RemunerationType == null) ||
                (_view.EmploymentDetails.RemunerationType.Key == (int)RemunerationTypes.Salaried ||
                _view.EmploymentDetails.RemunerationType.Key == (int)RemunerationTypes.BasicAndCommission))
                _view.EmploymentDetails.BasicIncomeReadOnly = true;
            else
                _view.EmploymentDetails.BasicIncomeReadOnly = false;
        }

        #endregion

    }
}
