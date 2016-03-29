using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Cap.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Cap.Presenters
{
    public class CapSalesConfigurationAdd : CapSalesConfigurationBase
    {
        #region Private Variables

        private IList<ICapType> _capTypeList;
        private DateTime _capEffectiveDate;
        private DateTime _capClosureDate;
        private double _capRate;
        private double _premium;
        IResetConfiguration _resetConfiguration;
        private ICommonRepository commonRepository;

        public ICommonRepository CommonRepository
        {
            get
            {
                if (commonRepository == null)
                {
                    commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();
                }
                return commonRepository;
            }
        }

        private IControlRepository controlRepository;

        public IControlRepository ControlRepository
        {
            get
            {
                if (controlRepository == null)
                    controlRepository = RepositoryFactory.GetRepository<IControlRepository>();

                return controlRepository;
            }
        }

        #endregion Private Variables

        #region Constructor

        public CapSalesConfigurationAdd(ICapSalesConfiguration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion Constructor

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);
            _view.CapSalesConfigGridPostBackType = GridPostBackType.NoneWithClientSelect;
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _view.ShowAddControls();

            BindResetDateDropDown();
            BindCapTypeDropDown();

            BindGrid();
            BindAddControls();
        }

        private void BindCapTypeDropDown()
        {
            _capTypeList = _capRepository.GetCapTypes();
            _view.BindCapTypeDropDown(_capTypeList);
        }

        private void BindAddControls()
        {
            DateTime resetDate = DateTime.Now;
            ICapType capType = null;
            if (!_view.IsPostBack)
            {
                if (_resetDates != null && _resetDates.Rows.Count > 0)
                {
                    resetDate = Convert.ToDateTime(_resetDates.Rows[0]["ResetDate"]);
                }
                else
                {
                    _view.ClearAddControls();
                    return;
                }

                capType = _capTypeList[0];
            }
            else
            {
                if (!string.IsNullOrEmpty(_view.SelectedCapResetConfigDate))
                {
                    resetDate = DateTime.ParseExact(_view.SelectedCapResetConfigDate, SAHL.Common.Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    _view.ClearAddControls();
                    return;
                }

                if (_view.SelectedCapType != -1)
                    capType = _capTypeList[_view.SelectedCapType - 1];
            }

            _capEffectiveDate = DateTime.Now;
            _capRate = 0;
            _premium = 0;
            for (int i = 0; i < _resetDates.Rows.Count; i++)
            {
                if (Convert.ToDateTime(_resetDates.Rows[i]["ResetDate"]) == resetDate)
                {
                    _capEffectiveDate = Convert.ToDateTime(_resetDates.Rows[i]["ActionDate"]);

                    IReset previousReset = _capRepository.GetPreviousResetByResetDateAndRCKey(resetDate,
                                    Convert.ToInt32(_resetDates.Rows[i]["ResetConfigurationKey"]));

                    _resetConfiguration = previousReset.ResetConfiguration;
                    double jibarRate = previousReset.JIBARRate;
                    if (capType != null)
                        _capRate = capType.Value + jibarRate;
                }
            }

            _capClosureDate = Convert.ToDateTime(resetDate);
            _capClosureDate = _capClosureDate.AddMonths(_view.TermValue);

            _capClosureDate = CommonRepository.GetNextBusinessDay(_capClosureDate);

            IControl control = ControlRepository.GetControlByDescription(SAHL.Common.Constants.ControlTable.CAP.CapEndDateTolerence);
            int capEndDays = Convert.ToInt16(control.ControlNumeric);

            _capClosureDate = _capClosureDate.AddDays(capEndDays);

            _premium = _view.PremiumFeeValue + _view.AdminFeeValue;

            _view.BindAddControls(_capEffectiveDate, _capClosureDate, _capRate, _premium);
        }

        #region Event Handlers

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("CapSalesConfiguration");
        }

        private void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            int _resetConfigurationKey = -1;
            DateTime _resetDate = DateTime.ParseExact(_view.SelectedCapResetConfigDate, SAHL.Common.Constants.DateFormat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime _applicationStartDate = _view.ApplicationStartDateValue;

            //
            string _applicationEndDateStr = _view.ApplicationEndDateValue.ToString(SAHL.Common.Constants.DateFormat) + " 23:59:59";
            DateTime _applicationEndDate = DateTime.ParseExact(_applicationEndDateStr, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            //
            for (int i = 0; i < _resetDates.Rows.Count; i++)
            {
                if (Convert.ToDateTime(_resetDates.Rows[i]["ResetDate"]) == _resetDate)
                {
                    _resetConfigurationKey = Convert.ToInt32(_resetDates.Rows[i]["ResetConfigurationKey"]);
                }
            }

            ICapTypeConfiguration capConfig = CheckConfigRowExists(
                                                            _resetConfigurationKey,
                                                            _resetDate,
                                                            _capEffectiveDate,
                                                             _capClosureDate,
                                                             _applicationStartDate,
                                                             _applicationEndDate);

            if (capConfig == null)
            {
                capConfig = _capRepository.CreateCapTypeConfiguration();
                capConfig.ApplicationStartDate = _applicationStartDate;
                capConfig.ApplicationEndDate = _applicationEndDate;
                capConfig.CapEffectiveDate = _capEffectiveDate;
                capConfig.CapClosureDate = _capClosureDate;
                capConfig.ChangeDate = DateTime.Now;
                capConfig.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                capConfig.ResetConfiguration = _resetConfiguration;
                capConfig.ResetDate = _resetDate;
                capConfig.Term = _view.TermValue;
                capConfig.UserID = _view.CurrentPrincipal.Identity.Name;
            }
            else
            {
                capConfig.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                capConfig.ChangeDate = DateTime.Now;
            }

            ICapTypeConfigurationDetail capConfigDetail = _capRepository.CreateCapTypeConfigurationDetail();
            capConfigDetail.CapType = _capTypeList[_view.SelectedCapType - 1];
            capConfigDetail.CapTypeConfiguration = capConfig;
            capConfigDetail.ChangeDate = DateTime.Now;
            capConfigDetail.FeeAdmin = _view.AdminFeeValue;
            capConfigDetail.FeePremium = _view.PremiumFeeValue;
            capConfigDetail.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
            capConfigDetail.Premium = _view.AdminFeeValue + _view.PremiumFeeValue;
            capConfigDetail.Rate = _capRate;
            capConfigDetail.RateFinance = _view.RateFinanceValue;
            capConfigDetail.UserID = _view.CurrentPrincipal.Identity.Name;

            //
            capConfig.CapTypeConfigurationDetails.Add(_view.Messages, capConfigDetail);

            //
            TransactionScope txn = new TransactionScope();
            try
            {
                _capRepository.SaveCapTypeConfiguration(capConfig);

                //_capRepository.SaveCapTypeConfigurationDetail(capConfigDetail);
                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }

            //
            BindGrid();
            _view.ClearAddControls();
            _view.BindAddControls(_capEffectiveDate, _capClosureDate, _capRate, _premium);
        }

        private ICapTypeConfiguration CheckConfigRowExists(int m_ResetConfigurationKey, DateTime m_ResetDate, DateTime m_CapEffectiveDate, DateTime m_CapClosureDate, DateTime m_OfferStartDate, DateTime m_OfferEndDate)
        {
            for (int i = 0; i < _capConfigList.Count; i++)
            {
                ICapTypeConfiguration checkRow = _capConfigList[i];

                if (
                    (checkRow.ResetConfiguration.Key == m_ResetConfigurationKey) &&
                    (checkRow.ResetDate == m_ResetDate) &&
                    (checkRow.CapEffectiveDate == m_CapEffectiveDate) &&
                    (checkRow.CapClosureDate == m_CapClosureDate) &&
                    (checkRow.ApplicationStartDate == m_OfferStartDate) &&
                    (checkRow.ApplicationEndDate == m_OfferEndDate)
                    )
                {
                    return checkRow;
                }
            }
            return null;
        }

        #endregion Event Handlers
    }
}