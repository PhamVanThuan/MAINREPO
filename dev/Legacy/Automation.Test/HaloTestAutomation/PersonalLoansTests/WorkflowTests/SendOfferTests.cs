using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class SendOfferTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            //Create mandatory data
            base.Service<IApplicationService>().InsertOfferMailingAddress(base.GenericKey);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Dispose();
        }

        /// <summary>
        /// Performs the send offer action, ensuring the correpondence record is added.
        /// </summary>
        [Test]
        public void SendOffer()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.SendOffer);
            //get expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            base.Browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Post);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageApplication);
            Assert.That(offerExists);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_SendOffer);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.ManageApplication, Workflows.PersonalLoans);
            //check the correspondence record has been added
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.PersonalLoanOffer, CorrespondenceMedium.Post);
        }

        /// <summary>
        /// Performs the send offer action without an offer mailing address, ensuring the correct notification is displayed.
        /// </summary>
        [Test]
        public void SendOfferWithoutOfferMailingAddress()
        {
            //Remove Mandatory data
            base.Service<IApplicationService>().DeleteOfferMailingAddress(base.GenericKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.SendOffer);
            base.View.AssertNotificationDisplayed("Each Application must have one valid Application mailing address.");
        }
    }
}