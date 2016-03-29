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
    public class NTUTests : PersonalLoansWorkflowTestBase<CommonReasonCommonDecline>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.NTU);
        }

        /// <summary>
        /// This test NTU's an application and ensures that the reasons are added to the application, that the case remains open and that
        /// it is correctly assigned.
        /// </summary>
        [Test, Description(@"This test NTU's an application and ensures that the reasons are added to the application, that the case remains open and that
        it is correctly assigned.")]
        public void NTUApplication()
        {
            //get expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            //select reasons
            string selectedReason = base.View.SelectReasonAndSubmit(ReasonType.PersonalLoanNTU);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.NTU);
            Assert.That(offerExists);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.NTU);
            //check reason added
            ReasonAssertions.AssertReason(selectedReason, ReasonType.PersonalLoanNTU, base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.NTU, Workflows.PersonalLoans);
            //offer status is set to open
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.Open);
            //check timer created
            X2Assertions.AssertScheduledActivityTimer(base.GenericKey.ToString(), ScheduledActivities.PersonalLoans.NTUTimer, 30, false);
        }

        /// <summary>
        /// Only a single NTU reason can be selected when NTU'ing a personal loan application.
        /// </summary>
        [Test]
        public void CanOnlySelectASingleNTUReason()
        {
            base.View.SelectReasonAndSubmit(ReasonType.PersonalLoanNTU, 2);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Only one reason can be selected");
            //case still at Manage Application
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageApplication);
            Assert.That(offerExists);
        }

        [Test, Description("Checks that the contents of the reasons list matches what we expect.")]
        public void CheckNTUReasonsList()
        {
            var reasonList = new List<string>()
                {
                    ReasonDescription.Tooexpensive,
                    ReasonDescription.NoNeedForCredit,
                    ReasonDescription.LoanSizeInsufficient,
                    ReasonDescription.AlreadyOverIndebted,
                    ReasonDescription.UnderDebtCounselling
                };
            base.View.AssertReasonListContents(reasonList, true);
        }
    }
}