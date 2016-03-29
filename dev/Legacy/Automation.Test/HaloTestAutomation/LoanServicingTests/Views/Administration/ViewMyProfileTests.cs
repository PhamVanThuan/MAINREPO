using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Navigation.CBO;
using BuildingBlocks.Presenters.Admin;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System.Linq;

namespace LoanServicingTests.Views.Administration
{
    [RequiresSTA]
    public class ViewMyProfileTests : BuildingBlocks.TestBase<ViewMyProfileDetails>
    {
        private Automation.DataModels.LegalEntity legalEntity;
        private Randomizer randomizer;
        protected override void OnTestFixtureSetup()
        {
            randomizer = new Randomizer();
            base.OnTestFixtureSetup();
            var adUser = Service<IADUserService>().GetADUserKeyByADUserName(TestUsers.HaloUser).First();
            var newContactDetails = new LegalEntityContactDetails
            {
                CellphoneNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                WorkPhoneNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                HomePhoneNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                FaxNumber = new PhoneNumber(string.Format(@"{0} {1}", randomizer.Next(999), randomizer.Next(9999999))),
                EmailAddress = "clintons@sahomeloans.com"
            };
            Service<ILegalEntityService>().UpdateLegalEntityContactDetails(adUser.LegalEntityKey, newContactDetails);
            Service<ILegalEntityService>().UpdateDocumentLanguage(adUser.LegalEntityKey, Common.Enums.LanguageEnum.English);
            Service<ILegalEntityService>().UpdateEducationLevel(adUser.LegalEntityKey, Common.Enums.EducationEnum.UniversityDegree);
            Service<ILegalEntityService>().UpdateHomeLanguage(adUser.LegalEntityKey, Common.Enums.LanguageEnum.English);
            Service<ILegalEntityService>().UpdatePreferredName(adUser.LegalEntityKey, "Clint");
            Service<ILegalEntityService>().UpdateLegalEntityInitials(adUser.LegalEntityKey, "CT");
            legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: adUser.LegalEntityKey);
            base.Browser = new TestBrowser(TestUsers.HaloUser, TestUsers.Password);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().AdministrationNode();
            base.Browser.Navigate<AdministrationActions>().ViewMyProfileDetails();
        }

        [Test]
        public void when_navigating_to_the_view_my_profile_screen_all_my_details_should_display()
        {
            base.View.AssertLegalEntityDetailsAreDisplayed(legalEntity);
        }
    }
}