using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Reflection;
using SAHL.Internet.SAHL.Web.Services.Application;

[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.AffordabilityCalculator.js", "application/javascript")]
[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.Input.js", "application/javascript")]

namespace SAHL.Internet.Components.Calculators
{
    /// <summary>
    /// The SA Home Loans affordability calculator.
    /// </summary>
    public partial class AffordabilityCalculator : UserControl, IScriptControl
    {
        /// <summary>
        /// Gets or sets the calculator web service.
        /// </summary>
        protected WebCalculatorBase BaseCalculator
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
        protected SAHLWebSession SahlSession
        {
            get { return Session["SAHLWebSession"] as SAHLWebSession; }
        }

        #region Calculator State Variables

        /// <summary>
        /// Gets or sets the current currency format.
        /// </summary>
        protected string CurrencyFormat
        {
            get { return ViewState["CurrencyFormat"] as string; }
            private set { ViewState["CurrencyFormat"] = value; }
        }

        /// <summary>
        /// Gets or sets the current currency format.
        /// </summary>
        protected string CurrencyFormatNoCents
        {
            get { return ViewState["CurrencyFormatNoCents"] as string; }
            private set { ViewState["CurrencyFormatNoCents"] = value; }
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
        /// Gets the maximum loan term in years.
        /// </summary>
        protected double MaxTerm
        {
            get
            {
                var result = ViewState["MaxTerm"];
                if (result == null) return 20.0;
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
        /// Gets the interest rate.
        /// </summary>
        protected double InterestRate
        {
            get
            {
                var result = ViewState["InterestRate"];
                if (result == null) return 0.0;
                return (double)result;
            }
            private set { ViewState["InterestRate"] = value; }
        }
        
        #endregion

        /// <summary>
        /// Indicates whether the installment value is valid.
        /// </summary>
        protected bool IsInstallmentValueValid
        {
            get
            {
                CalculateMaximumInstallment();

                var max = BaseCalculator.PreProspect.MaximumInstallment;
                var instalment = string.IsNullOrWhiteSpace(txtMonthlyInstalment.Text) ? 0.0 : Convert.ToDouble(txtMonthlyInstalment.Text);
                if (instalment > max) return false;
                return instalment > 0;
            }
        }

        /// <summary>
        /// Gets or sets the control validation group.
        /// </summary>
        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"] as string; }
            set { ViewState["ValidationGroup"] = value; }
        }

        /// <summary>
        /// Calculates the interest rate.
        /// </summary>
        private void CalculateInterestRate()
        {
            var calculator = BaseCalculator;

            var baseRate = calculator.ReturnBaseRate();
            var productKey = calculator.ProductsNewPurchaseNewVariableLoan();
            var mortgageLoanPurpose = calculator.MortgageLoanPurposesNewpurchase();
            var employmentTypeKey = calculator.EmploymentTypeSalaried();
            var originationSourcesKey = calculator.OriginationSourcesSAHomeLoans();

            var criteria = calculator.GetAffordabilityCreditCriteria(originationSourcesKey, productKey, mortgageLoanPurpose, employmentTypeKey, 500000);
            var linkRate = criteria.CreditCriteriaMarginValue;
            var interestRate = ((linkRate + baseRate) * 100);

            calculator.PreProspect.CreditMatrixKey = criteria.CreditCriteriaCreditMatrixKey;
            calculator.PreProspect.CategoryKey = criteria.CreditCriteriaCategoryKey;
            calculator.PreProspect.MarginKey = criteria.CreditCriteriaMarginKey;
            calculator.PreProspect.InterestRate = interestRate;
            calculator.PreProspect.LinkRate = linkRate;
            calculator.PreProspect.RegistrationFee = 0;
            calculator.PreProspect.CancellationFee = 0;
            calculator.PreProspect.ValuationFee = 0;
            calculator.PreProspect.InitiationFee = 0;
            calculator.PreProspect.TransferFee = 0;

            InterestRate = interestRate;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack) Reset(); // Reset the form.
            SetupErrorMessages();

            applicationAffordability.BaseCalculator = BaseCalculator;
            if (!IsPostBack) applicationAffordability.Reset(); // just make sure the application form was reset.

            if (string.IsNullOrWhiteSpace(txtInterestRate.Text))
                txtInterestRate.Text = Convert.ToString(InterestRate); // make sure there's always an interest rate set.
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterClientScripts();

            DataBind();
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
            pnlAffordabilityCalculator.Visible = false;
            applicationAffordability.Visible = true;

            ScriptManager.RegisterClientScriptBlock(pnlAffordabilityCalculator, GetType(), "AffordabilityCalculator.Tracking", "if (_gaq) _gaq.push([\"_trackPageview\", \"/track/application-displayed\"]);", true);
        }

        /// <summary>
        /// Calculates the loan affordability.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Calculate(object sender, EventArgs e)
        {
            if (IsInstallmentValueValid)
            {
                pnlResults.Visible = true;
                pnlFiller.Visible = false;
            }
            else
            {
                pnlResults.Visible = false;
                pnlFiller.Visible = true;
                return;
            }

            var calculator = BaseCalculator;

            // Do the Calculations and Update the display

            // Calculate present Value
            var instalment = Convert.ToDouble(txtMonthlyInstalment.Text);
            var interestRate = Convert.ToDouble(txtInterestRate.Text) / 100;
            var earnings = Convert.ToDouble(txtSalaryOne.Text);
            earnings += string.IsNullOrWhiteSpace(txtSalaryTwo.Text) ? 0.0 : Convert.ToDouble(txtSalaryTwo.Text);
            var months = Convert.ToInt32(txtLoanTerm.Text) * 12;

            var bondAmount = calculator.CalculatePresentValue(instalment, months, interestRate);
            var homeValue = Math.Round(bondAmount);
            lblLoanAmount.Text = homeValue.ToString(CurrencyFormatNoCents);

            // check if there is extra cash from the sale of a previous home
            var profitFromSale = string.IsNullOrWhiteSpace(txtProfitFromSale.Text) ? 0.0 : Convert.ToDouble(txtProfitFromSale.Text);
            profitFromSale += string.IsNullOrWhiteSpace(txtOtherContribution.Text) ? 0.0 : Convert.ToDouble(txtOtherContribution.Text);

            if (profitFromSale == 0) lblApproximateHomeValue.Text = lblLoanAmount.Text;
            else
            {
                //SavingsRow.Visible = true;
                // add previous home sale profit to the home value
                homeValue += profitFromSale;
                lblApproximateHomeValue.Text = homeValue.ToString(CurrencyFormatNoCents);
            }

            lblAvailableSavings.Text = profitFromSale.ToString(CurrencyFormatNoCents);

            var ltv = calculator.CalculateLTV(bondAmount, homeValue);
            var pti = calculator.CalculatePTI(instalment, earnings);
            lblLTVRatio.Text = Convert.ToString(ltv) + "%";
            lblPTIRatio.Text = Convert.ToString(Math.Round(pti * 100)) + "%";

            //UPDATE THE PREPROSPECTOBJECT
            // PurposeNumber = 1 - Unknown , 2 - Switch Loan, 3 - New Purchase, 4 - Refinance, 5 - Further Loan 
            calculator.PreProspect.PurposeNumber = 1;
            calculator.PreProspect.Term = months;
            calculator.PreProspect.LoanAmountRequired = Math.Round(bondAmount);
            calculator.PreProspect.EstimatedPropertyValue = Math.Round(bondAmount);
            calculator.PreProspect.Deposit = profitFromSale;
            calculator.PreProspect.TotalPrice = homeValue;// webCalculatorBase.PreProspect.LoanAmountRequired;
            calculator.PreProspect.InterestRate = interestRate;

            // Set up the Calculator warning Message
            lblMessage.Text = "";

            var minLtv = MinLtv / 100;
            var maxPti = MaxPti / 100;
            var hasWarning = false;

            if (minLtv <= ltv)
            {
                lblMessage.Text += "SA Home Loans lending policy only allows loans up to a " + Convert.ToString(MinLtv) + "% LTV ratio. ";
                hasWarning = true;
            }
            if (maxPti <= pti)
            {
                lblMessage.Text += "The payment to income ratio exceeds " + Convert.ToString(MaxPti) + "% . ";
                hasWarning = true;
            }
            if (earnings < MinHouseholdIncome)
            {
                lblMessage.Text += "The income specified is insufficient. ";
                hasWarning = true;
            }

            if (!hasWarning)
                lblMessage.Text = "<br/>Congratulations - SA Home Loans would welcome the opportunity to offer you home loan finance.";

            lblMessage.Visible = true;

            txtSalaryOne.Enabled = false;
            txtSalaryTwo.Enabled = false;
            txtMonthlyInstalment.Enabled = false;
            txtOtherContribution.Enabled = false;
            txtProfitFromSale.Enabled = false;
            txtLoanTerm.Enabled = false;
            bttnCalculate.Visible = false;
            txtInterestRate.Enabled = false;
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
                        MaxPti = calculator.GetMaximumPTI();
                        MinLtv = calculator.GetMinimumLTV();
                        MaxTerm = Math.Round(calculator.GetMaximumTerm()/12.0);
                        MinHouseholdIncome = calculator.GetMinimumHouseholdIncome();
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
                    // Initialise Preprospect with the Advertising variables
                    var session = SahlSession;
                    if (session != null)
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

            try
            {
                CalculateInterestRate();
            }
            catch
            {
                offline = true;
            }

            if (offline)
            {
                pnlCalculatorOffline.Visible = true;
                pnlCalculatorOnline.Visible = false;
            }

            txtSalaryOne.Text = "0";
            txtSalaryOne.Enabled = true;
            txtSalaryTwo.Text = "0";
            txtSalaryTwo.Enabled = true;
            txtMonthlyInstalment.Text = "0";
            txtMonthlyInstalment.Enabled = true;
            txtOtherContribution.Text = "0";
            txtOtherContribution.Enabled = true;
            txtProfitFromSale.Text = "0";
            txtProfitFromSale.Enabled = true;
            txtLoanTerm.Text = "20";
            txtLoanTerm.Enabled = true;
            txtInterestRate.Text = Convert.ToString(InterestRate); // make sure there's always an interest rate set.
            txtInterestRate.Enabled = true;
            bttnCalculate.Visible = true;

            if (!offline) applicationAffordability.Reset();

            pnlResults.Visible = false;
            pnlFiller.Visible = true;
            pnlAffordabilityCalculator.Visible = true;
            applicationAffordability.Visible = false;
        }
        
        /// <summary>
        /// Calculates the maximum installment and stores the result in the web calculator base.
        /// </summary>
        private void CalculateMaximumInstallment()
        {
            var salaryOne = string.IsNullOrWhiteSpace(txtSalaryOne.Text) ? 0.0 : Convert.ToDouble(txtSalaryOne.Text);
            var salaryTwo = string.IsNullOrWhiteSpace(txtSalaryTwo.Text) ? 0.0 : Convert.ToDouble(txtSalaryTwo.Text);
            var total = salaryOne + salaryTwo;
            var max = (total / 100) * 30;
            if (total < MaxTerm) return;

            BaseCalculator.PreProspect.HouseholdIncome = total;
            BaseCalculator.PreProspect.MaximumInstallment = max;
        }

        /// <summary>
        /// This method sets up the parameters for validation according to SAHL business rules
        /// </summary>
        private void SetupErrorMessages()
        {
            // Income 
            validatorSalary.ErrorMessage = "Your gross income must exceed " + MinHouseholdIncome.ToString(CurrencyFormat) + " to qualify.";
           
            // Loan Duration 
            validatorLoanTerm.MinimumValue = Convert.ToString(1);
            validatorLoanTerm.MaximumValue = Convert.ToString(MaxTerm);
            validatorLoanTerm.ErrorMessage = "The Term of Loan must be between 1 and " + Convert.ToString(MaxTerm) + " years.";
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
                                                     txtSalaryOne = txtSalaryOne.ClientID,
                                                     txtSalaryTwo = txtSalaryTwo.ClientID,
                                                     txtProfitFromSale = txtProfitFromSale.ClientID,
                                                     txtOtherContribution = txtOtherContribution.ClientID,
                                                     txtMonthlyInstalment = txtMonthlyInstalment.ClientID,
                                                     txtLoanTerm = txtLoanTerm.ClientID,
                                                     txtInterestRate = txtInterestRate.ClientID,
                                                     lblStatus = lblStatus.ClientID
                                                 },
                                  minHouseholdIncome = MinHouseholdIncome
                              };

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null) scriptManager.RegisterScriptControl(this);
            ScriptManager.RegisterStartupScript(pnlAffordabilityCalculator, GetType(), "AffordabilityCalculator.Initialization", "$.affordabilityCalculator.init(" + options.ToJson() + ");", true);
            ScriptManager.RegisterStartupScript(pnlAffordabilityCalculator, GetType(), "AffordabilityCalculator.TestimonialInit", string.Format("$('#{0} .testimonial').testimonial();", pnlFiller.ClientID), true);
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
            return new[]
                       {
                           new ScriptReference("SAHL.Internet.Components.Calculators.Scripts.Input.js", Assembly.GetExecutingAssembly().FullName),
                           new ScriptReference("SAHL.Internet.Components.Calculators.Scripts.AffordabilityCalculator.js", Assembly.GetExecutingAssembly().FullName)
                       };
        }
    }
}