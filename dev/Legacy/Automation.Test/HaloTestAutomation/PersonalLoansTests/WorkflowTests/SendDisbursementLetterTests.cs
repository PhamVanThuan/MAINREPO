using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class SendDisbursementLetterTests : PersonalLoansWorkflowTestBase<PersonalLoanDisbursement>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Disbursed, Common.Enums.WorkflowRoleTypeEnum.PLSupervisorD);
        }

        /// <summary>
        /// This test will complete the Disburse Funds action, and update the Disbursement Timer. After the disbursement timer expires, the case should fail the "can email disbursemd letter" check and
        /// the case should move to the Disbursed Letter Email Failed and then Send Disbursement Letter stage correctly. The correct stage transitions should be written.
        ///
        /// </summary>
        [Test]
        public void EmailDisbursedLetterFailed()
        {
            //check preferred correspondence medium
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(base.GenericKey);
            if (offerMailingAddress.CorrespondenceMedium != CorrespondenceMedium.Post)
                Service<IApplicationService>().UpdateCorrespondenceMedium(base.GenericKey, CorrespondenceMediumEnum.Post);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.FireDisbursementTimer, base.GenericKey);
            //wait for timer to fire
            Service<IX2WorkflowService>().WaitForX2ScheduledActivity(ScheduledActivities.PersonalLoans.DisbursedTimer, base.InstanceID);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DisbursedTimer);
            //case moves states
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.SendDisbursementLetter);
            Assert.That(offerExists);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_AutomatedCorrespondenceFailed);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.SendDisbursementLetter, WorkflowRoleTypeEnum.PLSupervisorD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.SendDisbursementLetter);
            base.Browser.Page<CorrespondenceProcessing>().SendCorrespondence(CorrespondenceSendMethodEnum.Post);
            //check the correspondence record has been added
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.DisbursementLetter, CorrespondenceMedium.Post);
            CorrespondenceAssertions.AssertImageIndex(base.GenericKey.ToString(), CorrespondenceReports.DisbursementLetter, CorrespondenceMedium.Post, 0, 0);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_CorrespondenceSent);
            //case moves states
            var offerExistsAfter = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ArchiveDisbursed);
            Assert.That(offerExistsAfter);
        }

        /// <summary>
        /// This test will complete the Disburse Funds action, and update the Disbursement Timer. After the disbursement timer expires, the case should pass the "can email disbursemd letter" check and
        /// the case should move to the Archive Disbursed stage correctly. The correct stage transitions should be written.
        ///
        /// </summary>
        [Test]
        public void EmailDisbursedLetter()
        {
            //check preferred correspondence medium
            var offerMailingAddress = Service<IApplicationService>().GetOfferMailingAddress(base.GenericKey);
            if (offerMailingAddress.CorrespondenceMedium == CorrespondenceMedium.Post)
                Service<IApplicationService>().InsertOfferMailingAddress(base.GenericKey);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, WorkflowAutomationScripts.PersonalLoans.FireDisbursementTimer, base.GenericKey);
            //wait for timer to fire
            Service<IX2WorkflowService>().WaitForX2ScheduledActivity(ScheduledActivities.PersonalLoans.DisbursedTimer, base.InstanceID);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DisbursedTimer);
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(ScheduledActivities.PersonalLoans.EmailDisbursedLetter, base.InstanceID, 1);
            //case moves states
            var offerExistsArchive = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ArchiveDisbursed);
            Assert.That(offerExistsArchive);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_AutomatedCorrespondenceSent);
            //check the correspondence record has been added
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.DisbursementLetter, CorrespondenceMedium.Email);
            CorrespondenceAssertions.AssertImageIndex(base.GenericKey.ToString(), CorrespondenceReports.DisbursementLetter, CorrespondenceMedium.Email, 0, 0);
        }
    }
}