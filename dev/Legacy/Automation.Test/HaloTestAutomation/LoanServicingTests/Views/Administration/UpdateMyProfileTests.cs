using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public class UpdateMyProfileTests : BuildingBlocks.TestBase<UpdateMyProfileDetails>
    {
        private Automation.DataModels.LegalEntity legalEntity;
        private Randomizer randomizer;
        protected override void OnTestFixtureSetup()
        {
            randomizer = new Randomizer();
            var adUser = Service<IADUserService>().GetADUserKeyByADUserName(TestUsers.ClintonS).First();
            var newContactDetails = GenerateNewContactDetails();
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: adUser.LegalEntityKey);
            base.Browser = new TestBrowser(TestUsers.ClintonS, TestUsers.Password_ClintonS);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
        }
        protected override void OnTestStart()
        {
 	        base.OnTestStart();
            base.Browser.Navigate<AdministrationActions>().UpdateMyProfileDetails();
        }

        [Test]
        public void when_updating_my_profile_a_firstName_and_surname_must_be_provided()
        {
            base.View.RemoveFirstNameAndSurnameAndSubmit();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity First Name Required", "Legal Entity Surname Required");
        }

        [Test]
        public void when_updating_my_profile_my_contact_details_can_be_changed()
        {
            var newContactDetails = GenerateNewContactDetails();
            base.View.UpdateContactDetails(newContactDetails);
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntity.LegalEntityKey);
            Assert.That(legalEntity.HomePhoneCode == newContactDetails.HomePhoneNumber.Code);
            Assert.That(legalEntity.HomePhoneNumber == newContactDetails.HomePhoneNumber.Number);
            Assert.That(legalEntity.WorkPhoneCode == newContactDetails.WorkPhoneNumber.Code);
            Assert.That(legalEntity.WorkPhoneNumber == newContactDetails.WorkPhoneNumber.Number);
            Assert.That(legalEntity.FaxCode == newContactDetails.FaxNumber.Code);
            Assert.That(legalEntity.FaxNumber == newContactDetails.FaxNumber.Number);
            Assert.That(legalEntity.EmailAddress == newContactDetails.EmailAddress);
            Assert.That(legalEntity.CellPhoneNumber == newContactDetails.CellphoneNumber.ToString());
        }

        [Test]
        public void when_updating_my_profile_my_personal_details_can_be_changed()
        {
            string newFirstName = "Clinton Thomas";
            string newSurname = "Speed";
            string newInitials = "C";
            string newPreferredName = "Clinton";
            base.View.UpdateDetails(newFirstName, newSurname, newPreferredName, newInitials);
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntity.LegalEntityKey);
            Assert.That(legalEntity.FirstNames.Equals(newFirstName));
            Assert.That(legalEntity.Surname.Equals(newSurname));
            Assert.That(legalEntity.Initials.Equals(newInitials));
            Assert.That(legalEntity.PreferredName.Equals(newPreferredName));
        }

        [Test]
        public void when_updating_my_profile_my_education_level_can_be_changed()
        {
            var educationLevel = base.View.UpdateEducationLevel();
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntity.LegalEntityKey);
            Assert.That(legalEntity.EducationDescription.Equals(educationLevel));
        }

        [Test]
        public void when_updating_my_profile_my_document_language_can_be_changed()
        {
            var documentLanguage = base.View.UpdateDocumentLanguage();
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntity.LegalEntityKey);
            Assert.That(legalEntity.DocumentLanguageDescription.Equals(documentLanguage));
        }

        [Test]
        public void when_updating_my_profile_my_home_language_can_be_changed()
        {
            var homeLanguage = base.View.UpdateHomeLanguage();
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntity.LegalEntityKey);
            Assert.That(legalEntity.HomeLanguageDescription.Equals(homeLanguage));
        }

        private LegalEntityContactDetails GenerateNewContactDetails()
        {
            return new Automation.DataModels.LegalEntityContactDetails
            {
                CellphoneNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                WorkPhoneNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                HomePhoneNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                FaxNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                EmailAddress = "clintons@sahomeloans.com"
            };
        }
    }
}
