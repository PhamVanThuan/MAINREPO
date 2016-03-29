using NUnit.Framework;
using SAHL.Common.BusinessModel.DataTransferObjects;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Test.DataTransferObjects
{
    [TestFixture]
    public class LeadInputInformationTests  : TestBase
    {
        [Test]
        public void TestValidation()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                OfferSourceKey = (int)OfferSources.InternetLead,
                
                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Newpurchase,

                ProductKey = (int) Products.NewVariableLoan,

                HouseholdIncome = 50000,
                Term = 240,
                //Lead
                MonthlyInstalment = 50000,
                InterestRate = 0.08,
                TotalPrice = 1500000,
                PropertyValue = 150000,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                EmploymentTypeKey = (int) EmploymentTypes.Salaried,
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            var errors = "";
            var validationResult = leadInput.Validate(out errors);

            Assert.True(validationResult, string.Format("LeadInput validation Failed: {0}",errors));
        }


        [Test]
        public void TestValidation_RefinanceOfferType_Fails()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                OfferSourceKey = (int)OfferSources.InternetLead,

                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Refinance, // only OfferTypes Unknown, NewPurchase and SwitchLoan accepted - not Refinance

                ProductKey = (int)Products.NewVariableLoan,

                HouseholdIncome = 50000,
                Term = 240,
                //Lead
                MonthlyInstalment = 50000,
                InterestRate = 0.08,
                TotalPrice = 1500000,
                PropertyValue = 150000,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                EmploymentTypeKey = (int)EmploymentTypes.Salaried,
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            var errors = "";
            var validationResult = leadInput.Validate(out errors);

            Assert.False(validationResult, string.Format("LeadInput validation Failed: {0}", errors));
        }


        [Test]
        public void TestValidation_NoEmploymentType_Fails()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                OfferSourceKey = (int)OfferSources.InternetLead,

                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Switchloan,

                ProductKey = (int)Products.NewVariableLoan,

                HouseholdIncome = 50000,
                Term = 240,
                //Lead
                MonthlyInstalment = 50000,
                InterestRate = 0.08,
                TotalPrice = 1500000,
                PropertyValue = 150000,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                EmploymentTypeKey = 0,
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            var errors = "";
            var validationResult = leadInput.Validate(out errors);

            Assert.False(validationResult, string.Format("LeadInput validation Failed: {0}", errors));
        }


        [Test]
        public void TestValidation_IfOfferTypeKnownProductKeyShouldBe_Variable_NewVariable_Varifix_Fails()
        {
            ILeadInputInformation leadInput = new LeadInputInformation
            {
                OfferSourceKey = (int)OfferSources.InternetLead,

                MortgageLoanPurposeKey = (int)MortgageLoanPurposes.Newpurchase,

                ProductKey = (int)Products.PersonalLoan,

                HouseholdIncome = 50000,
                Term = 240,
                //Lead
                MonthlyInstalment = 50000,
                InterestRate = 0.08,
                TotalPrice = 1500000,
                PropertyValue = 150000,

                //  LegalEntity
                FirstNames = "Some",
                Surname = "Dude",
                EmploymentTypeKey = 0,
                //EmailAddress = "sd@ano.com",
                HomePhoneCode = "031",
                HomePhoneNumber = "5555555",
                NumberOfApplicants = 1,
                //Referer
                AdvertisingCampaignID = "AdvertisingCampaignID",
                ReferringServerURL = "ReferringServerURL",
                UserURL = "UserURL",
                UserAddress = "UserAddress"
            };

            var errors = "";
            var validationResult = leadInput.Validate(out errors);

            Assert.False(validationResult, string.Format("LeadInput validation Failed: {0}", errors));
        }



    }
}
