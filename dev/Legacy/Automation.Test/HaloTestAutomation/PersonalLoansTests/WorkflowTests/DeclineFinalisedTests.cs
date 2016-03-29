using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class DeclineFinalisedTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.DeclinedbyCredit, WorkflowRoleTypeEnum.PLConsultantD);
        }

        /// <summary>
        /// This test perform the decline finalised action on a personal application, ensuring the case is moved to the archive
        /// and that the offer status is set to declined.
        /// </summary>
        [Test, Description(@"This test perform the decline finalised action on a personal application, ensuring the case is moved to the archive
        and that the offer status is set to declined.")]
        public void DeclinedFinalised()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DeclineFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ArchiveNTUDeclines);
            Assert.That(offerExists);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DeclineFinalised);
            //check the offer status
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.Declined);
        }
    }
}