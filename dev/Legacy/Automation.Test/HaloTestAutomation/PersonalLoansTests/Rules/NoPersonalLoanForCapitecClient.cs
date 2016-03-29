using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace PersonalLoansTests.Rules
{
    [RequiresSTA]
    public class NoPersonalLoanForCapitecClient : PersonalLoansWorkflowTestBase<BasePageAssertions>
    {
        public int AccountKey { get; set; }

        public int OfferKey { get; set; }

        public int LegalEntityKey { get; set; }

        public string IDNumber { get; set; }

        private Dictionary<int, LegalEntityTypeEnum> Roles;

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.PersonalLoanConsultant2, TestUsers.Password);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
        }

        /// <summary>
        /// Tests that if a Capitec account exists, one cannot apply for a personal Loan.
        /// </summary>
        [Test]
        public void Given_That_A_Capitec_Account_Exists_Assert_That_One_Cannot_Apply_For_A_Personal_Loan()
        {
            // Get account details
            GetAccountDetails();

            // Delete Offer Attribute if it already exists
            Service<IApplicationService>().DeleteOfferAttribute(this.OfferKey, OfferAttributeTypeEnum.Capitec);

            // Insert the Capitec offer attribute against the offer
            Service<IApplicationService>().InsertOfferAttribute(this.OfferKey, OfferAttributeTypeEnum.Capitec);

            // Attempt to create the Personal Loan lead
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().PersonalLoansMenu(base.Browser);
            base.Browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(this.IDNumber);

            // Check that the correct validation message is thrown
            base.View.AssertValidationMessageExists("This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.");

            // Delete the inserted offer attribute.
            Service<IApplicationService>().DeleteOfferAttribute(this.OfferKey, OfferAttributeTypeEnum.Capitec);
        }

        /// <summary>
        /// Test that an existing Capitec Account cannot be used for the creation of a Capitec Lead
        /// </summary>
        [Test]
        public void Given_That_A_Capitec_Account_Exists_Assert_That_One_Cannot_Use_Account_When_Generating_A_Personal_Loan_Lead()
        {
            // Get account details
            GetAccountDetails();

            // Delete Offer Attribute if it already exists
            Service<IApplicationService>().DeleteOfferAttribute(this.OfferKey, OfferAttributeTypeEnum.Capitec);

            // Insert the Capitec offer attribute against the offer
            Service<IApplicationService>().InsertOfferAttribute(this.OfferKey, OfferAttributeTypeEnum.Capitec);

            // Click Personal Loan action
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().PersonalLoansMenu(base.Browser);

            // Add the LegalEntity IDNumber and click the Create Lead Button
            base.Browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(this.IDNumber);

            // Check that the correct validation message is thrown
            base.View.AssertValidationMessageExists("This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.");

            // Delete the inserted offer attribute.
            Service<IApplicationService>().DeleteOfferAttribute(this.OfferKey, OfferAttributeTypeEnum.Capitec);
        }

        /// <summary>
        /// Tests that if a Capitec application exists, one cannot apply for a personal Loan.
        /// </summary>
        [Test]
        public void Given_That_A_Capitec_Application_Exists_Assert_That_One_Cannot_Apply_For_A_Personal_Loan()
        {
            // Get a case to work with
            var offer = Service<IApplicationService>().GetCapitecMortgageLoanApplication();
            var idNumber = Service<IApplicationService>().GetFirstApplicantIDNumberOnOffer(offer.OfferKey);

            // Load Legal Entity onto the CBO
            Helper.LoadOfferApplicantInCBO(base.Browser, idNumber);

            // Attempt to create the lead
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            // Check that the correct validation message is thrown
            base.View.AssertValidationMessageExists("This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.");
        }

        /// <summary>
        /// Test that an existing Capitec Application cannot be used for the creation of a Capitec Lead
        /// </summary>
        [Test]
        public void Given_That_A_Capitec_Application_Exists_Assert_That_One_Cannot_Use_Application_When_Generating_A_Personal_Loan_Lead()
        {
            // Get a case to work with
            var offer = Service<IApplicationService>().GetCapitecMortgageLoanApplication();
            var idNumber = Service<IApplicationService>().GetFirstApplicantIDNumberOnOffer(offer.OfferKey);

            // Click Personal Loan action
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().PersonalLoansMenu(base.Browser);

            // Add the LegalEntity IDNumber and click the Create Lead Button
            base.Browser.Page<BuildingBlocks.Presenters.LegalEntity.LegalEntityDetailsLeadApplicantAdd>().AddExistingLegalEntity(idNumber.ToString());

            // Check that the correct validation message is thrown
            base.View.AssertValidationMessageExists("This is a Capitec Client and therefore cannot be offered a Personal Loan with SAHL.");
        }

        /// <summary>
        /// Sets the related legal entity properties for a given account
        /// </summary>
        private void GetAccountDetails()
        {
            // Get an open account that does not have a personal loan
            var results = Service<IAccountService>().GetOpenAccountAndAssociatedOfferWithoutAGivenProductType(OfferTypeEnum.NewPurchase, ProductEnum.PersonalLoan);
            this.AccountKey = int.Parse(results.First().Columns[0].Value);
            this.OfferKey = int.Parse(results.First().Columns[1].Value);
            this.Roles = Service<IAccountService>().AccountRoleLegalEntityKeys(this.AccountKey);
            this.LegalEntityKey = (from r in this.Roles select r.Key).FirstOrDefault();
            this.IDNumber = Service<ILegalEntityService>().GetLegalEntityIDNumber(this.LegalEntityKey);
        }
    }
}