using Automation.DataModels;
using BuildingBlocks;
using BuildingBlocks.Navigation.FLOBO.Common;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace ApplicationCaptureTests.Rules
{
    /// <summary>
    /// Contains rule tests for Legal Entities.
    /// </summary>
    [RequiresSTA]
    public class LegalEntityRules : ApplicationCaptureTests.TestBase<BasePage>
    {
        private TestBrowser browser;

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            if (browser != null)
            {
                try
                {
                    browser.Page<BasePage>().CheckForErrorMessages();
                }
                finally
                {
                    browser.Dispose();
                    browser = null;
                }
            }
        }

        /// <summary>
        /// Check that a validation message is displayed
        /// when Submitting an existing application in application capture,
        /// where the Date Of Birth on one of the applicants, is less than 18 years old
        /// </summary>
        [Test, Description(@"Check that a validation message is displayed
        when Submitting an existing application in application capture,
        where the Date Of Birth on one of the applicants, is less than 18 years old")]
        public void _001_SubmitApplicationApplicantIsYoungerThan18OnNextBirthday()
        {
            //Get an Open Offer pre Submission
            var results = Service<IX2WorkflowService>().GetOpenOffersForPreSubmission(1);
            var offerKey = results.Rows(0).Column(0).GetValueAs<int>();
            var adUsername = results.Rows(0).Column(1).Value;
            var legalEntityKey = results.Rows(0).Column(2).GetValueAs<int>();
            var origIDNumber = results.Rows(0).Column(3).GetValueAs<string>();
            var originalDateOfBirth = results.Rows(0).Column(4).GetValueAs<string>();
            var legalEntityName = Service<ILegalEntityService>().GetLegalEntityLegalName(legalEntityKey);
            //clean up offer
            Service<IApplicationService>().CleanupNewBusinessOffer(offerKey);
            //we set it to 16 so that the legal entity's age on their NEXT birthday will be less than 18
            var date = DateTime.Now.AddYears(-16);
            var month = date.Month.ToString();
            month = month.Length == 1 ? string.Format(@"0{0}", month) : month;
            var day = date.Day.ToString();
            day = day.Length == 1 ? string.Format(@"0{0}", day) : day;
            string dateString = string.Format(@"{0}{1}{2}", date.Year.ToString().Substring(2, 2), month, day);
            var idNumber = IDNumbers.GetNextIDNumber(dateString);
            //update le idnumber
            Service<ILegalEntityService>().UpdateLegalEntityIDNumber(idNumber, legalEntityKey);
            //Set the Date of Birth
            Service<ILegalEntityService>().UpdateDateOfBirth(legalEntityKey, date);
            //Start the browser and select the offer
            browser = new TestBrowser(adUsername, TestUsers.Password);
            browser.Page<X2Worklist>().SelectCaseFromWorklist(browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, offerKey);

            try
            {
                //Attempt to submit the offer
                browser.ClickAction(WorkflowActivities.ApplicationCapture.SubmitApplication);
                browser.Page<WorkflowYesNo>().Confirm(true, true);
                browser.Page<BasePageAssertions>().AssertValidationMessageExists(string.Format("{0}: The minimum age for a legal entity is 18 years.", legalEntityName));
            }
            finally
            {
                //Clean up
                Service<ILegalEntityService>().UpdateDateOfBirth(legalEntityKey, DateTime.Parse(originalDateOfBirth));
                Service<ILegalEntityService>().UpdateLegalEntityIDNumber(origIDNumber, legalEntityKey);
            }
        }

        [Test, Description(@"Assert that when trying to remove a legal entity that is linked to an affordability assessment, the expected rule runs")]
        public void _002_when_removing_a_legal_entity_that_is_linked_to_an_affordability_assessment_expected_rule_should_run()
        {
            var offerKey = Service<IAffordabilityAssessmentService>().GetffordabilityAssessmentByWorkflowState(WorkflowStates.ApplicationCaptureWF.ApplicationCapture).FirstOrDefault().GenericKey;
            browser = new TestBrowser(TestUsers.BranchConsultant10);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(browser);
            browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            browser.Page<WorkflowSuperSearch>().Search(offerKey);
            var legalEntityName = Service<ILegalEntityService>().GetLegalEntityLegalNamesForOffer(offerKey).FirstOrDefault().Column("LegalEntityLegalName").Value;
            browser.Navigate<ApplicantsNode>().Applicants(NodeTypeEnum.Delete);
            browser.Page<ApplicantsRemove>().RemoveApplicant(legalEntityName);
            browser.Page<BasePageAssertions>().AssertValidationMessageExists("This client and\\or a legal entity related to this client is linked to an affordability assessment.");
        }
    }
}