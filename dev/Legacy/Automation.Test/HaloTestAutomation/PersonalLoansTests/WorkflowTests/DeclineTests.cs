using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Collections.Generic;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class DeclineTests : PersonalLoansWorkflowTestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.Decline);
        }

        /// <summary>
        /// This test declines an application and ensures that the reasons are added to the application, that the case moves states and that
        /// it is correctly assigned.
        /// </summary>
        [Test, Description(@"This test declines an application and ensures that the reasons are added to the application, that the case moves
        states and that it is correctly assigned.")]
        public void DeclineApplication()
        {
            //get expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            //select reasons
            string selectedReason = base.View.SelectReasonAndSubmit(ReasonType.PersonalLoanDecline);
            //check case movement
            bool offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.DeclinedbyCredit);
            Assert.That(offerExists);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_Decline);
            //check reason added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.PersonalLoanDecline, base.GenericKey, GenericKeyTypeEnum.OfferInformation_OfferInformationKey);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.DeclinedbyCredit, Workflows.PersonalLoans);
        }

        [Test]
        public void CheckDeclineReasonsList()
        {
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
        }

        [Test]
        public void OnlyOneDeclineReasonCanBeSelected()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.Decline);
            base.View.SelectMultipleReasons(ReasonType.PersonalLoanDecline, 2);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Only one reason can be selected");
        }
    }
}