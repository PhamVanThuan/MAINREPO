using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class NTUFinalisedTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.NTU, WorkflowRoleTypeEnum.PLConsultantD);
        }

        /// <summary>
        /// This test perform the NTU finalised action on a personal application, ensuring the case is moved to the archive
        /// and that the offer status is set to NTU.
        /// </summary>
        [Test, Description(@"This test perform the NTU Finalised action on a personal application, ensuring the case is moved to the archive
        and that the offer status is set to NTU.")]
        public void NTUFinalised()
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.NTUFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ArchiveNTUDeclines);
            Assert.That(offerExists);
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_NTUFinalised);
            //check the offer status
            OfferAssertions.AssertOfferStatus(base.GenericKey, OfferStatusEnum.NTU);
        }
    }
}