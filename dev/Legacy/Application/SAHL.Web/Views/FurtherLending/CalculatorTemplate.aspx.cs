using System;
using System.Web.UI.WebControls;
using SAHL.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Web.Views.FurtherLending.Interfaces;

namespace SAHL.Web.Views.FurtherLending
{
    public partial class CalculatorTemplate : SAHLCommonBaseView, ICalculatorTemplate
    {
        //TODO Set this up through an exposed method
        // Viewtype describes what the claculator is being used for
        private int ViewType;

        private readonly ILegalEntityRepository LegalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        private readonly IApplicationRepository ApplicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        private readonly ILookupRepository LookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
        private readonly IControlRepository ControlRepository = RepositoryFactory.GetRepository<IControlRepository>();

        public event EventHandler btnBackToCalculatorClicked;
        public event EventHandler btnBackToSummaryClicked;
        public event EventHandler btnCalculateClicked;
        public event EventHandler btnCreateApplicationClicked;
        public event KeyChangedEventHandler IDNumberChanged;
        public event EventHandler btnSubmit_ClickClicked;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ViewType = 0;
            if (!ShouldRunPage)
                return;
            int[] iPurposes = new int[] { (int)MortgageLoanPurposes.Refinance, (int)MortgageLoanPurposes.Newpurchase, (int)MortgageLoanPurposes.Switchloan, };


            // Initialise the Page and the Dropdowns
            RegisterClientScripts();
            BindProductDropdown(ApplicationRepository.GetOriginationProducts());
            BindPurposeDropdown(ApplicationRepository.GetMortgageLoanPurposes(iPurposes));
            BindEmploymentType(LookupRepository.EmploymentTypes);
            PopulateMarketingSource(LookupRepository.ApplicationSources);
            SetupTabs(0);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return;

            if (ViewType > 0) // 
                RegisterWebService(ServiceConstants.LegalEntity);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;
            SetupCalculatorCaptureFields();
            if (IsPostBack)
            {
                if (chkCapitaliseFees.Checked)
                {
                    lblFeeInfo.Text = "** Total Fees included because you elected to have them capitalised.";
                    lblFeeInfoFix.Text = "** Total Fees included because you elected to have them capitalised.";
                    lblFeeInfoInd.Text = "**";
                    lblFeeInfoIndFix.Text = "**";
                }
                else
                {
                    lblFeeInfo.Text = "";
                    lblFeeInfoFix.Text =
                    lblFeeInfoInd.Text = "";
                    lblFeeInfoIndFix.Text = "";
                }
            }
            SetupControlState();
        }

        protected void btnBackToSummary_Click(object sender, EventArgs e)
        {
            StoreLeadVariables();
            Calculate();
            SetupControlState();
            SetupTabs(1); // go back to display
        }

        protected void btnBackToCalculator_Click(object sender, EventArgs e)
        {
            RetrieveCalculatorVariables();
            SetupTabs(0); // go back to calculator
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            StoreCalculatorVariables();
            if (Calculate())
                SetupTabs(1); // go to loan summary
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCalculate_Click(object sender, EventArgs e)
        {
            if (Calculate())
                SetupTabs(1);
            //else
            //    Messages.
        }

        protected void btnCreateApplication_Click(object sender, EventArgs e)
        {
            SetupTabs(2);
            RetrieveLeadVariables(); // Navigate to contact capture form
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            // This determines what response happens after this control is used
            // It is handled differently internally and externally
            CheckContactBusinessRules();
            if (!Messages.HasErrorMessages)
                switch (ViewType)
                {
                    case 0: // 0 = Client Facing
                        SetupTabs(3);
                        break;
                    case 1: // internal
                        SetupTabs(3);
                        break;
                }
            else
                SetupTabs(2);
        }


        void StoreCalculatorVariables()
        {
            if (ddlPurpose != null) Session["purpose"] = ddlPurpose.SelectedIndex;
            if (ddlProduct != null) Session["product"] = ddlProduct.SelectedIndex;
            if (ddlEmploymentType != null) Session["employmenttype"] = ddlEmploymentType.SelectedIndex;
            if (tbPurchasePrice != null) Session["purchaseprice"] = tbPurchasePrice.Text;
            if (tbCashDeposit != null) Session["deposit"] = tbCashDeposit.Text;
            if (tbMarketValue != null) Session["marketvalue"] = tbMarketValue.Text;
            if (tbCurrentLoan != null) Session["currentloan"] = tbCurrentLoan.Text;
            if (tbCashOut != null) Session["cashout"] = tbCashOut.Text;
            if (tbCashRequired != null) Session["cashrequired"] = tbCashRequired.Text;
            if (tbLoanTerm != null) Session["loanterm"] = tbLoanTerm.Text;
            if (chkCapitaliseFees != null) Session["capitalisefees"] = chkCapitaliseFees.Checked;
            if (chkInterestOnly != null) Session["interestonly"] = chkInterestOnly.Checked;
            if (tbFixPercentage != null) Session["fixpercentage"] = tbFixPercentage.Text;
            if (tbHouseholdIncome != null) Session["householdincome"] = tbHouseholdIncome.Text;
        }

        void RetrieveCalculatorVariables()
        {
            if (Session["purpose"] != null) ddlPurpose.SelectedIndex = (int)Session["purpose"];
            if (Session["product"] != null) ddlProduct.SelectedIndex = (int)Session["product"];
            if (Session["employmenttype"] != null) ddlEmploymentType.SelectedIndex = (int)Session["employmenttype"];
            if (Session["purchaseprice"] != null) tbPurchasePrice.Text = (string)Session["purchaseprice"];
            if (Session["deposit"] != null) tbCashDeposit.Text = (string)Session["deposit"];
            if (Session["marketvalue"] != null) tbMarketValue.Text = (string)Session["marketvalue"];
            if (Session["currentloan"] != null) tbCurrentLoan.Text = (string)Session["currentloan"];
            if (Session["cashout"] != null) tbCashOut.Text = (string)Session["cashout"];
            if (Session["cashrequired"] != null) tbCashRequired.Text = (string)Session["cashrequired"];
            if (Session["loanterm"] != null) tbLoanTerm.Text = (string)Session["loanterm"];
            if (Session["capitalisefees"] != null) chkCapitaliseFees.Checked = (bool)Session["capitalisefees"];
            if (Session["interestonly"] != null) chkInterestOnly.Checked = (bool)Session["interestonly"];
            if (Session["fixpercentage"] != null) tbFixPercentage.Text = (string)Session["fixpercentage"];
            if (Session["householdincome"] != null) tbHouseholdIncome.Text = (string)Session["householdincome"];
        }

        void StoreLeadVariables()
        {
            // Lead Info
            if (ddlMarketingSource != null) Session["marketingsource"] = ddlMarketingSource.SelectedIndex;
            if (txtClientIDNumber != null) Session["clientidnumber"] = txtClientIDNumber.Text;
            if (txtIDNumber != null) Session["idnumber"] = txtIDNumber.Text;
            if (txtFirstNames != null) Session["firstnames"] = txtFirstNames.Text;
            if (txtSurname != null) Session["surname"] = txtSurname.Text;
            if (phCode != null) Session["phonecode"] = phCode.Text;
            if (phNumber != null) Session["phonenumber"] = phNumber.Text;
            if (tbNumApplicants != null) Session["numberofapplicants"] = tbNumApplicants.Text;
            if (txtEmail != null) Session["emailaddress"] = txtEmail.Text;
        }

        void RetrieveLeadVariables()
        {
            // Lead Info
            if (Session["marketingsource"] != null) ddlMarketingSource.SelectedIndex = (int)Session["marketingsource"];
            if (Session["clientidnumber"] != null) txtClientIDNumber.Text = (string)Session["clientidnumber"];
            if (Session["idnumber"] != null) txtIDNumber.Text = (string)Session["idnumber"];
            if (Session["firstnames"] != null) txtFirstNames.Text = (string)Session["firstnames"];
            if (Session["surname"] != null) txtSurname.Text = (string)Session["surname"];
            if (Session["phonecode"] != null) phCode.Text = (string)Session["phonecode"];
            if (Session["phonenumber"] != null) phNumber.Text = (string)Session["phonenumber"];
            if (Session["numberofapplicants"] != null) tbNumApplicants.Text = (string)Session["numberofapplicants"];
            if (Session["emailaddress"] != null) txtEmail.Text = (string)Session["emailaddress"];

        }


        // This method sets the Control up for the correct View state of the Component
        void SetupControlState()
        {
            switch (ViewType)
            {

                case 0:// 0 = Client Facing
                    txtIDNumber.Visible = false;
                    txtHelp.Visible = true;

                    TRSpec1.Visible = true;
                    TRSpec2.Visible = true;
                    TRSpec3.Visible = true;
                    TRSpec4.Visible = true;
                    TRSpec5.Visible = true;
                    TRSpec6.Visible = true;
                    TRSpec7.Visible = true;
                    TRSpec8.Visible = true;

                    tdBanks.Visible = true;
                    tdLoanrequirement.Visible = true;
                    tdInterestRate.Visible = true;
                    tdMonthlyInstalment.Visible = true;
                    tdTermInterest.Visible = true;

                    TrSavings.Visible = true;

                    // Text
                    btnCalculate.Text = "Check if I qualify..";
                    lblCallme.Visible = true;
                    lblNote.Visible = true;

                    break;

                case 1:// 1 = Internal


                    // Tab Grouping Text for internal
                    pnlcalculator.GroupingText = "Calculator";
                    pnlResults.GroupingText = "Results";
                    pnlSignup.GroupingText = "Contact Details";

                    txtClientIDNumber.Visible = false;

                    TRSpec1.Visible = false;
                    TRSpec2.Visible = false;
                    TRSpec3.Visible = false;
                    TRSpec4.Visible = false;
                    TRSpec5.Visible = false;
                    TRSpec6.Visible = false;
                    TRSpec7.Visible = false;
                    TRSpec8.Visible = false;

                    tdBanks.Visible = false;
                    tdLoanrequirement.Visible = false;
                    tdInterestRate.Visible = false;
                    tdMonthlyInstalment.Visible = false;
                    tdTermInterest.Visible = false;
                    TrSavings.Visible = false;

                    lblCallme.Visible = false;
                    lblNote.Visible = false;

                    break;
            }
        }

        private void SetupTabs(int TabIndex)
        {
            switch (TabIndex)
            {
                case 0: // View Details back to calculator
                    pnlcalculator.Visible = true;
                    pnlResults.Visible = false;
                    pnlSignup.Visible = false;
                    btnCalculate.Visible = true;
                    btnBackToCalculator.Visible = false;
                    btnBackToSummary.Visible = false;
                    btnCreateApplication.Visible = false;
                    btnSubmit.Visible = false;
                    pnlButtons.Width = 620;
                    break;
                case 1: // View Details back to calculator
                    pnlcalculator.Visible = false;
                    pnlResults.Visible = true;
                    pnlSignup.Visible = false;
                    btnCalculate.Visible = false;
                    btnBackToCalculator.Visible = true;
                    btnBackToSummary.Visible = false;
                    btnCreateApplication.Visible = true;
                    btnSubmit.Visible = false;
                    pnlButtons.Width = 550;
                    break;

                case 2: // Application From back to details
                    pnlcalculator.Visible = false;
                    pnlResults.Visible = false;
                    pnlSignup.Visible = true;
                    btnCalculate.Visible = false;
                    btnBackToCalculator.Visible = false;
                    btnBackToSummary.Visible = true;
                    btnCreateApplication.Visible = false;
                    btnSubmit.Visible = true;
                    pnlButtons.Width = 470;
                    break;
                case 3: // Application confirmation
                    pnlcalculator.Visible = false;
                    pnlResults.Visible = false;
                    pnlSignup.Visible = false;
                    btnCalculate.Visible = false;
                    pnlConfirmApplication.Visible = true;
                    btnBackToCalculator.Visible = false;
                    btnBackToSummary.Visible = false;
                    btnCreateApplication.Visible = false;
                    btnSubmit.Visible = false;
                    break;
            }
        }




        /// <summary>
        ///ID Number Postback handler 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void acNatAddIDNumber_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            GetLegalEntityAndDisplay(Convert.ToInt32(e.Key));

            //check whther the recalculate must be fired for display purposes
            //if (!validcalc)
            //    OnCalculateButtonClicked(sender, e);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="leKey"></param>
        private void GetLegalEntityAndDisplay(int leKey)
        {
            //populate the legalentity
            ILegalEntityNaturalPerson le = (ILegalEntityNaturalPerson)LegalEntityRepository.GetLegalEntityByKey(leKey);

            if (le != null)
            {
                txtFirstNames.Text = le.FirstNames;
                txtSurname.Text = le.Surname;
                txtIDNumber.Text = le.IDNumber;
                phCode.Text = le.HomePhoneCode;
                phNumber.Text = le.HomePhoneNumber;
                txtEmail.Text = le.EmailAddress;
                legalentitykey = le.Key;

                StoreLeadVariables();
                SetupTabs(2);

            }
        }



        private bool Calculate()
        {
            RetrieveCalculatorVariables();

            // Business rule checks, populate the messages and return if any have been added.
            ApplicationRepository.CreditDisqualifications(false, 0, 0, HouseholdIncome, LoanAmountRequired, EstimatedPropertyValue, EmploymentTypeKey, true, Term);
            CheckBusinessRules();
            // If its internal return the error messages in the component
            if (Messages.HasErrorMessages)
                return false;

            IEventList<IMarketRate> mRates = LookupRepository.MarketRates;
            foreach (IMarketRate mR in mRates)
            {
                if (mR.Key == 4) //18th reset rate
                    baserate = mR.Value;

                if (mR.Key == 3) //VF rate
                    baseratefix = mR.Value;
            }

            loanamount = LoanAmountRequired;

            // 1. Calculate Fees
            //IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            //BUGGER: Should be using application types, not MortgageLoanPurpose....
            ApplicationTypes appType;
            switch (MortgageLoanPurpose)
            {
                case MortgageLoanPurposes.Newpurchase:
                    appType = ApplicationTypes.NewPurchaseLoan;
                    break;
                case MortgageLoanPurposes.Refinance:
                    appType = ApplicationTypes.RefinanceLoan;
                    break;
                case MortgageLoanPurposes.Switchloan:
                    appType = ApplicationTypes.SwitchLoan;
                    break;
                default:
                    throw new NotSupportedException("This application type is not supported.");
            }
            double bondtoregister;
            ApplicationRepository.CalculateOriginationFees(loanamount, 0, appType, out initiationfee, out registrationfee, out cancellationfee, out interiminterest, out bondtoregister);

            totalfees = initiationfee + registrationfee + cancellationfee;

            loanamount += interiminterest;

            if (CapitaliseFees)
                loanamount += totalfees;

            ltv = SAHL.Common.LoanCalculator.CalculateLTV(loanamount, EstimatedPropertyValue);

            oskey = CurrentPrincipal.PrimaryOriginationSourceKey;

            IFinancialsService finsS = ServiceFactory.GetService<IFinancialsService>();
            ICreditCriteria cc = finsS.GetCreditCriteriaByLTV(Messages, oskey, ProductKey, (int)MortgageLoanPurpose, EmploymentTypeKey, loanamount, EstimatedPropertyValue);

            if (cc == null)
                cc = finsS.GetCreditCriteriaException(Messages, oskey, ProductKey, (int)MortgageLoanPurpose, EmploymentTypeKey, loanamount);

            if (cc == null)
                Messages.Add(new Error("No matching credit matrix was found.", "No matching credit matrix was found."));

            if (Messages.Count > 0)
                return false;

            if (cc != null) CreditMatrixKey = cc.CreditMatrix.Key;
            if (cc != null) CategoryKey = cc.Category.Key;
            //rates get
            if (cc != null) linkrate = cc.Margin.Value;
            interestrate = linkrate + baserate;
            interestratefix = linkrate + baseratefix;
            percentfix = FixPercent;

            //cc.Margin
            //cc.Margin.RateConfigurations.

            //rates set
            if (cc != null) MarginKey = cc.Margin.Key;
            ActiveMarketRate = baserate;
            LinkRate = linkrate;
            InterestRateFix = interestratefix;
            InterestRate = interestrate;


            //Calculate instalment
            instalmentvar = SAHL.Common.LoanCalculator.CalculateInstallment(loanamount, interestrate, Term, InterestOnly);

            //Calculate PTI: always done against the full amortising instalment
            pti = SAHL.Common.LoanCalculator.CalculatePTI(SAHL.Common.LoanCalculator.CalculateInstallment(loanamount, interestrate, Term, false), HouseholdIncome);

            //Calculate interest over term
            interestovertermvar = SAHL.Common.LoanCalculator.CalculateInterestOverTerm(loanamount, interestrate, Term, InterestOnly);

            //Calculate fixed amounts
            if (ProductKey == (int)Products.VariFixLoan)
            {
                loanamountfix = (loanamount * percentfix);
                loanamountvar = (loanamount * (1 - percentfix));

                //instalment VF accounts can not be interest only
                //fix
                instalmentfix = SAHL.Common.LoanCalculator.CalculateInstallment(loanamountfix, interestratefix, Term, false); // 
                //var
                instalmentvar = loanamountvar <= 0 ? 0 : SAHL.Common.LoanCalculator.CalculateInstallment(loanamountvar, interestrate, Term, false);

                //pti
                ptifix = SAHL.Common.LoanCalculator.CalculatePTI((instalmentfix + instalmentvar), HouseholdIncome);

                //interest over term
                //Fix
                interestovertermfix = SAHL.Common.LoanCalculator.CalculateInterestOverTerm(loanamountfix, interestratefix, Term, false);
                //Var
                interestovertermvar = loanamountvar <= 0 ? 0 : SAHL.Common.LoanCalculator.CalculateInterestOverTerm(loanamountvar, interestrate, Term, false);
            }

            //Calculate min income against the CC max pti
            instalmenttotal = instalmentvar + instalmentfix;
            if (cc != null) minincome = SAHL.Common.LoanCalculator.CalculateMinimumIncomeRequired(instalmenttotal, cc.PTI.Value);

            // set display of the form

            //Warnings:
            if (minincome > HouseholdIncome)
                IncomeSufficient = false;

            //ltv pti
            LTV = ltv;
            PTI = pti;
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
            lblPurchasePrice.Text = PurchasePrice.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblCashDeposit.Text = Deposit.ToString(SAHL.Common.Constants.CurrencyFormat);
            lblEmployedType.Text = EmploymentType;
            lblLoanTerm.Text = Term + " months";
            lblMonthlyHouseholdIncome.Text = HouseholdIncome.ToString(SAHL.Common.Constants.CurrencyFormat);

            lblCapitalFees.Text = CapitaliseFees ? "yes" : "no";

            lblFixLoan.Text = LoanAmountFix > 0 ? "yes" : "no";

            // Set display of the form
            if (ProductKey == (int)Products.VariFixLoan)
            {
                Tr1.Visible = false;
                Tr2.Visible = false;
                Tr3.Visible = false;
                Tr4.Visible = false;
                Tr5.Visible = false;
                Tr6.Visible = false;

                TrVF1.Visible = true;
                TrVF2.Visible = true;
                TrVF3.Visible = true;
                TrVF4.Visible = true;
                TrVF5.Visible = true;
                TrVF6.Visible = true;
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
                TrVF2.Visible = false;
                TrVF3.Visible = false;
                TrVF4.Visible = false;
                TrVF5.Visible = false;
                TrVF6.Visible = false;
            }

            ApplicationRepository.CreditDisqualifications(true, ltv, pti, HouseholdIncome, loanamount, EstimatedPropertyValue, EmploymentTypeKey, true, Term);

            CheckBusinessRules();
            if (Messages.HasErrorMessages)
                return false;

            //if we got here the application qualifies
            ApplicationQualifies = true;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckBusinessRules()
        {
            ResetErrorLabels();
            //TODO: get these values from the CM or CC or DB
            IControl ctrl = ControlRepository.GetControlByDescription("Calc - minValuation");
            double minPurchasePrice = (ctrl.ControlNumeric.HasValue ? Convert.ToDouble(ctrl.ControlNumeric) : 0);
            const double minMarketValue = 170000;
            const double minHouseholdIncome = 6000;
            const double minFixFinServiceAmount = 50000;
            const double minFixTotalLoanAmount = 200000;

            Messages.Clear();

            if (ProductKey == 0)
            {
                Messages.Add(new Error("No product was selected.", "No product was selected."));
                if ((ViewType == 0))
                {
                    lblErrorProduct.Text = "Please select a product.";
                    lblErrorProduct.Visible = true;
                }
            }

            if ((int)MortgageLoanPurpose == 1)
            {
                Messages.Add(new Error("No loan purpose was selected.", "No loan purpose was selected."));
                if ((ViewType == 0))
                {
                    lblErrorLoanPurpose.Text = "Please select a loan purpose.";
                    lblErrorLoanPurpose.Visible = true;
                }

            }
            if (EmploymentTypeKey == 0)
            {
                Messages.Add(new Error("No employment type was selected.", "No employment type was selected."));
                if ((ViewType == 0))
                {
                    lblErrorEmploymentType.Text = "Please select your employment type.";
                    lblErrorEmploymentType.Visible = true;
                }
            }

            if (HouseholdIncome < minHouseholdIncome)
            {
                Messages.Add(new Error("Household income must be at least R " + minHouseholdIncome + ".", "Household income must be at least R " + minHouseholdIncome + "."));
                if ((ViewType == 0))
                {
                    lblErrorMonthlyIncome.Text = "Monthly income must be at least R " + minHouseholdIncome + ".00 to qualify.";
                    lblErrorMonthlyIncome.Visible = true;
                }
            }


            switch (MortgageLoanPurpose)
            {
                case MortgageLoanPurposes.Switchloan: //2: Switch loan
                    if (EstimatedPropertyValue < minMarketValue)
                    {
                        Messages.Add(new Error("Property value must be greater than R " + minMarketValue + ".", "Property value must be greater than R " + minMarketValue + "."));
                        if ((ViewType == 0))
                        {
                            lblErrorMarketValue.Text = "The property value must be greater than R " + minMarketValue + ".00 .";
                            lblErrorMarketValue.Visible = true;
                        }
                    }
                    if (CurrentLoan <= 0)
                    {
                        Messages.Add(new Error("Current loan must be greater than R 0.00", "Current loan must be greater than R 0.00"));
                        if ((ViewType == 0))
                        {
                            lblErrorCurrentLoanAmount.Text = "Your current loan must be greater than R 0.00";
                            lblErrorCurrentLoanAmount.Visible = true;
                        }
                    }

                    if (CashOut <= minPurchasePrice)
                    {
                        Messages.Add(new Error("Cash out must be greater than R " + minPurchasePrice + ".", "Cash out must be greater than R " + minPurchasePrice + "."));
                        if ((ViewType == 0))
                        {
                            lblErrorCashOut.Text = "Your cash our must be greater than R " + minPurchasePrice + ".00 .";
                            lblErrorCashOut.Visible = true;
                        }
                    }
                    break;


                case MortgageLoanPurposes.Newpurchase: //3: New purchase
                    if (EstimatedPropertyValue < minPurchasePrice)
                    {
                        Messages.Add(new Error("Purchase price must be greater than R " + minPurchasePrice + ".", "Purchase price must be greater than R " + minPurchasePrice + "."));
                        if ((ViewType == 0))
                        {
                            lblErrorPurchasePrice.Text = "The purchase price must be greater than R " + minPurchasePrice + ".00 .";
                            lblErrorPurchasePrice.Visible = true;
                        }
                    }
                    break;

                case MortgageLoanPurposes.Refinance: //4: Refinance
                    if (EstimatedPropertyValue < minMarketValue)
                    {
                        Messages.Add(new Error("Property value must be greater than R " + minMarketValue + ".", "Property value must be greater than R " + minMarketValue + "."));
                        if ((ViewType == 0))
                        {
                            lblErrorMarketValue.Text = "The property value must be greater than R " + minMarketValue + ".00 .";
                            lblErrorMarketValue.Visible = true;
                        }
                    }
                    if (CashOut <= minPurchasePrice)
                    {
                        Messages.Add(new Error("Cash out must be greater than R " + minPurchasePrice + ".", "Cash out must be greater than R " + minPurchasePrice + "."));
                        if ((ViewType == 0))
                        {
                            lblErrorCashRequired.Text = "The cash required must be greater than R " + minPurchasePrice + ".00 .";
                            lblErrorCashRequired.Visible = true;
                        }
                    }
                    break;

                default:
                    break;
            }

            if (ProductKey == (int)Products.VariFixLoan)
            {
                if (LoanAmountRequired < minFixTotalLoanAmount)
                {
                    Messages.Add(new Error("Minimum loan amount for Varifix product is R " + minFixTotalLoanAmount + ".", "Minimum loan amount for Varifix product is R " + minFixTotalLoanAmount + "."));
                    if ((ViewType == 0))
                    {
                        lblErrorCurrentLoanAmount.Text = "Minimum Varifix value is R " + minFixTotalLoanAmount + ".";
                        lblErrorCurrentLoanAmount.Visible = true;
                    }
                }

                string minFixedMessage = "The fixed amount is " + (LoanAmountRequired * FixPercent).ToString(SAHL.Common.Constants.CurrencyFormat) + ", less than the minimum R " + minFixFinServiceAmount + ".";

                if (LoanAmountRequired * FixPercent < minFixFinServiceAmount)
                {
                    Messages.Add(new Error(minFixedMessage, minFixedMessage));
                    if ((ViewType == 0))
                    {
                        lblErrorPercentageTofix.Text = " Fixed amount is " + (LoanAmountRequired * FixPercent).ToString(SAHL.Common.Constants.CurrencyFormat) + ", less than the minimum R " + minFixFinServiceAmount + ".";
                        lblErrorPercentageTofix.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CheckContactBusinessRules()
        {
            lblErrorMarketingSource.Visible = false;
            lblErrorIDNumber.Visible = false;
            lblErrorFirstNames.Visible = false;
            lblErrorSurname.Visible = false;
            lblErrorContactNumber.Visible = false;
            lblErrorNumberOfApplicants.Visible = false;

            if (ddlMarketingSource.SelectedIndex < 1)
            {
                Messages.Add(new Error("No marketing source was selected.", "No marketing source was selected."));
                if ((ViewType == 0))
                {
                    lblErrorMarketingSource.Text = "Where did you hear about us?";
                    lblErrorMarketingSource.Visible = true;
                }
            }

            switch (ViewType)
            {
                case 0:
                    if (txtClientIDNumber.Text.Length < 1)
                    {
                        lblErrorIDNumber.Text = "Please add your ID/Passport Number.";
                        lblErrorIDNumber.Visible = true;
                    }
                    break;
                default:
                    if (txtIDNumber.Text.Length < 1)
                        Messages.Add(new Error("No ID number was added.", "No ID number was added."));
                    break;
            }


            if (txtFirstNames.Text.Length < 1)
            {
                Messages.Add(new Error("No first name was added.", "No first name was added."));
                if ((ViewType == 0))
                {
                    lblErrorFirstNames.Text = "Please add your first name.";
                    lblErrorFirstNames.Visible = true;
                }
            }


            if (txtSurname.Text.Length < 1)
            {
                Messages.Add(new Error("No surname name was added.", "No surname was added."));
                if ((ViewType == 0))
                {
                    lblErrorSurname.Text = "Plase add your surname.";
                    lblErrorSurname.Visible = true;
                }
            }


            if (phCode.Text.Length < 1 | phNumber.Text.Length < 1)
            {
                Messages.Add(new Error("No contact number was added.", "No contact number was added."));
                if ((ViewType == 0))
                {
                    lblErrorContactNumber.Text = "Please add your contact number.";
                    lblErrorContactNumber.Visible = true;
                }
            }


            if ((Convert.ToInt32(tbNumApplicants.Text) < 1))
            {
                Messages.Add(new Error("No applicant number was added.", "No applicant number was added."));
                if ((ViewType == 0))
                {
                    lblErrorNumberOfApplicants.Text = "Please indicate how many applicants there will be.";
                    lblErrorNumberOfApplicants.Visible = true;
                }
            }


        }

        private void ResetErrorLabels()
        {
            lblErrorLoanPurpose.Visible = false;
            lblErrorProduct.Visible = false;
            lblErrorPurchasePrice.Visible = false;
            lblErrorCashDeposit.Visible = false;
            lblErrorMarketValue.Visible = false;
            lblErrorCurrentLoanAmount.Visible = false;
            lblErrorCashOut.Visible = false;
            lblErrorCashRequired.Visible = false;
            lblErrorEmploymentType.Visible = false;
            lblErrorTermOfLoan.Visible = false;
            lblErrorCapitaliseFees.Visible = false;
            lblErrorPercentageTofix.Visible = false;
            lblErrorMonthlyIncome.Visible = false;
        }


        /// <summary>
        /// Bind the Product Dropdown
        /// </summary>
        /// <param name="listProducts"></param>
        public void BindProductDropdown(ReadOnlyEventList<IProduct> listProducts)
        {
            ddlProduct.DataValueField = "Key";
            ddlProduct.DataTextField = "Description";
            ddlProduct.DataSource = listProducts;
            ddlProduct.DataBind();
        }

        /// <summary>
        /// Bind the Product Dropdown
        /// </summary>
        /// <param name="listPurpose"></param>
        public void BindPurposeDropdown(ReadOnlyEventList<IMortgageLoanPurpose> listPurpose)
        {
            ddlPurpose.DataValueField = "Key";
            ddlPurpose.DataTextField = "Description";
            ddlPurpose.DataSource = listPurpose;
            ddlPurpose.DataBind();
        }

        /// <summary>
        /// Bind the Employment type dropdown
        /// </summary>
        /// <param name="listEmploymentType"></param>
        public void BindEmploymentType(IEventList<IEmploymentType> listEmploymentType)
        {
            ddlEmploymentType.DataSource = listEmploymentType.BindableDictionary;
            ddlEmploymentType.DataBind();

            // remove the 'Unknown' employment type
            ListItem li = ddlEmploymentType.Items.FindByValue(Convert.ToString((int)EmploymentTypes.Unknown));
            if (li != null)
                ddlEmploymentType.Items.Remove(li);
        }



        /// <summary>
        /// What it says on the tin
        /// </summary>
        /// <param name="applicationSource"></param>
        public void PopulateMarketingSource(IEventList<IApplicationSource> applicationSource)
        {
            ddlMarketingSource.DataTextField = "Description";
            ddlMarketingSource.DataValueField = "Key";
            ddlMarketingSource.DataSource = applicationSource;
            ddlMarketingSource.DataBind();

        }


        private int legalentitykey;
        /// <summary>
        /// 
        /// </summary>
        public int LegalEntityKey
        {
            get
            {
                if (legalentitykey.ToString().Length > 0)
                    return legalentitykey;

                return 0;
            }
            set { legalentitykey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LEFirstNames
        {
            get { return txtFirstNames.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LESurname
        {
            get { return txtSurname.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LEIDNumber
        {
            get { return txtIDNumber.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PhoneCode
        {
            get { return Convert.ToInt32(phCode.Text); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PhoneNumber
        {
            get { return Convert.ToInt32(phNumber.Text); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfApplicants
        {
            get
            {
                if (tbNumApplicants.Text.Length > 0)
                    return Convert.ToInt32(tbNumApplicants.Text);

                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MarketingSource
        {
            get
            {
                if (ddlMarketingSource.SelectedValue == "-select-")
                    return String.Empty;

                return ddlMarketingSource.SelectedValue;
            }
        }

        /// <summary>
        /// The selected product key
        /// </summary>
        public int ProductKey
        {
            get { return Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue); }
        }

        /// <summary>
        /// The Loan Purpose Key 
        /// MortgageLoanPurposeKey	Description
        /// 2	Switch loan
        /// 3	New purchase
        /// 4	Refinance
        /// </summary>
        public MortgageLoanPurposes MortgageLoanPurpose
        {
            get { return (MortgageLoanPurposes)Enum.ToObject(typeof(MortgageLoanPurposes), Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "1" : ddlPurpose.SelectedValue)); }
        }

        /// <summary>
        /// The employment type key
        /// </summary>
        public int EmploymentTypeKey
        {
            get { return Convert.ToInt16(ddlEmploymentType.SelectedValue == "-select-" ? "0" : ddlEmploymentType.SelectedValue); }

        }

        /// <summary>
        /// Gets the employment type as a string
        /// </summary>
        public string EmploymentType
        {
            get
            {
                switch (ddlEmploymentType.SelectedValue)
                {
                    case "-select-":
                        return "";
                    default:
                        {
                            string retval = LookupRepository.EmploymentTypes[Convert.ToInt32(ddlEmploymentType.SelectedValue) - 1].Description;
                            return retval;
                        }
                }
            }
        }

        /// <summary>
        /// Market value of the property
        /// </summary>
        public double EstimatedPropertyValue
        {
            get
            {
                switch (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue))
                {
                    case (int)MortgageLoanPurposes.Newpurchase:
                        return tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0;
                    default:
                        return tbMarketValue.Text.Length > 0 ? Convert.ToDouble(tbMarketValue.Text) : 0;
                }
            }
        }


        /// <summary>
        /// Deposit to pay for a new purchase
        /// </summary>
        public double Deposit
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Newpurchase && tbCashDeposit.Text.Length > 0)
                    return Convert.ToDouble(tbCashDeposit.Text);
                return 0;
            }
        }

        /// <summary>
        /// Existing loan amount with another provider for switch loans
        /// </summary>
        public double CurrentLoan
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Switchloan && tbCurrentLoan.Text.Length > 0)
                    return Convert.ToDouble(tbCurrentLoan.Text);
                return 0;
            }
        }

        // TODO: Fix this anomoly
        /// <summary>
        /// Cash value required by the client for switch and refinance
        /// </summary>
        public double CashOut
        {
            get
            {
                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Switchloan && tbCashOut.Text.Length > 0)
                    return Convert.ToDouble(tbCashOut.Text);

                if (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) == (int)MortgageLoanPurposes.Refinance && tbCashRequired.Text.Length > 0)
                    return Convert.ToDouble(tbCashRequired.Text);

                return 0;
            }
        }

        /// <summary>
        /// The term of the Loan in months
        /// </summary>
        public Int16 Term
        {
            get { return tbLoanTerm.Text.Length > 0 ? Convert.ToInt16(tbLoanTerm.Text) : (Int16)0; }

        }


        /// <summary>
        /// Should fees be capitalised to the Loan amount for switch and refinance
        /// </summary>
        public bool CapitaliseFees
        {
            get
            {
                return Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue) != (int)MortgageLoanPurposes.Newpurchase && chkCapitaliseFees.Checked;
            }
        }

        /// <summary>
        /// Indicates interest only mortgage loan application
        /// </summary>
        public bool InterestOnly
        {
            get
            {
                return Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue) != (int)Products.VariFixLoan && chkInterestOnly.Checked;
            }
        }

        /// <summary>
        /// Total income of the household occupants
        /// </summary>
        public double HouseholdIncome
        {
            get { return tbHouseholdIncome.Text.Length > 0 ? Convert.ToDouble(tbHouseholdIncome.Text) : 0; }
        }

        /// <summary>
        /// Loan amount required
        /// </summary>
        public double LoanAmountRequired
        {
            get
            {
                double bondrequired;
                switch (Convert.ToInt16(ddlPurpose.SelectedValue == "-select-" ? "0" : ddlPurpose.SelectedValue))
                {
                    case (int)MortgageLoanPurposes.Newpurchase:
                        bondrequired = (tbPurchasePrice.Text.Length > 0 ? Convert.ToDouble(tbPurchasePrice.Text) : 0) - (tbCashDeposit.Text.Length > 0 ? Convert.ToDouble(tbCashDeposit.Text) : 0);
                        break;
                    case (int)MortgageLoanPurposes.Switchloan:
                        bondrequired = (tbCurrentLoan.Text.Length > 0 ? Convert.ToDouble(tbCurrentLoan.Text) : 0) + (tbCashOut.Text.Length > 0 ? Convert.ToDouble(tbCashOut.Text) : 0); //include interim interest
                        break;
                    case (int)MortgageLoanPurposes.Refinance:
                        bondrequired = tbCashRequired.Text.Length > 0 ? Convert.ToDouble(tbCashRequired.Text) : 0;
                        break;
                    default:
                        bondrequired = 0;
                        break;
                }
                return bondrequired;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PurchasePrice
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
        protected void ddlProduct_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetupCalculatorCaptureFields();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            SetupCalculatorCaptureFields();
        }

        private void SetupCalculatorCaptureFields()
        {
            rowMarketValue.Visible = false;
            rowCurrentLoan.Visible = false;
            rowCashOut.Visible = false;
            rowCapitaliseFees.Visible = false;
            rowPurchasePrice.Visible = false;
            rowCashDeposit.Visible = false;
            rowCashRequired.Visible = false;
            rowInterestOnly.Visible = false;
            rowVarifix.Visible = false;

            switch (ddlPurpose.SelectedItem.Value)
            {
                case "2": // switch loan
                    rowMarketValue.Visible = true;
                    rowCurrentLoan.Visible = true;
                    rowCashOut.Visible = true;
                    rowCapitaliseFees.Visible = true;
                    break;
                case "3": // new purchase
                    rowPurchasePrice.Visible = true;
                    rowCashDeposit.Visible = true;
                    break;
                case "4": //Refinance
                    rowMarketValue.Visible = true;
                    rowCashRequired.Visible = true;
                    rowCapitaliseFees.Visible = true;
                    break;
                default: // - select only - 


                    break;
            }

            switch (ddlProduct.SelectedItem.Value)
            {
                case "2": //VariFix Loan
                    rowVarifix.Visible = true;
                    break;
                case "5": //Super Lo
                case "9": //New Variable Loan
                    rowInterestOnly.Visible = true;
                    break;
                default: // -Select- only
                    rowInterestOnly.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Create and Register the Views Javascript Model. this must only be run AFTER all objects have been created
        /// </summary>
        public void RegisterClientScripts()
        {
            System.Text.StringBuilder mBuilder = new System.Text.StringBuilder();
            // CHECK FOR THE MAXIMUM VALUE
            mBuilder.AppendLine("function checkValMax(control, max) ");
            mBuilder.AppendLine("{ ");
            mBuilder.AppendLine("   var val = Number(control.value); ");
            mBuilder.AppendLine(" ");
            mBuilder.AppendLine("   if (isNaN(val) || val > max) ");
            mBuilder.AppendLine("   { ");
            mBuilder.AppendLine("       alert('Maximum allowed value is ' + max.toString()); ");
            mBuilder.AppendLine("       control.value = max; ");
            mBuilder.AppendLine("    } ");
            mBuilder.AppendLine("} ");
            mBuilder.AppendLine(" ");

            if (!Page.ClientScript.IsClientScriptBlockRegistered(GetType(), "lstScripts"))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), "lstScripts", mBuilder.ToString(), true);
            }

            tbLoanTerm.Attributes.Add("onchange", "checkValMax(this, 360);");
            tbFixPercentage.Attributes.Add("onchange", "checkValMax(this, 100);");

        }

        //////////////////////////////////////////////////////////////////////////////
        // PUBLIC VARIABLES

        private int categorykey;
        /// <summary>
        /// 
        /// </summary>
        public int CategoryKey
        {
            get { return categorykey; }
            set { categorykey = value; }
        }

        private double activemarketrate;
        /// <summary>
        /// 
        /// </summary>
        public double ActiveMarketRate
        {
            get { return activemarketrate; }
            set { activemarketrate = value; }
        }


        //private bool validcalc;
        private bool applicationqualifies;
        /// <summary>
        /// 
        /// </summary>
        public bool ApplicationQualifies
        {
            get { return applicationqualifies; }
            set
            {
                lblQualifyMsg.Visible = value;
                btnCreateApplication.Enabled = value;
                //validcalc = value;
                applicationqualifies = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DisableCreateApplication
        {
            set { btnCreateApplication.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CreateApplicationButtonText
        {
            set { btnCreateApplication.Text = value; }
        }


        private bool incomesufficient;
        /// <summary>
        /// 
        /// </summary>
        public bool IncomeSufficient
        {
            get { return incomesufficient; }
            set
            {
                incomesufficient = value;
                if (value == false)
                    lblNotQualifyMsg.Visible = true;
            }
        }

        private double initiationfee;
        /// <summary>
        /// 
        /// </summary>
        public double InitiationFee
        {
            get { return initiationfee; }
            set
            {
                initiationfee = value;
                lblBondPrepFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }

        }


        private double cancellationfee;
        /// <summary>
        /// 
        /// </summary>
        public double CancellationFee
        {
            get { return cancellationfee; }
            set
            {
                cancellationfee = value;
                lblCancellationFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }


        private double financechargesvar;
        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesVar
        {
            get { return financechargesvar; }
            set
            {
                lblIntPaidTermVar.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                financechargesvar = value;
            }
        }


        //private double fixpercent;
        /// <summary>
        /// The percentage of the Loan amount to fix for VariFix product
        /// </summary>
        public double FixPercent
        {
            get
            {
                if (Convert.ToInt16(ddlProduct.SelectedValue == "-select-" ? "0" : ddlProduct.SelectedValue) == (int)Products.VariFixLoan && tbFixPercentage.Text.Length > 0)
                    return Convert.ToDouble(tbFixPercentage.Text) / 100; // convert user integer to percentage
                return 0;
            }
            set
            {
                //fixpercent = value;
                lblFixPercent.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblFixedPercent.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblVariablePercent.Text = (1 - value).ToString(SAHL.Common.Constants.RateFormat);
            }
        }


        private double financechargesfix;
        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesFix
        {
            get { return financechargesfix; }
            set
            {
                lblIntPaidTermFix.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                financechargesfix = value;
            }
        }


        private double financechargestotal;
        /// <summary>
        /// 
        /// </summary>
        public double FinanceChargesTotal
        {
            get { return financechargestotal; }
            set
            {
                financechargestotal = value;
                lblSAHLIntOverTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotFixIntPaidTerm.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }


        private double interiminterest;
        /// <summary>
        /// 
        /// </summary>
        public double InterimInterest
        {
            get { return interiminterest; }
            set
            {
                lblInterimIntProv.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                interiminterest = value;
            }
        }


        private double baserate;
        private double baseratefix;


        private int creditmatrixkey;
        /// <summary>
        /// 
        /// </summary>
        public int CreditMatrixKey
        {
            get { return creditmatrixkey; }
            set { creditmatrixkey = value; }
        }

        private double loanamount;

        private double loanamountvar;
        /// <summary>
        /// 
        /// </summary>
        public double LoanAmountVar
        {
            get { return loanamountfix; }
            set
            {
                lblVarLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                loanamountvar = value;
            }
        }

        private double loanamountfix;
        /// <summary>
        /// 
        /// </summary>
        public double LoanAmountFix
        {
            get { return loanamountfix; }
            set
            {
                lblFixLoanAmount.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                loanamountfix = value;

            }
        }

        private double loanamounttotal;
        /// <summary>
        /// 
        /// </summary>
        public double LoanAmountTotal
        {

            get { return loanamounttotal; }
            set
            {
                loanamounttotal = value;
                lblSAHLTotLoan.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotalFixLoan.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        private double linkrate;
        /// <summary>
        /// 
        /// </summary>
        public double LinkRate
        {
            get { return linkrate; }
            set { linkrate = value; }
        }

        private double interestrate;
        /// <summary>
        /// 
        /// </summary>
        public double InterestRate
        {
            get { return interestrate; }
            set
            {
                interestrate = value;
                lblSAHLIntRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblVarRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
            }
        }


        private double interestratefix;
        /// <summary>
        /// 
        /// </summary>
        public double InterestRateFix
        {
            get { return interestratefix; }
            set
            {
                interestratefix = value;
                lblFixRate.Text = value.ToString(SAHL.Common.Constants.RateFormat);
            }
        }

        private double instalmenttotal;
        /// <summary>
        /// 
        /// </summary>
        public double InstalmentTotal
        {
            get { return instalmenttotal; }
            set
            {
                lblSAHLMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                lblTotFixMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                instalmenttotal = value;
            }
        }

        private double instalmentvar;
        /// <summary>
        /// 
        /// </summary>
        public double InstalmentVar
        {
            get { return instalmentvar; }
            set
            {
                instalmentvar = value;
                lblVarMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        private double instalmentfix;
        /// <summary>
        /// 
        /// </summary>
        public double InstalmentFix
        {
            get { return instalmentfix; }
            set
            {
                lblFixMonthlyInst.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                instalmentfix = value;
            }
        }

        private double pti;
        /// <summary>
        /// 
        /// </summary>
        public double PTI
        {
            get { return pti; }
            set
            {
                lblPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                lblVarPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                pti = value; //to get again
            }
        }

        private double ptifix;
        /// <summary>
        /// 
        /// </summary>
        public double PTIFix
        {
            get { return ptifix; }
            set
            {
                ptifix = value;
                lblFixPTI.Text = value.ToString(SAHL.Common.Constants.RateFormat);
            }
        }

        private double interestovertermvar;
        private double interestovertermfix;
        private double minincome;

        private int marginkey;
        /// <summary>
        /// 
        /// </summary>
        public int MarginKey
        {
            get { return marginkey; }
            set { marginkey = value; }
        }

        private double ltv;
        /// <summary>
        /// 
        /// </summary>
        public double LTV
        {
            get { return ltv; }
            set
            {
                lblLTV.Text = value.ToString(SAHL.Common.Constants.RateFormat);
                ltv = value; //for getter
            }
        }

        private double totalfees;
        /// <summary>
        /// 
        /// </summary>
        public double TotalFees
        {
            get
            {
                return totalfees;
            }
            set
            {
                lblTotalFees.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
                totalfees = value;
            }
        }


        private int oskey;
        private double percentfix;


        private double registrationfee;

        //public CalculatorTemplate()
        //{
        //    ViewType = 0;
        //}

        /// <summary>
        /// 
        /// </summary>
        public double RegistrationFee
        {
            get { return registrationfee; }
            set
            {
                registrationfee = value;
                lblRegFee.Text = value.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        // OTHER CAPTURED VALUES






    }
}
