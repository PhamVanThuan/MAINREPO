using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class AdminDeclineTests : PersonalLoansWorkflowTestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestStart()
        {
            base.FindCaseAtState(WorkflowStates.ApplicationManagementWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD, true);
            base.OnTestStart();
        }

        [Test]
        public void when_admin_decline_action_performed_should_show_reasons_view()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.AdminDecline);
            base.View.AssertViewDisplayed();
        }

        [Test]
        public void when_admin_decline_action_performed_should_show_decline_reasons()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.AdminDecline);
            var reasonList = new List<string>()
                {
                    ReasonDescription.NoticeInsolvent,
                    ReasonDescription.NoticeDebtreview,
                    ReasonDescription.NoticeAdministrationOrder,
                    ReasonDescription.FailedAffordabilityTest,
                    ReasonDescription.PoorEmpiricaScore,
                    ReasonDescription.PoorCreditBehaviourITCJudgements
                };
            base.View.AssertReasonListContents(reasonList, true);
            base.View.AssertViewDisplayed();
        }

        [Test]
        public void when_admin_decline_action_performed_should_archive_case()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.AdminDecline);
            string selectedReason = base.View.SelectReasonAndSubmit(ReasonType.PersonalLoanAdminDecline);
            X2Assertions.AssertCurrentX2State(base.InstanceID, WorkflowStates.PersonalLoansWF.ArchiveNTUDeclines);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_AdministrativeDecline);
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.Declined);
            Thread.Sleep(2000);
            ReasonAssertions.AssertReason(selectedReason, ReasonType.PersonalLoanAdminDecline, base.GenericKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
        }

        [Test]
        public void when_admin_decline_action_performed_multiple_reasons_cannot_be_selected()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.AdminDecline);
            base.View.SelectMultipleReasons(ReasonType.PersonalLoanAdminDecline, 2);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Only one reason can be selected");
        }
    }
}