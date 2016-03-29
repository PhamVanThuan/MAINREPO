using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class ExternalNTUTests : PersonalLoansWorkflowTestBase<BasePage>
    {
        private int legalEntityKey;
        private int accountKey;

        /// <summary>
        /// This test will create a personal loans application for a legal entity and then put the legal entity's corresponding mortgage loan account
        /// under debt counselling. This will fire the EXT_NTU flag that results in the case being moved from its current state into the NTU state.
        /// </summary>
        [Test]
        public void _01_ExternalNTUViaDebtCounsellingCaseCreate()
        {
            base.Browser = new TestBrowser(TestUsers.PersonalLoanConsultant1, TestUsers.Password);
            this.legalEntityKey = base.Service<ILegalEntityService>().GetMainApplicantLegalEntityWithoutPersonalLoanOffer().LegalEntityKey;
            var idnumber = String.Empty;
            this.accountKey = base.Service<IAccountService>().GetMortgageAccountsByLegalEntity(ref this.legalEntityKey, ref idnumber).FirstOrDefault();
            Helper.LoadLegalEntityOnCBO(base.Browser, legalEntityKey, accountKey);
            //create lead
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<WorkflowYesNo>().ClickYes();
            var extRole = base.Service<IExternalRoleService>().GetActiveExternalRoleByLegalEntity(GenericKeyTypeEnum.Offer_OfferKey,
                                                                ExternalRoleTypeEnum.Client, this.legalEntityKey);
            base.GenericKey = extRole.GenericKey;
            base.InstanceID = Service<IX2WorkflowService>().GetPersonalLoanInstanceId(base.GenericKey);
            var caseCreated = Service<IDebtCounsellingService>().CreateDebtCounsellingCase(this.accountKey);
            if (caseCreated)
            {
                //wait for EXT NTU to be performed
                Service<IX2WorkflowService>().WaitForX2WorkflowHistoryActivity("EXT_NTU", base.InstanceID, 1);
                //we expect our case to have moved to the NTU state
                var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.NTU);
                Assert.That(offerExists);
                StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_ExternalNTU);
                //we should have a reason
                ReasonAssertions.AssertReason(ReasonDescription.UnderDebtCounselling, ReasonType.PersonalLoanNTU, base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey);
            }
        }

        /// <summary>
        /// Once the case has been sent to the NTU state, the user cannot complete the Reinstate NTU activity while there is an open Debt Counselling
        /// case linked to the applicant. A message should be displayed when the consultant tries to start the activity.
        /// </summary>
        [Test]
        public void _02_CannotReinstateNTU()
        {
            //load the case
            base.ReloadCase(WorkflowStates.PersonalLoansWF.NTU, WorkflowRoleTypeEnum.PLConsultantD);
            //message should be on the application summary
            base.Browser.Page<BasePageAssertions>().AssertValidationMessagesContains("is under Debt Counselling");
            //try to reinstate
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ReinstateNTU);
            base.Browser.Page<BasePageAssertions>().AssertNotification("is under Debt Counselling");
        }
    }
}