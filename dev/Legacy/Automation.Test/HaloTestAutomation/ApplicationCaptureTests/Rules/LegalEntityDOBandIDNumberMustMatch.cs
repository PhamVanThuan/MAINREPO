using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using NUnit.Framework;
using System;

namespace ApplicationCaptureTests.Rules
{
    [RequiresSTA]
    public class LegalEntityDOBandIDNumberMustMatch : ApplicationCaptureTests.TestBase<BuildingBlocks.Views.LoanCalculator>
    {
        private TestBrowser browser;
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
        }
        /// <summary>
        /// Ensures that a Legal Entity's Date of Birth matches the first 6 Digits of the ID Number
        /// </summary>
        [Test, Description("Ensures that a Legal Entity's Date of Birth matches the first 6 Digits of the ID Number")]
        public void LegalEntityDateOfBirthAndIDNumberMatch()
        {
            //create an application
            int offerKey;
            string idNumber;
            Helper.CreateApplication(ref browser, TestUsers.BranchConsultant, Common.Enums.OfferTypeEnum.NewPurchase, out offerKey, out idNumber);
            Service<IApplicationService>().CleanupNewBusinessOffer(offerKey);
            //get the legalentitykey
            int legalEntityKey = Helper.GetLegalEntityKey(offerKey);
            //Set the Date of Birth
            Service<ILegalEntityService>().UpdateDateOfBirth(legalEntityKey, new DateTime(1901, 01, 01));
            string legalEntityName = Service<ILegalEntityService>().GetLegalEntityLegalNameByLegalEntityKey(legalEntityKey).SQLScalarValue;
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
            browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists(legalEntityName + " : The birth date does not match the first 6 digits of the ID Number.");
        }

        [Test, Description("Ensures that a Legal Entity's Id Number is required prior to submitting into new business")]
        public void LegalEntityIDNumberIsRequired()
        {
            //create an application
            int offerKey;
            string idNumber;
            Helper.CreateApplication(ref browser, TestUsers.BranchConsultant, Common.Enums.OfferTypeEnum.NewPurchase, out offerKey, out idNumber);
            Service<IApplicationService>().CleanupNewBusinessOffer(offerKey);
            //get the legalentitykey
            int legalEntityKey = Helper.GetLegalEntityKey(offerKey);
            Service<ILegalEntityService>().UpdateLegalEntityIDNumber("", legalEntityKey);
            string legalEntityName = Service<ILegalEntityService>().GetLegalEntityLegalNameByLegalEntityKey(legalEntityKey).SQLScalarValue;
            browser.Page<WorkflowSuperSearch>().Search(browser, offerKey, WorkflowStates.ApplicationCaptureWF.ApplicationCapture);
            browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
            browser.Page<WorkflowYesNo>().Confirm(true, false);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity ID Number Required For South African Citizens");
        }
    }
}