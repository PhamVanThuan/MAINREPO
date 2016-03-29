using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.Origination;
using BuildingBlocks.Timers;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core.Logging;
namespace ApplicationCaptureTests.Workflow.Capitec
{
    [TestFixture, RequiresSTA]
    public sealed class ContactWithClient : CapitecBase
    {
        [Test]
        public void ContactWithClient_WhenAtCapitecApplicationsStage_ShouldDisplayAction()
        {
            //Setup Test Pack
            this.SetupApp();
            this.LoadApp();

            //Assert Precondition

            //Execute Test
            var actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationCapture.ContactwithClient);

            //Assert
            Assert.True(actionExists, "Contact with Client action is not visible at the Capitec Applications stage.");
        }
        [Test]
        public void ContactWithClient_WhenAtCapitecApplicationsStage_ShouldLoadContactClientPresenter()
        {
            //Setup Test Pack
            this.SetupApp();
            this.LoadApp();

            //Assert Precondition
            var actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationCapture.ContactwithClient);
            Assert.True(actionExists, "Contact with Client action is not visible at the Capitec Applications stage.");

            //Execute Test
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContactwithClient);

            //Assert
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("WF_Capitec_Contact_Client");
        }
        [Test]
        public void ContactWithClient_WhenCancel_ShouldFocusOnApplicationSummary()
        {
            //Setup Test Pack
            this.SetupApp();
            this.LoadApp();

            //Assert Precondition
            var actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationCapture.ContactwithClient);
            Assert.True(actionExists, "Contact with Client action is not visible at the Capitec Applications stage.");
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContactwithClient);
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("WF_Capitec_Contact_Client");

            //Execute Test
            base.Browser.Page<ClientWithContact>().ClickCancel();

            //Assert
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("ApplicationSummary");
        }
        [Test]
        public void ContactWithClient_WhenClearingDate_ShouldHaveValidationOnSubmit()
        {
            //Setup Test Pack
            this.SetupApp();
            this.LoadApp();

            //Assert Precondition
            var actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationCapture.ContactwithClient);
            Assert.True(actionExists, "Contact with Client action is not visible at the Capitec Applications stage.");
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContactwithClient);
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("WF_Capitec_Contact_Client");

            //Execute Test
            base.Browser.Page<ClientWithContact>().ClearAll();
            base.Browser.Page<ClientWithContact>().ClickSubmit();

            //Assert
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(new string[] {
                "Contact Date must be entered",
                "Comments must captured"
            });
        }
        [Test]
        public void ContactWithClient_WhenFutureDate_ShouldHaveValidationOnSubmit()
        {
            //Setup Test Pack
            this.SetupApp();
            this.LoadApp();
            var expectedEndDate = DateTime.Now.AddDays(1);
            var expectedEndDateStr = expectedEndDate.Date.ToString(Formats.DateFormat).Replace("-", "/");

            //Assert Precondition
            var actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationCapture.ContactwithClient);
            Assert.True(actionExists, "Contact with Client action is not visible at the Capitec Applications stage.");
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContactwithClient);
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("WF_Capitec_Contact_Client");

            //Execute Test
            base.Browser.Page<ClientWithContact>().PopulateFields(expectedEndDateStr, "");
            base.Browser.Page<ClientWithContact>().ClickSubmit();

            //Assert
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(new string[] {
                "Contact Date cannot be greater than today",
            });
        }
        [Test]
        public void ContactWithClient_WhenCapturingDateAndComments_ShouldCreateStageTransitionAndReasons()
        {
            //Setup Test Pack
            this.SetupApp();
            this.LoadApp();

            var expectedEndDate = DateTime.Now;
            var expectedEndDateStr = expectedEndDate.Date.ToString(Formats.DateFormat).Replace("-", "/");
            var expectedComments = "ContactwithClient automation test.";
            var expectedStageTransitionComment = String.Format("Contacted on : {0}", expectedEndDateStr);

            //Assert Precondition
            var actionExists = base.Browser.ActionExists(WorkflowActivities.ApplicationCapture.ContactwithClient);
            Assert.True(actionExists, "Contact with Client action is not visible at the Capitec Applications stage.");
            base.Browser.ClickAction(WorkflowActivities.ApplicationCapture.ContactwithClient);
            base.Browser.Page<BasePageAssertions>().AssertViewLoaded("WF_Capitec_Contact_Client");

            //Execute Test
            base.Browser.Page<ClientWithContact>().PopulateFields(expectedEndDateStr, expectedComments);
            base.Browser.Page<ClientWithContact>().ClickSubmit();

            //Assert
            StageTransitionAssertions.AssertStageTransitionCreated(
                base.Assertable.ExpectedApplicationKey,
                StageDefinitionStageDefinitionGroupEnum.CapitecClientContacted);

            StageTransitionAssertions.AssertStageTransitionEndDate(
                base.Assertable.ExpectedApplicationKey,
                StageDefinitionStageDefinitionGroupEnum.CapitecClientContacted, expectedEndDate);

            StageTransitionAssertions.AssertStageTransitionComment(
                base.Assertable.ExpectedApplicationKey,
               StageDefinitionStageDefinitionGroupEnum.CapitecClientContacted, expectedStageTransitionComment);

            ReasonAssertions.AssertReason(ReasonDescription.CapitecClientContacted, ReasonType.ContactWithClient
                , base.Assertable.ExpectedApplicationKey, GenericKeyTypeEnum.Offer_OfferKey, reasonExists: true, expectedComment: expectedComments);
        }
        
        private void LoadApp()
        {
            base.Browser.Navigate<WorkFlowsNode>().WorkFlows(base.Browser);
            base.Browser.Page<WorkflowSuperSearch>().WorkflowSearch(base.Assertable.ExpectedApplicationKey);
            GeneralTimer.Wait(2000);
        }
        private void SetupApp()
        { 
            //Setup Test Pack
            var idnumber = IDNumbers.GetNextIDNumber();

            //Assert Precondition

            //Execute Test
            base.CreateApplication(MortgageLoanPurposeEnum.Newpurchase,
                true,
                idNumber: idnumber,
                firstNames: String.Format("CapitecApplicationContactWithClient{0}", randomizer.Next(0, 10)));

            //Assert
            base.AssertApplication();
        }
    }
}
