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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using Castle.ActiveRecord;
using System.Collections;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class TradeManagementUpdate : TradeManagementBase
    {

        IList<ICapType> _capTypes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public TradeManagementUpdate(ITradeManagement view, SAHLCommonBaseController controller)
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
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _capRepo = RepositoryFactory.GetRepository<ICapRepository>();

            _view.OnTradeTypeSelectedIndexChanged += new KeyChangedEventHandler(_view_OnTradeTypeSelectedIndexChanged);
            _view.OnResetDateSelectedIndexChanged += new KeyChangedEventHandler(_view_OnResetDateSelectedIndexChanged);
            _view.OnTradeGroupingGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnTradeGroupingGridSelectedIndexChanged);

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnSubmitButtonClicked += new EventHandler(_view_OnSubmitButtonClicked);

            _view.CancelButtonVisible = true;
            _view.SubmitButtonVisible = false;
            _view.SubmitButtonText = "Update";
            _view.AddUpdatePanelVisible = false;
            _view.AddUpdatePanelText = "Update Trade";
            _view.TradeCapTypeVisible = false;
            _view.TradeGridPostbackType = GridPostBackType.SingleClick;

            BindTradeTypeDropDown();

            if (!_view.IsPostBack)
            {
                _view.GroupingGridSelectedIndex = -1;

                BindResetDatesDropDown("C");

                if (this.PrivateCacheData.ContainsKey("TradeType"))
                    this.PrivateCacheData["TradeType"] = "C";
                else
                    this.PrivateCacheData.Add("TradeType", "C");
            }
            else
            {
                if (this.PrivateCacheData.ContainsKey("TradeType"))
                {
                    BindResetDatesDropDown(this.PrivateCacheData["TradeType"].ToString());

                    if (this.PrivateCacheData["TradeType"].ToString() == "C")
                    {
                        BindCapTypes();
                        _view.TradeCapTypeVisible = true;
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return;

            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (_view.SelectedReset != -1 && (this.PrivateCacheData.ContainsKey("ResetDateKey") && this.PrivateCacheData.ContainsKey("TradeType")))
                BindTradeGroupingGrid(Convert.ToInt32(this.PrivateCacheData["ResetDateKey"]), this.PrivateCacheData["TradeType"].ToString());

            if (this.PrivateCacheData.ContainsKey("TradeGroupingGridKey") && Convert.ToInt32(this.PrivateCacheData["TradeGroupingGridKey"]) != -1)
            {
                if (Convert.ToInt32(this.PrivateCacheData["TradeGroupingGridKey"]) < _tradeGroupings.Rows.Count)
                {
                    DataRow tradeGroupingRow = _tradeGroupings.Rows[Convert.ToInt32(this.PrivateCacheData["TradeGroupingGridKey"])];
                    BindTradeGrid(Convert.ToInt32(tradeGroupingRow["CapTypeKey"]), _view.SelectedReset, Convert.ToDateTime(tradeGroupingRow["EffectiveDate"]), Convert.ToDateTime(tradeGroupingRow["ClosureDate"]));
                }
                else if (_tradeGroupings.Rows.Count > 0)
                {
                    DataRow tradeGroupingRow = _tradeGroupings.Rows[0];
                    BindTradeGrid(Convert.ToInt32(tradeGroupingRow["CapTypeKey"]), _view.SelectedReset, Convert.ToDateTime(tradeGroupingRow["EffectiveDate"]), Convert.ToDateTime(tradeGroupingRow["ClosureDate"]));
                }

                if (_trades != null && _trades.Count > 0)
                {
                    int tradeSelectedIndex = _view.TradeGridSelectedIndex;
                    if (tradeSelectedIndex != -1 && _trades.Count > tradeSelectedIndex)
                    {
                        PopulateControlsFromSelectedTrade(_trades[tradeSelectedIndex]);
                    }
                    else
                    {
                        _view.ClearUpdateControls();
                    }
                }
                else
                {
                    _view.ClearUpdateControls();
                }
            }
            else
            {
                _view.ClearUpdateControls();
            }
        }

        protected void _view_OnTradeTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_view.SelectedTradeType))
            {
                if (this.PrivateCacheData.ContainsKey("TradeType"))
                {
                    if (this.PrivateCacheData["TradeType"].ToString() != _view.SelectedTradeType)
                    {
                        if (this.PrivateCacheData.ContainsKey("ResetDateKey"))
                            this.PrivateCacheData.Remove("ResetDateKey");

                        if (this.PrivateCacheData.ContainsKey("TradeGroupingGridKey"))
                            this.PrivateCacheData.Remove("TradeGroupingGridKey");

                        BindResetDatesDropDown(_view.SelectedTradeType);
                        _view.GroupingGridSelectedIndex = -1;
                    }
                    this.PrivateCacheData["TradeType"] = _view.SelectedTradeType;
                }
                else
                {
                    this.PrivateCacheData.Add("TradeType", _view.SelectedTradeType);
                }

                if (this.PrivateCacheData["TradeType"].ToString() == "C")
                {
                    BindCapTypes();
                    _view.TradeCapTypeVisible = true;
                }
            }
        }

        protected void _view_OnResetDateSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (_view.SelectedReset != -1)
            {
                _view.AddUpdatePanelVisible = true;
                _view.SubmitButtonVisible = true;
                //
                if (this.PrivateCacheData.ContainsKey("ResetDateKey"))
                {
                    if (Convert.ToInt32(this.PrivateCacheData["ResetDateKey"]) != _view.SelectedReset)
                    {
                        if (this.PrivateCacheData.ContainsKey("TradeGroupingGridKey"))
                            this.PrivateCacheData.Remove("TradeGroupingGridKey");

                        _view.GroupingGridSelectedIndex = -1;
                    }
                    this.PrivateCacheData["ResetDateKey"] = _view.SelectedReset;
                }
                else
                {
                    this.PrivateCacheData.Add("ResetDateKey", _view.SelectedReset);
                    _view.GroupingGridSelectedIndex = -1;
                }
            }
        }

        protected void _view_OnTradeGroupingGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            if (Convert.ToInt32(e.Key) != -1)
            {
                if (this.PrivateCacheData.ContainsKey("TradeGroupingGridKey"))
                    this.PrivateCacheData["TradeGroupingGridKey"] = Convert.ToInt32(e.Key);
                else
                    this.PrivateCacheData.Add("TradeGroupingGridKey", Convert.ToInt32(e.Key));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeType"></param>
        private void BindResetDatesDropDown(string tradeType)
        {
            _resetDates = _capRepo.GetResetDatesForAddingByTradeType(tradeType);
            _view.BindResetDatesDropDown(_resetDates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="tradeType"></param>
        private void BindTradeGroupingGrid(int resetConfigurationKey, string tradeType)
        {
            _tradeGroupings = _capRepo.GetActiveTradeGroupingsByResetConfigurationKey(resetConfigurationKey, tradeType);
            _view.BindTradeGroupingGrid(_tradeGroupings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capTypeKey"></param>
        /// <param name="resetConfigurationKey"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="closureDate"></param>
        private void BindTradeGrid(int capTypeKey, int resetConfigurationKey, DateTime effectiveDate, DateTime closureDate)
        {
            _trades = _capRepo.GetTradeByGrouping(capTypeKey, resetConfigurationKey, effectiveDate, closureDate);
            _view.BindTradeGrid( _trades);
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindCapTypes()
        {
            _capTypes = _capRepo.GetCapTypesForTrade();
            _view.BindCapTypes(_capTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedTrade"></param>
        private void PopulateControlsFromSelectedTrade(ITrade selectedTrade)
        {
            _view.PopulateControlsFromSelectedTrade(selectedTrade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("TradeManagementDisplay");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (_view.SelectedReset != -1)
            {
                IResetConfiguration resetConfig = _capRepo.GetResetConfigurationByKey(_view.SelectedReset);
                ITrade tradeRecord = null;

                int tradeSelectedIndex = _view.TradeGridSelectedIndex;
                if (tradeSelectedIndex != -1)
                {
                    if (_trades == null)
                    {
                        _tradeGroupings = _capRepo.GetActiveTradeGroupingsByResetConfigurationKey(Convert.ToInt32(this.PrivateCacheData["ResetDateKey"]), this.PrivateCacheData["TradeType"].ToString());
                        DataRow tradeGroupingRow = _tradeGroupings.Rows[Convert.ToInt32(this.PrivateCacheData["TradeGroupingGridKey"])];
                        _trades = _capRepo.GetTradeByGrouping(Convert.ToInt32(tradeGroupingRow["CapTypeKey"]), _view.SelectedReset, Convert.ToDateTime(tradeGroupingRow["EffectiveDate"]), Convert.ToDateTime(tradeGroupingRow["ClosureDate"]));
                    }
                    tradeRecord = _trades[tradeSelectedIndex];
                }

                _view.GetAddValuesFromScreen(tradeRecord, _capTypes, resetConfig);

                TransactionScope txn = new TransactionScope();
                try
                {
                    _capRepo.SaveTrade(tradeRecord);
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

                if (_view.Messages.Count == 0)
                    Navigator.Navigate("Submit");
            }
        }
    }
}
