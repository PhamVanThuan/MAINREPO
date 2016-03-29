using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;

[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.ApplicationForm.js", "application/javascript")]
[assembly: WebResource("SAHL.Internet.Components.Calculators.Scripts.Input.js", "application/javascript")]

namespace SAHL.Internet.Components.Calculators
{
    public partial class ApplicationForm : UserControl, IScriptControl
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
        /// Gets the SA Home Loans session object.
        /// </summary>
        private SAHLWebSession SahlSession
        {
            get { return Session["SAHLWebSession"] as SAHLWebSession; }
        }

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
        /// Gets or sets the current rate format.
        /// </summary>
        protected string RateFormat
        {
            get { return ViewState["RateFormat"] as string; }
            private set { ViewState["RateFormat"] = value; }
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
        /// Resets the form.
        /// </summary>
        /// <param name="resetState">Specifies whether to reset the form state as well.</param>
        public void Reset(bool resetState = true)
        {
            var calculator = BaseCalculator;
            if (resetState && calculator != null)
            {
                try
                {
                    CurrencyFormat = calculator.CurrencyFormat();
                    CurrencyFormatNoCents = calculator.CurrencyFormatNoCents();
                    RateFormat = calculator.RateFormat();
                }
                catch
                {
                    /* error */
                }
            }

            txtFirstNames.Text = string.Empty;
            txtSurname.Text = string.Empty;
            txtEmailAddress.Text = string.Empty;
            tbNumApplicants.Text = "1";
            phCode.Text = string.Empty;
            phNumber.Text = string.Empty;

            pnlAlreadyApplied.Visible = false;
            pnlApplicationForm.Visible = true;
            summaryApplication.Visible = false;
            pnlApplicationDetails.Visible = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack) Reset();

            summaryApplication.BaseCalculator = BaseCalculator;
            if (!IsPostBack) summaryApplication.Reset(); // Ensure the summary state has been reset.
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data. </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            RegisterClientScripts();

            var calculator = BaseCalculator;
            if (calculator != null)
            {
                lblTotalLoan.Text = (calculator.PreProspect.LoanAmountRequired).ToString(CurrencyFormatNoCents);
                //lblTotalLoan.Text = (PreProspect.PurchasePrice + PreProspect.CashOut).ToString(SAHL.Common.Constants.CurrencyFormatNoCents);
                lblLoanPeriod.Text = calculator.PreProspect.Term + " months";
                lblInterestRate.Text = (calculator.PreProspect.InterestRate).ToString(RateFormat);
                lblMonthlyIncome.Text = (calculator.PreProspect.HouseholdIncome).ToString(CurrencyFormatNoCents);

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
                        lblFixedInterestRate.Text = calculator.PreProspect.ElectedFixedRate.ToString(RateFormat);
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

            DataBind();

            // The user has already submitted an offer during this session - hide everything
            if (SahlSession == null || (SahlSession != null && !SahlSession.ApplicationSubmitted)) return;

            pnlAlreadyApplied.Visible = true;
            pnlApplicationDetails.Visible = false;

            if (SahlSession.ApplicationKey != null)
            {
                pnlReferenceNumber.Visible = true;
                lblReferenceNumber.Text = Convert.ToString(SahlSession.ApplicationKey.Value);
            }
            pnlExtra.Visible = false;
        }

        protected void ApplicationFormCommand(object sender, EventArgs e)
        {
            var calculator = BaseCalculator;

            // Populate the Summary Variables 
            if (calculator == null) return;
            calculator.PreProspect.PreProspectSurname = txtSurname.Text;
            calculator.PreProspect.PreProspectFirstNames = txtFirstNames.Text;
            calculator.PreProspect.PreProspectEmailAddress = txtEmailAddress.Text;
            calculator.PreProspect.PreProspectHomePhoneCode = phCode.Text;
            calculator.PreProspect.PreProspectHomePhoneNumber = phNumber.Text;
            calculator.PreProspect.NumberOfApplicants = Convert.ToInt32(tbNumApplicants.Text);

            // Create The Lead
            calculator.PreProspect.ReferenceNumber = calculator.CreateLead(calculator.PreProspect).ToString();
            calculator.PreProspect.OfferSubmitted = true;

            if (SahlSession == null) return;
            if (calculator.PreProspect.ReferenceNumber != null) SahlSession.ApplicationKey = Convert.ToInt32(calculator.PreProspect.ReferenceNumber);
            SahlSession.ApplicationSubmitted = true;

            pnlApplicationForm.Visible = false;
            summaryApplication.Visible = true;

            ScriptManager.RegisterClientScriptBlock(pnlApplicationForm, GetType(), "Application.Tracking", "if (_gaq) _gaq.push([\"_trackPageview\", \"/track/application-sent\"]);", true);
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
                    phCode = phCode.ClientID,
                    phNumber = phNumber.ClientID,
                    tbNumApplicants =  tbNumApplicants.ClientID,
                }
            };

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null) scriptManager.RegisterScriptControl(this);
            ScriptManager.RegisterStartupScript(pnlApplicationForm, GetType(), "ApplicationForm.Initialization", "$.applicationForm.init(" + options.ToJson() + ");", true);
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
                           new ScriptReference("SAHL.Internet.Components.Calculators.Scripts.ApplicationForm.js", Assembly.GetExecutingAssembly().FullName)
                       };
        }
    }
}

