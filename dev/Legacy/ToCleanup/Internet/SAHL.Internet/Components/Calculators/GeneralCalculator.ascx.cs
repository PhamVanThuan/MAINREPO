using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using SAHL.Internet.SAHL.Web.Services.Application;
using SAHL.Internet.SAHL.Web.Services.Calculators;
using CreditDisqualifications = SAHL.Internet.SAHL.Web.Services.Calculators.CreditDisqualifications;
using System.Web.UI;

[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.GeneralCalculator.js", "application/javascript")]
[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.Input.js", "application/javascript")]

namespace SAHL.Internet.Components.Calculators
{
	/// <summary>
	/// This mode is set by setting the value of Calculato Mode through Open Smat Module
	/// CalculatorMode = 1 - Unknown
	/// CalculatorMode = 2 - Switch Loan (mode used here)
	/// CalculatorMode= 3 - New Purchase (mode used here)
	/// CalculatorMode = 4 - Refinance 
	/// CalculatorMode = 5 - Further Loan
	/// These Tie to the sahlDB Purpose Numbers
	/// </summary>
	public partial class GeneralCalculator : UserControl, IScriptControl
	{
		//Public Variables are set up per instance in DotNetNuke
		// Viewtype describes what the claculator is being used for
		// This variable is set through Open Smart module 
		// calculator Mode defaults to 2 - Switch loaan;

        /// <summary>
        /// Gets or sets the calculator web service.
        /// </summary>
        private WebCalculatorBase BaseCalculator
        {
            get
            {
                var result = Session["WebCalculatorBase"] as WebCalculatorBase ?? (BaseCalculator = new WebCalculatorBase());
                return result;
            }
            set { Session["WebCalculatorBase"] = value; }
        }

        /// <summary>
        /// Gets the SA Home Loans session object.
        /// </summary>
        private SAHLWebSession SahlSession
        {
            get { return Session["SAHLWebSession"] as SAHLWebSession; }
        }

        #region Calculator State Variables

        /// <summary>
        /// Gets the current currency format.
        /// </summary>
        protected string CurrencyFormat
        {
            get { return ViewState["CurrencyFormat"] as string; }
            private set { ViewState["CurrencyFormat"] = value; }
        }

        /// <summary>
        /// Gets the current currency format.
        /// </summary>
        protected string CurrencyFormatNoCents
        {
            get { return ViewState["CurrencyFormatNoCents"] as string; }
            private set { ViewState["CurrencyFormatNoCents"] = value; }
        }

        /// <summary>
        /// Gets the current rate format.
        /// </summary>
        protected string RateFormat
        {
            get { return ViewState["RateFormat"] as string; }
            private set { ViewState["RateFormat"] = value; }
        }

        /// <summary>
        /// Gets the maximum acceptable PTI.
        /// </summary>
        protected double MaxPti
        {
            get
            {
                var result = ViewState["MaxPti"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MaxPti"] = value; }
        }

        /// <summary>
        /// Gets the minimum LTV required to not pay a deposit.
        /// </summary>
        protected double MinLtv
        {
            get
            {
                var result = ViewState["MinLtv"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MinLtv"] = value; }
        }

        /// <summary>
        /// Gets the maximum LTV allowed for Self Employed applicants.
        /// </summary>
        protected double MaxSelfLTV
        {
            get
            {
                var result = ViewState["MaxSelfLTV"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MaxSelfLTV"] = value; }
        }

        /// <summary>
        /// Gets the maximum loan term in years.
        /// </summary>
        protected double MaxTerm
        {
            get
            {
                var result = ViewState["MaxTerm"];
                if (result == null) return 240.0;
                return (double)result;
            }
            private set { ViewState["MaxTerm"] = value; }
        }

        /// <summary>
        /// Gets the minimum allowed combined household income.
        /// </summary>
        protected double MinHouseholdIncome
        {
            get
            {
                var result = ViewState["MinHouseholdIncome"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MinHouseholdIncome"] = value; }
        }

        /// <summary>
        /// Gets the minimum allowed loan amount.
        /// </summary>
	    protected double MinLoanAmount
	    {
	        get 
            { 
                var result = ViewState["MinLoanAmount"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MinLoanAmount"] = value; }
	    }

        /// <summary>
        /// Gets the maximum allowed loan amount.
        /// </summary>
        protected double MaxLoanAmount
        {
            get 
            { 
                var result = ViewState["MaxLoanAmount"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MaxLoanAmount"] = value; }
        }

        /// <summary>
        /// Gets the minimum market value.
        /// </summary>
        protected double MinMarketValue
        {
            get
            {
                var result = ViewState["MinMarketValue"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MinMarketValue"] = value; }
        }

        /// <summary>
        /// Gets the minimum switch loan amount.
        /// </summary>
        protected double MinSwitchLoanAmount
        {
            get
            {
                var result = ViewState["MinSwitchLoanAmount"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MinSwitchLoanAmount"] = value; }
        }

        /// <summary>
        /// Gets the minimum fixed financial service amount.
        /// </summary>
        protected double MinFixFinServiceAmount
        {
            get
            {
                var result = ViewState["MinFixFinServiceAmount"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["MinFixFinServiceAmount"] = value; }
        }
        
        #endregion

        /// <summary>
        /// Gets or sets the mode to display the calculator in.
        /// </summary>
        public int CalculatorMode
        {
            get 
            { 
                var result = ViewState["CalculatorMode"];
                if (result == null) return 2;
                return (int)result;
            }
            set { ViewState["CalculatorMode"] = value; }
        }

        /// <summary>
        /// Gets or sets the calculator introduction text.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control validation group.
        /// </summary>
        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"] as string; }
            set { ViewState["ValidationGroup"] = value; }
        }

		//////////////////////////////////////////////////////////////////////////////
		// PUBLIC VARIABLES
		private double _baserate;
		private double _baseratefix;
		private double _loanamount;
		private double _interestovertermvar;
		private double _interestovertermfix;

		private double _initiationfee;
        private double InitiationFee
		{
			set
			{
				_initiationfee = value;
                lblBondPrepFee.Text = value.ToString(CurrencyFormat);
			}
		}
        
		private double _cancellationfee;
        private double CancellationFee
		{
			set
			{
				_cancellationfee = value;
                lblCancellationFee.Text = value.ToString(CurrencyFormat);
			}
		}

		private double FinanceChargesVar
		{
            set { lblIntPaidTermVar.Text = value.ToString(CurrencyFormatNoCents); }
		}

    	/// <summary>
		/// The percentage of the Loan amount to fix for VariFix product
		/// </summary>
		private double FixPercent
		{
			get { return (rbFixedRate.Checked && !string.IsNullOrWhiteSpace(tbFixPercentage.Text)) ? Convert.ToDouble(tbFixPercentage.Text) / 100.0 : 0.0; }
			set
			{
                lblFixPercent.Text = value.ToString(RateFormat);
                lblFixedPercent.Text = value.ToString(RateFormat);
                lblVariablePercent.Text = (1 - value).ToString(RateFormat);
			}
		}

		private double FinanceChargesFix
		{
            set { lblIntPaidTermFix.Text = value.ToString(CurrencyFormatNoCents); }
		}
        
		private double FinanceChargesTotal
		{
			set
			{
                lblSAHLIntOverTerm.Text = value.ToString(CurrencyFormatNoCents);
                lblTotFixIntPaidTerm.Text = value.ToString(CurrencyFormatNoCents);
			}
		}
        
		private double _interiminterest;

		private double InterimInterest
		{
			get
			{
                if (MortgageLoanPurpose == BaseCalculator.MortgageLoanPurposesSwitchloan())
					return _interiminterest;

				return 0;
			}
			set
			{
                lblInterimIntProv.Text = value.ToString(CurrencyFormat);
				_interiminterest = value;
			}
		}

		private double _loanamountvar;
		private double LoanAmountVar
		{
			set
			{
                lblVarLoanAmount.Text = value.ToString(CurrencyFormatNoCents);
				_loanamountvar = value;
			}
		}

		private double _loanamountfix;
		private double LoanAmountFix
		{
			//get { return loanamountfix; }
			set
			{
                lblFixLoanAmount.Text = value.ToString(CurrencyFormatNoCents);
				_loanamountfix = value;

			}
		}

		//private double loanamounttotal;
		private double LoanAmountTotal
		{
			set
			{
				//loanamounttotal = value;
                lblSAHLTotLoan.Text = value.ToString(CurrencyFormatNoCents);
                lblTotalFixLoan.Text = value.ToString(CurrencyFormatNoCents);
			}
		}

		private double _linkrate;
		private double LinkRate
		{
			set { _linkrate = value; }
		}

		private double _interestrate;
		private double InterestRate
		{
			set
			{
				_interestrate = value;
                lblSAHLIntRate.Text = value.ToString(RateFormat);
                lblVarRate.Text = value.ToString(RateFormat);
			}
		}

		private double _interestratefix;
		private double InterestRateFix
		{
			set
			{
				_interestratefix = value;
                lblFixRate.Text = value.ToString(RateFormat);
			}
		}

		private double _instalmenttotal;
		private double InstalmentTotal
		{
			get { return _instalmenttotal; }
			set
			{
                lblSAHLMonthlyInst.Text = value.ToString(CurrencyFormatNoCents);
                lblTotFixMonthlyInst.Text = value.ToString(CurrencyFormatNoCents);
				_instalmenttotal = value;
			}
		}

		private double _instalmentvar;
		private double InstalmentVar
		{
			set
			{
				_instalmentvar = value;
                lblVarMonthlyInst.Text = value.ToString(CurrencyFormatNoCents);
			}
		}

		private double _instalmentfix;
		private double InstalmentFix
		{
			set
			{
                lblFixMonthlyInst.Text = value.ToString(CurrencyFormatNoCents);
				_instalmentfix = value;
			}
		}

		private double _pti;
		private double Pti
		{
			//get { return pti; }
			set
			{
                lblPTI.Text = value.ToString(RateFormat);
				_pti = value; //to get again
			}
		}

		private double _ptifix;
		private double PtiFix
		{
			set
			{
				//ptifix = value;
                lblFixPTI.Text = value.ToString(RateFormat);
			}
		}
        
		private double _ltv;
		private double Ltv
		{
			set
			{
                lblLTV.Text = value.ToString(RateFormat);
				_ltv = value; //for getter
			}
		}

		private double _totalfees;
		private double TotalFees
		{
			get
			{
				return _totalfees;
			}
			set
			{
                lblTotalFees.Text = value.ToString(CurrencyFormat);
				_totalfees = value;
			}
		}
        
		private double _percentfix;
		private double _registrationfee;
		
		private double RegistrationFee
		{
			set
			{
				_registrationfee = value;
                lblRegFee.Text = value.ToString(CurrencyFormat);
			}
		}
        
		private int ProductKey
		{
			get
			{

				switch (CalculatorMode)
				{
					case 2: // CalculatorMode = 2 - Switch Loan (mode used here)
						if (rbVariableRate.Checked) return BaseCalculator.ProductsSwitchLoanNewVariableLoan();
                        if (rbFixedRate.Checked) return BaseCalculator.ProductsSwitchLoanVariFixLoan();
						//if (rbProduct2.Checked) return webCalculatorBase.ProductsSwitchLoanSuperLo();
						break;
					case 3: // CalculatorMode= 3 - New Purchase (mode used here)
                        if (rbVariableRate.Checked) return BaseCalculator.ProductsNewPurchaseNewVariableLoan();
                        if (rbFixedRate.Checked) return BaseCalculator.ProductsNewPurchaseVariFixLoan();
						//if (rbProduct2.Checked) return webCalculatorBase.ProductsNewPurchaseSuperLo();
						break;
					case 4: // CalculatorMode = 4 - Refinance 
                        if (rbVariableRate.Checked) return BaseCalculator.ProductsRefinanceNewVariableLoan();
                        if (rbFixedRate.Checked) return BaseCalculator.ProductsRefinanceVariFixLoan();
						//if (rbProduct2.Checked) return webCalculatorBase.ProductsRefinanceSuperLo();
						break;
				}

                return BaseCalculator.VariableLoan();
			}
		}

		private int MortgageLoanPurpose
		{
			get
			{
				switch (CalculatorMode)
				{
					case 2: return BaseCalculator.MortgageLoanPurposesSwitchloan();
					case 3: return BaseCalculator.MortgageLoanPurposesNewpurchase();
					default: return BaseCalculator.MortgageLoanPurposesRefinance();
				}
			}
		}

		/// <summary>
		/// The employment type key
		/// </summary>
		private int EmploymentTypeKey
		{
			get
			{
				if (rbSalaried.Checked) return BaseCalculator.EmploymentTypeSalaried();
				return BaseCalculator.EmploymentTypeSelfEmployed();
			}

		}
        
		/// <summary>
		/// Market value of the property
		/// </summary>
		private double EstimatedPropertyValue
		{
			get
			{
				switch (CalculatorMode)
				{
					case 3: //Newpurchase
						return tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0;
					default:
						return tbMarketValue.Text.Length > 0 ? Convert.ToDouble(tbMarketValue.Text) : 0;
				}
			}
		}
        
		/// <summary>
		/// Deposit to pay for a new purchase
		/// </summary>
		private double Deposit
		{
			get
			{
				if (CalculatorMode == 3 && tbCashDeposit.Text.Length > 0)
					return Convert.ToDouble(tbCashDeposit.Text);
				return 0;
			}
		}

		/// <summary>
		/// Cash value required by the client for switch and refinance
		/// </summary>
		private double CashOut
		{
			get
			{
				if ((CalculatorMode == 2 | CalculatorMode == 4) && tbCashOut.Text.Length > 0)
					return Convert.ToDouble(tbCashOut.Text);
				return 0;
			}
		}

		/// <summary>
		/// The term of the Loan in months
		/// </summary>
		private Int16 Term
		{
			get { return tbLoanTerm.Text.Length > 0 ? Convert.ToInt16(tbLoanTerm.Text) : (Int16)0; }

		}
        
		/// <summary>
		/// Should fees be capitalised to the Loan amount for switch and refinance
		/// </summary>
		private bool CapitaliseFees
		{
			get
			{
				if (CalculatorMode == 3) return false;
				return ((CalculatorMode == 2 | CalculatorMode == 4) && chkCapitaliseFees.Checked);
			}
		}

		/// <summary>
		/// Indicates interest only mortgage loan application
		/// </summary>
		private bool InterestOnly
		{
			get
			{
				return ((rbFixedRate.Checked == false) && chkInterestOnly.Checked);
			}
		}

		/// <summary>
		/// Total income of the household occupants
		/// </summary>
		private double HouseholdIncome
		{
			get { return tbHouseholdIncome.Text.Length > 0 ? Convert.ToDouble(tbHouseholdIncome.Text) : 0; }
		}

		/// <summary>
		/// Loan amount required
		/// </summary>
		private double LoanAmountRequired
		{
			get
			{
				double bondrequired;
				switch (CalculatorMode)
				{
					case 2://Switchloan
						bondrequired = (tbCurrentLoan.Text.Length > 0 ? Convert.ToDouble(tbCurrentLoan.Text) : 0) + (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0) + InterimInterest; //include interim interest
						break;
					case 3://Newpurchase
						bondrequired = (tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0) - (tbCashDeposit.Text.Length > 0 ? Convert.ToDouble(tbCashDeposit.Text) : 0);
						break;
					case 4://Refinance
						bondrequired = (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0);
						break;
					default:
						bondrequired = 0;
						break;
				}

				if (CapitaliseFees)
					bondrequired += TotalFees;

				return bondrequired;
			}
		}
        
		/// <summary>
		/// Loan amount required
		/// </summary>
		private double TotalLoanAmountRequired
		{
			get
			{
				double bondrequired;
				switch (CalculatorMode)
				{
					case 2://Switchloan
						bondrequired = (tbCurrentLoan.Text.Length > 0 ? Convert.ToDouble(tbCurrentLoan.Text) : 0) + (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0) + InterimInterest; //include interim interest
						break;
					case 3://Newpurchase
						bondrequired = (tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0);
						break;
					case 4://Refinance
						bondrequired = (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0);
						break;
					default:
						bondrequired = 0;
						break;
				}

				if (CapitaliseFees)
					bondrequired += TotalFees;

				return bondrequired;
			}
		}

        /// <summary>
        /// Gets the purchase price.
        /// </summary>
		private double PurchasePrice
		{
			get { return string.IsNullOrWhiteSpace(tbPurchasePrice.Text) ? 0.0 : Convert.ToDouble(tbPurchasePrice.Text); }
		}

	    /// <summary>
	    /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
	    /// </summary>
	    /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
	    protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack) Reset();

            applicationCalculator.BaseCalculator = BaseCalculator;
            if (!IsPostBack) applicationCalculator.Reset(); // just make sure the application form was reset.

	        SetupErrorMessages();
        }

	    /// <summary>
	    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
	    /// </summary>
	    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
	    protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

            lblDescription.Text = Description;

			SetupCalculatorCaptureFields();
			if (IsPostBack)
			{
				if (chkCapitaliseFees.Checked & chkCapitaliseFees.Visible)
				{
					//lblFeeInfo.Text = "** Total Fees included because you elected to have them capitalised.";
					lblFeeInfoFix.Text = "** Total Fees included because you elected to have them capitalised.";
					lblFeeInfoInd.Text = "**";
				}
				else
				{
					// lblFeeInfo.Text = "";
					lblFeeInfoFix.Text =
					lblFeeInfoInd.Text = "";
				}
			}
			SetupControlState();
            RegisterClientScripts();

            DataBind();
		}

		private void StoreCalculatorVariables()
		{
			//TODO this line below might be throwing an error.....
            var calculator = BaseCalculator;

            calculator.PreProspect.PurposeNumber = CalculatorMode;

            if (rbFixedRate.Checked) calculator.PreProspect.Product = 1;
			//if (rbProduct2.Checked) webCalculatorBase.PreProspect.Product = 2;
            if (rbVariableRate.Checked) calculator.PreProspect.Product = 3;

            if (rbSalaried.Checked) calculator.PreProspect.IncomeType = 1;
            if (rbSelfEmployed.Checked) calculator.PreProspect.IncomeType = 2;

            if (tbPurchasePrice.Text.Length > 0) calculator.PreProspect.LoanAmountRequired = Convert.ToInt32(tbPurchasePrice.Text);
            if (tbCashDeposit.Text.Length > 0) calculator.PreProspect.Deposit = Convert.ToInt32(tbCashDeposit.Text);
            if (tbMarketValue.Text.Length > 0) calculator.PreProspect.EstimatedPropertyValue = Convert.ToInt32(tbMarketValue.Text);
            if (tbCurrentLoan.Text.Length > 0) calculator.PreProspect.CurrentLoan = Convert.ToInt32(tbCurrentLoan.Text);
			if (tbCashOut.Text.Length > 0)
                calculator.PreProspect.CashOut = Convert.ToInt32(tbCashOut.Text);
            if (tbLoanTerm.Text.Length > 0) calculator.PreProspect.Term = Convert.ToInt32(tbLoanTerm.Text);
            calculator.PreProspect.CapitaliseFees = CapitaliseFees;// chkCapitaliseFees.Checked;
            calculator.PreProspect.InterestOnly = chkInterestOnly.Checked;
			if (rbFixedRate.Checked) // Fixed rate loan
                calculator.PreProspect.FixPercent = tbFixPercentage.Text.Length > 0 ? Convert.ToDouble(tbFixPercentage.Text) : 0;

            if (tbHouseholdIncome.Text.Length > 0) calculator.PreProspect.HouseholdIncome = Convert.ToInt32(tbHouseholdIncome.Text);
		}

		private void RetrieveCalculatorVariables()
		{
		    var calculator = BaseCalculator;

            CalculatorMode = calculator.PreProspect.PurposeNumber;
            switch (calculator.PreProspect.Product)
			{
				case 1:
					rbFixedRate.Checked = true;
					break;
				//case 2:
				//    rbProduct2.Checked = true;
				//    break;
				case 3:
					rbVariableRate.Checked = true;
					break;
			}

            if (calculator.PreProspect.IncomeType == 1) rbSalaried.Checked = true;
            if (calculator.PreProspect.IncomeType == 2) rbSelfEmployed.Checked = true;


            tbPurchasePrice.Text = Convert.ToString(calculator.PreProspect.LoanAmountRequired);
            tbCashDeposit.Text = Convert.ToString(calculator.PreProspect.Deposit);
            tbMarketValue.Text = Convert.ToString(calculator.PreProspect.EstimatedPropertyValue);
            tbCurrentLoan.Text = Convert.ToString(calculator.PreProspect.CurrentLoan);
            tbCashOut.Text = Convert.ToString(calculator.PreProspect.CashOut);
            tbLoanTerm.Text = Convert.ToString(calculator.PreProspect.Term);
            chkCapitaliseFees.Checked = calculator.PreProspect.CapitaliseFees;
            chkInterestOnly.Checked = calculator.PreProspect.InterestOnly;
            tbFixPercentage.Text = Convert.ToString(calculator.PreProspect.FixPercent);
            tbHouseholdIncome.Text = Convert.ToString(calculator.PreProspect.HouseholdIncome);


		}
        
        // This method sets the Control up for the correct View state of the Component
		private void SetupControlState()
		{
			if (CalculatorMode == 2) // Switch Loan
			{
				pnlPurchasePrice.Visible = false;
                pnlCashDeposit.Visible = false;
                bttnCalculate.Text = "Calculate how much you can save";
			}
			else // New Loan
			{
                pnlPurchasePrice.Visible = true;
                pnlCashDeposit.Visible = true;
                bttnCalculate.Text = "Calculate what you can afford";
			}
		}

        /// <summary>
        /// Resets the application form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Reset(object sender, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Displays the application form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Apply(object sender, EventArgs e)
        {
            pnlGeneralCalculator.Visible = false;
            applicationCalculator.Visible = true;

            ScriptManager.RegisterClientScriptBlock(pnlGeneralCalculator, GetType(), "Calculator.Tracking", "if (_gaq) _gaq.push([\"_trackPageview\", \"/track/application-displayed\"]);", true);
        }

        /// <summary>
        /// Calculates the loan affordability.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected void Calculate(object sender, EventArgs e)
		{
            StoreCalculatorVariables();

            var calculator = BaseCalculator;

			double employmentPTI = 0d;
			RetrieveCalculatorVariables();

			CreditDisqualifications creditDisqualifications = new CreditDisqualifications();
			creditDisqualifications.calculationdone = false;
			creditDisqualifications.ltv = 0;
			creditDisqualifications.pti = 0;
			creditDisqualifications.householdincome = HouseholdIncome;
			creditDisqualifications.loanamountrequired = LoanAmountRequired;
			creditDisqualifications.estimatedproeprtyvalue = EstimatedPropertyValue;
			creditDisqualifications.EmploymentTypeKey = EmploymentTypeKey;
			creditDisqualifications.ifFurtherLending = false;
			creditDisqualifications.term = Term;

            creditDisqualifications = calculator.CreditDisqualifications(creditDisqualifications);

			Ltv = creditDisqualifications.ltv;

			if (EmploymentTypeKey == 1)
			{
                Pti = calculator.GetControlValueByDescription("Calc - MaxPTI Salaried");
                employmentPTI = calculator.GetControlValueByDescription("Calc - MaxPTI Salaried");
			}

			if (EmploymentTypeKey == 2)
			{
                Pti = calculator.GetControlValueByDescription("Calc - MaxPTI SelfEmployed");
                employmentPTI = calculator.GetControlValueByDescription("Calc - MaxPTI SelfEmployed");
			}


			//PTI = creditDisqualifications.pti;
			_loanamount = creditDisqualifications.loanamountrequired;

            _baserate = calculator.ReturnBaseRate();
            _baseratefix = calculator.ReturnBaseRateFixed();

			// 1. Calculate Fees
            int appType = calculator.ReturnApplicationTypeByCalculatorMode(CalculatorMode);

			//remove any calculated additions from the loan amount
			_loanamount = LoanAmountRequired - InterimInterest;

			// remove Total fees if applicable
			if (CapitaliseFees)
				_loanamount -= TotalFees;

			double bondtoregister = 0d;
			OriginationFees originationfees = new OriginationFees();

			originationfees.appType = appType;
			originationfees.bondrequired = 0;
			originationfees.bondtoregister = bondtoregister;
			originationfees.cancellationfee = _cancellationfee;
			originationfees.capitalisefees = CapitaliseFees;
			originationfees.cashout = CashOut;
			originationfees.initiationfee = _initiationfee;
			originationfees.interiminterest = _interiminterest;
			originationfees.loanamount = LoanAmountRequired;
			originationfees.OverrideCancelFeeamount = 0;
			originationfees.registrationfee = _registrationfee;

            originationfees = calculator.CalculateOriginationFees(originationfees);

			_initiationfee = originationfees.initiationfee;
			_registrationfee = originationfees.registrationfee;
			_cancellationfee = originationfees.cancellationfee;
			_interiminterest = originationfees.interiminterest;
			//bondtoregister = originationfees.bondtoregister;

			// add any newly calculated interim interest back
			_loanamount += _interiminterest;

			_totalfees = _initiationfee + _registrationfee + _cancellationfee;

			// add Total fees if applicable
			if (CapitaliseFees)
				_loanamount += _totalfees;

			//ltv = webCalculatorBase.CalculateLTV(loanamount, EstimatedPropertyValue);
            Ltv = calculator.CalculateLTVThroughBusinessModel(_loanamount, EstimatedPropertyValue);
            int oskey = calculator.OriginationSourcesSAHomeLoans();

            CreditCriteria creditCriteria = calculator.SetupCreditCriteria(oskey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, _loanamount, EstimatedPropertyValue, _baserate, Term, HouseholdIncome);

            calculator.PreProspect.CreditMatrixKey = creditCriteria.CreditCriteriaCreditMatrixKey;

			_linkrate = creditCriteria.CreditCriteriaMarginValue;
			_interestrate = _linkrate + _baserate;
			_interestratefix = _linkrate + _baseratefix;
			_percentfix = FixPercent;
			LinkRate = _linkrate;
			InterestRateFix = _interestratefix;
			InterestRate = _interestrate;


			//Calculate instalment
            _instalmentvar = calculator.CalculateInstallment(_loanamount, _interestrate, Term, InterestOnly);

			//Calculate PTI: always done against the full amortising instalment
			//pti = Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanamount, interestrate, Term, false), HouseholdIncome);
            Pti = calculator.CalculatePTIThroughBusinessModel(_loanamount, _linkrate, _baserate, Term, HouseholdIncome);


			//Calculate interest over term
			//interestovertermvar = Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);
            _interestovertermvar = calculator.CalculateInterestOverTerm(_loanamount, _interestrate, Term, InterestOnly);

			//Calculate fixed amounts
            if (ProductKey == calculator.VariFixLoan())
			{
				_loanamountfix = (_loanamount * _percentfix);
				_loanamountvar = (_loanamount * (1 - _percentfix));

				//instalment VF accounts can not be interest only
				//fix
                _instalmentfix = calculator.CalculateInstallment(_loanamountfix, _interestratefix, Term, false); // 
				//var
                _instalmentvar = _loanamountvar <= 0 ? 0 : calculator.CalculateInstallment(_loanamountvar, _interestrate, Term, false);

				//pti
				//ptifix = Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI((instalmentfix + instalmentvar), HouseholdIncome);
                _ptifix = calculator.CalculatePTI((_instalmentfix + _instalmentvar), HouseholdIncome);
				//ptifix = webCalculatorBase.CalculatePTIThroughBusinessModel(loanamountfix, linkrate, baserate, Term, HouseholdIncome);

				//interest over term
				//Fix
                _interestovertermfix = calculator.CalculateInterestOverTerm(_loanamountfix, _interestratefix, Term, false);
				//Var
                _interestovertermvar = _loanamountvar <= 0 ? 0 : calculator.CalculateInterestOverTerm(_loanamountvar, _interestrate, Term, false);
			}

			//Calculate min income against the CC max pti
			_instalmenttotal = _instalmentvar + _instalmentfix;
            double requiredincome = calculator.CalculateMinimumIncomeRequired(InstalmentTotal, creditCriteria.CreditCriteriaPTIValue);
			// set display of the form

			//Warnings:

			//ltv pti
			//LTV = ltv;
			//PTI = pti;
			PtiFix = _ptifix;

			//Percentages
			FixPercent = FixPercent;

			//Amounts
			LoanAmountTotal = _loanamount;
			LoanAmountFix = _loanamountfix;
			LoanAmountVar = _loanamountvar;

			//instalment
			InstalmentVar = _instalmentvar;
			InstalmentFix = _instalmentfix;
			InstalmentTotal = _instalmentvar + _instalmentfix;

			//finance charges
			FinanceChargesVar = _interestovertermvar;
			FinanceChargesFix = _interestovertermfix;
			FinanceChargesTotal = _interestovertermvar + _interestovertermfix;

			//Fees
			CancellationFee = _cancellationfee;
			RegistrationFee = _registrationfee;
			InitiationFee = _initiationfee;
			TotalFees = _totalfees;
			InterimInterest = _interiminterest;

			// Summary Display
            lblPurchasePrice.Text = PurchasePrice.ToString(CurrencyFormat);
            lblCashDeposit.Text = Deposit.ToString(CurrencyFormat);



			creditDisqualifications.calculationdone = true;
			creditDisqualifications.ltv = _ltv;
			creditDisqualifications.pti = _pti;
			creditDisqualifications.householdincome = HouseholdIncome;
			creditDisqualifications.loanamountrequired = _loanamount;
			creditDisqualifications.estimatedproeprtyvalue = EstimatedPropertyValue;
			creditDisqualifications.EmploymentTypeKey = EmploymentTypeKey;
			creditDisqualifications.ifFurtherLending = true;
			creditDisqualifications.term = Term;

            creditDisqualifications = calculator.CreditDisqualifications(creditDisqualifications);

			_ltv = creditDisqualifications.ltv;
			_pti = creditDisqualifications.pti;
			_loanamount = creditDisqualifications.loanamountrequired;


			creditDisqualifications.term = Term;

			// Store the session variables for the summary
			//PreProspect.LoanAmountRequired = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(loanamount);

            calculator.PreProspect.CategoryKey = creditCriteria.CreditCriteriaCategoryKey;

            calculator.PreProspect.EstimatedPropertyValue = EstimatedPropertyValue;
            calculator.PreProspect.MortgageLoanPurpose = MortgageLoanPurpose;
            calculator.PreProspect.InterestRate = _interestrate;
            calculator.PreProspect.LinkRate = _linkrate;
            calculator.PreProspect.RegistrationFee = _registrationfee;
            calculator.PreProspect.CancellationFee = _cancellationfee;
            calculator.PreProspect.ValuationFee = 0;
            calculator.PreProspect.InitiationFee = _initiationfee;
            calculator.PreProspect.TransferFee = 0;
            calculator.PreProspect.TotalFee = TotalFees;
            calculator.PreProspect.Term = Term;
            calculator.PreProspect.HouseholdIncome = HouseholdIncome;
            calculator.PreProspect.PurchasePrice = LoanAmountRequired + InterimInterest;
            calculator.PreProspect.TotalPrice = TotalLoanAmountRequired;

            calculator.PreProspect.MarginKey = creditCriteria.CreditCriteriaMarginKey;

            calculator.PreProspect.ElectedFixedRate = _interestratefix;
            calculator.PreProspect.EmploymentType = EmploymentTypeKey;

            calculator.PreProspect.PTI = _pti; //PTI
            calculator.PreProspect.LTV = _ltv;//LTV

            calculator.PreProspect.Product = ProductKey;
            calculator.PreProspect.ProductKey = ProductKey;
            calculator.PreProspect.LoanAmountRequired = Math.Round(_loanamount);

			// Set up the Calculator warning Message
			lblNotQualifyMsg.Text = "";
            //this max LTV check, not min as the parameter describes
			double minLTV = MinLtv / 100;
            //The LTV limits could be different depending on the employment type
            //the existing LTV values are based on Salaried, so only need to check Self Employed
            if (EmploymentTypeKey != 1)
                minLTV = MaxSelfLTV / 100;

			double maxPTI = MaxPti / 100;
			bool hasWarning = false;
			string notQualifyMsg = string.Empty;
			string employmentType = EmploymentTypeKey == 1 ? "Salaried" : "Self-Employed";

			if (employmentPTI <= _pti || minLTV <= _ltv)
			{
                notQualifyMsg = string.Format(@" SA Home Loans lending policy for {0} applicants allows loans up to {1}% LTV ratio and PTI ratio of no more than {2}%. Please call us on 0860 2 4 6 8 10 should you require assistance in structuring your home loan.", employmentType, Convert.ToString(minLTV * 100.0).ToString(), (employmentPTI * 100.0).ToString("0"));
				lblNotQualifyMsg.Text = notQualifyMsg;
				hasWarning = true;
			}
			

			if (!hasWarning)
			{
				lblNotQualifyMsg.Text = string.Empty;
				lblNotQualifyMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#f37021");
				lblNotQualifyMsg.Text = "Congratulations - SA Home Loans would welcome the opportunity to offer you home loan finance.";
				lblQualifyMsg.Text = "The application provisionally qualifies";
			}
			else
			{
				lblNotQualifyMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
				lblQualifyMsg.Text = string.Empty;
			}


			lblQualifyMsg.Visible = true;

			SetupSummaryDisplay(ProductKey);

            pnlResults.Visible = true;
            bttnCalculate.Visible = false;
            pnlFiller.Visible = false;

            tbPurchasePrice.Enabled = false;
            tbCashDeposit.Enabled = false;
            tbMarketValue.Enabled = false;
            tbCurrentLoan.Enabled = false;
            tbCashOut.Enabled = false;
            rbSalaried.Enabled = false;
            tbHouseholdIncome.Enabled = false;
            rbVariableRate.Enabled = false;
            tbLoanTerm.Enabled = false;
            tbFixPercentage.Enabled = false;
            chkCapitaliseFees.Enabled = false;
            chkInterestOnly.Enabled = false;
		}
        
        /// <summary>
        /// Resets the form.
        /// </summary>
        /// <param name="resetState">Specifies whether to reset the calculator state as well.</param>
        public void Reset(bool resetState = true)
        {
            var offline = false;

            pnlCalculatorOffline.Visible = false;
            pnlCalculatorOnline.Visible = true;

            var calculator = BaseCalculator;
            if (resetState)
            {
                calculator = new WebCalculatorBase();
                if (calculator.PreProspect != null)
                {
                    try
                    {
                        CurrencyFormat = calculator.CurrencyFormat();
                        CurrencyFormatNoCents = calculator.CurrencyFormatNoCents();
                        RateFormat = calculator.RateFormat();
                        MaxPti = calculator.GetMaximumPTI();
                        MinLtv = calculator.GetMinimumLTV();
                        MaxSelfLTV = calculator.GetMaximumSelfEmployedLTV();
                        MaxTerm = calculator.GetMaximumTerm();
                        MinHouseholdIncome = calculator.GetMinimumHouseholdIncome();
                        MinLoanAmount = calculator.GetMinimumPurchasePrice();
                        MaxLoanAmount = calculator.GetMaximumPurchasePrice();
                        MinMarketValue = calculator.GetMinimumMarketValue();
                        MinSwitchLoanAmount = calculator.GetMinimumPurchasePrice();
                        MinFixFinServiceAmount = calculator.GetFlexiMinFixed();
                    }
                    catch
                    {
                        offline = true;
                    }
                }
                else
                    offline = true;

                BaseCalculator = calculator;
            }

            if (calculator != null)
            {
                if (calculator.CreatePreProspect()) // Initialize a new preprospect object.
                {
                    var session = SahlSession;
                    if (SahlSession != null)
                    {
                        calculator.PreProspect.ReferringServerURL = session.URLReferrer;
                        calculator.PreProspect.UserURL = session.HostAddress;
                        calculator.PreProspect.AdvertisingCampaignID = session.QueryString;
                        calculator.PreProspect.UserAddress = session.UserHostName;
                        calculator.PreProspect.OfferSubmitted = session.ApplicationSubmitted;
                        calculator.PreProspect.ApplicationKey = session.ApplicationKey.GetValueOrDefault();
                    }
                }
                else
                    offline = true;
            }

            if (offline)
            {
                pnlCalculatorOffline.Visible = true;
                pnlCalculatorOnline.Visible = false;
            }

	        tbPurchasePrice.Text = "0";
            tbPurchasePrice.Enabled = true;
            tbCashDeposit.Text = "0";
            tbCashDeposit.Enabled = true;
            tbMarketValue.Text = "0";
            tbMarketValue.Enabled = true;
            tbCurrentLoan.Text = "0";
            tbCurrentLoan.Enabled = true;
            tbCashOut.Text = "0";
            tbCashOut.Enabled = true;
            rbSalaried.Checked = true;
            rbSalaried.Enabled = true;
            tbHouseholdIncome.Text = "0";
            tbHouseholdIncome.Enabled = true;
            rbVariableRate.Checked = true;
            rbVariableRate.Enabled = true;
            tbLoanTerm.Text = "240";
            tbLoanTerm.Enabled = true;
            tbFixPercentage.Text = "100";
            tbFixPercentage.Enabled = true;
            chkCapitaliseFees.Checked = true;
            chkCapitaliseFees.Enabled = true;
            chkInterestOnly.Checked = false;
            chkInterestOnly.Enabled = true;
            if (!offline) applicationCalculator.Reset();

            pnlResults.Visible = false;
            pnlFiller.Visible = true;
            pnlGeneralCalculator.Visible = true;
            applicationCalculator.Visible = false;
            bttnCalculate.Visible = true;
        }

		private void SetupSummaryDisplay(int productkey)
		{
			// Set display of the form
			if (productkey == BaseCalculator.VariFixLoan())
			{
				pnlTotalLoanRequirement.Visible = false;
                pnlInterestRate.Visible = false;
                pnlMonthlyInstallment.Visible = false;
                pnlInterestPaidOverTerm.Visible = false;
                tblVarifix.Visible = true;
				pnlVarifixPti.Visible = true;
				pnlSuperLoPti.Visible = false;
			}
			else
			{
                pnlTotalLoanRequirement.Visible = true;
                pnlInterestRate.Visible = true;
                pnlMonthlyInstallment.Visible = true;
                pnlInterestPaidOverTerm.Visible = true;
                tblVarifix.Visible = false;
				pnlVarifixPti.Visible = false;
				pnlSuperLoPti.Visible = true;
			}
		}

		private void SetupCalculatorCaptureFields()
		{
            pnlMarketValue.Visible = false;
            pnlCurrentLoan.Visible = false;
            pnlCashOut.Visible = false;
            pnlPurchasePriceInput.Visible = false;
            pnlCashDepositInput.Visible = false;

			switch (CalculatorMode)
			{
				case 2: // switch loan
                    pnlMarketValue.Visible = true;
                    pnlCurrentLoan.Visible = true;
                    pnlCashOut.Visible = true;
					break;
				case 3: // new purchase
                    pnlPurchasePriceInput.Visible = true;
                    pnlCashDepositInput.Visible = true;
					break;
				case 4: //Refinance
                    pnlMarketValue.Visible = true;
                    pnlCashOut.Visible = true;
					break;
			}
		}

		/// <summary>
		/// This method sets up the parameters for validation according to SAHL business rules
		/// </summary>
		private void SetupErrorMessages()
		{

            cvHouseholdIncome.ErrorMessage = "Your gross income must exceed " + MinHouseholdIncome.ToString(CurrencyFormat) + " to qualify.";
			cvHouseholdIncome.ValueToCompare = Convert.ToString(MinHouseholdIncome);

            cvMarketValue.ErrorMessage = "Your home's value must be at least " + MinMarketValue.ToString(CurrencyFormat);
			cvMarketValue.ValueToCompare = Convert.ToString(MinMarketValue);

			rvLoanTerm.MinimumValue = Convert.ToString(1);
            rvLoanTerm.MaximumValue = Convert.ToString(MaxTerm);
            rvLoanTerm.ErrorMessage = "The loan term must be between 1 and" + Convert.ToString(MaxTerm) + " months.";

			cvCurrentLoanAmount.MinimumValue = Convert.ToString(MinMarketValue);
			cvCurrentLoanAmount.MaximumValue = Convert.ToString(MaxLoanAmount);
            cvCurrentLoanAmount.ErrorMessage = "Your total loan amount (including cash out) must be between " + MinMarketValue.ToString(CurrencyFormat) + " and " + MaxLoanAmount.ToString(CurrencyFormat);

		}

        /// <summary>
		/// Create and Register the Views Javascript Model
		/// </summary>
		private void RegisterClientScripts()
		{
            var options = new
                              {
                                  controls = new
                                                 {
                                                     rbFixedRate = rbFixedRate.ClientID,
                                                     rbVariableRate = rbVariableRate.ClientID,
                                                     chkCapitaliseFees = chkCapitaliseFees.ClientID,
                                                     chkInterestOnly = chkInterestOnly.ClientID,
                                                     pnlFixPercentage = pnlFixPercentage.ClientID,
                                                     tbCurrentLoan = tbCurrentLoan.ClientID,
                                                     tbPurchasePrice = tbPurchasePrice.ClientID,
                                                     tbCashOut = tbCashOut.ClientID,
                                                     tbFixPercentage = tbFixPercentage.ClientID,
                                                     tbLoanTerm = tbLoanTerm.ClientID,
                                                     tbCashDeposit = tbCashDeposit.ClientID,
                                                     tbMarketValue = tbMarketValue.ClientID,
                                                     tbHouseholdIncome = tbHouseholdIncome.ClientID
                                                 },
                                  totalFees = TotalFees,
                                  minFixFinServiceAmount = MinFixFinServiceAmount,
                                  mode = CalculatorMode
                              };

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null) scriptManager.RegisterScriptControl(this);
            ScriptManager.RegisterStartupScript(pnlGeneralCalculator, GetType(), "Calculator.Initialization", "$.calculator.init(" + options.ToJson() + ");", true);
            ScriptManager.RegisterStartupScript(pnlGeneralCalculator, GetType(), "Calculator.TestimonialInit", string.Format("$('#{0} .testimonial').testimonial();", pnlFiller.ClientID), true);
		}

	    /// <summary>
	    /// Gets a collection of script descriptors that represent ECMAScript (JavaScript) client components.
	    /// </summary>
	    /// <returns>
	    /// An <see cref="T:System.Collections.IEnumerable"/> collection of <see cref="T:System.Web.UI.ScriptDescriptor"/> objects.
	    /// </returns>
	    IEnumerable<ScriptDescriptor> IScriptControl.GetScriptDescriptors()
	    {
            return new ScriptDescriptor[0];
	    }

	    /// <summary>
	    /// Gets a collection of <see cref="T:System.Web.UI.ScriptReference"/> objects that define script resources that the control requires.
	    /// </summary>
	    /// <returns>
	    /// An <see cref="T:System.Collections.IEnumerable"/> collection of <see cref="T:System.Web.UI.ScriptReference"/> objects.
	    /// </returns>
	    IEnumerable<ScriptReference> IScriptControl.GetScriptReferences()
	    {
            return new []
                       {
                           new ScriptReference("SAHL.Internet.Components.Calculators.Scripts.Input.js", Assembly.GetExecutingAssembly().FullName),
                           new ScriptReference("SAHL.Internet.Components.Calculators.Scripts.GeneralCalculator.js", Assembly.GetExecutingAssembly().FullName)
                       };
	    }
	}
}