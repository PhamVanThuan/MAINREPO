using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;

namespace PersonalLoansTests.LoanServicing
{
    [RequiresSTA]
    public class PersonalLoansCorrespondenceTests : PersonalLoansWorkflowTestBase<CorrespondenceProcessing>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.PersonalLoanClientServiceUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.GenericKey = Helper.FindPersonalLoanAccount(true);
            base.PersonalLoanAccount = Service<IAccountService>().GetAccountByKey(this.GenericKey);
            var legalentitykeys = from role in base.Service<ILegalEntityService>().GetLegalEntityRoles(PersonalLoanAccount.AccountKey)
                                  select role.LegalEntityKey;
            foreach (var leKey in legalentitykeys)
                base.Service<IClientEmailService>().UpdateLegalEntityEmailAddress(leKey, "clintons@sahomeloans.com");
            base.SearchAndLoadAccount();
        }

        [Test]
        public void when_sending_capitalisation_letter_should_write_to_image_index()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().CapitalisationLetter();
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            CorrespondenceAssertions.AssertImageIndex(PersonalLoanAccount.AccountKey.ToString(), CorrespondenceReports.CapitalisationLetter, CorrespondenceMedium.Email, PersonalLoanAccount.AccountKey, 0);
        }

        [Test]
        public void when_sending_capitalisation_letter_should_show_fax_email_post_options()
        {
            base.Browser.Navigate<LoanServicingCBO>().Correspondence(0);
            base.Browser.Navigate<LoanServicingCBO>().CapitalisationLetter();

            base.View.AssertPostAvailable();
            base.View.AssertEmailAvailable();
            base.View.AssertFaxAvailable();
        }
    }
}