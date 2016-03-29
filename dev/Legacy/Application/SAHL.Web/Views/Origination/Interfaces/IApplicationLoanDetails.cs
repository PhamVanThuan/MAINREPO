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
using SAHL.Web.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Origination.Interfaces
{
	public interface IApplicationLoanDetails : IViewBase
	{
		#region interface methods
		/// <summary>
		/// Binds a list of products.
		/// </summary>
		/// <param name="Products"></param>
		/// <param name="Default"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
		void BindProducts(IDictionary<string, string> Products, string Default);

		#region Old Dropdowns
		/// <summary>
		/// Binds a list of available SPVs.
		/// </summary>
		/// <param name="SPVs"></param>
		/// <param name="Default"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
		void BindSPVs(IDictionary<string, string> SPVs, string Default);

		/// <summary>
		/// Get the selected SPV from the dropdown
		/// </summary>
		string SelectedSPV { get; }

		///// <summary>
		///// Binds a list of Loan Categories.
		///// </summary>
		///// <param name="Categories"></param>
		///// <param name="Default"></param>
		//void BindCategories(IDictionary<string, string> Categories, string Default);
		#endregion

		#region not used
		///// <summary>
		///// Binds a list of Link Rates. (Binds to the ff controls: LoanDetails, VarifixDetails)
		///// </summary>
		///// <param name="LinkRates"></param>
		///// <param name="Default"></param>
		//void BindLinkRates(IDictionary<string, string> LinkRates, string Default);
		#endregion


		void SetInterestOnly(IApplication app);

		/// <summary>
		/// Binds all the application details to the display controls.
		/// </summary>
		/// <param name="Application"></param>
		void BindApplicationDetails(IApplication Application);

		/// <summary>
		/// Binds the readonly Product Description to the appropriate display field
		/// </summary>
		/// <param name="Product"></param>
		void BindProduct(string Product);

		/// <summary>
		/// Populates an IApplication object with the Application details on the screen. Usually used in cunjuction with the save.
		/// </summary>
		/// <param name="ApplicationMortgageLoan"></param>
		void GetApplicationDetails(IApplicationMortgageLoan ApplicationMortgageLoan);

		/// <summary>
		/// Sets the caption for appear on the submit button caption.
		/// </summary>
		/// <param name="SubmitCaption"></param>
		void SetSubmitCaption(string SubmitCaption);
		#endregion

		#region interface properties

		// Is the box checked
		bool QuickPayFeeChecked { get; }

		/// <summary>
		/// Should only be available to the Credit Exception User at the Credit State
		/// </summary>
		bool IsQuickPayFeeReadOnly { get; set; }

		int SetEdgeTerm { set; }

		/// <summary>
		/// Sets whether the Edge Home Loan Details section (controls) is visible. Default: false.
		/// </summary>
		bool IsEdgeVisible { set; get; }

		/// <summary>
		/// Sets whether the Loan Details section (controls) is visible. Default: false.
		/// </summary>
		bool IsLoanDetailsVisible { set; }

		/// <summary>
		/// Sets whether the Varifix Details section (controls) is visible. Default: false.
		/// </summary>
		bool IsVarifixDetailsVisible { set; }

		/// <summary>
		/// Sets whether the SuperLo section (controls) is visible. Default: false.
		/// </summary>
		bool IsSuperLoInfoVisible { set; }

		/// <summary>
		/// Sets whether the Quick Cash section (controls) is visible. Default: false.
		/// </summary>
		bool IsQuickCashVisible { set; }

		/// <summary>
		/// Sets whether the Quick Cash section (controls) is visible. Default: false.
		/// </summary>
		bool CanShowQuickCash { get; set; }

		/// <summary>
		/// Sets whether the view is displayed in readonly mode.
		/// </summary>
		bool IsReadOnly { set; }

		///// <summary>
		///// Set this property after setting the isReadOnly property.
		///// Sets whether the Category is readonly.
		///// </summary>
		//bool IsCategoryReadOnly { set; }

		/// <summary>
		/// Set this property after setting the isReadOnly property.
		/// Sets whether the Link Rate is readonly.
		/// </summary>
		bool IsLinkRateReadOnly { set; }

		/// <summary>
		/// Set this property after setting the isReadOnly property.
		/// Sets whether the Override Fees is readonly.
		/// </summary>
		bool IsOverrideFeesReadOnly { set; }

		/// <summary>
		/// Set this property after setting the isReadOnly property.
		/// Sets whether the Cancellation Fees is readonly.
		/// </summary>
		bool IsCancellationFeesReadOnly { set; }

		/// <summary>
		/// Set this property after setting the isReadOnly property.
		/// Sets whether the Quick Cash Details is readonly.
		/// </summary>
		bool IsQuickCashDetailsReadOnly { set; }

		/// <summary>
		/// Sets whether the Quick Cash Details can be edited,
		/// </summary>
		bool HasQuickCashDeclineReasons { get; set; }

		/// <summary>
		/// 
		/// </summary>
		bool HasQuickCashValue
		{
			get;
			set;
		}

		bool EnforceQuickCash { get; set; }

		///// <summary>
		///// Sets whether or not the Interest Only data may be modified.
		///// Applies to the Loan Details control.
		///// </summary>
		//bool IsInterestOnlyVisible { set; }

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the SPV data may be modified.
		/// </summary>
		bool IsSPVReadOnly { set; }

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the Discount data may be modified.
		/// </summary>
		bool IsDiscountReadonly { get; set; }

		/// <summary>
		/// Get or set the Discount value from the Loan Details panel.
		/// </summary>
		double Discount
		{
			get;
			//set; 
		}

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the Bond to Register can be edited,
		/// and allows the BondToRegister to be the same as the Loan Amount
		/// </summary>
		bool IsBondToRegisterExceptionAction { get; set; }

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the Cash Out data may be modified.
		/// </summary>
		bool IsCashOutReadOnly { set; }

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the Cash Deposit data may be modified.
		/// </summary>
		bool IsCashDepositReadOnly { set; }

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the property value may be modified.
		/// </summary>
		bool IsPropertyValueReadOnly { set; }

		/// <summary>
		/// Set this property after setting IsReadOnly.
		/// Sets whether or not the property value may be modified.
		/// </summary>
		bool IsPropertyValueValuation { set; }

		/// <summary>
		/// Panel used for Switch application types
		/// </summary>
		bool IsSwitchPanelVisible { set; }

		/// <summary>
		/// Panel used for New Purchase application types
		/// </summary>
		bool IsNewPurchasePanelVisible { set; }

		/// <summary>
		/// Panel used for Refinance application types
		/// </summary>
		bool IsRefinancePanelVisible { set; }

		/// <summary>
		/// Set this property after setting the isReadOnly property.
		/// Sets whether the ButtonsPanel is visible
		/// </summary>
		bool IsButtonsPanelReadonly { set; }

		/// <summary>
		/// Sets whether the CancellationFee is visible
		/// </summary>
		bool IsCancellationFeeVisible { set; }

		/// <summary>
		/// Sets whether the Recalculate button is visible
		/// </summary>
		bool ShowCalculateButton
		{ set; }

		/// <summary>
		/// Determines whether a revision should be created based on the type of credit decision
		/// ApproveWithPricingChanges and DeclineWithOffer create revisions because changes are 
		/// being made to the application that was submitted to Credit.
		/// </summary>
		bool CreateRevision { get; set; }

		/// <summary>
		/// If a non new business application is loaded, hide all controls on the page
		/// </summary>
		bool HideAll { get; set; }

		/// <summary>
		/// Is Submit Button Enabled
		/// </summary>
		bool IsSubmitButtonEnabled
		{
			set;
		}

		/// <summary>
		/// Is Recalculate Button Enabled
		/// </summary>
		bool IsRecalculateButtonEnabled
		{
			set;
		}
		#endregion

		#region interface events

		/// <summary>
		/// 
		/// </summary>
		event KeyChangedEventHandler QuickPayFeeCheckedChange;

		/// <summary>
		/// Event raised when varifix details need to be calculated.
		/// </summary>
		event EventHandler OnRecalculateApplication;

		/// <summary>
		/// Event raised when the quick cash decline button is clicked.
		/// </summary>
		event EventHandler OnQCDeclineReasons;

		/// <summary>
		/// Event raised when a varifix maximum needs to be suggested.
		/// </summary>
		event EventHandler OnCalculateMaximumFixedPercentage;

		/// <summary>
		/// Event raised when the Cancel button is clicked.
		/// </summary>
		event EventHandler OnCancelClicked;

		/// <summary>
		/// Event raised when the Update button is clicked.
		/// </summary>
		event EventHandler OnUpdateClicked;

		/// <summary>
		/// Event raised when the Application Options change.
		/// </summary>
		event KeyChangedEventHandler ApplicationOptionsChange;

		/// <summary>
		/// Event raised when the override fees option is changed.
		/// </summary>
		event KeyChangedEventHandler OverrideFeesChange;

		/// <summary>
		/// Event raised when the capitilize fees option is changed.
		/// </summary>
        event KeyChangedEventHandler CapitaliseFeesChange;

        /// <summary>
        /// Event raised when the capitilize initiation fees option is changed.
        /// </summary>
        event KeyChangedEventHandler CapitaliseInitiationFeesChange;

		#endregion

		#region Rate Adjustment Elements
		/// <summary>
		/// Bind the Rate Adjustment Elements
		/// </summary>
		/// <param name="rateAdjustmentElements"></param>
		/// <param name="selectedRateAdjustmentElementKey"></param>
		void BindRateAdjustments(Dictionary<int, string> rateAdjustmentElements, int? selectedRateAdjustmentElementKey);

		/// <summary>
		/// Selected Rate Adjustment Key
		/// </summary>
		int? SelectedRateAdjustmentKey { get; }

		/// <summary>
		/// Rate Adjustment Label
		/// </summary>
		string RateAdjustmentLabel { set; }

		/// <summary>
		/// Show Rate Adjustment
		/// </summary>
		bool ShowRateAdjustment { get; set; }

		/// <summary>
		/// Set the Discount, this is used for the pricing adjustments
		/// </summary>
		void SetDiscount();
		#endregion
	}
}