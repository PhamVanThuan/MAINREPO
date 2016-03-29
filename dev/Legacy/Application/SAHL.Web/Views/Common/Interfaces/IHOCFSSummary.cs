using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// HOCFSSummary Base presenter
    /// </summary>
    public interface IHOCFSSummary : IViewBase
    {
        /// <summary>
        /// Cancel Button Event handler
        /// </summary>
        event EventHandler onCancelButtonClicked;
        /// <summary>
        /// Update button event handler
        /// </summary>
        event EventHandler onUpdateHOCButtonClicked;
        /// <summary>
        /// Event handler of Selected Index Change on properties grid
        /// </summary>
        event KeyChangedEventHandler OnPropertiesGridSelectedIndexChanged;

        /// <summary>
        /// Event Handler of Selected Index Changed of Drop Down Lists
        /// </summary>
        event KeyChangedEventHandler OnddlHOCInsurerSelectedIndexChanged;

        //event EventHandler OnddlHOCInsurerSelectedIndexChanged;
        # region Properties

        /// <summary>
        /// Sets whether Read Only HOC details are visible
        /// </summary>
        bool HOCDetailsDisplay { set; }

        /// <summary>
        /// Sets whether the HOC Details input form is visible.
        /// </summary>
        bool HOCDetailsUpdate { set; }

        /// <summary>
        /// Sets whether Premium Panel is visible
        /// </summary>
        bool HOCPremiumPanelVisible { set; }

        /// <summary>
        /// Sets whether HOC Update Button is Visible
        /// </summary>
        bool HOCUpdateButtonVisible { set; }

        /// <summary>
        /// Sets whether HOC Cancel Button is Visible
        /// </summary>
        bool HOCCancelButtonVisible { set; }

        /// <summary>
        /// Sets whether HOC Updateable controls are Visible
        /// </summary>
        bool HOCDetailsUpdateDisplay { set; }

        /// <summary>
        /// Sets value of HOC Insurer when another control has changed
        /// </summary>
        int HOCInsurerValueChange { set; }

        /// <summary>
        /// Gets the key of the Status type that is selected on the view.
        /// </summary>
        string SelectedHOCStatusValue { get; }

        /// <summary>
        /// Gets the key of the HOC Insurer Key that is selected on the view.
        /// </summary>
        string SelectedHOCInsurerValue { get; set; }

        /// <summary>
        ///
        /// </summary>
        string SetUpdateButtonText { set; }

        /// <summary>
        ///
        /// </summary>
        IValuation valuation { set; }

        /// <summary>
        ///
        /// </summary>
        double TotalHOCSumInsured { set; }

        /// <summary>
        ///
        /// </summary>
        bool UseDefaultValue { set; }

        /// <summary>
        ///
        /// </summary>
        int SetHOCInsurerKey { set; }

        /// <summary>
        ///
        /// </summary>
        bool SetBindValue { set; }

        /// <summary>
        ///
        /// </summary>
        bool IsHOCFSSummary { set; }

        /// <summary>
        ///
        /// </summary>
        bool UseHOCHistoryDetail { set; }

        /// <summary>
        ///
        /// </summary>
        IHOCHistoryDetail HOCHistoryDetail { set; }

        /// <summary>
        ///
        /// </summary>
        bool ShowProRataPremium { set; }

        # endregion

        #region Methods

        /// <summary>
        /// Binds HOC Details Summary to HOC View
        /// </summary>
        /// <param name="HOC"></param>
        void BindHOCSummaryData(IHOC HOC);

        /// <summary>
        /// Bind All Look Ups on View
        /// </summary>
        /// <param name="hocInsurer"></param>
        /// <param name="status"></param>
        /// <param name="Subsidence"></param>
        /// <param name="Construction"></param>
        void BindHOCLookUpControls(IEventList<IHOCInsurer> hocInsurer, IDictionary<string, string> status, IDictionary<string, string> Subsidence, IDictionary<string, string> Construction);

        /// <summary>
        /// Calculate HOC premiums
        /// </summary>
        /// <param name="action"></param>
        void ShowCalculatedHOCPremiums(string action);

        /// <summary>
        /// Returns updated HOC entity populated with the values entered onto the HOC form.
        /// </summary>
        /// <returns></returns>
        IHOC GetCapturedHOC(IHOC hoc);

        /// <summary>
        /// Reset HOCPolicy Number - called when HOC Insurer is changed
        /// </summary>
        void RemoveHOCPolicyNumber();

        /// <summary>
        /// Bind properties Grid
        /// </summary>
        /// <param name="lstPropertyAddresses"></param>
        void BindPropertiesGrid(IEventList<IAddress> lstPropertyAddresses);

        /// <summary>
        /// Set Controls for Add
        /// </summary>
        void SetControlsForAdd();

        /// <summary>
        /// Bind HOC Insurer
        /// </summary>
        /// <param name="hocinsurer"></param>
        void BindHOCInsurer(IEventList<IHOCInsurer> hocinsurer);

        /// <summary>
        /// Set Page Default values for Add Mode
        /// </summary>
        /// <param name="hocstatus"></param>
        /// <param name="hocsubsidence"></param>
        /// <param name="hocroof"></param>
        /// <param name="hocconstruction"></param>
        void SetDefaultValuesForAdd(IEventList<IHOCStatus> hocstatus, IEventList<IHOCSubsidence> hocsubsidence, IEventList<IHOCRoof> hocroof, IEventList<IHOCConstruction> hocconstruction);

        /// <summary>
        /// Get Captured HOC Record for Add
        /// </summary>
        /// <param name="valuation"></param>
        /// <param name="HOC"></param>
        /// <returns></returns>
        IHOC GetCapturedHOCRecordForAdd(IValuation valuation, IHOC HOC);

        /// <summary>
        ///
        /// </summary>
        /// <param name="InfoMessage"></param>
        void ShowDefaultView(string InfoMessage);

        /// <summary>
        ///
        /// </summary>
        void OnLinePremiumCalc();

        #endregion Methods
    }
}