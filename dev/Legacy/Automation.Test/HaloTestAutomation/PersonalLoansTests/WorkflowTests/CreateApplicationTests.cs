using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class CreateApplicationTests : PersonalLoansWorkflowTestBase<PersonalLoanCalculator>
    {
        private int amount = 1140;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageLead, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.CalculateApplication);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Service<IWatiNService>().KillAllIEProcesses();
        }

        [Test]
        public void when_calculating_application_with_credit_life_premium_account_should_move_to_manage_application_and_remain_with_personal_loan_consultant()
        {
            decimal loanAmount = 25000;
            //Calculate loan
            base.View.Calculate(0, loanAmount);

            //Select option from grid
            base.View.SelectOptionFromGrid(6);

            //Select Create Application
            base.View.CreateApplication();

            //Should still be assigned to pl user
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, base.CaseOwner);
            //transition should be written
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_CalculateApplication);

            //Personal loan account should move to manage application
            base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageApplication);

            //Assert OfferInformaion Updated
            OfferAssertions.OfferInformationUpdated(base.GenericKey, OfferInformationTypeEnum.OriginalOffer, ProductEnum.PersonalLoan);

            //Assert OfferExpense Updated
            OfferAssertions.OfferExpenseUpdated(base.GenericKey, ExpenseType.PersonalLoanInitiationFee, amount);

            //Offer attribute of Life should exist
            OfferAssertions.AssertOfferAttributeExists(base.GenericKey, OfferAttributeTypeEnum.Life);
        }

        /// <summary>
        /// This test creates
        /// </summary>
        [Test]
        public void when_calculating_application_without_credit_life_premium_account_should_move_to_manage_application_and_remain_with_personal_loan_consultant()
        {
            decimal loanAmount = 25000;

            //uncheck life premium
            base.View.UncheckCreditLifePremium();

            //Calculate loan
            base.View.Calculate(0, loanAmount);

            //Select option from grid
            base.View.SelectOptionFromGrid(6);

            //Select Create Application
            base.View.CreateApplication();

            //Should still be assigned to pl user
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(base.GenericKey, WorkflowRoleTypeEnum.PLConsultantD, base.CaseOwner);
            //transition should be written
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_CalculateApplication);

            //Personal loan account should move to manage application
            var offerexists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageApplication);
            Assert.That(offerexists);
            //Assert OfferInformaion Updated
            OfferAssertions.OfferInformationUpdated(base.GenericKey, OfferInformationTypeEnum.OriginalOffer, ProductEnum.PersonalLoan);

            //Assert OfferExpense Updated
            OfferAssertions.OfferExpenseUpdated(base.GenericKey, ExpenseType.PersonalLoanInitiationFee, amount);

            //Offer attribute of Life should not exist
            OfferAssertions.AssertOfferAttributeExists(base.GenericKey, OfferAttributeTypeEnum.Life, false);
        }
    }
}