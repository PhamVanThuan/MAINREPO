using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class AlteredApproveApplicationTests : PersonalLoansWorkflowTestBase<PersonalLoanAlteredApproval>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.Credit, WorkflowRoleTypeEnum.PLCreditAnalystD);
        }

        /// <summary>
        /// This test will complete the Approve action and ensure that the case is moved to the legal agreements stage, assigned to the
        /// PL Admin user. The offer information should also be marked as accepted.
        /// </summary>
        [Test]
        public void AlteredApproveApplication()
        {
            //generates a random loan amount and Term between the respective boundaries.
            Random random = new Random();
            decimal loanAmount = random.Next(10000, 30000);
            int term = random.Next(6, 48);

            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.AlteredApproval);
            //Calculate loan
            base.View.Calculate(term, loanAmount);

            //Select option from grid
            base.View.SelectOptionFromGrid(6);

            //Select Create Application
            base.View.UpdateApplication();

            //get expected user
            string expectedUser = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey,
                WorkflowRoleTypeEnum.PLConsultantD, RoundRobinPointerEnum.PLConsultant);
            //check case movement
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.LegalAgreements);
            Assert.That(offerExists);
            //check transition
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey,
                StageDefinitionStageDefinitionGroupEnum.PersonalLoans_Altered_Approval);
            //check the assignment
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, expectedUser,
                WorkflowStates.PersonalLoansWF.LegalAgreements, Workflows.PersonalLoans);
            //offer information should have marked as accepted
            OfferAssertions.OfferInformationUpdated(base.GenericKey, OfferInformationTypeEnum.AcceptedOffer, ProductEnum.PersonalLoan);
        }
    }
}