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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.Employment
{
    /// <summary>
    /// Allows the capture of extended employment information for a legal entity.
    /// </summary>
    public class LegalEntityEmploymentExtendedUpdate : LegalEntityEmploymentBase<IEmploymentExtended>
    {

        int _origEmploymentStatusKey;
        int _origConfirmedEmploymentFlag;
        double _origConfirmedIncome;
        IEmployment _employment;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public LegalEntityEmploymentExtendedUpdate(IEmploymentExtended view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #region Methods

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;
            View.SaveButtonClicked += new EventHandler(View_SaveButtonClicked);
            View.BackButtonClicked += new EventHandler(View_BackButtonClicked);
            View.CancelButtonClicked += new EventHandler(View_CancelButtonClicked);

            GetEmployment();

            if (_employment != null)
            {
                ExtendedLogic();
                IEventList<IEmploymentConfirmationSource> empConLst = LookupRepository.EmploymentConfirmationSources;
                _view.BindConfirmationSourceList(empConLst);
                _view.BindUniomMemberShipList();
                DataTable dtVerificationProcess = EmploymentRepository.GetVerificationProcessDT(_employment);
                _view.BindVerificationProcessList(dtVerificationProcess);
            }
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            // this still runs after Navigate is called, so we need to check for null (if it's null, it means the object
            // has been saved and we are moving away from the page
            if (_employment != null)
            {
                _view.SetEmployment(_employment);
                _view.SetExtendedEmployment(_employment);
                _view.SetConfirmationEdit(_employment);

                string saveButtonText = (_employment.Key <= 0 ? "Add" : "Update");
                View.SaveButtonText = ((_employment is IEmploymentSubsidised) ? "Next" : saveButtonText);
            }
        }

        void View_CancelButtonClicked(object sender, EventArgs e)
        {
            ClearCachedData();
            View.ShouldRunPage = false;
            View.Navigator.Navigate("Cancel");
        }

        void View_BackButtonClicked(object sender, EventArgs e)
        {
            View.GetExtendedDetails(_employment);
            CachedEmployment = _employment;
            View.ShouldRunPage = false;
            View.Navigator.Navigate("Back");
        }

        void View_SaveButtonClicked(object sender, EventArgs e)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IRuleService svc = ServiceFactory.GetService<IRuleService>();

            // Retrieve Values From View
            View.GetExtendedDetails(_employment);

            // Cache Employment Confirmation Source
            CachedEmploymentConfirmationSourceKey = (_employment.EmploymentConfirmationSource != null?_employment.EmploymentConfirmationSource.Key:new int?());

            // Update Verification Options
            if (_view.PanelConfirmedEnabled && _view.VerificationProcessPanelEnabled)
            {
                List<int> verificationList = _view.GetVerificationProcessList;

                // Remove items that are no longer applicable
                if (_employment.EmploymentVerificationProcesses != null && _employment.EmploymentVerificationProcesses.Count > 0)
                {
                    List<int> verificationRemoveList = new List<int>();

                    foreach (IEmploymentVerificationProcess evp in _employment.EmploymentVerificationProcesses)
                    {
                        if (!verificationList.Contains(evp.EmploymentVerificationProcessType.Key))
                            verificationRemoveList.Add(evp.EmploymentVerificationProcessType.Key);
                    }

                    if (verificationRemoveList != null && verificationRemoveList.Count > 0)
                        CachedDeleteEmploymentVerificationProcess = verificationRemoveList;
                }
                // Add Items
                if (verificationList != null && verificationList.Count > 0)
                    CachedAddEmploymentVerificationProcess = verificationList;
            }

            CachedEmployment = _employment;

            if (_employment.RemunerationType.Key == (int)RemunerationTypes.Salaried ||
                _employment.RemunerationType.Key == (int)RemunerationTypes.BasicAndCommission)
            {
                svc.ExecuteRule(spc.DomainMessages, "EmploymentPreviousConfirmedIncomeCannotChange", _employment, _origEmploymentStatusKey, _origConfirmedIncome);
                svc.ExecuteRule(spc.DomainMessages, "EmploymentConfirmedSetYesSave", _employment);
                svc.ExecuteRule(spc.DomainMessages, "EmploymentConfirmedSetBackToNo", _employment, _origConfirmedEmploymentFlag);
            }

            if (_employment.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
            {
                if (_view.PanelConfirmedEnabled && _view.ConfirmedDetailsEnabled == true)
                {
                    svc.ExecuteRule(spc.DomainMessages, "EmploymentContactPersonMandatory", _employment);
                    svc.ExecuteRule(spc.DomainMessages, "EmploymentContactPhoneMandatory", _employment);
                    svc.ExecuteRule(spc.DomainMessages, "EmploymentDepartmentMandatory", _employment);
                    svc.ExecuteRule(spc.DomainMessages, "EmploymentConfirmationSourceMandatory", _employment);
                    svc.ExecuteRule(spc.DomainMessages, "EmploymentSalaryPayDayMandatory", _employment);
                    if (_employment.SalaryPaymentDay.HasValue)
                    {
                        if (_employment.SalaryPaymentDay.Value <= 0 || _employment.SalaryPaymentDay.Value > 31)
                        {
                            _view.Messages.Add(new DomainMessage("Salary payment day must be between 1 and 31", "Salary payment day must be between 1 and 31"));
                        }
                    }
                }
                if (_view.PanelConfirmedEnabled && _view.VerificationProcessPanelEnabled == true)
                {
                    svc.ExecuteRule(spc.DomainMessages, "EmploymentVerificationProcessMinimum", _employment, CachedAddEmploymentVerificationProcess);
                }
            }

            // validate the employment record before allowing the user to continue
            _employment.ValidateEntity();

            // if the employment is valid, first check to see if we need to go to the next screen
            if (View.IsValid)
            {
                if (_employment.EmploymentType.Key == (int)EmploymentTypes.SalariedwithDeduction)
                {
                    PreviousView = View.ViewName;
                    View.Navigator.Navigate("SubsidyDetails");
                }
                else
                {
                    SaveEmployment(View, _employment, "LegalEntityEmploymentDisplay");
                }
            }

        }

        private void ExtendedLogic()
        {
            _view.ContactPersonReadOnly = false;
            _view.PhoneNumberReadOnly = false;
            _view.DepartmentReadOnly = false;
            _view.ConfirmationSourceReadOnly = false;
            _view.VerificationProcessReadOnly = false;
            _view.UnionMemberReadOnly = false;

            if (_employment.EmploymentStatus.Key == (int)EmploymentStatuses.Previous)
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "EmploymentPreviousValuesCannotChange", _employment);

                View.MonthlyIncomeEnabled = false;
                View.ConfirmedIncomeEnabled = false;
                View.ConfirmedDetailsEnabled = false;
                _view.VerificationProcessPanelEnabled = false;
                return;
            }

            if (_employment.RemunerationType.Key == (int)RemunerationTypes.Salaried ||
                _employment.RemunerationType.Key == (int)RemunerationTypes.BasicAndCommission)
            {
                // Which Rand Value column will be editable
                // Set Verification Process panel to be editable
                if (!_employment.ConfirmedIncomeFlag.HasValue || 
                    (_employment.ConfirmedIncomeFlag.HasValue && Convert.ToInt32(_employment.ConfirmedIncomeFlag.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.No))
                {
                    
                    View.MonthlyIncomeEnabled = true;
                    View.ConfirmedIncomeEnabled = false;
                    _view.VerificationProcessPanelEnabled = false;
                }
                else
                {
                    View.MonthlyIncomeEnabled = false;
                    View.ConfirmedIncomeEnabled = true;
                    _view.VerificationProcessPanelEnabled = true;
                }

                // Set confirm details panel to be editable
                if (!_employment.ConfirmedEmploymentFlag.HasValue ||
                    (_employment.ConfirmedEmploymentFlag.HasValue && Convert.ToInt32(_employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.No))
                    View.ConfirmedDetailsEnabled = false;
                else
                    View.ConfirmedDetailsEnabled = true;
            }
            else
            {
                // Set Verification Process panel to be editable
                if (!_employment.ConfirmedIncomeFlag.HasValue ||
                    (_employment.ConfirmedIncomeFlag.HasValue && Convert.ToInt32(_employment.ConfirmedIncomeFlag.Value) == (int)SAHL.Common.Globals.ConfirmedIncome.No))
                    _view.VerificationProcessPanelEnabled = false;
                else
                    
                    _view.VerificationProcessPanelEnabled = true;

                // Set confirm details panel to be editable
                if (!_employment.ConfirmedEmploymentFlag.HasValue ||
                    (_employment.ConfirmedEmploymentFlag.HasValue && Convert.ToInt32(_employment.ConfirmedEmploymentFlag.Value) == (int)SAHL.Common.Globals.ConfirmedEmployment.No))
                    
                    View.ConfirmedDetailsEnabled = false;
                else
                    View.ConfirmedDetailsEnabled = true;
            }
        }

        private void GetEmployment()
        {
            if (CachedEmployment.Key == 0)
                _employment = EmploymentRepository.GetEmptyEmploymentByType(CachedEmployment.EmploymentType);
            else
                _employment = EmploymentRepository.GetEmploymentByKey(CachedEmployment.Key);

            _origEmploymentStatusKey = _employment.EmploymentStatus != null ? _employment.EmploymentStatus.Key : -1;
            _origConfirmedEmploymentFlag = _employment.ConfirmedEmploymentFlag.HasValue ? Convert.ToInt32(_employment.ConfirmedEmploymentFlag.Value) : -1;
            _origConfirmedIncome = _employment.ConfirmedIncome;

            CopyCachedValues(View.CurrentPrincipal, _employment);
        }

        #endregion

    }
}
