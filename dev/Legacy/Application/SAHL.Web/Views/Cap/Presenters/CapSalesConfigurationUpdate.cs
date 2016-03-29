using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Cap.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Cap.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class CapSalesConfigurationUpdate : CapSalesConfigurationBase
    {
        #region Private Variables

        private string _cachedDataLastSelectedResetDateKey = "LastSelectedResetDate";

        #endregion Private Variables

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public CapSalesConfigurationUpdate(ICapSalesConfiguration view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        #endregion Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage) return;

            _view.OnGridSelectedIndexChanged += new EventHandler(_view_OnGridSelectedIndexChanged);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _view.CapSalesConfigGridPostBackType = GridPostBackType.SingleClick;

            _view.ShowUpdateControls();

            BindResetDateDropDown();
            BindStatusDropDown();
            BindGrid();
            BindUpdateControls();

            if (_capConfigDetailList == null || _capConfigDetailList.Count == 0)
                _view.SubmitButtonEnabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        private void BindStatusDropDown()
        {
            _view.BindStatusDropDown(new List<IGeneralStatus>(_lookupRepo.GeneralStatuses.Values));
        }

        /// <summary>
        ///
        /// </summary>
        private void BindUpdateControls()
        {
            int selectedIndex = -1;
            if (!_view.IsPostBack)
            {
                if (_capConfigDetailList != null && _capConfigDetailList.Count > 0)
                    selectedIndex = 0;
                if (selectedIndex != -1)
                {
                    _view.BindUpdateControls(_capConfigDetailList[selectedIndex], true);
                }
                else
                {
                    _view.ClearUpdateControls();
                }

                if (_resetDates != null && _resetDates.Rows.Count > 0)
                    PrivateCacheData[_cachedDataLastSelectedResetDateKey] = Convert.ToDateTime(_resetDates.Rows[0]["ResetDate"]);
            }
            else
            {
                DateTime selectedCapResetConfigDate = Convert.ToDateTime(getDateFromDDMMYYYY(_view.SelectedCapResetConfigDate));
                if (PrivateCacheData.ContainsKey(_cachedDataLastSelectedResetDateKey) && Convert.ToDateTime(PrivateCacheData[_cachedDataLastSelectedResetDateKey]) != selectedCapResetConfigDate)
                {
                    PrivateCacheData[_cachedDataLastSelectedResetDateKey] = selectedCapResetConfigDate;
                    if (_capConfigDetailList != null && _capConfigDetailList.Count > 0)
                        _view.BindUpdateControls(_capConfigDetailList[0], true);
                    else
                        _view.ClearUpdateControls();
                }
                else
                {
                    selectedIndex = _view.CapSalesConfigGridSelectedIndex;
                    _view.BindUpdateControls(_capConfigDetailList[selectedIndex], false);
                }
            }
        }

        #region Event Handlers

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnGridSelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = _view.CapSalesConfigGridSelectedIndex;
            if (_capConfigDetailList != null && selectedIndex < _capConfigDetailList.Count)
                _view.BindUpdateControls(_capConfigDetailList[selectedIndex], true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            int selectedIndex = _view.CapSalesConfigGridSelectedIndex;
            if (selectedIndex != -1)
            {
                ICapTypeConfiguration capConfig = _capConfigDetailList[selectedIndex].CapTypeConfiguration;
                ICapTypeConfigurationDetail capConfigDetail = _capConfigDetailList[selectedIndex];
                int capConfigKey = capConfig.Key;

                _view.GetUpdateValuesFromScreen(capConfigDetail, capConfig);

                bool allInactive = true;
                for (int i = 0; i < _capConfigDetailList.Count; i++)
                {
                    if (_capConfigDetailList[i].CapTypeConfiguration.Key == capConfigKey &&
                        _capConfigDetailList[i].GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active))
                    {
                        allInactive = false;
                    }
                }

                if (allInactive == true)
                {
                    capConfig.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Inactive];
                }
                else
                {
                    capConfig.GeneralStatus = _lookupRepo.GeneralStatuses[GeneralStatuses.Active];
                }

                TransactionScope txn = new TransactionScope();
                try
                {
                    _capRepository.SaveCapTypeConfiguration(capConfig);
                    _capRepository.SaveCapTypeConfigurationDetail(capConfigDetail);
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
                BindGrid();
                _view.BindUpdateControls(_capConfigDetailList[selectedIndex], true);
            }
        }

        #endregion Event Handlers
    }
}