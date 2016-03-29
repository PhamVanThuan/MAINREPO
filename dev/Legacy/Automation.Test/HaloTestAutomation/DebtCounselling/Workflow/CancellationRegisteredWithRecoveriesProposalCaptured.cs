using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public class CancellationRegisteredWithRecoveriesProposalCaptured : DebtCounsellingTests.TestBase<BasePage>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
        }

        [Test]
        public void when_cancellation_registered_with_recoveries_proposal_in_place_the_correct_correspondence_is_sent()
        {
            base.StartTest(WorkflowStates.DebtCounsellingWF.PendCancellation, TestUsers.DebtCounsellingConsultant);
            //we need the debt counsellor's email address
            var debtCounsellorCorrespondenceDetails = Service<IExternalRoleService>().GetExternalRoleEmailAddress(base.TestCase.DebtCounsellingKey, ExternalRoleTypeEnum.DebtCounsellor).First();
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.CaptureRecoveriesProposal, base.TestCase.DebtCounsellingKey);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.EscalateRecoveriesProposal, base.TestCase.DebtCounsellingKey);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.ApproveShortfall, base.TestCase.DebtCounsellingKey);
            //register cancellation
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.EXTCancellationRegistered, base.TestCase.DebtCounsellingKey);
            //Assert that debtcounselling case is archived
            DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatusEnum.Cancelled, base.TestCase.DebtCounsellingKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_UnderCancellation);
            DebtCounsellingAssertions.AssertX2State(base.TestCase.DebtCounsellingKey, WorkflowStates.DebtCounsellingWF.ArchiveDebtCounselling);
            //Assert email created
            CorrespondenceAssertions.AssertClientEmailByCorrespondenceTemplate(base.TestCase.DebtCounsellingKey, Common.Enums.CorrespondenceTemplateEnum.MortgageLoanCancelledContinuePaying,
                                 DateTime.Now.Date, debtCounsellorCorrespondenceDetails.Value, base.TestCase.AccountKey);
        }
    }
}