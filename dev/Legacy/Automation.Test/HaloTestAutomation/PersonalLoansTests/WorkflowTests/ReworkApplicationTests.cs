using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using Common.Constants;
using Common.Enums;
using Common.Extensions;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class ReworkApplicationTests : PersonalLoansWorkflowTestBase<PersonalLoanCalculator>
    {
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageLead, Common.Enums.WorkflowRoleTypeEnum.PLConsultantD);
        }

        /// <summary>
        /// This test will approve a case and then return it to manage application. At that point we need to try and rework the application. This should result
        /// in a new offer information record being inserted as the previously approved one should not be updated.
        /// </summary>
        [Test]
        public void ReworkApplicationPostCreditApproval()
        {
            //we need to process it through credit and then return it to Manage Application
            var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, "CreateAndReturnToManageApplicationPostCreditApproval", base.GenericKey);
            //reload the case
            if (results.LastActivitySucceeded())
            {
                base.ReloadCase(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
                base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReworkApplication);
                var loanAmount = base.RandomGenerator.Next(10000, 30000);
                var term = 15;
                base.View.Calculate(term, loanAmount);
                base.View.SelectOptionFromGrid(term);
                base.View.UpdateApplication();
                //we should have a new offer information record
                OfferAssertions.AssertOfferInformationCount(base.GenericKey, 2);
                OfferAssertions.OfferInformationUpdated(base.GenericKey, Common.Enums.OfferInformationTypeEnum.RevisedOffer,
                    Common.Enums.ProductEnum.PersonalLoan);
                base.RefreshPersonalApplication();
                Assert.That(personalLoanApplication.LoanAmount == loanAmount && personalLoanApplication.Term == term);
                StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ReworkApplication);
            }
        }

        /// <summary>
        /// In this test, an application is reworked at the Manage Application state prior to credit approval. In this case we should be updating the
        /// original offer information as it has not been marked as accepted.
        /// </summary>
        [Test]
        public void ReworkApplicationPriorToCreditApproval()
        {
            var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, "CalculateApplication", base.GenericKey);
            if (results.LastActivitySucceeded())
            {
                base.Browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(base.Browser, base.GenericKey.ToString(),
                    new string[] { WorkflowStates.PersonalLoansWF.ManageApplication }, ApplicationType.Any);
                base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReworkApplication);
                var loanAmount = base.RandomGenerator.Next(30000, 50000);
                var term = 16;
                base.View.Calculate(term, loanAmount);
                base.View.SelectOptionFromGrid(term);
                base.View.UpdateApplication();
                //we should still have one offer information record
                OfferAssertions.AssertOfferInformationCount(base.GenericKey, 1);
                OfferAssertions.OfferInformationUpdated(base.GenericKey, Common.Enums.OfferInformationTypeEnum.OriginalOffer,
                    Common.Enums.ProductEnum.PersonalLoan);
                base.RefreshPersonalApplication();
                Assert.That(personalLoanApplication.LoanAmount == loanAmount && personalLoanApplication.Term == term);
                StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ReworkApplication);
            }
        }

        /// <summary>
        /// This test reworks the application to add/remove the credit life premium, ensuring that the offer attribute is removed/added and the premium is
        /// recalculated.
        /// </summary>
        [Test]
        public void ReworkApplicationCreditLifePremium()
        {
            var results = scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, "CalculateApplication", base.GenericKey);
            if (results.LastActivitySucceeded())
            {
                base.RefreshPersonalApplication();
                base.Browser.Page<WorkflowSuperSearch>().SearchByUniqueIdentifierAndApplicationType(base.Browser, base.GenericKey.ToString(),
                    new string[] { WorkflowStates.PersonalLoansWF.ManageApplication }, ApplicationType.Any);
                ReworkApplicationLifePremium(false);
                Assert.That(personalLoanApplication.CreditLifeTakenUp == 0);
                Assert.That(personalLoanApplication.LifePremium == 0);
                ReworkApplicationLifePremium(true);
                Assert.That(personalLoanApplication.CreditLifeTakenUp == 1);
                Assert.That(personalLoanApplication.LifePremium == (base.personalLoanApplication.LoanAmount + base.personalLoanApplication.FeesTotal) * 0.00375);
            }
        }

        /// <summary>
        /// Rework an application to either include or not include the life premium.
        /// </summary>
        /// <param name="includeLifePremium"></param>
        private void ReworkApplicationLifePremium(bool includeLifePremium)
        {
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReworkApplication);
            base.View.SelectLifePremiumAndRecalc(includeLifePremium);
            base.View.SelectOptionFromGrid(base.personalLoanApplication.Term);
            base.View.UpdateApplication();
            base.RefreshPersonalApplication();
        }
    }
}