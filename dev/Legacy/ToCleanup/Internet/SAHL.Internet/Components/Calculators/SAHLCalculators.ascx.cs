using System;
using System.Configuration;
using SAHL.Internet.SAHL.Web.Services.Calculators;
using CreditDisqualifications = SAHL.Internet.SAHL.Web.Services.Calculators.CreditDisqualifications;

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
	/// 
	/// </summary>

	public partial class SAHLCalculator : System.Web.UI.UserControl
	{
		//Public Variables are set up per instance in DotNetNuke
		// Viewtype describes what the claculator is being used for
		// This variable is set through Open Smart module 
		// calculator Mode defaults to 2 - Switch loaan;
		/// <summary>
		/// 
		/// </summary>
		public int CalculatorMode = 2;
		/// <summary>
		/// 
		/// </summary>
		public string CalculatorName = "";
		/// <summary>
		/// 
		/// </summary>
		public string CalculatorDescriptionText = "";

		private WebCalculatorBase webCalculatorBase = new WebCalculatorBase();
		private SAHLWebSession DNNWebSession = new SAHLWebSession();
		///private bool RulesTestPassed;

		private double MinLoanAmount
		{
			get { return webCalculatorBase.GetMinimumPurchasePrice(); }
		}

		private double MaxLoanAmount
		{
			get { return webCalculatorBase.GetMaximumPurchasePrice(); }
		}

		private double MaxPTI
		{
			get { return webCalculatorBase.GetMaximumPTI(); }
		}

		private double MinLTV
		{
			get { return webCalculatorBase.GetMinimumLTV(); }
		}

		private double MaxTerm
		{
			get { return webCalculatorBase.GetMaximumTerm(); }
		}


		private double MinHouseholdIncome
		{
			get { return webCalculatorBase.GetMinimumHouseholdIncome(); }
		}

		private double MinMarketValue
		{
			get { return webCalculatorBase.GetMinimumMarketValue(); }
		}


		private double MinSwitchLoanAmount
		{
			get { return webCalculatorBase.GetMinimumPurchasePrice(); }
		}

		private double MinFixFinServiceAmount
		{
			get { return webCalculatorBase.GetFlexiMinFixed(); }
		}


		private string navigateto = "";
		/// <summary>
		/// Get or Set The Page Navigation Value
		/// </summary>
		public string NavigateTo
		{
			get { return navigateto; }
			set { navigateto = value; }
		}


		private string stepimageurlNewPurchase = "";
		private string stepimageurlSwitchLoan = "";

		/// <summary>
		/// Get or Set The URL for the Step image for the New Purchase Calculator
		/// </summary>
		public string StepImageURLNewPurchase
		{
			get { return stepimageurlNewPurchase; }
			set { stepimageurlNewPurchase = value; }
		}

		/// <summary>
		/// Get or Set The URL for the Step image for the Switch Loan Calculator
		/// </summary>
		public string StepImageURLSwitchLoan
		{
			get { return stepimageurlSwitchLoan; }
			set { stepimageurlSwitchLoan = value; }
		}


		//////////////////////////////////////////////////////////////////////////////
		// PUBLIC VARIABLES
		private double baserate;
		private double baseratefix;
		private double loanamount;
		private double interestovertermvar;
		private double interestovertermfix;

		private double initiationfee;
		/// <summary>
		/// 
		/// </summary>
		private double InitiationFee
		{
			set
			{
				initiationfee = value;
				lblBondPrepFee.Text = value.ToString(webCalculatorBase.CurrencyFormat());
			}
		}


		private double cancellationfee;
		/// <summary>
		/// 
		/// </summary>
		private double CancellationFee
		{
			set
			{
				cancellationfee = value;
				lblCancellationFee.Text = value.ToString(webCalculatorBase.CurrencyFormat());
			}
		}


		// private double financechargesvar;
		/// <summary>
		/// 
		/// </summary>
		private double FinanceChargesVar
		{
			set
			{
				lblIntPaidTermVar.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				// financechargesvar = value;
			}
		}


		//private double fixpercent;
		/// <summary>
		/// The percentage of the Loan amount to fix for VariFix product
		/// </summary>
		private double FixPercent
		{
			get
			{
				if (rbProduct1.Checked && tbFixPercentage.Text.Length > 0)
					return Convert.ToDouble(tbFixPercentage.Text) / 100; // convert user integer to percentage
				return 0;
			}
			set
			{
				//fixpercent = value;
				lblFixPercent.Text = value.ToString(webCalculatorBase.RateFormat());
				lblFixedPercent.Text = value.ToString(webCalculatorBase.RateFormat());
				lblVariablePercent.Text = (1 - value).ToString(webCalculatorBase.RateFormat());
			}
		}


		//private double financechargesfix;
		/// <summary>
		/// 
		/// </summary>
		private double FinanceChargesFix
		{
			set
			{
				lblIntPaidTermFix.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				// financechargesfix = value;
			}
		}


		// private double financechargestotal;
		/// <summary>
		/// 
		/// </summary>
		private double FinanceChargesTotal
		{
			set
			{
				//financechargestotal = value;
				lblSAHLIntOverTerm.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				lblTotFixIntPaidTerm.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
			}
		}


		private double interiminterest;
		/// <summary>
		/// 
		/// </summary>
		private double InterimInterest
		{
			get
			{
				if (MortgageLoanPurpose == webCalculatorBase.MortgageLoanPurposesSwitchloan())
					return interiminterest;

				return 0;
			}
			set
			{
				lblInterimIntProv.Text = value.ToString(webCalculatorBase.CurrencyFormat());
				interiminterest = value;
			}
		}

		private double loanamountvar;
		private double LoanAmountVar
		{
			set
			{
				lblVarLoanAmount.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				loanamountvar = value;
			}
		}

		private double loanamountfix;
		private double LoanAmountFix
		{
			//get { return loanamountfix; }
			set
			{
				lblFixLoanAmount.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				loanamountfix = value;

			}
		}

		//private double loanamounttotal;
		private double LoanAmountTotal
		{
			set
			{
				//loanamounttotal = value;
				lblSAHLTotLoan.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				lblTotalFixLoan.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
			}
		}

		private double linkrate;
		private double LinkRate
		{
			set { linkrate = value; }
		}

		private double interestrate;
		private double InterestRate
		{
			set
			{
				interestrate = value;
				lblSAHLIntRate.Text = value.ToString(webCalculatorBase.RateFormat());
				lblVarRate.Text = value.ToString(webCalculatorBase.RateFormat());
			}
		}

		private double interestratefix;
		private double InterestRateFix
		{
			set
			{
				interestratefix = value;
				lblFixRate.Text = value.ToString(webCalculatorBase.RateFormat());
			}
		}

		private double instalmenttotal;
		private double InstalmentTotal
		{
			get { return instalmenttotal; }
			set
			{
				lblSAHLMonthlyInst.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				lblTotFixMonthlyInst.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				instalmenttotal = value;
			}
		}

		private double instalmentvar;
		private double InstalmentVar
		{
			set
			{
				instalmentvar = value;
				lblVarMonthlyInst.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
			}
		}

		private double instalmentfix;
		private double InstalmentFix
		{
			set
			{
				lblFixMonthlyInst.Text = value.ToString(webCalculatorBase.CurrencyFormatNoCents());
				instalmentfix = value;
			}
		}

		private double pti;
		private double PTI
		{
			//get { return pti; }
			set
			{
				lblPTI.Text = value.ToString(webCalculatorBase.RateFormat());
				pti = value; //to get again
			}
		}

		private double ptifix;
		private double PTIFix
		{
			set
			{
				//ptifix = value;
				lblFixPTI.Text = value.ToString(webCalculatorBase.RateFormat());
			}
		}



		private double ltv;
		private double LTV
		{
			set
			{
				lblLTV.Text = value.ToString(webCalculatorBase.RateFormat());
				ltv = value; //for getter
			}
		}

		private double totalfees;
		private double TotalFees
		{
			get
			{
				return totalfees;
			}
			set
			{
				lblTotalFees.Text = value.ToString(webCalculatorBase.CurrencyFormat());
				totalfees = value;
			}
		}


		private double percentfix;
		private double registrationfee;
		/// <summary>
		/// 
		/// </summary>
		private double RegistrationFee
		{
			set
			{
				registrationfee = value;
				lblRegFee.Text = value.ToString(webCalculatorBase.CurrencyFormat());
			}
		}


		private int ProductKey
		{
			get
			{

				switch (CalculatorMode)
				{
					case 2: // CalculatorMode = 2 - Switch Loan (mode used here)
						if (rbProduct3.Checked) return webCalculatorBase.ProductsSwitchLoanNewVariableLoan();
						if (rbProduct1.Checked) return webCalculatorBase.ProductsSwitchLoanVariFixLoan();
						//if (rbProduct2.Checked) return webCalculatorBase.ProductsSwitchLoanSuperLo();
						break;
					case 3: // CalculatorMode= 3 - New Purchase (mode used here)
						if (rbProduct3.Checked) return webCalculatorBase.ProductsNewPurchaseNewVariableLoan();
						if (rbProduct1.Checked) return webCalculatorBase.ProductsNewPurchaseVariFixLoan();
						//if (rbProduct2.Checked) return webCalculatorBase.ProductsNewPurchaseSuperLo();
						break;
					case 4: // CalculatorMode = 4 - Refinance 
						if (rbProduct3.Checked) return webCalculatorBase.ProductsRefinanceNewVariableLoan();
						if (rbProduct1.Checked) return webCalculatorBase.ProductsRefinanceVariFixLoan();
						//if (rbProduct2.Checked) return webCalculatorBase.ProductsRefinanceSuperLo();
						break;
				}

				return webCalculatorBase.VariableLoan();
			}
		}

		private int MortgageLoanPurpose
		{
			get
			{
				switch (CalculatorMode)
				{
					case 2: return webCalculatorBase.MortgageLoanPurposesSwitchloan();
					case 3: return webCalculatorBase.MortgageLoanPurposesNewpurchase();
					default: return webCalculatorBase.MortgageLoanPurposesRefinance();
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
				if (rbSalaried.Checked) return webCalculatorBase.EmploymentTypeSalaried();
				return webCalculatorBase.EmploymentTypeSelfEmployed();
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
				return ((rbProduct1.Checked == false) && chkInterestOnly.Checked);
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
		/// 
		/// </summary>
		private double PurchasePrice
		{
			get
			{
				if (tbPurchasePrice.Text.Length > 0)
				{
					double retval = Convert.ToDouble(tbPurchasePrice.Text);
					return retval;

				}
				lblPurchasePrice.Text = "R 0.00";
				return 0;
			}
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			DNNWebSession = Session["SAHLWebSession"] as SAHLWebSession;
			//CheckIfInProduction();

			if (DNNWebSession != null)
			{
				DNNWebSession.LastUsedCalculator = CalculatorMode;
			}


			switch (CalculatorMode)
			{
				case 2: // switch loan
					stepimage.ImageUrl = stepimageurlSwitchLoan;
					break;
				case 3: // new purchase loan
					stepimage.ImageUrl = stepimageurlNewPurchase;
					break;
			}


			A1.HRef = Request.Url.ToString();
			SetupErrorMessages();
			RegisterClientScripts();

			if (!IsPostBack)
			{

				lblCalculatorName.Text = CalculatorName;
				lblCalculatorDescriptionText.Text = CalculatorDescriptionText;

				calcButton.CausesValidation = false;  // otherwise you get double validation pop-ups
				calcButton.Attributes.Add("onclick", String.Format("{0}{1}{2}",
																   "if(typeof(Page_ClientValidate) == 'function') Page_ClientValidate();",
                                                                   "if(Page_IsValid){ShowHideErrorForm(0); disable('" + calcButton.ClientID + "');}",
																   "else{ShowHideErrorForm(1); return true;}"
														 ));

				rbProduct1.Attributes.Add("onclick", "SetupFixedRateLoanDisplay();");// fixed rate
				//rbProduct2.Attributes.Add("onclick", "SetupLoyaltyLoanDisplay(); ");// Loyalty 
				rbProduct3.Attributes.Add("onclick", "SetupVariableRateLoanDisplay(); ");//variable

				tbLoanTerm.Attributes.Add("onkeypress", "return NumOnly(event); ");
				tbLoanTerm.Attributes.Add("onclick", "this.select();");

				tbFixPercentage.Attributes.Add("onkeypress", "return NumOnly(event);");
				tbFixPercentage.Attributes.Add("onclick", "this.select();");

				tbPurchasePrice.Attributes.Add("onkeypress", "return NumOnly(event);");
				tbPurchasePrice.Attributes.Add("onclick", "this.select();");

				tbCashDeposit.Attributes.Add("onkeypress", "return NumOnly(event); ");
				tbCashDeposit.Attributes.Add("onclick", "this.select();");

				tbMarketValue.Attributes.Add("onkeypress", "return NumOnly(event);;");
				tbMarketValue.Attributes.Add("onclick", "this.select();");

				tbCurrentLoan.Attributes.Add("onkeypress", "return NumOnly(event);");
				tbCurrentLoan.Attributes.Add("onclick", "this.select();");

				tbCashOut.Attributes.Add("onkeypress", "return NumOnly(event);");
				tbCashOut.Attributes.Add("onclick", "this.select();");

				tbHouseholdIncome.Attributes.Add("onkeypress", "return NumOnly(event);");
				tbHouseholdIncome.Attributes.Add("onclick", "this.select();");

				if (DNNWebSession != null)
				{
					webCalculatorBase.PreProspect.ReferringServerURL = DNNWebSession.URLReferrer;
					webCalculatorBase.PreProspect.UserURL = DNNWebSession.HostAddress;
					webCalculatorBase.PreProspect.AdvertisingCampaignID = DNNWebSession.QueryString;
					webCalculatorBase.PreProspect.UserAddress = DNNWebSession.UserHostName;
				}

				if (DNNWebSession != null) webCalculatorBase.PreProspect.OfferSubmitted = DNNWebSession.ApplicationSubmitted;
				if (DNNWebSession != null) webCalculatorBase.PreProspect.ApplicationKey = DNNWebSession.ApplicationKey;

				Session["webCalculatorBase"] = webCalculatorBase;
			}
			else
				webCalculatorBase = Session["webCalculatorBase"] as WebCalculatorBase;


		}

		/// <summary>
		/// This method sets up the View for production values
		/// </summary>
		private void CheckIfInProduction()
		{

			if (Convert.ToBoolean(ConfigurationManager.AppSettings["InProduction"]) == false)
			{
				if (DNNWebSession != null)
					navigateto = "http://" + DNNWebSession.HostAddress + "/" + NavigateTo;


			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
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
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CalculateAffordabilityCommand(object sender, EventArgs e)
		{
			StoreCalculatorVariables();
			if (Calculate())
				pnlResults.Visible = true;
		}


		// Redirect to the Application Form
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ApplyCommand(object sender, EventArgs e)
		{
			Response.Redirect(ResolveClientUrl(navigateto));
		}


		void StoreCalculatorVariables()
		{
			//TODO this line below might be throwing an error.....
			webCalculatorBase.PreProspect.PurposeNumber = CalculatorMode;

			if (rbProduct1.Checked) webCalculatorBase.PreProspect.Product = 1;
			//if (rbProduct2.Checked) webCalculatorBase.PreProspect.Product = 2;
			if (rbProduct3.Checked) webCalculatorBase.PreProspect.Product = 3;

			if (rbSalaried.Checked) webCalculatorBase.PreProspect.IncomeType = 1;
			if (rbSelfEmployed.Checked) webCalculatorBase.PreProspect.IncomeType = 2;

			if (tbPurchasePrice.Text.Length > 0) webCalculatorBase.PreProspect.LoanAmountRequired = Convert.ToInt32(tbPurchasePrice.Text);
			if (tbCashDeposit.Text.Length > 0) webCalculatorBase.PreProspect.Deposit = Convert.ToInt32(tbCashDeposit.Text);
			if (tbMarketValue.Text.Length > 0) webCalculatorBase.PreProspect.EstimatedPropertyValue = Convert.ToInt32(tbMarketValue.Text);
			if (tbCurrentLoan.Text.Length > 0) webCalculatorBase.PreProspect.CurrentLoan = Convert.ToInt32(tbCurrentLoan.Text);
			if (tbCashOut.Text.Length > 0)
				webCalculatorBase.PreProspect.CashOut = Convert.ToInt32(tbCashOut.Text);
			if (tbLoanTerm.Text.Length > 0) webCalculatorBase.PreProspect.Term = Convert.ToInt32(tbLoanTerm.Text);
			webCalculatorBase.PreProspect.CapitaliseFees = CapitaliseFees;// chkCapitaliseFees.Checked;
			webCalculatorBase.PreProspect.InterestOnly = chkInterestOnly.Checked;
			if (rbProduct1.Checked) // Fixed rate loan
				webCalculatorBase.PreProspect.FixPercent = tbFixPercentage.Text.Length > 0 ? Convert.ToDouble(tbFixPercentage.Text) : 0;

			if (tbHouseholdIncome.Text.Length > 0) webCalculatorBase.PreProspect.HouseholdIncome = Convert.ToInt32(tbHouseholdIncome.Text);

			Session["webCalculatorBase"] = webCalculatorBase;
		}

		void RetrieveCalculatorVariables()
		{

			CalculatorMode = webCalculatorBase.PreProspect.PurposeNumber;
			switch (webCalculatorBase.PreProspect.Product)
			{
				case 1:
					rbProduct1.Checked = true;
					break;
				//case 2:
				//    rbProduct2.Checked = true;
				//    break;
				case 3:
					rbProduct3.Checked = true;
					break;
			}

			if (webCalculatorBase.PreProspect.IncomeType == 1) rbSalaried.Checked = true;
			if (webCalculatorBase.PreProspect.IncomeType == 2) rbSelfEmployed.Checked = true;


			tbPurchasePrice.Text = Convert.ToString(webCalculatorBase.PreProspect.LoanAmountRequired);
			tbCashDeposit.Text = Convert.ToString(webCalculatorBase.PreProspect.Deposit);
			tbMarketValue.Text = Convert.ToString(webCalculatorBase.PreProspect.EstimatedPropertyValue);
			tbCurrentLoan.Text = Convert.ToString(webCalculatorBase.PreProspect.CurrentLoan);
			tbCashOut.Text = Convert.ToString(webCalculatorBase.PreProspect.CashOut);
			tbLoanTerm.Text = Convert.ToString(webCalculatorBase.PreProspect.Term);
			chkCapitaliseFees.Checked = webCalculatorBase.PreProspect.CapitaliseFees;
			chkInterestOnly.Checked = webCalculatorBase.PreProspect.InterestOnly;
			tbFixPercentage.Text = Convert.ToString(webCalculatorBase.PreProspect.FixPercent);
			tbHouseholdIncome.Text = Convert.ToString(webCalculatorBase.PreProspect.HouseholdIncome);


		}





		// This method sets the Control up for the correct View state of the Component
		void SetupControlState()
		{
			if (CalculatorMode == 2) // Switch Loan
			{
				TRSpec2.Visible = false;
				TRSpec3.Visible = false;
				calcButton.ImageUrl = "~/images/SAHomeLoans/Buttons/How-Much-Can-I-Save.jpg";

			}
			else // New Loan
			{
				TRSpec2.Visible = true;
				TRSpec3.Visible = true;
				calcButton.ImageUrl = "~/images/SAHomeLoans/Buttons/What-Can-I-Afford.jpg";
			}
		}


		private bool Calculate()
		{
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

			creditDisqualifications = webCalculatorBase.CreditDisqualifications(creditDisqualifications);

			LTV = creditDisqualifications.ltv;

			if (EmploymentTypeKey == 1)
			{
				PTI = webCalculatorBase.GetControlValueByDescription("Calc - MaxPTI Salaried");
				employmentPTI = webCalculatorBase.GetControlValueByDescription("Calc - MaxPTI Salaried");
			}

			if (EmploymentTypeKey == 2)
			{
				PTI = webCalculatorBase.GetControlValueByDescription("Calc - MaxPTI SelfEmployed");
				employmentPTI = webCalculatorBase.GetControlValueByDescription("Calc - MaxPTI SelfEmployed");
			}


			//PTI = creditDisqualifications.pti;
			loanamount = creditDisqualifications.loanamountrequired;

			baserate = webCalculatorBase.ReturnBaseRate();
			baseratefix = webCalculatorBase.ReturnBaseRateFixed();

			// 1. Calculate Fees
			int appType = webCalculatorBase.ReturnApplicationTypeByCalculatorMode(CalculatorMode);

			//remove any calculated additions from the loan amount
			loanamount = LoanAmountRequired - InterimInterest;

			// remove Total fees if applicable
			if (CapitaliseFees)
				loanamount -= TotalFees;

			double bondtoregister = 0d;
			OriginationFees originationfees = new OriginationFees();

			originationfees.appType = appType;
			originationfees.bondrequired = 0;
			originationfees.bondtoregister = bondtoregister;
			originationfees.cancellationfee = cancellationfee;
			originationfees.capitalisefees = CapitaliseFees;
			originationfees.cashout = CashOut;
			originationfees.initiationfee = initiationfee;
			originationfees.interiminterest = interiminterest;
			originationfees.loanamount = LoanAmountRequired;
			originationfees.OverrideCancelFeeamount = 0;
			originationfees.registrationfee = registrationfee;

			originationfees = webCalculatorBase.CalculateOriginationFees(originationfees);

			initiationfee = originationfees.initiationfee;
			registrationfee = originationfees.registrationfee;
			cancellationfee = originationfees.cancellationfee;
			interiminterest = originationfees.interiminterest;
			//bondtoregister = originationfees.bondtoregister;

			// add any newly calculated interim interest back
			loanamount += interiminterest;

			totalfees = initiationfee + registrationfee + cancellationfee;

			// add Total fees if applicable
			if (CapitaliseFees)
				loanamount += totalfees;

			//ltv = webCalculatorBase.CalculateLTV(loanamount, EstimatedPropertyValue);
			LTV = webCalculatorBase.CalculateLTVThroughBusinessModel(loanamount, EstimatedPropertyValue);
			int oskey = webCalculatorBase.OriginationSourcesSAHomeLoans();

			CreditCriteria creditCriteria = webCalculatorBase.SetupCreditCriteria(oskey, ProductKey, MortgageLoanPurpose, EmploymentTypeKey, loanamount, EstimatedPropertyValue, baserate, Term, HouseholdIncome);

			webCalculatorBase.PreProspect.CreditMatrixKey = creditCriteria.CreditCriteriaCreditMatrixKey;

			linkrate = creditCriteria.CreditCriteriaMarginValue;
			interestrate = linkrate + baserate;
			interestratefix = linkrate + baseratefix;
			percentfix = FixPercent;
			LinkRate = linkrate;
			InterestRateFix = interestratefix;
			InterestRate = interestrate;


			//Calculate instalment
			instalmentvar = webCalculatorBase.CalculateInstallment(loanamount, interestrate, Term, InterestOnly);

			//Calculate PTI: always done against the full amortising instalment
			//pti = Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI(Common.BusinessModel.Helpers.LoanCalculator.CalculateInstallment(loanamount, interestrate, Term, false), HouseholdIncome);
			PTI = webCalculatorBase.CalculatePTIThroughBusinessModel(loanamount, linkrate, baserate, Term, HouseholdIncome);


			//Calculate interest over term
			//interestovertermvar = Common.BusinessModel.Helpers.LoanCalculator.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);
			interestovertermvar = webCalculatorBase.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);

			//Calculate fixed amounts
			if (ProductKey == webCalculatorBase.VariFixLoan())
			{
				loanamountfix = (loanamount * percentfix);
				loanamountvar = (loanamount * (1 - percentfix));

				//instalment VF accounts can not be interest only
				//fix
				instalmentfix = webCalculatorBase.CalculateInstallment(loanamountfix, interestratefix, Term, false); // 
				//var
				instalmentvar = loanamountvar <= 0 ? 0 : webCalculatorBase.CalculateInstallment(loanamountvar, interestrate, Term, false);

				//pti
				//ptifix = Common.BusinessModel.Helpers.LoanCalculator.CalculatePTI((instalmentfix + instalmentvar), HouseholdIncome);
				ptifix = webCalculatorBase.CalculatePTI((instalmentfix + instalmentvar), HouseholdIncome);
				//ptifix = webCalculatorBase.CalculatePTIThroughBusinessModel(loanamountfix, linkrate, baserate, Term, HouseholdIncome);

				//interest over term
				//Fix
				interestovertermfix = webCalculatorBase.CalculateInterestOverTerm(loanamountfix, interestratefix, Term, false);
				//Var
				interestovertermvar = loanamountvar <= 0 ? 0 : webCalculatorBase.CalculateInterestOverTerm(loanamountvar, interestrate, Term, false);
			}

			//Calculate min income against the CC max pti
			instalmenttotal = instalmentvar + instalmentfix;
			double requiredincome = webCalculatorBase.CalculateMinimumIncomeRequired(InstalmentTotal, creditCriteria.CreditCriteriaPTIValue);
			// set display of the form

			//Warnings:

			//ltv pti
			//LTV = ltv;
			//PTI = pti;
			PTIFix = ptifix;

			//Percentages
			FixPercent = FixPercent;

			//Amounts
			LoanAmountTotal = loanamount;
			LoanAmountFix = loanamountfix;
			LoanAmountVar = loanamountvar;

			//instalment
			InstalmentVar = instalmentvar;
			InstalmentFix = instalmentfix;
			InstalmentTotal = instalmentvar + instalmentfix;

			//finance charges
			FinanceChargesVar = interestovertermvar;
			FinanceChargesFix = interestovertermfix;
			FinanceChargesTotal = interestovertermvar + interestovertermfix;

			//Fees
			CancellationFee = cancellationfee;
			RegistrationFee = registrationfee;
			InitiationFee = initiationfee;
			TotalFees = totalfees;
			InterimInterest = interiminterest;

			// Summary Display
			lblPurchasePrice.Text = PurchasePrice.ToString(webCalculatorBase.CurrencyFormat());
			lblCashDeposit.Text = Deposit.ToString(webCalculatorBase.CurrencyFormat());



			creditDisqualifications.calculationdone = true;
			creditDisqualifications.ltv = ltv;
			creditDisqualifications.pti = pti;
			creditDisqualifications.householdincome = HouseholdIncome;
			creditDisqualifications.loanamountrequired = loanamount;
			creditDisqualifications.estimatedproeprtyvalue = EstimatedPropertyValue;
			creditDisqualifications.EmploymentTypeKey = EmploymentTypeKey;
			creditDisqualifications.ifFurtherLending = true;
			creditDisqualifications.term = Term;

			creditDisqualifications = webCalculatorBase.CreditDisqualifications(creditDisqualifications);

			ltv = creditDisqualifications.ltv;
			pti = creditDisqualifications.pti;
			loanamount = creditDisqualifications.loanamountrequired;


			creditDisqualifications.term = Term;

			// Store the session variables for the summary
			//PreProspect.LoanAmountRequired = SAHL.Common.BusinessModel.Helpers.LoanCalculator.CalculateBondAmount(loanamount);

			webCalculatorBase.PreProspect.CategoryKey = creditCriteria.CreditCriteriaCategoryKey;

			webCalculatorBase.PreProspect.EstimatedPropertyValue = EstimatedPropertyValue;
			webCalculatorBase.PreProspect.MortgageLoanPurpose = MortgageLoanPurpose;
			webCalculatorBase.PreProspect.InterestRate = interestrate;
			webCalculatorBase.PreProspect.LinkRate = linkrate;
			webCalculatorBase.PreProspect.RegistrationFee = registrationfee;
			webCalculatorBase.PreProspect.CancellationFee = cancellationfee;
			webCalculatorBase.PreProspect.ValuationFee = 0;
			webCalculatorBase.PreProspect.InitiationFee = initiationfee;
			webCalculatorBase.PreProspect.TransferFee = 0;
			webCalculatorBase.PreProspect.TotalFee = TotalFees;
			webCalculatorBase.PreProspect.Term = Term;
			webCalculatorBase.PreProspect.HouseholdIncome = HouseholdIncome;
			webCalculatorBase.PreProspect.PurchasePrice = LoanAmountRequired + InterimInterest;
			webCalculatorBase.PreProspect.TotalPrice = TotalLoanAmountRequired;

			webCalculatorBase.PreProspect.MarginKey = creditCriteria.CreditCriteriaMarginKey;

			webCalculatorBase.PreProspect.ElectedFixedRate = interestratefix;
			webCalculatorBase.PreProspect.EmploymentType = EmploymentTypeKey;

			webCalculatorBase.PreProspect.PTI = pti; //PTI
			webCalculatorBase.PreProspect.LTV = ltv;//LTV

			webCalculatorBase.PreProspect.Product = ProductKey;
			webCalculatorBase.PreProspect.ProductKey = ProductKey;
			webCalculatorBase.PreProspect.LoanAmountRequired = Math.Round(loanamount);

			Session["webCalculatorBase"] = webCalculatorBase;

			// Set up the Calculator warning Message
			lblNotQualifyMsg.Text = "";
			double minLTV = MinLTV / 100;
			double maxPTI = MaxPTI / 100;
			bool hasWarning = false;
			string notQualifyMsg = string.Empty;
			string employmentType = EmploymentTypeKey == 1 ? "Salaried" : "Self-Employed";			


			if (employmentPTI <= pti || minLTV <= ltv)
			{
				notQualifyMsg = string.Format(@" SA Home Loans lending policy for {0} applicants allows loans up to {1}% LTV ratio and PTI ratio of no more than {2}%. Please call us on 0860 2 4 6 8 10 should you require assistance in structuring your home loan.", employmentType, Convert.ToString(MinLTV).ToString(), (employmentPTI * 100.0).ToString("0"));
				lblNotQualifyMsg.Text = notQualifyMsg;
				hasWarning = true;
			}
			

			if (!hasWarning)
			{
				lblNotQualifyMsg.Text = string.Empty;
				lblNotQualifyMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#f37021");
				lblNotQualifyMsg.Text = "<br/>Congratulations - SA Home Loans would welcome the opportunity to offer you home loan finance.";
				lblQualifyMsg.Text = "The application provisionally qualifies";
				btnApply.Visible = true;
			}
			else
			{
				lblNotQualifyMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
				lblQualifyMsg.Text = string.Empty;
				btnApply.Visible = false;
			}


			lblQualifyMsg.Visible = true;

			SetupSummarySisplay(ProductKey);

			return true;
		}

		private void SetupSummarySisplay(int productkey)
		{
			// Set display of the form
			if (productkey == webCalculatorBase.VariFixLoan())
			{
				Tr1.Visible = false;
				Tr2.Visible = false;
				Tr3.Visible = false;
				Tr4.Visible = false;
				Tr5.Visible = false;
				Tr6.Visible = false;

				TrVF1.Visible = true;
				Tr7.Visible = true;
				TrVF2.Visible = true;
				TrVF3.Visible = true;
				TrVF4.Visible = true;
				TrVF5.Visible = true;
				TrVF6.Visible = true;

				VarifixRedline1.Visible = false;
				VarifixRedline2.Visible = false;
				VarifixRedline3.Visible = false;

				SuperLoRedline1.Visible = true;
				SuperLoRedline2.Visible = true;
				SuperLoRedline3.Visible = true;

				VarifixPTI.Visible = true;
				SuperLoPTI.Visible = false;
			}
			else
			{
				Tr1.Visible = true;
				Tr2.Visible = true;
				Tr3.Visible = true;
				Tr4.Visible = true;
				Tr5.Visible = true;
				Tr6.Visible = true;

				TrVF1.Visible = false;
				Tr7.Visible = false;
				TrVF2.Visible = false;
				TrVF3.Visible = false;
				TrVF4.Visible = false;
				TrVF5.Visible = false;
				TrVF6.Visible = false;

				VarifixRedline1.Visible = true;
				VarifixRedline2.Visible = true;
				VarifixRedline3.Visible = true;

				SuperLoRedline1.Visible = false;
				SuperLoRedline2.Visible = false;
				SuperLoRedline3.Visible = false;

				VarifixPTI.Visible = false;
				SuperLoPTI.Visible = true;
			}
		}


		private void SetupCalculatorCaptureFields()
		{
			rowMarketValue.Visible = false;
			rowMarketValuespacer.Visible = false;
			rowCurrentLoan.Visible = false;
			rowCurrentLoanSpacer.Visible = false;
			rowCashOut.Visible = false;
			rowPurchasePrice.Visible = false;
			rowPurchasePricespacer.Visible = false;
			rowCashDeposit.Visible = false;
			rowCashDepositspacer.Visible = false;

			switch (CalculatorMode)
			{
				case 2: // switch loan
					rowMarketValue.Visible = true;
					rowMarketValuespacer.Visible = true;
					rowCurrentLoan.Visible = true;
					rowCurrentLoanSpacer.Visible = true;
					rowCashOut.Visible = true;
					break;
				case 3: // new purchase
					rowPurchasePrice.Visible = true;
					rowPurchasePricespacer.Visible = true;
					rowCashDeposit.Visible = true;
					rowCashDepositspacer.Visible = true;
					break;
				case 4: //Refinance
					rowMarketValue.Visible = true;
					rowMarketValuespacer.Visible = true;
					rowCashOut.Visible = true;
					break;
				default: // - select only - 

					break;
			}



		}

		/// <summary>
		/// This method sets up the parameters for validation according to SAHL business rules
		/// </summary>
		private void SetupErrorMessages()
		{

			cvHouseholdIncome.ErrorMessage = "Your gross income must exceed " + MinHouseholdIncome.ToString(webCalculatorBase.CurrencyFormat()) + " to qualify.";
			cvHouseholdIncome.ValueToCompare = Convert.ToString(MinHouseholdIncome);

			cvMarketValue.ErrorMessage = "Your home's value must be at least " + MinMarketValue.ToString(webCalculatorBase.CurrencyFormat());
			cvMarketValue.ValueToCompare = Convert.ToString(MinMarketValue);

			RangeValidator1.MinimumValue = Convert.ToString(1);
			RangeValidator1.MaximumValue = Convert.ToString(MaxTerm);
			RangeValidator1.ErrorMessage = "The loan term must be between 1 and " + Convert.ToString(MaxTerm) + " months.";

			cvCurrentLoanAmount.MinimumValue = Convert.ToString(1);
			cvCurrentLoanAmount.MaximumValue = Convert.ToString(MaxTerm);
			cvCurrentLoanAmount.ErrorMessage = "Your total loan amount (including cash out) must be between " + MinMarketValue.ToString(webCalculatorBase.CurrencyFormat()) + " and " + MaxLoanAmount.ToString(webCalculatorBase.CurrencyFormat());

		}



		/// <summary>
		/// Create and Register the Views Javascript Model
		/// </summary>
		void RegisterClientScripts()
		{
			System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();

			mBuilder.AppendLine("  var maximuminstallment = 0;");
			mBuilder.AppendLine("  var bondrequired = 0; ");
			mBuilder.AppendLine("  var totalfees = " + TotalFees + ";");
			mBuilder.AppendLine("  var MinFixFinServiceAmount = " + MinFixFinServiceAmount + ";");
			mBuilder.AppendLine("  var MinLoanAmount = " + MinLoanAmount + ";");
			mBuilder.AppendLine("  var MaxLoanAmount = " + MaxLoanAmount + ";");
			mBuilder.AppendLine("  var MaxPTI = " + MaxPTI + ";");
			mBuilder.AppendLine("  var MinLTV = " + MinLTV + ";");
			mBuilder.AppendLine("  var MaxTerm = " + MaxTerm + ";");

			mBuilder.AppendLine("  var calculatormode = " + CalculatorMode + " ;");
			mBuilder.AppendLine("if (window.addEventListener) ");//DOM method for binding an event
			mBuilder.AppendLine("   window.addEventListener('load', initIt, false);");
			mBuilder.AppendLine("else if (window.attachEvent)"); //IE exclusive method for binding an event
			mBuilder.AppendLine("   window.attachEvent('onload', initIt);");
			mBuilder.AppendLine("else if (document.getElementById) ");//support older modern browsers
			mBuilder.AppendLine("   window.onload=initIt;");
			mBuilder.AppendLine("  ");

			mBuilder.AppendLine("  function initIt()");
			mBuilder.AppendLine("  {");
			mBuilder.AppendLine("   document.getElementById('rowVarifix').style.display = 'none';");
			mBuilder.AppendLine("   document.getElementById('rowVarifixSpacer').style.display = 'none';");
			mBuilder.AppendLine("   if (calculatormode != 3) "); // not new purchase
			mBuilder.AppendLine("  {");
			mBuilder.AppendLine("       document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("       document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("   }");
			mBuilder.AppendLine("   else");
			mBuilder.AppendLine("  {");
			mBuilder.AppendLine("       document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("       document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("   }");
			mBuilder.AppendLine("    ");
			mBuilder.AppendLine("  if (document.getElementById('" + rbProduct1.ClientID + "').checked == true)");
			mBuilder.AppendLine("       SetupFixedRateLoanDisplay();");
			//mBuilder.AppendLine("  if (document.getElementById('" + rbProduct2.ClientID + "').checked == true)");
			//mBuilder.AppendLine("       SetupLoyaltyLoanDisplay(); ");
			mBuilder.AppendLine("  if (document.getElementById('" + rbProduct3.ClientID + "').checked == true)");
			mBuilder.AppendLine("       SetupVariableRateLoanDisplay();");
			mBuilder.AppendLine("    ");
			mBuilder.AppendLine("  }");


			mBuilder.AppendLine("      function SetupVariableRateLoanDisplay()");
			mBuilder.AppendLine("      {");
            mBuilder.AppendLine("           if (document.getElementById('" + tbFixPercentage.ClientID + "').value == null || document.getElementById('" + tbFixPercentage.ClientID + "').value == '') { document.getElementById('" + tbFixPercentage.ClientID + "').value = 0; }");
			mBuilder.AppendLine("          document.getElementById('rowVarifix').style.display = 'none';");
			mBuilder.AppendLine("          document.getElementById('rowVarifixSpacer').style.display = 'none';");
			mBuilder.AppendLine("          ");
			mBuilder.AppendLine("          if (calculatormode != 3) // not new purchase");
			mBuilder.AppendLine("              {");
			mBuilder.AppendLine("              document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("              document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("              }");
			mBuilder.AppendLine("          else");
			mBuilder.AppendLine("              {");
			mBuilder.AppendLine("              document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("              document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("              }");
			mBuilder.AppendLine("              ");
			mBuilder.AppendLine("          document.getElementById('" + lblInterestOnly.ClientID + "').style.display = '';");
			mBuilder.AppendLine("          document.getElementById('" + chkInterestOnly.ClientID + "').style.display = '';");
			mBuilder.AppendLine("      }");
			mBuilder.AppendLine("      function SetupFixedRateLoanDisplay()");
			mBuilder.AppendLine("      {");
            mBuilder.AppendLine("           if (document.getElementById('" + tbFixPercentage.ClientID + "').value == null || document.getElementById('" + tbFixPercentage.ClientID + "').value == '') { document.getElementById('" + tbFixPercentage.ClientID + "').value = 0; }");
            mBuilder.AppendLine("          document.getElementById('rowVarifix').style.display = '';");
			mBuilder.AppendLine("          document.getElementById('rowVarifixSpacer').style.display = '';");
			mBuilder.AppendLine("          if (calculatormode != 3) // not new purchase");
			mBuilder.AppendLine("              {");
			mBuilder.AppendLine("              document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("              document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("               }");
			mBuilder.AppendLine("          else");
			mBuilder.AppendLine("              {");
			mBuilder.AppendLine("              document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("              document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("               }");
			mBuilder.AppendLine("          document.getElementById('" + lblInterestOnly.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("          document.getElementById('" + chkInterestOnly.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("      }");
			mBuilder.AppendLine("      function SetupLoyaltyLoanDisplay()");
			mBuilder.AppendLine("      {");
			mBuilder.AppendLine("          document.getElementById('rowVarifix').style.display = 'none';");
			mBuilder.AppendLine("          document.getElementById('rowVarifixSpacer').style.display = 'none';");
			mBuilder.AppendLine("          if (calculatormode != 3) // not new purchase");
			mBuilder.AppendLine("              {");
			mBuilder.AppendLine("              document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("              document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = '';");
			mBuilder.AppendLine("               }");
			mBuilder.AppendLine("          else");
			mBuilder.AppendLine("              {");
			mBuilder.AppendLine("              document.getElementById('" + lblCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("              document.getElementById('" + chkCapitaliseFees.ClientID + "').style.display = 'none';");
			mBuilder.AppendLine("               }");
			mBuilder.AppendLine("          document.getElementById('" + lblInterestOnly.ClientID + "').style.display = '';");
			mBuilder.AppendLine("          document.getElementById('" + chkInterestOnly.ClientID + "').style.display = '';");
			mBuilder.AppendLine("      }");


			mBuilder.AppendLine("  ");
			mBuilder.AppendLine("  function CalculateFixedPercentageValues(source, arguments)");
			mBuilder.AppendLine("  {");
			mBuilder.AppendLine("   // check if this is a fixed rate loan, if not ignore");
			mBuilder.AppendLine("  if (document.getElementById('" + rbProduct1.ClientID + "').checked == false)");
			mBuilder.AppendLine("  {");
			mBuilder.AppendLine("     arguments.IsValid=true;");
			mBuilder.AppendLine("     return;");
			mBuilder.AppendLine("  }");
			mBuilder.AppendLine("  if (100 < document.getElementById('" + tbFixPercentage.ClientID + "').value)");
			mBuilder.AppendLine("   {");
			mBuilder.AppendLine("     document.getElementById('" + CustomValidator2.ClientID + "').errormessage = 'The fixed portion of your loan cannot exceed 100%.'; ");
			mBuilder.AppendLine("     arguments.IsValid=false;");
			mBuilder.AppendLine("     return;");
			mBuilder.AppendLine("   }");
			mBuilder.AppendLine("  var fixedpercent = 0; ");
			mBuilder.AppendLine("  if (calculatormode == 2) //Switchloan");
			mBuilder.AppendLine("   {");
			mBuilder.AppendLine("       var currentloan = document.getElementById('" + tbCurrentLoan.ClientID + "').value;");
			mBuilder.AppendLine("       if (currentloan.length > 0)");
			mBuilder.AppendLine("           bondrequired = parseInt(currentloan);");
			mBuilder.AppendLine("   }");
			mBuilder.AppendLine("  if (calculatormode == 3)//Newpurchase");
			mBuilder.AppendLine("   {");
			mBuilder.AppendLine("       var purchaseprice = document.getElementById('" + tbPurchasePrice.ClientID + "').value;");
			mBuilder.AppendLine("       if (purchaseprice.length > 0)");
			mBuilder.AppendLine("           bondrequired = parseInt(purchaseprice);");
			mBuilder.AppendLine("   }");
			mBuilder.AppendLine("  if (calculatormode == 4) //Refinance");
			mBuilder.AppendLine("   {");
			mBuilder.AppendLine("       var cashout = document.getElementById('" + tbCashOut.ClientID + "').value;");
			mBuilder.AppendLine("       if (cashout.length > 0)");
			mBuilder.AppendLine("           bondrequired = parseInt(cashout);");
			mBuilder.AppendLine("   }");
			mBuilder.AppendLine("  ");
			mBuilder.AppendLine("  if (document.getElementById('" + chkCapitaliseFees.ClientID + "').checked == true)");
			mBuilder.AppendLine("       bondrequired = bondrequired + totalfees;");
			mBuilder.AppendLine("  ");

			mBuilder.AppendLine("  if ((document.getElementById('" + rbProduct1.ClientID + "').checked == true) && (document.getElementById('" + tbFixPercentage.ClientID + "').value.length > 0) )");
			mBuilder.AppendLine("       fixedpercent = document.getElementById('" + tbFixPercentage.ClientID + "').value / 100;");
			mBuilder.AppendLine("  else");
			mBuilder.AppendLine("       fixedpercent = 0;");
			mBuilder.AppendLine("  ");
			mBuilder.AppendLine("  var minpercent = MinFixFinServiceAmount / bondrequired ;");
			mBuilder.AppendLine("  ");
			mBuilder.AppendLine("     document.getElementById('" + CustomValidator2.ClientID + "').errormessage = 'The selected fixed amount is R ' + (bondrequired * fixedpercent) + ' , less than the minimum of R ' + MinFixFinServiceAmount ; ");
			//mBuilder.AppendLine("     document.getElementById('" + CustomValidator2.ClientID + "').errormessage = bondrequired + '   ' + minpercent + '  '  + fixedpercent + '  ' + (fixedpercent * 100) + '   '  + MinFixFinServiceAmount");
			mBuilder.AppendLine("  ");

			//mBuilder.AppendLine("  if ((bondrequired * fixedpercent) < MinFixFinServiceAmount)");
			mBuilder.AppendLine("  if (minpercent <= fixedpercent)");
			mBuilder.AppendLine("     arguments.IsValid=true;");
			mBuilder.AppendLine("  else");
			mBuilder.AppendLine("     arguments.IsValid=false;");
			mBuilder.AppendLine("  }");
			mBuilder.AppendLine("  ");


			//cvFixedPercentage.ErrorMessage = " Fixed amount is " + (bondrequired * fixedpercent).ToString(SAHL.Common.Constants.CurrencyFormat) + ", less than the minimum R " + MinFixFinServiceAmount.ToString(SAHL.Common.Constants.CurrencyFormat) + ".";


			if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
				Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);


		}





	}
}