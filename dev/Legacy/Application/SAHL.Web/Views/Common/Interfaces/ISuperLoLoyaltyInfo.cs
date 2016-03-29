using System;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// Interface for SuperLoLoyaltyInfo View
    /// </summary>
    public interface ISuperLoLoyaltyInfo : IViewBase
    {
        /// <summary>
        /// Bind Loyalty Benefit Information
        /// </summary>
        /// <param name="mlInfoOpen"></param>
        void BindLoyaltyBenefitInfo(ISuperLo superLo);

        /// <summary>
        /// Bind Loyalty Benefit Payment Grid
        /// </summary>
        /// <param name="dtLoanTransactions"></param>
        void BindLoyaltyBenefitPaymentGrid(DataTable dtLoanTransactions);

        /// <summary>
        /// Cancel button clicked
        /// </summary>
        event KeyChangedEventHandler CancelButtonClicked;

        /// <summary>
        /// Super Lo Opt Out button clicked
        /// </summary>
        event KeyChangedEventHandler SuperLoOptOutButtonClicked;

        /// <summary>
        /// Event handler for Update Button being Clicked
        /// </summary>
        event KeyChangedEventHandler UpdateButtonClicked;

        /// <summary>
        /// Set the value of the Exclude from Opt Out Check Box
        /// </summary>
        bool ExcludeFromOptOut { get; set; }

        /// <summary>
        /// Set the value of the Exclusion Date
        /// </summary>
        DateTime? ExclusionDate { get; set; }

        /// <summary>
        /// Set the value of the Exclusion Reason
        /// </summary>
        string ExclusionReason { get; set; }

        /// <summary>
        /// This Sets the threshold management edit state
        /// </summary>
        bool SetThresholdManagementEditable { set; }

        /// <summary>
        /// This Sets the visibility of the Super Lo Opt out button
        /// </summary>
        bool SuperLoOptOutButtonVisible { set; }

        /// <summary>
        /// This Sets the visibility of the Update Threshold button
        /// </summary>
        bool UpdateThresholdButtonVisible { set; }

        /// <summary>
        /// This Sets the visibility of the Cancel button
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// This Sets the visibility of the Update Threshold button
        /// </summary>
        void AddOptOutConfirmation();

        /// <summary>
        /// This Sets the visibility of the Threshold Management panel
        /// </summary>
        bool ThresholdManagementPanelVisible { set; }

        /// <summary>
        /// This Sets the visibility of the Loyalty Benefit panel
        /// </summary>
        bool LoyaltyBenefitPanelVisible { set; }

        /// <summary>
        /// This Sets the visibility of the Prepayment Thresholds Panel
        /// </summary>
        bool PrepaymentThresholdsPanelVisible { set; }

        /// <summary>
        /// This Sets the visibility of the Loyalty Payment Grid
        /// </summary>
        bool LoyaltyPaymentGridVisible { set; }

        /// <summary>
        ///
        /// </summary>
        bool CreateSpaceTable { set; }
    }
}