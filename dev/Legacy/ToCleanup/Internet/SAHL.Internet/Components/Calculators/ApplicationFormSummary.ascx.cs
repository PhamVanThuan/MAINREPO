using System;

namespace SAHL.Internet.Components.Calculators
{
    public partial class ApplicationFormSummary : System.Web.UI.UserControl
    {
        private WebCalculatorBase webCalculatorBase = new WebCalculatorBase();
        private SAHLWebSession DNNWebSession = new SAHLWebSession();

        private string stepimageurlAffordability = "";
        private string stepimageurlNewPurchase = "";
        private string stepimageurlSwitchLoan = "";
        /// <summary>
        /// Get or Set The URL for the Step image for the Affordability Calculator
        /// </summary>
        public string StepImageURLAffordability
        {
            get { return stepimageurlAffordability; }
            set { stepimageurlAffordability = value; }
        }


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
           // DNNWebSession = Session["SAHLWebSession"] as SAHLWebSession;

            A1.HRef = Request.Url.ToString();



            webCalculatorBase = Session["webCalculatorBase"] as WebCalculatorBase;
            DNNWebSession = Session["SAHLWebSession"] as SAHLWebSession;

            if (DNNWebSession != null)
            {
                switch (DNNWebSession.LastUsedCalculator)
                {
                    case 1: // affordability
                        stepimage.ImageUrl = stepimageurlAffordability;
                        break;
                    case 2: // switch loan
                        stepimage.ImageUrl = stepimageurlSwitchLoan;
                        break;
                    case 3: // new purchase loan
                        stepimage.ImageUrl = stepimageurlNewPurchase;
                        break;
                }
            }


            //Update the Preprospect Object and store it in the session
            if (webCalculatorBase != null)
            {
                lblTotalLoan.Text = (webCalculatorBase.PreProspect.LoanAmountRequired).ToString(webCalculatorBase.CurrencyFormatNoCents());
                lblLoanPeriod.Text = webCalculatorBase.PreProspect.Term + " months";
                lblInterestRate.Text = (webCalculatorBase.PreProspect.InterestRate).ToString(webCalculatorBase.RateFormat());
                lblMonthlyIncome.Text = (webCalculatorBase.PreProspect.HouseholdIncome).ToString(webCalculatorBase.CurrencyFormatNoCents());
                lblSummaryReferenceNumber.Text = webCalculatorBase.PreProspect.ReferenceNumber;

                if (webCalculatorBase.PreProspect.PurposeNumber == 1) // not Unknown - so set by SAHL calculators
                {
                    if (webCalculatorBase.PreProspect.CapitaliseFees)
                    {
                        rowCapitaliseFees.Visible = true;
                        lblCapitaliseFees.Text = "Yes";
                    }
                    else
                    {
                        rowCapitaliseFees.Visible = false;
                        lblCapitaliseFees.Text = "No";
                    }

                    rowInterestOnly.Visible = true;
                    lblInterestOnly.Visible = true;

                    lblInterestOnly.Text = webCalculatorBase.PreProspect.InterestOnly ? "Yes" : "No";



                    // Set the Fixed Portion to invisible
                    lblIsFixedPortion.Text = "No";
                    rowFixedPortionElected.Visible = true;


                    if (webCalculatorBase.PreProspect.FixPercent != 0)
                    {
                        lblIsFixedPortion.Text = "Yes";

                        rowFixedPortion.Visible = true;
                        lblFixedPortionElected.Text = Convert.ToString(webCalculatorBase.PreProspect.FixPercent) + "%";

                        rowFixedPortionRate.Visible = true;
                        lblFixedInterestRate.Text = webCalculatorBase.PreProspect.ElectedFixedRate.ToString(webCalculatorBase.RateFormat());
                    }
                    else
                    {
                        lblIsFixedPortion.Text = "No";
                        rowFixedPortion.Visible = false;
                        rowFixedPortionRate.Visible = false;
                    }


                }
                else
                {
                    rowInterestOnly.Visible = false;
                }
            }
            else
            {
                // This page has been reached directly - send them home...
                Response.Redirect(ResolveClientUrl("http://www.sahomeloans.com"));
            }

            //TODO create a websession reset object - and run it here 
            // Clear the Session Object
            //Session.Remove("SAHLWebSession");

        }

    }
}