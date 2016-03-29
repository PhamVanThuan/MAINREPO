namespace SAHL.Web.Services.Test
{
    using System;

    public partial class TestHarness : System.Web.UI.Page
    {
        protected void Button1_Click(object sender, EventArgs e)
        {
            string errors;
            Application app_ws = new Application();

            // InternetLead = 96, InternetApplication = 97, MobisiteLead = 174, MobisiteApplication = 175
            int offerSourceKey = 96;

            if (RadioButton2.Checked)
                offerSourceKey = 97;
            else if (RadioButton3.Checked)
                offerSourceKey = 174;
            else if (RadioButton4.Checked)
                offerSourceKey = 175;

            PreProspect data = new PreProspect { PreProspectFirstNames = "bob", 
                                            PreProspectSurname = "smith", 
                                            PreProspectEmailAddress = @"bob.smith@just_testing.123", 
                                            PreProspectHomePhoneCode = "000",
                                            PreProspectHomePhoneNumber = "1234567", 
                                            NumberOfApplicants = 1,
                                            PurposeNumber = 1, //Unknown
                                            OfferSourceKey = offerSourceKey,
                                            EmploymentType = 1, // Salaried
                                            ProductKey = 9, //New Variable Loan
                                            EstimatedPropertyValue = 250000,
                                            HouseholdIncome = 10000,
                                            Deposit = 50000,
                                            Term = 240,
                                            TotalPrice = 250000, 
                                            MaximumInstallment = 3000,
                                            InterestRate = .08,
                                            AdvertisingCampaignID = "blah",
                                            ReferringServerURL = @"http://blah_ReferringServerURL",
                                            UserURL = @"http://blah_UserURL",
                                            UserAddress = @"10.0.0.1"
            };

            app_ws.CreateInternetLead(data, out errors);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Calculators cal = new Calculators();

            lblMaximumPurchasePrice.Text = cal.GetMaximumPurchasePriceForSelfEmployedOrSalaried(CheckBox1.Checked).ToString();
        }
    }
}