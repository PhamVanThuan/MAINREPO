using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace DebtCounsellingTests.Workflow
{
    [RequiresSTA]
    public sealed class Bypass10DayTimerTests : DebtCounsellingTests.TestBase<BasePage>
    {
        /// <summary>
        /// If court details have been captured on a debt counselling case the Bypass 10 Day Timer action should circumvent the termination rules
        /// and send the case to the termination archive.
        /// </summary>
        [Test, Description(@"If court details have been captured on a debt counselling case the Bypass 10 Day Timer action should circumvent the termination rules
        and send the case to the termination archive.")]
        public void BypassTenDayTerminationCourtDetailsExist()
        {
            //search for case at Manage Proposal
            base.StartTest(WorkflowStates.DebtCounsellingWF.ManageProposal, TestUsers.DebtCounsellingConsultant);
            //remove existing court details
            base.Service<ICourtDetailsService>().DeleteCourtDetails(base.TestCase.DebtCounsellingKey);
            //insert new court details
            base.Service<ICourtDetailsService>().InsertCourtDetails(base.TestCase.DebtCounsellingKey, HearingTypeEnum.Court, HearingAppearanceTypeEnum.CourtCourtApplication, DateTime.Now.AddDays(5));
            //terminate application
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.TerminateApplication, base.TestCase.DebtCounsellingKey);
            scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.DebtCounselling, WorkflowAutomationScripts.DebtCounselling.SendTerminationLetter, base.TestCase.DebtCounsellingKey);
            Service<IX2WorkflowService>().WaitForX2State(base.TestCase.DebtCounsellingKey, Workflows.DebtCounselling, WorkflowStates.DebtCounsellingWF.TerminationLetterSent);
            //start browser as debt counselling supervisor
            base.Browser = new TestBrowser(TestUsers.DebtCounsellingSupervisor);
            //bypass the 10 day timer
            base.ReloadTestCase();
            base.LoadCase(WorkflowStates.DebtCounsellingWF.TerminationLetterSent);
            base.Browser.ClickAction(WorkflowActivities.DebtCounselling.Bypass10DayTimer);
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowYesNo>().ClickYes();
            //assert case is terminated
            Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity(WorkflowActivities.DebtCounselling.Bypass10DayTimer, base.TestCase.InstanceID, 1);
            X2Assertions.AssertCurrentX2State(base.TestCase.InstanceID, WorkflowStates.DebtCounsellingWF.TerminationArchive);
            DebtCounsellingAssertions.AssertDebtCounsellingStatus(DebtCounsellingStatusEnum.Terminated, base.TestCase.DebtCounsellingKey);
            StageTransitionAssertions.AssertStageTransitionCreated(base.TestCase.DebtCounsellingKey, StageDefinitionStageDefinitionGroupEnum.DebtCounselling_DebtCounsellingTerminated);
        }
    }
}