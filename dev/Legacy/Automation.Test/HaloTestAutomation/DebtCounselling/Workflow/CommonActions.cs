using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.DebtCounselling;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    /// <summary>
    /// Test class for the Debt Counselling Common Actions
    /// </summary>
    [TestFixture, RequiresSTA]
    public sealed class CommonActions : TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingConsultant);
        }

        #region CommonSendCounterProposal

        /// <summary>
        /// This test performs the common Send Counter Proposal action. It ensures that an active counter proposal can be sent to the debt counsellor via the
        /// correspondence screen.
        /// </summary>
        [Test, Description(@"This test performs the common Send Counter Proposal action. It ensures that an active counter proposal can be sent to the debt counsellor via the
			correspondence screen")]
        public void CommonSendCounterProposal()
        {
            //search for a case at Manage Proposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //insert an active counter proposal
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 5);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendCounterProposal);
            var externalRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.TestCase.DebtCounsellingKey, GenericKeyTypeEnum.debtCounselling_debtCounsellingKey,
                ExternalRoleTypeEnum.DebtCounsellor);
            base.Browser.Page<CorrespondenceProcessingMultipleWorkflowDebtCounsellor>().SelectCorrespondenceRecipient(externalRole.LegalEntityKey);
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, base.TestCase.AssignedUser, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            //Check that the letter has been sent
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.DebtCounsellingCounterProposalLetter,
                 CorrespondenceMedium.Post);
            CorrespondenceAssertions.AssertImageIndex(base.TestCase.DebtCounsellingKey.ToString(), CorrespondenceReports.DebtCounsellingCounterProposalLetter,
                CorrespondenceMedium.Post, base.TestCase.AccountKey, base.TestCase.DebtCounsellingKey);
            //check that the proposal details have been sent
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.TestCase.DebtCounsellingKey, CorrespondenceReports.CounterProposalDetail,
                    CorrespondenceMedium.Post);
            CorrespondenceAssertions.AssertImageIndex(base.TestCase.DebtCounsellingKey.ToString(), CorrespondenceReports.CounterProposalDetail,
                CorrespondenceMedium.Post, base.TestCase.AccountKey, base.TestCase.DebtCounsellingKey);
        }

        /// <summary>
        /// This test performs the common Send Counter Proposal action. It ensures that the Send Counter Proposal action cannot be completed if no active counter
        /// proposal exists. The Notification screen is displayed stating that no active counter proposal exists for this debt counselling case.
        /// </summary>
        [Test, Description(@"This test performs the common Send Counter Proposal action. It ensures that the Send Counter Proposal action cannot be completed if no
			active counter proposal exists. The Notification screen is displayed stating that no active counter proposal exists for this debt counselling case.")]
        public void CommonSendCounterProposalLatestCounterProposalNotActive()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //delete any active counter proposals
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Active);
            //insert a draft counter proposal
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5);
            //send the counter proposal
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendCounterProposal);
            //assert notification screen is displayed stating that there is no active counter proposal
            base.Browser.Page<BasePageAssertions>().AssertNotification("The latest Counter Proposal must be active for this case.");
            WorkflowRoleAssignmentAssertions.AssertLatestDebtCounsellingAssignment(base.TestCase.InstanceID, TestUsers.DebtCounsellingConsultant, WorkflowRoleTypeEnum.DebtCounsellingConsultantD,
                true, true);
            Service<IProposalService>().DeleteProposal(base.TestCase.DebtCounsellingKey, ProposalTypeEnum.CounterProposal, ProposalStatusEnum.Draft);
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Active, 5);
            Service<IProposalService>().InsertCounterProposalByStatus(base.TestCase.DebtCounsellingKey, ProposalStatusEnum.Draft, 5);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.SendCounterProposal);
            //assert notification screen is displayed stating that there is no active counter proposal
            base.Browser.Page<BasePageAssertions>().AssertNotification("The latest Counter Proposal must be active for this case.");
        }

        #endregion CommonSendCounterProposal

        #region CommonChangeInCircumstance

        /// <summary>
        /// This test will perform the Change in Circumstance action on a case, ensuring that the correct stage transition records are being written.
        /// </summary>
        [Test, Description(@"This test will perform the Change in Circumstance action on a case, ensuring that the correct stage transition records are being written.")]
        public void CommonChangeInCircumstance()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            string dateFilter = DateTime.Now.AddSeconds(-30).ToString(Formats.DateTimeFormatSQL);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ChangeinCircumstance);
            base.Browser.Page<ChangeInCircumstance>().Enter17pt3Date(DateTime.Now);
            string comment = "Change in Circumstance";
            base.Browser.Page<ChangeInCircumstance>().EnterComments(comment);
            base.Browser.Page<ChangeInCircumstance>().ClickSave();
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_17_3Received, comment: comment);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_ChangeinCircumstances);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.ManageProposal);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ManageProposal);
        }

        /// <summary>
        /// This test will ensure that the user is provided with a validation message indicating that a valid 17.3 date has to be provided before the user
        /// is allowed to continue. It will then assert that the case has been sent to Manage Proposal state.
        /// </summary>
        [Test, Description(@"This test will ensure that the user is provided with a validation message indicating that a valid 17.3 date has to be provided before the user
		is allowed to continue. It will then assert that the case has been sent to Manage Proposal state.")]
        public void CommonChangeInCircumstanceDateRequired()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendPayment, TestUsers.DebtCounsellingConsultant);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.ChangeinCircumstance);
            base.Browser.Page<ChangeInCircumstance>().ClickSave();
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Please enter a valid date");
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.PendPayment);
        }

        #endregion CommonChangeInCircumstance

        #region CommonUpdateCaseDetails

        /// <summary>
        /// Once a debtcounselling case has been created
        /// a DCA user can update case at certain stages eg.Review Notification
        /// </summary>
        [Test, Description(@"Once a debtcounselling case has been created a DCA user can update case at certain stages eg.Review Notification")]
        public void UpdateReferenceToDebtCounsellingCase()
        {
            //search for a case at Pend Proposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendProposal, TestUsers.DebtCounsellingConsultant);
            string referenceNumber = "TestNewReferenceNumber";
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.UpdateCaseDetails);
            base.Browser.Page<DebtCounsellingCreateCase>().UpdateReference(referenceNumber);
            //the updated reference number should be correct
            DebtCounsellingAssertions.AssertReference(base.TestCase.DebtCounsellingKey, referenceNumber);
        }

        #endregion CommonUpdateCaseDetails
    }
}