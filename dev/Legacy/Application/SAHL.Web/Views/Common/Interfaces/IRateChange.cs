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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Interface for RateChange
    /// </summary>
    public interface IRateChange : IViewBase
    {

        /// <summary>
        /// Enable/Disable the calculate button
        /// </summary>
        bool SetAbilityofCalculateButton { set;}
        /// <summary>
        /// Set visiblity of term controls for interest only
        /// </summary>
        bool SetTermControlVisibilityForInterestOnly { set;}

        bool SetTermControlVisibilityForTermComment { set;}
        /// <summary>
        /// Set ability of submit button
        /// </summary>
        bool SetAbilityofSubmitButton { set;}

        /// <summary>
        /// set visibility of rate controls for Varifix
        /// </summary>
        bool SetRateControlVisibilityForVarifix { set;}

        /// <summary>
        /// Set visiblity of term controls for Varifix
        /// </summary>
        bool SetTermControlVisibilityForVarifix { set;}

        /// <summary>
        /// Sets visibility of term controls
        /// </summary>
        bool SetTermControlsVisibility { set;}

        /// <summary>
        /// Gets or Sets the Maximum loan term in months
        /// </summary>
        int MaximumTerm { get; set;}

        /// <summary>
        /// Sets visibility of rate controls
        /// </summary>
        bool SetRatesControlVisibility { set;}
        /// <summary>
        /// Sets visibility of rate controls - Interest Only
        /// </summary>
        bool SetRateAmortisationControlVisibility { set;}

        /// <summary>
        /// Sets visibility of PTI controls
        /// </summary>
        bool SetPTIVisibility { set;}

        /// <summary>
        /// Bind Grid for RateChange TermChange presenter where loan is not interest only
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGridTermNotInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans);

        /// <summary>
        /// Bind Grid for RateChange Term Change presenter where loan is interest only
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGridTermInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans);

        /// <summary>
        /// Bind Grid for RateChangeInstallmentChange presenter where loan is not interest only
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGridInstallmentNotInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans);

        /// <summary>
        /// Bind Grid for RateChangeInstallmentChange presenter where loan is interest only
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGridInstallmentInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans);

        /// <summary>
        /// Bind Grid for RateChange - Change Rate presenter where loan is interest only
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGridRateChangeInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans);

        /// <summary>
        /// Bind Grid for RateChange - Change Rate presenter where loan is not interest only
        /// </summary>
        /// <param name="lstMortgageLoans"></param>
        void BindGridRateChangeNotInterestOnly(IEventList<IMortgageLoan> lstMortgageLoans);

        /// <summary>
        /// Set text and access keys on Submit button
        /// </summary>
        /// <param name="action"></param>
        /// <param name="accesskey"></param>
        void SetSubmitButtonText(string action, string accesskey);
        /// <summary>
        /// get value of logged in user
        /// </summary>
        string GetLoggedOnUser { get; }

        string MemoComments { get; }

        /// <summary>
        /// Get Term Captured on screen
        /// </summary>
        int CapturedTerm { get; set;}

        /// <summary>
        /// Calculate the terms
        /// </summary>
        void CalculateTerms();

        /// <summary>
        /// Update button clicked
        /// </summary>
        event KeyChangedEventHandler SubmitButtonClicked;
        /// <summary>
        /// Cancel button Clicked
        /// </summary>
        event EventHandler CancelButtonClicked;
        /// <summary>
        /// populate link rates drop down
        /// </summary>
        /// <param name="margin"></param>
        void PopulateLinkRates(IEventList<IMargin> margin);
    }
}