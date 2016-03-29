using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ITradeManagement : IViewBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool CancelButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        string SubmitButtonText { set;}

        /// <summary>
        /// 
        /// </summary>
        bool AddUpdatePanelVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        string AddUpdatePanelText { set;}

        /// <summary>
        /// 
        /// </summary>
        bool TradeCapTypeVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedTradeType { get;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedReset { get;}

        /// <summary>
        /// 
        /// </summary>
        int GroupingGridSelectedIndex { get;set;}

        /// <summary>
        /// 
        /// </summary>
        int TradeGridSelectedIndex { get;}

        /// <summary>
        /// 
        /// </summary>
        GridPostBackType TradeGridPostbackType { set;}

        /// <summary>
        /// 
        /// </summary>
        IResetConfiguration ResetConfigurationValue { set;}

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnTradeGroupingGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnTradeGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnTradeTypeSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnResetDateSelectedIndexChanged;

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeList"></param>
        void BindTradeTypeDropDown(IDictionary<string, string> tradeList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDates"></param>
        void BindResetDatesDropDown(DataTable resetDates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeGroupings"></param>
        void BindTradeGroupingGrid(DataTable tradeGroupings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trades"></param>
        void BindTradeGrid(IList<ITrade> trades);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capTypes"></param>
        void BindCapTypes(IList<ICapType> capTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeRecord"></param>
        /// <param name="capTypes"></param>
        /// <param name="resetConfig"></param>
        void GetAddValuesFromScreen(ITrade tradeRecord, IList<ICapType> capTypes, IResetConfiguration resetConfig);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedTrade"></param>
        void PopulateControlsFromSelectedTrade(ITrade selectedTrade);

        /// <summary>
        /// 
        /// </summary>
        void ClearUpdateControls();

        /// <summary>
        /// 
        /// </summary>
        void SetDefaultValues();

    }
}
