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
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// 
    /// </summary>
    public class LegalEntityEmploymentUpdate : LegalEntityEmploymentBasic
    {
        IEmployment _emp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentUpdate(IEmploymentView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _view.EmployerDetails.EditMode = EmployerDetailsEditMode.EditName;
            _view.EmployerDetails.EmployerSelected += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(EmployerDetails_EmployerSelected);
            _view.SaveButtonClicked += new EventHandler(_view_SaveButtonClicked);
            _view.SubsidyDetailsClicked += new EventHandler(_view_SubsidyDetailsClicked);

            if (!_view.IsPostBack && CachedEmployment == null && _view.SelectedEmployment != null)
                CachedEmployment = _view.SelectedEmployment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewLoaded(object sender, EventArgs e)
        {
            base.OnViewLoaded(sender, e);
            if (!View.ShouldRunPage) return;

            //if (!_view.IsPostBack)
            //{
            //    if (CachedEmployment != null)
            //    {
            //        PopulateEmploymentDetails(CachedEmployment, _view);
            //    }
            //    else if (_view.SelectedEmployment != null)
            //    {
            //        PopulateEmploymentDetails(_view.SelectedEmployment, _view);
            //        CachedEmployment = View.SelectedEmployment;
            //        if (CachedEmployment.Employer != null)
            //            CachedEmployerKey = CachedEmployment.Employer.Key;
            //    }
            //}
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

            _emp = CachedEmployment;

            if (!_view.IsPostBack)
                PopulateEmploymentDetails(_emp, _view);

            UpdateLogic();

            if (_view.IsPostBack)
                PopulateReadOnlyEmploymentDetails(_emp, _view);
        }

        #region EmploymentDetails Control Event Handlers

        void _view_SubsidyDetailsClicked(object sender, EventArgs e)
        {
            PreviousView = "LegalEntityEmploymentUpdate";
            View.ShouldRunPage = false;
            View.Navigator.Navigate("LegalEntitySubsidyDetailsUpdate");
        }

        void _view_SaveButtonClicked(object sender, EventArgs e)
        {
            IEmployment employment = EmploymentRepository.GetEmploymentByKey(CachedEmployment.Key);

            int origEmploymentStatusKey = employment.EmploymentStatus.Key;
            int origConfirmedEmploymentFlag = employment.ConfirmedEmploymentFlag.HasValue ? Convert.ToInt32(employment.ConfirmedEmploymentFlag.Value) : -1;
            double origConfirmedIncome = employment.ConfirmedIncome;

            CopyCachedValues(View.CurrentPrincipal, employment);

            CachedEmployment = View.GetCapturedEmployment(employment);
            employment.LegalEntity = GetLegalEntity(View.CurrentPrincipal);
            ValidateEmployment(employment);

            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            if (employment.RemunerationType.Key != (int)RemunerationTypes.Salaried &&
                employment.RemunerationType.Key != (int)RemunerationTypes.BasicAndCommission)
            {
                svc.ExecuteRule(spc.DomainMessages, "EmploymentPreviousConfirmedIncomeCannotChange", employment, origEmploymentStatusKey, origConfirmedIncome);
                svc.ExecuteRule(spc.DomainMessages, "EmploymentConfirmedSetYesSave", employment);
                svc.ExecuteRule(spc.DomainMessages, "EmploymentConfirmedSetBackToNo", employment, origConfirmedEmploymentFlag);
            }
            svc.ExecuteRule(spc.DomainMessages, "ExistingConfirmedEmployment", employment);
            svc.ExecuteRule(spc.DomainMessages, "EmploymentConfirmedIncomeMandatory", employment);
            svc.ExecuteRule(spc.DomainMessages, "EmploymentSubsidisedSetEmploymentToPrevious", employment, origEmploymentStatusKey);

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

        void EmployerDetails_EmployerSelected(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            CachedEmployerKey = Convert.ToInt32(e.Key);
        }

        #endregion

        #region Helper Methods

        private void UpdateLogic()
        {
            // if there's an employer key in the cache, then use that to set the displayed employer, otherwise 
            // use the employer from the selected record
            if (CachedEmployerKey > 0)
                _view.EmployerDetails.Employer = EmploymentRepository.GetEmployerByKey(CachedEmployerKey);
            else if (_emp != null && _view.SelectedEmployment.Employer != null)
                _view.EmployerDetails.Employer = _view.SelectedEmployment.Employer;

            if (_emp != null)
            {
                _view.EmploymentDetails.EmploymentStatusReadOnly = false;
                _view.EmploymentDetails.StartDateReadOnly = false;
                _view.EmploymentDetails.EndDateReadOnly = false;
                //if (_emp.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                if (_view.EmploymentDetails.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
                {
                    _view.EmploymentDetails.ConfirmedEmploymentReadOnly = true;
                    _view.EmploymentDetails.ConfirmedIncomeReadOnly = true;
                }
                else
                {
                    _view.EmploymentDetails.ConfirmedEmploymentReadOnly = false;
                    _view.EmploymentDetails.ConfirmedIncomeReadOnly = false;
                }
                _view.CancelButtonVisible = true;
                _view.SaveButtonVisible = true;

                if (_emp.ConfirmedEmploymentFlag.HasValue && 
                    Convert.ToInt32(_emp.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.Yes)
                    _view.EmployerDetails.EditMode = EmployerDetailsEditMode.ReadOnly;

                if ((_view.EmploymentDetails.RemunerationType != null) &&
                    (_view.EmploymentDetails.RemunerationType.Key == (int)RemunerationTypes.Salaried ||
                    _view.EmploymentDetails.RemunerationType.Key == (int)RemunerationTypes.BasicAndCommission))
                {
                    _view.EmploymentDetails.BasicIncomeReadOnly = true;
                    _view.EmploymentDetails.ConfirmedBasicIncomeReadOnly = true;
                }
                else
                {
                    if (_view.EmploymentDetails.ConfirmedIncome.HasValue
                        && Convert.ToInt32(_view.EmploymentDetails.ConfirmedIncome.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.Yes)
                    {
                        _view.EmploymentDetails.BasicIncomeReadOnly = true;
                        _view.EmploymentDetails.ConfirmedBasicIncomeReadOnly = false;
                    }
                    else
                    {
                        _view.EmploymentDetails.BasicIncomeReadOnly = false;
                        _view.EmploymentDetails.ConfirmedBasicIncomeReadOnly = true;
                    }
                }

                // Reset values
                if (_view.EmploymentDetails.ConfirmedIncome.HasValue
                && Convert.ToInt32(_view.EmploymentDetails.ConfirmedIncome.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.No)
                {
                    _view.EmploymentDetails.ConfirmedBasicIncome = new double?();
                    if (_emp != null)
                        ResetConfirmedEmploymentValues(_emp);
                }

                // Set button text
                if (_view.EmploymentDetails.ConfirmedIncome.HasValue && _view.EmploymentDetails.ConfirmedEmployment.HasValue)
                {
                    if (Convert.ToInt32(_view.EmploymentDetails.ConfirmedIncome.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.No &&
                        Convert.ToInt32(_view.EmploymentDetails.ConfirmedEmployment.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.No)
                        _view.SaveButtonText = "Update";
                    else
                        _view.SaveButtonText = "Next";
                }
                else
                    _view.SaveButtonText = "Update";

                // Check Employment Status
                if (_view.EmploymentDetails.EmploymentStatusKey.Value == (int)EmploymentStatuses.Previous)
                {
                    _view.EmploymentDetails.BasicIncomeReadOnly = true;
                    _view.EmploymentDetails.ConfirmedBasicIncomeReadOnly = true;
                }
            }
            else
            {
                _view.EmployerDetails.Visible = false;
                _view.EmploymentDetails.Visible = false;
            }
        }

        #endregion

    }
}
