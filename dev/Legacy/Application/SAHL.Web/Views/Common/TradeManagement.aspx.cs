using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common
{
    public partial class TradeManagement : SAHLCommonBaseView, ITradeManagement
    {
        private IResetConfiguration _resetConfiguration;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool CancelButtonVisible
        {
            set { CancelButton.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonVisible
        {
            set { SubmitButton.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set { SubmitButton.Enabled = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SubmitButtonText
        {
            set { SubmitButton.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AddUpdatePanelVisible
        {
            set { AddUpdatePanel.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AddUpdatePanelText
        {
            set { AddUpdatePanel.GroupingText = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TradeCapTypeVisible
        {
            set { CapTypeRow.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedTradeType
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlTradeType.UniqueID]))
                    return Request.Form[ddlTradeType.UniqueID];
                else
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedReset
        {
            /*get
            {
                if (!string.IsNullOrEmpty(Request.Form[ddlResetDate.UniqueID]) && Request.Form[ddlResetDate.UniqueID] != "-select-")
                    return Convert.ToInt32( Request.Form[ddlResetDate.UniqueID] );
                else
                    return -1;            
            }*/

            get
            {
                if (!string.IsNullOrEmpty(ddlResetDate.SelectedValue) && ddlResetDate.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlResetDate.SelectedValue);
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GroupingGridSelectedIndex
        {
            get
            {
                return TradeGroupingGrid.SelectedIndex;
            }
            set
            {
                TradeGroupingGrid.SelectedIndex = value;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public int TradeGridSelectedIndex
        {
            get
            {
                return TradeGrid.SelectedIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType TradeGridPostbackType
        {
            set { TradeGrid.PostBackType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IResetConfiguration ResetConfigurationValue
        {
            set { _resetConfiguration = value; }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnTradeGroupingGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnTradeGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnTradeTypeSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnResetDateSelectedIndexChanged;

        #endregion

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
            {
                OnCancelButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TradeGroupingGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(TradeGroupingGrid.SelectedIndex);
            if (OnTradeGroupingGridSelectedIndexChanged != null)
            {
                OnTradeGroupingGridSelectedIndexChanged(sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TradeGroupingGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TradeGrid_SelectedIndexChanged (object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(TradeGrid.SelectedIndex);
            if (OnTradeGridSelectedIndexChanged != null)
            {
                OnTradeGridSelectedIndexChanged(sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TradeGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #endregion 

        #region ITradeManagement methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeList"></param>
        public void BindTradeTypeDropDown(IDictionary<string, string> tradeList)
        {
            ddlTradeType.DataSource = tradeList;
            ddlTradeType.DataValueField = "Key";
            ddlTradeType.DataTextField = "Value";
            ddlTradeType.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDates"></param>
        public void BindResetDatesDropDown(DataTable resetDates)
        {
            ddlResetDate.DataSource = resetDates;
            ddlResetDate.DataValueField = "ResetConfigurationKey";
            ddlResetDate.DataTextField = "Description";
            ddlResetDate.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capTypes"></param>
        public void BindCapTypes(IList<ICapType> capTypes)
        {
            TradeCapType.DataSource = capTypes;
            TradeCapType.DataValueField = "Key";
            TradeCapType.DataTextField = "Description";
            TradeCapType.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeGroupings"></param>
        public void BindTradeGroupingGrid(DataTable tradeGroupings)
        {
            TradeGroupingGrid.DataSource = tradeGroupings;
            TradeGroupingGrid.AddGridBoundColumn("EffectiveDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Effective Date", false, Unit.Percentage(20), HorizontalAlign.Left, true);
            TradeGroupingGrid.AddGridBoundColumn("ClosureDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Closure Date", false, Unit.Percentage(20), HorizontalAlign.Left, true);
            TradeGroupingGrid.AddGridBoundColumn("TradeType", "Trade Type Description", Unit.Percentage(40), HorizontalAlign.Left, true);
            TradeGroupingGrid.AddGridBoundColumn("Status", "Status", Unit.Percentage(20), HorizontalAlign.Left, true);
            TradeGroupingGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trades"></param>
        public void BindTradeGrid(IList<ITrade> trades)
        {
            TradeGrid.DataSource = trades;
            TradeGrid.AddGridBoundColumn("Company", "Company", Unit.Percentage(15), HorizontalAlign.Left, true);
            TradeGrid.AddGridBoundColumn("TradeDate", SAHL.Common.Constants.DateFormat, GridFormatType.GridDate, "Trade Date", false, Unit.Percentage(15), HorizontalAlign.Left, true);
            TradeGrid.AddGridBoundColumn("StrikeRate", "Strike Rate", Unit.Percentage(15), HorizontalAlign.Left, true);
            TradeGrid.AddGridBoundColumn("Premium", "Premium", Unit.Percentage(18), HorizontalAlign.Left, true);
            TradeGrid.AddGridBoundColumn("TradeBalance", "Trade Balance", Unit.Percentage(18), HorizontalAlign.Left, true);
            TradeGrid.AddGridBoundColumn("CapBalance", "Cap Balance", Unit.Percentage(18), HorizontalAlign.Left, true);
            TradeGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeRecord"></param>
        /// <param name="capTypes"></param>
        /// <param name="resetConfig"></param>
        public void GetAddValuesFromScreen(ITrade tradeRecord, IList<ICapType> capTypes, IResetConfiguration resetConfig)
        {
            
            if (!string.IsNullOrEmpty(Request.Form[TradeCapType.UniqueID]) && Request.Form[TradeCapType.UniqueID] != "-select-")
            {
                if (capTypes!=null && capTypes.Count > 0)
                {
                    for (int i = 0 ; i < capTypes.Count ; i++)
                    {
                        if (Convert.ToInt32(Request.Form[TradeCapType.UniqueID]) == capTypes[i].Key)
                        {
                            tradeRecord.CapType = capTypes[i];
                            break;
                        }
                    }
                }
            }

           

            if (!string.IsNullOrEmpty(Request.Form[TradeCompany.UniqueID]))
            {
                tradeRecord.Company = Request.Form[TradeCompany.UniqueID];
            }

            DateTime startDate = DateTime.Today;
            if (TradeEffectiveDate.Date.HasValue)
            {
                tradeRecord.StartDate = TradeEffectiveDate.Date.Value;
                startDate = TradeEffectiveDate.Date.Value;
            }

            int term = -1;
            if (!string.IsNullOrEmpty(Request.Form[TradeTerm.UniqueID]))
            {
                term = Convert.ToInt32(Request.Form[TradeTerm.UniqueID]);
            }

            if (term != -1)
            {
                DateTime endate = startDate.AddMonths(term);
                tradeRecord.EndDate = endate;
            }
            else
            {
                tradeRecord.EndDate = startDate;
            }

            if (!string.IsNullOrEmpty(Request.Form[TradePremium.UniqueID]))
            {
                tradeRecord.Premium = Convert.ToDouble(Request.Form[TradePremium.UniqueID]);
            }

            if (resetConfig != null)
            {
                tradeRecord.ResetConfiguration = resetConfig;
            }            

            if (!string.IsNullOrEmpty(Request[TradeStrikeRate.UniqueID]))
            {
                if (Convert.ToDouble(Request.Form[TradeStrikeRate.UniqueID]) != 0)
                    tradeRecord.StrikeRate = Convert.ToDouble(Request.Form[TradeStrikeRate.UniqueID]) / 100;
                else
                    tradeRecord.StrikeRate = 0;
            }

            if (!string.IsNullOrEmpty(Request[TradeBalance.UniqueID]))
            {
                tradeRecord.TradeBalance = Convert.ToDouble(Request.Form[TradeBalance.UniqueID]);
            }
                    
        
            if (TradeDate.Date.HasValue)
            {
                if (tradeRecord.TradeDate != TradeDate.Date.Value)
                {
                    tradeRecord.TradeDate = TradeDate.Date.Value;
                }
            }
            
            if (!string.IsNullOrEmpty(Request[ddlTradeType.UniqueID]))
            {
                tradeRecord.TradeType = Request[ddlTradeType.UniqueID];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedTrade"></param>
        public void PopulateControlsFromSelectedTrade(ITrade selectedTrade)
        {
            if (selectedTrade.CapType != null)
            {
                TradeCapType.SelectedValue = selectedTrade.CapType.Key.ToString();
            }

            if (selectedTrade.TradeDate.Date != null)
            {
                TradeCapType.SelectedValue = selectedTrade.CapType.Key.ToString();
            }
            TradeCompany.Text = selectedTrade.Company;
            TradeEffectiveDate.Date = (DateTime?)selectedTrade.StartDate;
            TradeDate.Date = (DateTime?)selectedTrade.TradeDate;
            
            TradeClosureDate.Text = selectedTrade.EndDate.ToString(SAHL.Common.Constants.DateFormat);
          
            TradePremium.Text = Convert.ToDouble(selectedTrade.Premium).ToString(SAHL.Common.Constants.NumberFormat3Decimal);
            TradeStrikeRate.Text = Convert.ToDouble(selectedTrade.StrikeRate * 100).ToString(SAHL.Common.Constants.NumberFormat3Decimal);
            TradeBalance.Text = Convert.ToDouble(selectedTrade.TradeBalance).ToString(SAHL.Common.Constants.NumberFormat);

            int iStart = (selectedTrade.StartDate.Year * 12) + selectedTrade.StartDate.Month;
            int iEnd = (selectedTrade.EndDate.Year * 12) + selectedTrade.EndDate.Month;
            int term = iEnd - iStart;
            TradeTerm.Text = term.ToString();
        }

        public void ClearUpdateControls()
        {
            TradeCompany.Text = "";
            TradeEffectiveDate.Date = null;
            TradeDate.Date =  null;
            TradeClosureDate.Text = "";
            TradeCapType.SelectedValue = "-select-";
            TradePremium.Text = "";
            TradeStrikeRate.Text = "";
            TradeBalance.Text = "";
            TradeTerm.Text = "";
        }

        public void SetDefaultValues()
        {
            TradeDate.Date = DateTime.Today;
            TradePremium.Text = "0.00";
            TradeStrikeRate.Text = "0.000";
            TradeTerm.Text = "0";
            TradePremium.Text = "0.00";
            TradeBalance.Text = "0.00";
            TradeEffectiveDate.Date = _resetConfiguration.ActionDate;
        }

        #endregion

        public void ddlTradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnTradeTypeSelectedIndexChanged != null)
                OnTradeTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlTradeType.SelectedIndex));
        }

        public void ddlResetDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnResetDateSelectedIndexChanged != null)
                OnResetDateSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlResetDate.SelectedIndex));
        }

    }
}
