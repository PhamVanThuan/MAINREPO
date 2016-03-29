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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using System.Collections.Generic;
using SAHL.Web.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.DomainMessages;
using System.Linq;

namespace SAHL.Web.Views.Origination
{
	public partial class ApplicationLoanDetails : SAHLCommonBaseView, IApplicationLoanDetails
	{
		#region Private Variables
		private bool _isReadOnly;
		private bool _isLoanDetailsVisible;
		private bool _isVarifixDetailsVisible;
		private bool _isSuperLoInfoVisible;
		private bool _isEdgeVisible;
		//private bool _isCategoryReadOnly;
		private bool _isLinkRateReadOnly;
		private bool _isOverrideFeesReadOnly;
		private bool _isCancellationFeesReadOnly;
		//private bool _isInterestOnlyVisible;
		private bool _isCancellationFeeVisible;
		private bool _isSwitchPanelVisible;
		private bool _isNewPurchasePanelVisible;
		private bool _isRefinancePanelVisible;
		private bool _isQuickCashVisible;
		private bool _canShowQuickCash;
		private bool _isQuickCashDetailsReadOnly;
		private bool _isButtonsPanelReadonly;
		private string _submitCaption = "Submit";
		private bool _isSPVReadOnly;
		private bool _isDiscountReadonly;
		private bool _isPropertyValueReadOnly;
		// private bool _isPropertyValueValuation;
		private bool _isBondToRegisterExceptionAction;
		private bool _isCashOutReadOnly;
		private bool _isCashDepositReadOnly;
		private bool _showCalculateButton = true;
		private bool _hideAll;
		private bool _hasQuickCashDeclineReasons;
		private bool _hasQuickCashValue;
		private bool _enforceQuickCash;
		private bool _createRevision;
		private double _cashOut;
		private int _edgeTerm;
		private const int _defaultTerm = 240;
		private bool _isQuickPayFeeReadOnly = true;

		private IApplication _application;
		private IApplicationInformationVariableLoan _appInfoVL;
		private IApplicationProductMortgageLoan _applicationProduct;
		private IApplicationProductSuperLoLoan _applicationProductSuperLoLoan;
		private IApplicationProductVariFixLoan _applicationProductVariFixLoan;
		private IApplicationProductEdge _applicationProductEdgeLoan;
		private ILookupRepository _lookupRepository;
        private IControlRepository _controlRepository;
        private IControlRepository ControlRepository
        {
            get
            {
                if (_controlRepository == null)
                {
                    _controlRepository = RepositoryFactory.GetRepository<IControlRepository>();
                }
                return _controlRepository;
            }
        }
		private IApplicationType _appType;

		/// <summary>
		/// Rate Adjustment Label
		/// </summary>
		public string RateAdjustmentLabel
		{
			set
			{
				lblRateAdjustmentsDisplay.Text = value;
			}
		}

		/// <summary>
		/// Show Rate Adjustment
		/// </summary>
		public bool ShowRateAdjustment
		{
			get
			{
				return lblRateAdjustmentsDisplay.Visible;
			}
			set
			{
				lblRateAdjustmentsDisplay.Visible = !value;
				ddlRateAdjustmentElements.Visible = value;
			}
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			//VarifixDetails.ShouldRunPage = ShouldRunPage;
			//SuperLoInfo.ShouldRunPage = ShouldRunPage;
			//LoanDetails.ShouldRunPage = ShouldRunPage;
			//QuickCashDetails.ShouldRunPage = ShouldRunPage;
			//Need a seperate/new presenter for when QC can be edited
			//QuickCashDetails.IsReadOnly = true;

			if (!ShouldRunPage)
				return;

            _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
			QuickCashDetails.QCDeclineReasons += new EventHandler(QuickCashDetails_QCDeclineReasons);
			EHLDetails.TitleText = _lookupRepository.Products.ObjectDictionary[((int)Products.Edge).ToString()].Description;
		}

		void QuickCashDetails_QCDeclineReasons(object sender, EventArgs e)
		{
			if (OnQCDeclineReasons != null)
				OnQCDeclineReasons(sender, e);
		}

		protected override void OnPreRender(EventArgs e)
		{

			base.OnPreRender(e);

			if (!ShouldRunPage)
			{
				//if !ShouldRunPage and hideall, then no page controls must be visible
				if (HideAll)
				{
					Panel1.Visible = false;
					VarifixDetails.Visible = false;
					LoanDetails.Visible = false;
					SuperLoInfo.Visible = false;
					QuickCashDetails.Visible = false;
					EHLDetails.Visible = false;
					btnRecalc.Visible = false;
					btnCancel.Visible = false;
					btnUpdate.Visible = false;
				}
				return;
			}

			if (EnforceQuickCash && !HasQuickCashDeclineReasons && !HasQuickCashValue && QuickCashDetails != null)
			{
				if (!QuickCashDetails.HasQCValue) //also no unpersisted user inputs
					ClientScript.RegisterStartupScript(typeof(Page), "disableApproveButton", "<script type=\"text/javascript\">enableUpdateButton(false);</script>");
			}

			if (IsPostBack)
			{
				GetApplicationDetails((IApplicationMortgageLoan)_application);
				
				

				// Recalc
                _application.CalculateApplicationDetail(IsBondToRegisterExceptionAction, false);

				// Rebind to update the controls
				BindApplicationDetails(_application);

				
				if (Convert.ToInt32(_applicationProduct.ProductType) != (int)Products.VariFixLoan)
				{
					lblPTIVF.Visible = false;
				}
				if (!IsValid) //errors could have been loaded in the previous call
					return;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!ShouldRunPage)
				return;

			_appType = _application.ApplicationType;

			// Editable controls
			ddlProduct.Visible = !_isReadOnly;
			//ddlCategory.Visible = !_isReadOnly;
			txtBondtoRegister.Visible = !_isReadOnly;
			txtTerm.Visible = !_isReadOnly;
			txtSwitchExistingLoan.Visible = !_isReadOnly;
			txtSwitchCashOut.Visible = !_isReadOnly;
			txtRefinanceCashOut.Visible = !_isReadOnly;
			chkOverrideFees.Enabled = !_isReadOnly;
			txtCancellationFee.Visible = !_isReadOnly;
			chkCapitaliseFees.Enabled = !_isReadOnly;
            chkCapitaliseInitiationFees.Enabled = !_isReadOnly;
			chkHOC.Enabled = !_isReadOnly;
			chkLife.Enabled = !_isReadOnly;
			chkQuickCash.Enabled = !_isReadOnly;
			chkStaffLoan.Enabled = !_isReadOnly;
			txtTerm.ReadOnly = _isEdgeVisible;
			//chkDiscountedLinkRate.Enabled = !_isReadOnly;

			//Product type checks
			if (_applicationProductVariFixLoan != null)
			{
				chkCAP.Enabled = false;
				chkInterestOnly.Enabled = false;
			}
			else if (_applicationProductEdgeLoan != null)
			{
				chkCAP.Enabled = false;
				chkInterestOnly.Enabled = false;
			}
			else
			{
				chkCAP.Enabled = !_isReadOnly;
				chkInterestOnly.Enabled = !_isReadOnly;
			}

			//Application type checks
			if (_appType.Key == (int)OfferTypes.NewPurchaseLoan)
			{
				chkCapitaliseFees.Enabled = false;
				chkQuickCash.Enabled = false;
			}

            if (_appType.Key == (int)OfferTypes.NewPurchaseLoan && 
                _application.HasAttribute(OfferAttributeTypes.AlphaHousing) &&
                !_isReadOnly)
            {
                chkCapitaliseInitiationFees.Enabled = true;
            }
            else
            {
                chkCapitaliseInitiationFees.Enabled = false;
            }

			txtNewPurchasePurchasePrice.Visible = !_isReadOnly;

			// Readonly controls
			lblProduct.Visible = _isReadOnly;
			//lblCategory.Visible = _isReadOnly;
			lblBondToRegister.Visible = _isReadOnly;
			lblTerm.Visible = _isReadOnly;
			lblSwitchExistingLoan.Visible = _isReadOnly;
			lblSwitchCashOut.Visible = _isReadOnly;
			lblRefinanceCashOut.Visible = _isReadOnly;
			lblCancellationFee.Visible = _isReadOnly;
			VarifixDetails.IsReadOnly = _isReadOnly;
			LoanDetails.IsReadOnly = _isReadOnly;
			EHLDetails.IsReadOnly = _isReadOnly;
			lblNewPurchasePurchasePrice.Visible = _isReadOnly;

			LoanDetails.Visible = _isLoanDetailsVisible;
			//LoanDetails.InterestOnlyVisible = _isInterestOnlyVisible;
			VarifixDetails.Visible = _isVarifixDetailsVisible;
			SuperLoInfo.Visible = _isSuperLoInfoVisible;
			QuickCashDetails.Visible = _isQuickCashVisible;
			EHLDetails.Visible = _isEdgeVisible;


			pnlSwitch.Visible = _isSwitchPanelVisible;
			pnlNewPurchase.Visible = _isNewPurchasePanelVisible;
			pnlRefinance.Visible = _isRefinancePanelVisible;
			trCancellationFee.Visible = _isCancellationFeeVisible;
			chkOverrideFees.Visible = _isCancellationFeeVisible;

			#region Field level access overrides
			// When set, these should always override the values above
			//ddlCategory.Visible = !_isCategoryReadOnly;
			//lblCategory.Visible = _isCategoryReadOnly;

			VarifixDetails.IsReadOnly = _isLinkRateReadOnly;
			VarifixDetails.IsDiscountReadOnly = _isDiscountReadonly;
			LoanDetails.IsReadOnly = _isLinkRateReadOnly;
			LoanDetails.IsDiscountReadOnly = _isDiscountReadonly;
			EHLDetails.IsReadOnly = _isLinkRateReadOnly;
			EHLDetails.IsDiscountReadOnly = _isDiscountReadonly;

			QuickCashDetails.IsReadOnly = _isQuickCashDetailsReadOnly; //this method on the control does nothing!

			if (_isCancellationFeesReadOnly || !chkOverrideFees.Checked)
			{
				lblCancellationFee.Visible = true;
				txtCancellationFee.Visible = false;
			}
			else
			{
				lblCancellationFee.Visible = false;
				txtCancellationFee.Visible = true;
			}

			chkOverrideFees.Enabled = !_isOverrideFeesReadOnly;

			if (_showCalculateButton)
				btnRecalc.Visible = !_isButtonsPanelReadonly;
			else
				btnRecalc.Visible = _showCalculateButton;

			btnCancel.Visible = !_isButtonsPanelReadonly;
			btnUpdate.Visible = !_isButtonsPanelReadonly;

			ddlSPV.Visible = !_isSPVReadOnly;
			lblSPVName.Visible = _isSPVReadOnly;
			chkDiscountedLinkRate.Enabled = !_isDiscountReadonly;

			txtSwitchCashOut.Visible = !_isCashOutReadOnly;
			lblSwitchCashOut.Visible = _isCashOutReadOnly;
			txtRefinanceCashOut.Visible = !_isCashOutReadOnly;
			lblRefinanceCashOut.Visible = _isCashOutReadOnly;

			txtNewPurchaseCashDeposit.Visible = !_isCashDepositReadOnly;
			lblNewPurchaseCashDeposit.Visible = _isCashDepositReadOnly;

			txtPropertyValue.Visible = !_isPropertyValueReadOnly;
			lblPropertyValue.Visible = _isPropertyValueReadOnly;

			if (_isBondToRegisterExceptionAction)
			{
				lblBondToRegister.Visible = !_isBondToRegisterExceptionAction;
				txtBondtoRegister.Visible = _isBondToRegisterExceptionAction;
			}

			#endregion

			btnUpdate.Text = _submitCaption;
			chkQuickPayFee.Enabled = !IsQuickPayFeeReadOnly;

            CheckInterestOnlyExpiry();
		}

        private void CheckInterestOnlyExpiry()
        {
            //application is past the I/O cut off date
            if (_application.ApplicationStartDate >
                Convert.ToDateTime(ControlRepository.GetControlByDescription("Interest Only Switch Off Date").ControlText))
            {
                chkInterestOnly.Enabled = false;
            }
        }

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			if (OnCancelClicked != null)
				OnCancelClicked(sender, e);
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			if (OnUpdateClicked != null)
				OnUpdateClicked(sender, e);
		}

		#region IApplicationLoanDetails Members

		public void BindProducts(IDictionary<string, string> Products, string Default)
		{
			BindDropDown(ddlProduct, Products, Default);
		}

		public void BindSPVs(IDictionary<string, string> SPVs, string Default)
		{
			BindDropDown(ddlSPV, SPVs, Default);
		}

		//public void BindCategories(IDictionary<string, string> Categories, string Default)
		//{
		//    BindDropDown(ddlCategory, Categories, Default);
		//}

		//public void BindLinkRates(IDictionary<string, string> LinkRates, string Default)
		//{
		//    // LoanDetails.BindLinkRates(LinkRates);
		//    // VarifixDetails.BindLinkRates(LinkRates);
		//}

		public void BindApplicationDetails(IApplication Application)
		{
			if (_application == null)
				_application = Application;

			_applicationProduct = _application.CurrentProduct as IApplicationProductMortgageLoan;
			_applicationProductSuperLoLoan = _application.CurrentProduct as IApplicationProductSuperLoLoan;
			_applicationProductVariFixLoan = _application.CurrentProduct as IApplicationProductVariFixLoan;
			_applicationProductEdgeLoan = _application.CurrentProduct as IApplicationProductEdge;

			ISupportsVariableLoanApplicationInformation vlai = _applicationProduct as ISupportsVariableLoanApplicationInformation;
			if (vlai != null)
			{
				_appInfoVL = vlai.VariableLoanInformation;
				if (vlai.VariableLoanInformation.CreditCriteria != null && vlai.VariableLoanInformation.CreditCriteria.ExceptionCriteria.HasValue)
				{
					trIsExceptionCreditCriteria.Visible = vlai.VariableLoanInformation.CreditCriteria.ExceptionCriteria.Value;
				}
			}

			IApplicationMortgageLoan appml = _application as IApplicationMortgageLoan;
			if (appml != null && appml.Property != null && appml.Property.OccupancyType != null)
				lblOccupancyType.Text = appml.Property.OccupancyType.Description;

			// Set the product
			ddlProduct.SelectedValue = (Convert.ToInt32(_applicationProduct.ProductType)).ToString();

			switch ((OfferTypes)_application.ApplicationType.Key)
			{
				case OfferTypes.NewPurchaseLoan:
					lblApplicationType.Text = "New Purchase Loan";
					BindApplicationDetailsNewPurchase();
					break;
				case OfferTypes.RefinanceLoan:
					lblApplicationType.Text = "Refinance";
					BindApplicationDetailsRefinance();
					break;
				case OfferTypes.SwitchLoan:
					lblApplicationType.Text = "Switch Loan";
					BindApplicationDetailsSwitch();
					break;
				default:
					break;
			}

			if (_appInfoVL != null)
			{
				// Set the SPV
				if (_appInfoVL.SPV != null)
				{
					lblSPVName.Text = _appInfoVL.SPV.Description;
					ddlSPV.SelectedValue = _appInfoVL.SPV.Key.ToString();
				}
				else
					lblSPVName.Text = "-";

				// Category
				if (_appInfoVL.Category != null)
				{
					//ddlCategory.SelectedValue = _appInfoVL.Category.Key.ToString();
					lblCategory.Text = _appInfoVL.Category.Description;
				}
				else
				{
					//ddlCategory.SelectedValue = SAHLDropDownList.PleaseSelectValue;
					lblCategory.Text = "-";
				}

				// Bond to register
				double bondToRegister = _appInfoVL.BondToRegister.HasValue ? _appInfoVL.BondToRegister.Value : 0.0;
				lblBondToRegister.Text = bondToRegister.ToString(SAHL.Common.Constants.CurrencyFormat);
				txtBondtoRegister.Text = bondToRegister.ToString();

				// Property Value
				double propertyValue = _appInfoVL.PropertyValuation.HasValue ? _appInfoVL.PropertyValuation.Value : 0.0;
				lblPropertyValue.Text = propertyValue.ToString(SAHL.Common.Constants.CurrencyFormat);
				txtPropertyValue.Text = propertyValue.ToString();

				// Term
				double term = _appInfoVL.Term.HasValue ? _appInfoVL.Term.Value : 0.0;
				lblTerm.Text = term.ToString();

				// Term - if the product is Edge Home Loan the term is static
				if (_isEdgeVisible)
					txtTerm.Text = _edgeTerm.ToString();
				else
					txtTerm.Text = term.ToString();

				// Household Income
				double householdIncome = _appInfoVL.HouseholdIncome.HasValue ? _appInfoVL.HouseholdIncome.Value : 0.0;
				lblHouseHoldIncome.Text = householdIncome.ToString(SAHL.Common.Constants.CurrencyFormat);

				double pti = _appInfoVL.PTI.HasValue ? _appInfoVL.PTI.Value * 100 : 0;
				if (Convert.ToInt32(_applicationProduct.ProductType) == (int)Products.VariFixLoan)
				{
					lblPTIVF.Visible = true;
					lblPTIVF.Text = String.Format("PTI (VF) = {0}%", pti.ToString(SAHL.Common.Constants.NumberFormat));

					double feeTtl = _appInfoVL.FeesTotal.HasValue ? _appInfoVL.FeesTotal.Value : 0;
					double amnt = _appInfoVL.LoanAmountNoFees.HasValue ? _appInfoVL.LoanAmountNoFees.Value : 0;

					double LoanAmount = _appInfoVL.LoanAgreementAmount.HasValue ? _appInfoVL.LoanAgreementAmount.Value : (chkCapitaliseFees.Checked ? amnt + feeTtl : amnt);
					double rate = (_appInfoVL.MarketRate.HasValue ? _appInfoVL.MarketRate.Value : 0) + (_appInfoVL.RateConfiguration.Margin.Value);
					double vterm = _appInfoVL.Term.HasValue ? Convert.ToInt32(_appInfoVL.Term.Value) : 1D;
					double instalment = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(LoanAmount, rate, vterm, false);
					double varPTI = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(instalment, householdIncome);
					lblPTI.Text = String.Format("PTI = {0}%", (varPTI * 100).ToString(SAHL.Common.Constants.NumberFormat));
				}
				else
					lblPTI.Text = String.Format("PTI = {0}%", pti.ToString(SAHL.Common.Constants.NumberFormat));

				double ltv = _appInfoVL.LTV.HasValue ? _appInfoVL.LTV.Value * 100 : 0;
				lblLTV.Text = String.Format("LTV = {0}%", ltv.ToString(SAHL.Common.Constants.NumberFormat));
			}

			#region Binding the Loan Details control
			LoanDetails.Application = _application;
			#endregion

			#region Binding the SuperLo Details Control
			if (_applicationProductSuperLoLoan != null)
				SuperLoInfo.ApplicationProductSuperLoLoan = _applicationProductSuperLoLoan;
			#endregion

			#region Binding the Varifix Control
			if (_applicationProductVariFixLoan != null)
				VarifixDetails.BindApplicationProduct(_applicationProductVariFixLoan, _application);
			#endregion

			#region Binding the Edge Home Loan Details control
			if (_applicationProductEdgeLoan != null)
				EHLDetails.Application = _application;
			#endregion

			#region Binding the QuickCash Details Control
			ISupportsQuickCashApplicationInformation supportsQuickCashApplicationInformation = _application as ISupportsQuickCashApplicationInformation;

			IApplicationMortgageLoanWithCashOut applicationMortgageLoanWithCashOut = _application as IApplicationMortgageLoanWithCashOut;

			if (applicationMortgageLoanWithCashOut != null
				&& applicationMortgageLoanWithCashOut.RequestedCashAmount.HasValue
				&& supportsQuickCashApplicationInformation != null)
			{
				_cashOut = applicationMortgageLoanWithCashOut.RequestedCashAmount.Value;

				//If there is no cashout, then no quick cash
				if (!(_cashOut > 0D))
				{
					IsQuickCashVisible = false;
					EnforceQuickCash = false;
				}

				QuickCashDetails.BindQuickCash(supportsQuickCashApplicationInformation.ApplicationInformationQuickCash, _cashOut, HasQuickCashDeclineReasons);
			}
			else
			{
				IsQuickCashVisible = false;
				EnforceQuickCash = false;
			}

			#endregion

			SetDiscount();

			// Populate Application Options
			chkQuickCash.Checked = _application.HasAttribute(OfferAttributeTypes.QuickCash);
			chkHOC.Checked = _application.HasAttribute(OfferAttributeTypes.HOC);
			chkLife.Checked = _application.HasAttribute(OfferAttributeTypes.Life);
			chkCAP.Checked = _application.HasAttribute(OfferAttributeTypes.CAP2);
			chkStaffLoan.Checked = _application.HasAttribute(OfferAttributeTypes.StaffHomeLoan);

			// Skip the Interest Only Section if the product is EHL
			if (chkInterestOnly.Checked == true)
			{
				LoanDetails.InterestOnlyVisible = true;
			}
			else if (_applicationProductEdgeLoan == null)
			{
				chkInterestOnly.Checked = _application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
                LoanDetails.InterestOnlyVisible = _application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
			}

            chkDiscountedLinkRate.Checked = _application.HasFinancialAdjustment(FinancialAdjustmentTypeSources.DiscountedLinkrate);
			chkCapitaliseFees.Checked = _application.HasAttribute(OfferAttributeTypes.CapitalizeFees);
            chkCapitaliseInitiationFees.Checked = _application.HasAttribute(OfferAttributeTypes.CapitaliseInitiationFee);

			foreach (IApplicationExpense ae in _application.ApplicationExpenses)
			{
				if (ae.ExpenseType.Key == (int)ExpenseTypes.CancellationFee)
				{
					chkOverrideFees.Checked = ae.OverRidden;
					if (!_isReadOnly)
					{
						txtCancellationFee.Visible = chkOverrideFees.Checked;
						lblCancellationFee.Visible = !chkOverrideFees.Checked;
					}
					break;
				}
			}

			chkQuickPayFee.Checked = _application.HasAttribute(OfferAttributeTypes.QuickPayLoan);

			//Show Rate Adjustment
			//Get the Application Rate Override
			var applicationInformation = Application.GetLatestApplicationInformation();
			if (applicationInformation != null)
			{
				var applicationInformationFinancialAdjustment = (from financialAdjustment in applicationInformation.ApplicationInformationFinancialAdjustments
											   where financialAdjustment.FinancialAdjustmentTypeSource.Key == (int)SAHL.Common.Globals.FinancialAdjustmentTypeSources.CounterRate
											   select financialAdjustment).FirstOrDefault();
				//Get the Application Rate Adjustment
				if (applicationInformationFinancialAdjustment != null)
				{
					var appInfoRateAdjustment = (from applicationInformationRateAdjustment in applicationInformationFinancialAdjustment.ApplicationInformationAppliedRateAdjustments
												 select applicationInformationRateAdjustment).FirstOrDefault();
					RateAdjustmentLabel = String.Format("({0}) {1}", appInfoRateAdjustment.RateAdjustmentElement.RateAdjustmentElementType.Description, appInfoRateAdjustment.RateAdjustmentElement.RateAdjustmentValue.ToString(SAHL.Common.Constants.RateFormat));
				}
				else
				{
					RateAdjustmentLabel = "None";
				}
			}

            CheckInterestOnlyExpiry();
		}

		public void SetInterestOnly(IApplication app)
		{
			IApplicationProductEdge appProdEHL = app.CurrentProduct as IApplicationProductEdge;
			if (chkInterestOnly.Checked == true)
			{
				LoanDetails.InterestOnlyVisible = true;
			}
			else if (appProdEHL == null)
			{
				chkInterestOnly.Checked = app.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
                LoanDetails.InterestOnlyVisible = app.HasFinancialAdjustment(FinancialAdjustmentTypeSources.InterestOnly);
			}
		}

		public void SetDiscount()
		{
			if (IsPostBack)
			{
				//discount 
				string discount = Page.Request.Form[chkDiscountedLinkRate.UniqueID];
				if (discount == null || discount.Length == 0)
					discount = "off";

				ApplicationOptionsChange("chkDiscountedLinkRate", new KeyChangedEventArgs(discount == "on" ? true : false));

				#region Binding the Loan Details control
				LoanDetails.Application = _application;
				EHLDetails.Application = _application;
				LoanDetails.Reload((discount == "on"));
				VarifixDetails.Reload((discount == "on"));
				EHLDetails.Reload((discount == "on"));
				#endregion
			}
		}

		private void BindApplicationDetailsNewPurchase()
		{
			IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = _application as IApplicationMortgageLoanNewPurchase;

			if (applicationMortgageLoanNewPurchase != null)
			{
				// Minimum Bond
				lblMinimumBond.Text = applicationMortgageLoanNewPurchase.MinBondRequired.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Purchase Price
				double purchasePrice = applicationMortgageLoanNewPurchase.PurchasePrice.HasValue ? applicationMortgageLoanNewPurchase.PurchasePrice.Value : 0.0;
				txtNewPurchasePurchasePrice.Text = purchasePrice.ToString();
				lblNewPurchasePurchasePrice.Text = purchasePrice.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Cash Deposit
				double cashDeposit = applicationMortgageLoanNewPurchase.CashDeposit.HasValue ? applicationMortgageLoanNewPurchase.CashDeposit.Value : 0.0;
				txtNewPurchaseCashDeposit.Text = cashDeposit.ToString();
				lblNewPurchaseCashDeposit.Text = cashDeposit.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Total Fees
				double totalFees = applicationMortgageLoanNewPurchase.TotalFees.HasValue ? applicationMortgageLoanNewPurchase.TotalFees.Value : 0.0;
				lblNewPurchaseFees.Text = totalFees.ToString(SAHL.Common.Constants.CurrencyFormat);

                // Initiation Fees
                double initiationFees = applicationMortgageLoanNewPurchase.InitiationFee.HasValue ? applicationMortgageLoanNewPurchase.InitiationFee.Value : 0.0;
                lblInitiationFee.Text = initiationFees.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Total Loan Required
				double totalLoanRequired = applicationMortgageLoanNewPurchase.LoanAgreementAmount.HasValue ? applicationMortgageLoanNewPurchase.LoanAgreementAmount.Value : 0.0;
                if (_application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
                {
                    totalLoanRequired += totalFees;
                }
                
				lblTotalLoanRequired.Text = totalLoanRequired.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Registration Fees
				double registrationFees = applicationMortgageLoanNewPurchase.RegistrationFee.HasValue ? applicationMortgageLoanNewPurchase.RegistrationFee.Value : 0.0;
				lblRegistrationFee.Text = registrationFees.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		private void BindApplicationDetailsRefinance()
		{
			IApplicationMortgageLoanRefinance applicationMortgageLoanRefinance = _application as IApplicationMortgageLoanRefinance;

			if (applicationMortgageLoanRefinance != null)
			{
				// Minimum Bond
				lblMinimumBond.Text = applicationMortgageLoanRefinance.MinBondRequired.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Cash Out
				double cashOut = applicationMortgageLoanRefinance.CashOut.HasValue ? applicationMortgageLoanRefinance.CashOut.Value : 0.0;
				txtRefinanceCashOut.Text = cashOut.ToString();
				lblRefinanceCashOut.Text = cashOut.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Total Fees
				double totalFees = applicationMortgageLoanRefinance.TotalFees.HasValue ? applicationMortgageLoanRefinance.TotalFees.Value : 0.0;
				lblRefinanceFees.Text = totalFees.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Total Loan Required
				//double totalLoanRequired = applicationMortgageLoanRefinance.TotalLoanRequired.HasValue ? applicationMortgageLoanRefinance.TotalLoanRequired.Value : 0.0;
				//if (_application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
				//    totalLoanRequired += totalFees;
				//lblRefinanceTotalLoanRequired.Text = totalLoanRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblRefinanceTotalLoanRequired.Text = (applicationMortgageLoanRefinance.LoanAgreementAmount.HasValue ? applicationMortgageLoanRefinance.LoanAgreementAmount.Value : 0.0).ToString(SAHL.Common.Constants.CurrencyFormat);

				// Initiation Fees
				double initiationFees = applicationMortgageLoanRefinance.InitiationFee.HasValue ? applicationMortgageLoanRefinance.InitiationFee.Value : 0.0;
				lblInitiationFee.Text = initiationFees.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Registration Fees
				double registrationFees = applicationMortgageLoanRefinance.RegistrationFee.HasValue ? applicationMortgageLoanRefinance.RegistrationFee.Value : 0.0;
				lblRegistrationFee.Text = registrationFees.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		public void BindApplicationDetailsSwitch()
		{
			IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = _application as IApplicationMortgageLoanSwitch;

			if (applicationMortgageLoanSwitch != null)
			{
				// Minimum Bond
				lblMinimumBond.Text = applicationMortgageLoanSwitch.MinBondRequired.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Existing Loan
				double existingLoan = applicationMortgageLoanSwitch.ExistingLoan.HasValue ? applicationMortgageLoanSwitch.ExistingLoan.Value : 0.0;
				txtSwitchExistingLoan.Text = existingLoan.ToString();
				lblSwitchExistingLoan.Text = existingLoan.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Cash Out
				double cashOut = applicationMortgageLoanSwitch.CashOut.HasValue ? applicationMortgageLoanSwitch.CashOut.Value : 0.0;
				txtSwitchCashOut.Text = cashOut.ToString();
				lblSwitchCashOut.Text = cashOut.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Interim Interest
				double interimInterest = applicationMortgageLoanSwitch.InterimInterest.HasValue ? applicationMortgageLoanSwitch.InterimInterest.Value : 0.0;
				lblSwitchInterimInterest.Text = interimInterest.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Total Fees
				double totalFees = applicationMortgageLoanSwitch.TotalFees.HasValue ? applicationMortgageLoanSwitch.TotalFees.Value : 0.0;
				lblSwitchFees.Text = totalFees.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Total Loan Required
				//double totalLoanRequired = applicationMortgageLoanSwitch.TotalLoanRequired.HasValue ? applicationMortgageLoanSwitch.TotalLoanRequired.Value : 0.0;
				//if (_application.HasAttribute(OfferAttributeTypes.CapitalizeFees))
				//    totalLoanRequired += totalFees;
				//lblSwitchTotalLoanRequired.Text = totalLoanRequired.ToString(SAHL.Common.Constants.CurrencyFormat);
				lblSwitchTotalLoanRequired.Text = (applicationMortgageLoanSwitch.LoanAgreementAmount.HasValue ? applicationMortgageLoanSwitch.LoanAgreementAmount.Value : 0.0).ToString(SAHL.Common.Constants.CurrencyFormat);

				// Cancellation Fees
				double cancellationFees = applicationMortgageLoanSwitch.CancellationFee;
				lblCancellationFee.Text = cancellationFees.ToString(SAHL.Common.Constants.CurrencyFormat);
				txtCancellationFee.Text = cancellationFees.ToString();

				// Initiation Fees
				double initiationFees = applicationMortgageLoanSwitch.InitiationFee.HasValue ? applicationMortgageLoanSwitch.InitiationFee.Value : 0.0;
				lblInitiationFee.Text = initiationFees.ToString(SAHL.Common.Constants.CurrencyFormat);

				// Registration Fees
				double registrationFees = applicationMortgageLoanSwitch.RegistrationFee.HasValue ? applicationMortgageLoanSwitch.RegistrationFee.Value : 0.0;
				lblRegistrationFee.Text = registrationFees.ToString(SAHL.Common.Constants.CurrencyFormat);
			}
		}

		public void BindProduct(string Product)
		{
			lblProduct.Text = Product;
		}

		private void AddDomainErrorMessage(string errorMessage)
		{
			Messages.Add(new Error(errorMessage, errorMessage));
		}

		public void GetApplicationDetails(IApplicationMortgageLoan ApplicationMortgageLoan)
		{
			#region Validation

			double err;
			//Some validation
			if (txtBondtoRegister.Text.Length == 0)
				txtBondtoRegister.Text = "0";

			if (!Double.TryParse(txtTerm.Text, out err) || Convert.ToDouble(txtTerm.Text) == 0)
				AddDomainErrorMessage("Term is a required field and must be greater than 0.");

			if (!Double.TryParse(txtPropertyValue.Text, out err))
				AddDomainErrorMessage("Property value is required and must be greater than 0.");

			switch ((OfferTypes)ApplicationMortgageLoan.ApplicationType.Key)
			{
				case OfferTypes.NewPurchaseLoan:
					{
						if (!Double.TryParse(txtNewPurchasePurchasePrice.Text, out err))
							AddDomainErrorMessage("Purchase Price is required and must be greater than 0.");

						if (txtNewPurchaseCashDeposit.Text.Length == 0)
							txtNewPurchaseCashDeposit.Text = "0";
					}

					break;
				case OfferTypes.RefinanceLoan:
					if (!Double.TryParse(txtRefinanceCashOut.Text, out err))
						AddDomainErrorMessage("Cash out is required and must be greater than 0.");

					break;
				case OfferTypes.SwitchLoan:
					if (!Double.TryParse(txtSwitchExistingLoan.Text, out err))
						AddDomainErrorMessage("Existing Loan is required and must be greater than 0.");

					if (txtSwitchCashOut.Text.Length == 0)
						txtSwitchCashOut.Text = "0";

					break;
				default:
					break;
			}

			if (!IsValid)
				return;

			#endregion

			// Get the Application Product
			IApplicationProductMortgageLoan applicationProduct = _application.CurrentProduct as IApplicationProductMortgageLoan;
			ISupportsVariableLoanApplicationInformation vlai = applicationProduct as ISupportsVariableLoanApplicationInformation;


			//Get attributes from the postback and set them on the application
			//string discount = Page.Request.Form[chkDiscountedLinkRate.UniqueID];
			//    if (discount == null || discount.Length == 0)
			//        discount = "off";
			//ApplicationOptionsChange("chkDiscountedLinkRate", new KeyChangedEventArgs(discount == "on" ? true : false));
			if (IsPostBack)
			{
				//string qc = Page.Request.Form[chkQuickCash.UniqueID];
				//if (String.IsNullOrEmpty(qc))
				//    qc = "off";
				ApplicationOptionsChange("chkQuickCash", new KeyChangedEventArgs(chkQuickCash.Checked));

				//string hoc = Page.Request.Form[chkHOC.UniqueID];
				//if (String.IsNullOrEmpty(hoc))
				//    hoc = "off";
				ApplicationOptionsChange("chkHOC", new KeyChangedEventArgs(chkHOC.Checked));

				//string staff = Page.Request.Form[chkStaffLoan.UniqueID];
				//if (String.IsNullOrEmpty(staff))
				//    staff = "off";
				ApplicationOptionsChange("chkStaffLoan", new KeyChangedEventArgs(chkStaffLoan.Checked));

				//string life = Page.Request.Form[chkLife.UniqueID];
				//if (String.IsNullOrEmpty(life))
				//    life = "off";
				ApplicationOptionsChange("chkLife", new KeyChangedEventArgs(chkLife.Checked));

				//string cap = Page.Request.Form[chkCAP.UniqueID];
				//if (String.IsNullOrEmpty(cap))
				//    cap = "off";
				ApplicationOptionsChange("chkCAP", new KeyChangedEventArgs(chkCAP.Checked));

				//string io = Page.Request.Form[chkInterestOnly.UniqueID];
				//if (String.IsNullOrEmpty(io))
				//    io = "off";
				ApplicationOptionsChange("chkInterestOnly", new KeyChangedEventArgs(chkInterestOnly.Checked));
			}

			SetDiscount();

			switch ((OfferTypes)ApplicationMortgageLoan.ApplicationType.Key)
			{
				case OfferTypes.NewPurchaseLoan:
					{
						IApplicationMortgageLoanNewPurchase applicationMortgageLoanNewPurchase = ApplicationMortgageLoan as IApplicationMortgageLoanNewPurchase;
						if (!ddlProduct.SelectedValue.Equals(SAHLDropDownList.PleaseSelectValue)
							&& !ddlProduct.SelectedValue.Equals(Convert.ToString((int)applicationProduct.ProductType)))
						{
							applicationMortgageLoanNewPurchase.SetProduct((ProductsNewPurchase)Convert.ToInt32(ddlProduct.SelectedValue));
						}

						if (applicationMortgageLoanNewPurchase.PurchasePrice == null
							|| !Convert.ToDouble(txtNewPurchasePurchasePrice.Text).Equals(applicationMortgageLoanNewPurchase.PurchasePrice.Value))
						{
							if (String.IsNullOrEmpty(txtNewPurchasePurchasePrice.Text))
								applicationMortgageLoanNewPurchase.PurchasePrice = null;
							else
								applicationMortgageLoanNewPurchase.PurchasePrice = Convert.ToDouble(txtNewPurchasePurchasePrice.Text);
						}

						if (applicationMortgageLoanNewPurchase.CashDeposit == null
							|| !Convert.ToDouble(txtNewPurchaseCashDeposit.Text).Equals(applicationMortgageLoanNewPurchase.CashDeposit.Value))
						{
							if (String.IsNullOrEmpty(txtNewPurchaseCashDeposit.Text))
								applicationMortgageLoanNewPurchase.CashDeposit = null;
							else
								applicationMortgageLoanNewPurchase.CashDeposit = Convert.ToDouble(txtNewPurchaseCashDeposit.Text);
						}

                        applicationMortgageLoanNewPurchase.CapitaliseInitiationFees = chkCapitaliseInitiationFees.Checked;
					}

					break;
				case OfferTypes.RefinanceLoan:
					IApplicationMortgageLoanRefinance applicationMortgageLoanRefinance = ApplicationMortgageLoan as IApplicationMortgageLoanRefinance;
					if (!ddlProduct.SelectedValue.Equals(SAHLDropDownList.PleaseSelectValue)
						&& !ddlProduct.SelectedValue.Equals(Convert.ToString((int)applicationProduct.ProductType)))
					{
						applicationMortgageLoanRefinance.SetProduct((ProductsRefinance)Convert.ToInt32(ddlProduct.SelectedValue));
					}

					if (applicationMortgageLoanRefinance.CashOut == null
						|| !Convert.ToDouble(txtRefinanceCashOut.Text).Equals(applicationMortgageLoanRefinance.CashOut.Value))
					{
						if (String.IsNullOrEmpty(txtRefinanceCashOut.Text))
							applicationMortgageLoanRefinance.CashOut = null;
						else
							applicationMortgageLoanRefinance.CashOut = Convert.ToDouble(txtRefinanceCashOut.Text);
					}

					applicationMortgageLoanRefinance.CapitaliseFees = chkCapitaliseFees.Checked;

					break;
				case OfferTypes.SwitchLoan:
					IApplicationMortgageLoanSwitch applicationMortgageLoanSwitch = ApplicationMortgageLoan as IApplicationMortgageLoanSwitch;
					if (!ddlProduct.SelectedValue.Equals(SAHLDropDownList.PleaseSelectValue)
						&& !ddlProduct.SelectedValue.Equals(Convert.ToString((int)applicationProduct.ProductType)))
					{
						applicationMortgageLoanSwitch.SetProduct((ProductsSwitchLoan)Convert.ToInt32(ddlProduct.SelectedValue));
					}

					if (applicationMortgageLoanSwitch.ExistingLoan == null
						|| !Convert.ToDouble(txtSwitchExistingLoan.Text).Equals(applicationMortgageLoanSwitch.ExistingLoan.Value))
					{
						if (String.IsNullOrEmpty(txtSwitchExistingLoan.Text))
							applicationMortgageLoanSwitch.ExistingLoan = null;
						else
							applicationMortgageLoanSwitch.ExistingLoan = Convert.ToDouble(txtSwitchExistingLoan.Text);
					}

					if (applicationMortgageLoanSwitch.CashOut == null
						|| !Convert.ToDouble(txtSwitchCashOut.Text).Equals(applicationMortgageLoanSwitch.CashOut.Value))
					{
						if (String.IsNullOrEmpty(txtSwitchCashOut.Text))
							applicationMortgageLoanSwitch.CashOut = null;
						else
							applicationMortgageLoanSwitch.CashOut = Convert.ToDouble(txtSwitchCashOut.Text);
					}

					applicationMortgageLoanSwitch.CapitaliseFees = chkCapitaliseFees.Checked;
					//if (!String.IsNullOrEmpty(txtCancellationFee.Text)) // always set the cancellation fee for a switch loan
					//{
					string feeoverrride = Page.Request.Form[chkOverrideFees.UniqueID];
					if (String.IsNullOrEmpty(feeoverrride))
						feeoverrride = "off";

					//OverrideFeesChange(sender, new KeyChangedEventArgs(feeoverrride == "on" ? true : false));

					applicationMortgageLoanSwitch.SetCancellationFee(Convert.ToDouble(!String.IsNullOrEmpty(txtCancellationFee.Text) ? txtCancellationFee.Text : "0.0"), feeoverrride == "on" ? true : false);
					//}

					break;
				default:
					break;
			}

			// Refresh product
			applicationProduct = _application.CurrentProduct as IApplicationProductMortgageLoan;
			vlai = applicationProduct as ISupportsVariableLoanApplicationInformation;

			#region Get User Input

			#region Old Dropdowns - should not be user editable
			if (!ddlSPV.SelectedValue.Equals(SAHLDropDownList.PleaseSelectValue)
				&& !ddlSPV.SelectedValue.Equals(vlai.VariableLoanInformation.SPV.Key.ToString()))
			{
				vlai.VariableLoanInformation.SPV = _lookupRepository.SPVList.ObjectDictionary[ddlSPV.SelectedValue];
			}

			//if (!ddlCategory.SelectedValue.Equals(SAHLDropDownList.PleaseSelectValue)
			//    && !ddlCategory.SelectedValue.Equals(vlai.VariableLoanInformation.Category.Key.ToString()))
			//{
			//    vlai.VariableLoanInformation.Category = _lookupRepository.Categories.ObjectDictionary[ddlCategory.SelectedValue];
			//}
			#endregion

			if (vlai.VariableLoanInformation.BondToRegister == null
				|| !Convert.ToDouble(txtBondtoRegister.Text).Equals(vlai.VariableLoanInformation.BondToRegister.Value))
			{
				vlai.VariableLoanInformation.BondToRegister = Convert.ToDouble(txtBondtoRegister.Text);
			}

			if (vlai.VariableLoanInformation.Term == null
				|| !Convert.ToDouble(txtTerm.Text).Equals(vlai.VariableLoanInformation.Term.Value))
			{
				vlai.VariableLoanInformation.Term = int.Parse(txtTerm.Text);
			}

			if (vlai.VariableLoanInformation.PropertyValuation == null
				|| !Convert.ToDouble(txtPropertyValue.Text).Equals(vlai.VariableLoanInformation.PropertyValuation.Value))
			{
				vlai.VariableLoanInformation.PropertyValuation = int.Parse(txtPropertyValue.Text);
			}

			#endregion

			// Set Up RateOverRides for EHL
			IApplicationProductEdge appProdEHL = _application.CurrentProduct as IApplicationProductEdge;
			if (appProdEHL != null)
			{
				ApplicationOptionsChange("SetUpEdge", new KeyChangedEventArgs(null));
			}

			// Get the QC data
			ISupportsQuickCashApplicationInformation qcai = _application as ISupportsQuickCashApplicationInformation;
			if (qcai != null)
				QuickCashDetails.GetQuickCashDetails(qcai.ApplicationInformationQuickCash);

			// Do this last, so that the SetProduct has been called.
			// Varifix Control
			VarifixDetails.GetApplicationDetails(ApplicationMortgageLoan);

			// This should only be done after the application details are updated
			SetInterestOnly(_application);
		}

		public event EventHandler OnCancelClicked;

		public event EventHandler OnUpdateClicked;

		#endregion

		#region IApplicationLoanDetails Members

		public bool IsQuickPayFeeReadOnly
		{
			get { return _isQuickPayFeeReadOnly; }
			set { _isQuickPayFeeReadOnly = value; }
		}

		public int SetEdgeTerm
		{
			set { _edgeTerm = value; }
		}

		public bool IsEdgeVisible
		{
			set { _isEdgeVisible = value; }
			get { return _isEdgeVisible; }
		}

		public bool IsLoanDetailsVisible
		{
			set { _isLoanDetailsVisible = value; }
		}

		public bool IsVarifixDetailsVisible
		{
			set { _isVarifixDetailsVisible = value; }
			get { return _isVarifixDetailsVisible; }
		}

		public bool IsSuperLoInfoVisible
		{
			set { _isSuperLoInfoVisible = value; }
		}

		public bool IsReadOnly
		{
			set
			{
				_isReadOnly = value;

				//_isCategoryReadOnly = value;
				_isLinkRateReadOnly = value;
				_isOverrideFeesReadOnly = value;
				_isCancellationFeesReadOnly = value;
				_isQuickCashDetailsReadOnly = value;
				_isButtonsPanelReadonly = value;
				_isSPVReadOnly = value;
				_isDiscountReadonly = value;
				_isCashOutReadOnly = value;
				_isCashDepositReadOnly = value;
			}
		}

		//public bool IsCategoryReadOnly
		//{
		//    set { _isCategoryReadOnly = value; }
		//}

		public bool IsLinkRateReadOnly
		{
			set { _isLinkRateReadOnly = value; }
		}

		public bool IsOverrideFeesReadOnly
		{
			set { _isOverrideFeesReadOnly = value; }
		}

		public bool IsCancellationFeesReadOnly
		{
			set { _isCancellationFeesReadOnly = value; }
		}

		/// <summary>
		/// Even if QC is supported, do not display it if there is no RequestedCashAmount on the application
		/// </summary>
		public bool IsQuickCashVisible
		{
			set
			{
				if (_cashOut > 0)
					_isQuickCashVisible = value;
				else
					_isQuickCashVisible = false;
			}
		}

		public bool CanShowQuickCash
		{
			get { return _canShowQuickCash; }
			set { _canShowQuickCash = value; }
		}

		public bool IsQuickCashDetailsReadOnly
		{
			set { _isQuickCashDetailsReadOnly = value; }
		}

		public bool HasQuickCashDeclineReasons
		{
			get { return _hasQuickCashDeclineReasons; }
			set { _hasQuickCashDeclineReasons = value; }
		}

		public bool HasQuickCashValue
		{
			get { return _hasQuickCashValue; }
			set { _hasQuickCashValue = value; }
		}

		public bool EnforceQuickCash
		{
			get { return _enforceQuickCash; }
			set { _enforceQuickCash = value; }
		}

		public bool IsButtonsPanelReadonly
		{
			set { _isButtonsPanelReadonly = value; }
		}

		public bool IsSPVReadOnly
		{
			set { _isSPVReadOnly = value; }
		}

		public bool IsDiscountReadonly
		{
			get { return _isDiscountReadonly; }
			set { _isDiscountReadonly = value; }
		}

		public bool IsSubmitButtonEnabled
		{
			set
			{
				btnUpdate.Enabled = value;
			}
		}

		public bool IsRecalculateButtonEnabled
		{
			set
			{
				btnRecalc.Enabled = value;
			}
		}

		public double Discount
		{
			get
			{
				if (ddlProduct.SelectedValue == ((int)Products.VariFixLoan).ToString())
					return VarifixDetails.Discount;
				else if (ddlProduct.SelectedValue == ((int)Products.Edge).ToString())
					return EHLDetails.Discount;
				else
					return LoanDetails.Discount;

				//if (IsVarifixDetailsVisible)
				//    return VarifixDetails.Discount;

				//    return LoanDetails.Discount;
			}
			//set { LoanDetails.Discount = value; }
		}

		public bool IsCancellationFeeVisible
		{
			get { return _isCancellationFeeVisible; }
			set { _isCancellationFeeVisible = value; }
		}

		public void SetSubmitCaption(string SubmitCaption)
		{
			_submitCaption = SubmitCaption;
		}

		public event EventHandler OnRecalculateApplication;

		public event EventHandler OnQCDeclineReasons;

		public event KeyChangedEventHandler ApplicationOptionsChange;

		public event EventHandler OnCalculateMaximumFixedPercentage;

		public event KeyChangedEventHandler OverrideFeesChange;

        public event KeyChangedEventHandler CapitaliseFeesChange;

		public event KeyChangedEventHandler QuickPayFeeCheckedChange;

        public event KeyChangedEventHandler CapitaliseInitiationFeesChange;

		#endregion

		#region Private Helper Functions

		private static void BindDropDown(SAHLDropDownList DropDownList, IDictionary<string, string> IDataItems, string DefaultValue)
		{
			DropDownList.DataSource = IDataItems;
			DropDownList.DataBind();
			DropDownList.VerifyPleaseSelect();

			// Set the default value if supplied
			if (!String.IsNullOrEmpty(DefaultValue))
				DropDownList.SelectedValue = DefaultValue;
		}

		public bool IsSwitchPanelVisible
		{
			set { _isSwitchPanelVisible = value; }
		}

		public bool IsNewPurchasePanelVisible
		{
			set { _isNewPurchasePanelVisible = value; }
		}

		public bool IsRefinancePanelVisible
		{
			set { _isRefinancePanelVisible = value; }
		}

		#endregion

		protected void chkQuickCash_CheckedChanged(object sender, EventArgs e)
		{
			//ApplicationOptionsChange("chkQuickCash", new KeyChangedEventArgs(chkQuickCash.Checked));
		}

		protected void chkHOC_CheckedChanged(object sender, EventArgs e)
		{
			//ApplicationOptionsChange("chkHOC", new KeyChangedEventArgs(chkHOC.Checked));
		}

		protected void chkStaffLoan_CheckedChanged(object sender, EventArgs e)
		{
			//ApplicationOptionsChange("chkStaffLoan", new KeyChangedEventArgs(chkStaffLoan.Checked));
		}

		protected void chkLife_CheckedChanged(object sender, EventArgs e)
		{
			//ApplicationOptionsChange("chkLife", new KeyChangedEventArgs(chkLife.Checked));
		}

		protected void chkCAP_CheckedChanged(object sender, EventArgs e)
		{
			//ApplicationOptionsChange("chkCAP", new KeyChangedEventArgs(chkCAP.Checked));
		}

		protected void chkInterestOnly_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationOptionsChange("chkInterestOnly", new KeyChangedEventArgs(chkInterestOnly.Checked));

			//string isChecked = Page.Request.Form[chkInterestOnly.UniqueID];
			//if (!string.IsNullOrEmpty(isChecked))
			//    LoanDetails.InterestOnlyVisible = Convert.ToBoolean(isChecked);
			//else
			//    LoanDetails.InterestOnlyVisible = chkInterestOnly.Checked;

			//chkInterestOnly.Checked = _application.HasRateOverride(RateOverrideTypes.InterestOnly);
			//LoanDetails.InterestOnlyVisible = _application.HasRateOverride(RateOverrideTypes.InterestOnly);
		}

		protected void chkDiscountedLinkRate_CheckedChanged(object sender, EventArgs e)
		{
			ApplicationOptionsChange("chkDiscountedLinkRate", new KeyChangedEventArgs(chkDiscountedLinkRate.Checked));
			OnRecalculateApplication(sender, e);
			//LoanDetails.Application = _application;
			//LoanDetails.Init;
			//LoanDetails.Load;

		}

        protected void chkCapitaliseFees_CheckedChanged(object sender, EventArgs e)
		{
			string capfees = Page.Request.Form[chkCapitaliseFees.UniqueID];
			if (String.IsNullOrEmpty(capfees))
				capfees = "off";

            CapitaliseFeesChange(sender, new KeyChangedEventArgs(capfees == "on" ? true : false));
		}

        protected void chkCapitaliseInitiationFees_CheckedChanged(object sender, EventArgs e)
        {
            CapitaliseInitiationFeesChange(sender, new KeyChangedEventArgs(null));
        }

		protected void chkQuickPayFee_CheckedChanged(object sender, EventArgs e)
		{
			string quickPayFee = Page.Request.Form[chkQuickPayFee.UniqueID];

			QuickPayFeeCheckedChange(sender, new KeyChangedEventArgs(quickPayFee == "on" ? true : false));
		}

		public bool IsBondToRegisterExceptionAction
		{
			get { return _isBondToRegisterExceptionAction; }
			set { _isBondToRegisterExceptionAction = value; }
		}

		public bool IsCashOutReadOnly
		{
			set { _isCashOutReadOnly = value; }
		}

		public bool IsCashDepositReadOnly
		{
			set { _isCashDepositReadOnly = value; }
		}

		/// <summary>
		/// Sets whether the Recalculate button is visible
		/// </summary>
		public bool ShowCalculateButton
		{
			set { _showCalculateButton = value; }

		}

		/// <summary>
		/// If a non new business application is loaded, hide all controls on the page
		/// </summary>
		public bool HideAll
		{
			get { return _hideAll; }
			set { _hideAll = value; }
		}

		protected void chkOverrideFees_CheckedChanged(object sender, EventArgs e)
		{
			if (!ShouldRunPage)
				return;

			//string feeoverrride = Page.Request.Form[chkOverrideFees.UniqueID];
			//if (String.IsNullOrEmpty(feeoverrride))
			//    feeoverrride = "off";

			//OverrideFeesChange(sender, new KeyChangedEventArgs(feeoverrride == "on" ? true : false));

			//chkOverrideFees.Checked = (feeoverrride == "on" ? true : false);
			//ApplicationOptionsChange("chkCAP", new KeyChangedEventArgs(chkOverrideFees.Checked));

		}

		protected void VarifixDetails_OnCalculateMaximumFixedPercentage(object sender, VarifixLoanInfoEventArgs e)
		{
			if (!ShouldRunPage)
				return;

			OnCalculateMaximumFixedPercentage(sender, e);
		}

		protected void btnRecalc_Click(object sender, EventArgs e)
		{
			if (!ShouldRunPage)
				return;

			//Clear out these values pre-calc so that errors reported with values are consistant
			lblSwitchInterimInterest.Text = "-";
			lblSwitchFees.Text = "-";
			lblSwitchTotalLoanRequired.Text = "-";

			lblNewPurchaseFees.Text = "-";
			lblTotalLoanRequired.Text = "-";

			lblRefinanceFees.Text = "-";
			lblRefinanceTotalLoanRequired.Text = "-";

			lblInitiationFee.Text = "-";
			lblRegistrationFee.Text = "-";

			lblLTV.Text = "LTV = - %";
			lblPTI.Text = "PTI = - %";

			if (OnRecalculateApplication != null)
				OnRecalculateApplication(sender, e);

		}

		protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
		{
			Control control = null;
			string ctrlname = Page.Request.Params.Get("__EVENTTARGET");
			if (ctrlname != null && !String.IsNullOrEmpty(ctrlname))
			{
				control = Page.FindControl(ctrlname);

				if (control != null && control == ddlProduct)
				{
					if (ddlProduct.SelectedValue == ((int)Products.Edge).ToString())
						txtTerm.Text = _edgeTerm.ToString();
					else
						txtTerm.Text = _defaultTerm.ToString();
				}
			}

			//do product option changes if VariFix, use the ddl, as the app might not have the user selected product
			if (ddlProduct.SelectedValue == ((int)Products.VariFixLoan).ToString())
			{
				chkCAP.Checked = false;
				chkInterestOnly.Checked = false;
			}

			OnRecalculateApplication(sender, e);

			chkCAP.Enabled = !_isReadOnly;
			chkInterestOnly.Enabled = !_isReadOnly;
			chkDiscountedLinkRate.Enabled = !_isDiscountReadonly;

			switch (_application.CurrentProduct.ProductType)
			{
				case Products.NewVariableLoan:
					EHLDetails.Visible = false;
					LoanDetails.Visible = true;
					VarifixDetails.Visible = false;
					SuperLoInfo.Visible = false;
					txtTerm.ReadOnly = false;
					break;
				case Products.SuperLo:
					EHLDetails.Visible = false;
					LoanDetails.Visible = true;
					VarifixDetails.Visible = false;
					SuperLoInfo.Visible = true;
					txtTerm.ReadOnly = false;
					break;
				case Products.VariFixLoan:
					EHLDetails.Visible = false;
					LoanDetails.Visible = false;
					VarifixDetails.Visible = true;
					SuperLoInfo.Visible = false;

					chkCAP.Enabled = false;
					chkInterestOnly.Enabled = false;
					//chkDiscountedLinkRate.Enabled = false;
					txtTerm.ReadOnly = false;
					break;
				case Products.Edge:
					EHLDetails.Visible = true;
					LoanDetails.Visible = false;
					VarifixDetails.Visible = false;
					SuperLoInfo.Visible = false;
					//
					chkInterestOnly.Checked = false;
					chkInterestOnly.Enabled = false;
					txtTerm.Text = _edgeTerm.ToString();
					txtTerm.ReadOnly = true;
					break;
				default:
					break;
			}

            CheckInterestOnlyExpiry();
		}

		#region IApplicationLoanDetails Members

		public bool IsPropertyValueReadOnly
		{
			set
			{
				_isPropertyValueReadOnly = value;
			}
		}

		public bool IsPropertyValueValuation
		{
			set
			{
				// _isPropertyValueValuation = value;  - not used - why??!!!
				if (value == true)
				{
					lblPropertValuation.Text = "Latest Valuation";
					_isPropertyValueReadOnly = value;
				}
			}
		}

		public string SelectedSPV
		{
			get { return ddlSPV.SelectedValue; }
		}

		/// <summary>
		/// Determines whether a revision should be created based on the type of credit decision
		/// ApproveWithPricingChanges and DeclineWithOffer create revisions because changes are 
		/// being made to the application that was submitted to Credit.
		/// </summary>
		public bool CreateRevision
		{
			get { return _createRevision; }
			set { _createRevision = value; }
		}

		public bool QuickPayFeeChecked
		{
			get { return chkQuickPayFee.Checked; }
		}

		/// <summary>
		/// Bind the Rate Adjustment Elements
		/// </summary>
		/// <param name="rateAdjustmentElements"></param>
		/// <param name="selectedRateAdjustmentElementKey"></param>
		public void BindRateAdjustments(Dictionary<int, string> rateAdjustmentElements, int? selectedRateAdjustmentElementKey)
		{
			ddlRateAdjustmentElements.DataSource = rateAdjustmentElements;
			ddlRateAdjustmentElements.DataValueField = "Key";
			ddlRateAdjustmentElements.DataTextField = "Value";
			ddlRateAdjustmentElements.DataBind();
			ddlRateAdjustmentElements.Items.RemoveAt(0);
			if (selectedRateAdjustmentElementKey != null && selectedRateAdjustmentElementKey > 0)
			{
				ddlRateAdjustmentElements.SelectedValue = selectedRateAdjustmentElementKey.ToString();
			}
		}

		public int? SelectedRateAdjustmentKey
		{
			get
			{
				int value;
				if (int.TryParse(ddlRateAdjustmentElements.SelectedValue, out value))
				{
					return value;
				}
				return null;
			}
		}
		#endregion
	}
}