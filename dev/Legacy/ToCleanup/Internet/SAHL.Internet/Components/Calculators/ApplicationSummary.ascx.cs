using System;

namespace SAHL.Internet.Components.Calculators
{
    public partial class ApplicationSummary : System.Web.UI.UserControl
    {
        /// <summary>
        /// Gets or sets the calculator web service.
        /// </summary>
        public WebCalculatorBase BaseCalculator
        {
            get;
            set;
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
        /// Gets or sets the current rate format.
        /// </summary>
        protected string RateFormat
        {
            get { return ViewState["RateFormat"] as string; }
            private set { ViewState["RateFormat"] = value; }
        }

        /// <summary>
        /// Resets the form.
        /// </summary>
        public void Reset()
        {
            var calculator = BaseCalculator;
            if (calculator == null) return;

            try
            {
                CurrencyFormatNoCents = calculator.CurrencyFormatNoCents();
                RateFormat = calculator.RateFormat();
            }
            catch
            {
                /* error */
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!IsPostBack) Reset();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var calculator = BaseCalculator;
            if (calculator == null) return;

            lblTotalLoan.Text = (calculator.PreProspect.LoanAmountRequired).ToString(CurrencyFormatNoCents);
            lblLoanPeriod.Text = calculator.PreProspect.Term + " months";
            lblInterestRate.Text = (calculator.PreProspect.InterestRate).ToString(RateFormat);
            lblMonthlyIncome.Text = (calculator.PreProspect.HouseholdIncome).ToString(CurrencyFormatNoCents);
            lblSummaryReferenceNumber.Text = calculator.PreProspect.ReferenceNumber;

            if (calculator.PreProspect.PurposeNumber == 1) // not Unknown - so set by SAHL calculators
            {
                if (calculator.PreProspect.CapitaliseFees)
                {
                    pnlCapitaliseFees.Visible = true;
                    lblCapitaliseFees.Text = "Yes";
                }
                else
                {
                    pnlCapitaliseFees.Visible = false;
                    lblCapitaliseFees.Text = "No";
                }

                pnlInterestOnly.Visible = true;
                lblInterestOnly.Visible = true;

                lblInterestOnly.Text = calculator.PreProspect.InterestOnly ? "Yes" : "No";

                // Set the Fixed Portion to invisible
                lblIsFixedPortion.Text = "No";
                pnlFixedPortionElected.Visible = true;

                if (calculator.PreProspect.FixPercent != 0)
                {
                    lblIsFixedPortion.Text = "Yes";

                    pnlFixedPortion.Visible = true;
                    lblFixedPortionElected.Text = Convert.ToString(calculator.PreProspect.FixPercent) + "%";

                    pnlFixedPortionRate.Visible = true;
                    lblFixedInterestRate.Text =
                        calculator.PreProspect.ElectedFixedRate.ToString(RateFormat);
                }
                else
                {
                    lblIsFixedPortion.Text = "No";
                    pnlFixedPortion.Visible = false;
                    pnlFixedPortionRate.Visible = false;
                }
            }
            else
            {
                pnlInterestOnly.Visible = false;
            }
        }
    }
}