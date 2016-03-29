using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.Rules
{
    [RequiresSTA]
    public class LegalEntityHasOpenFurtherLending : PersonalLoansWorkflowTestBase<BasePageAssertions>
    {
        /// <summary>
        /// The application summary should display a warning when the legal entity involved in the personal loan application has an account where
        /// there is an open further lending application.
        /// </summary>
        [Test]
        public void ApplicationSummaryDisplaysWarningWhenLegalEntityHasFurtherLending()
        {
            base.Browser = new BuildingBlocks.TestBrowser(TestUsers.PersonalLoanConsultant2, TestUsers.Password);
            var offer = Service<IApplicationService>().GetRandomOfferRecord(ProductEnum.NewVariableLoan, OfferTypeEnum.FurtherAdvance, OfferStatusEnum.Open);
            var legalEntityKey = Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(offer.OfferKey);
            //go create
            Helper.LoadLegalEntityOnCBO(base.Browser, legalEntityKey, offer.AccountKey);
            //create lead
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<WorkflowYesNo>().ClickYes();
            //check case created
            var extRole = base.Service<IExternalRoleService>().GetActiveExternalRoleByLegalEntity(GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client, legalEntityKey);
            base.GenericKey = extRole.GenericKey;
            base.ReloadCase(WorkflowStates.PersonalLoansWF.ManageLead, WorkflowRoleTypeEnum.PLConsultantD);
            //we should have a validation warning
            base.View.AssertValidationMessagesContains(
                string.Format(@"The Account [{0}] has an open further lending application: {1}", offer.AccountKey, offer.OfferKey));
        }
    }
}