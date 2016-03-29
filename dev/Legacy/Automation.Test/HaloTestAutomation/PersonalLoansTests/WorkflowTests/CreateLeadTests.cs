using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Linq;
using System.Threading;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class CreateLeadTests : PersonalLoansWorkflowTestBase<ClientSuperSearch>
    {
        private int accountKey;
        private int legalEntityKey;
        private int generickey;
        private string consultant;

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser = new TestBrowser(TestUsers.PersonalLoanConsultant1, TestUsers.Password);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            Service<IWatiNService>().KillAllIEProcesses();
        }

        /// <summary>
        /// This test will create a Personal Loan lead, ensuring that the case is assigned to the worklist of the PL Consultant and that the case
        /// ends up in the Manage Lead state
        /// </summary>
        [Test]
        public void when_perform_creating_personal_loan()
        {
            CreateLead();
            BuildingBlocks.Assertions.StageTransitionAssertions.AssertStageTransitionCreated(generickey, StageDefinitionStageDefinitionGroupEnum.CreatePersonalLoanLead);
            OfferAssertions.AssertOfferCreated(this.generickey, OfferTypeEnum.UnsecuredLending);
            WorkflowRoleAssignmentAssertions.AssertActiveWorkflowRoleExists(this.generickey, WorkflowRoleTypeEnum.PLConsultantD, this.consultant);
            BuildingBlocks.Assertions.X2Assertions.AssertWorklistOwner(generickey, WorkflowStates.PersonalLoansWF.ManageLead,
                Workflows.PersonalLoans, this.consultant);
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(this.generickey, WorkflowStates.PersonalLoansWF.ManageLead);
            Thread.Sleep(3000);
            Assert.That(offerExists);
        }

        [Test]
        public void when_perform_create_personal_loan_where_lead_already_exist_for_client_should_display_error()
        {
            CreateLead();
            //try create duplicated lead.
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<WorkflowYesNo>().ClickYes();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowYesNo>().
                AssertMessageDisplayed(@"Personal Loan Lead already exists for the selected legal entity");
        }

        [Test]
        public void create_lead_on_legal_entity_with_prev_NTU_should_allow_to_create_lead()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.NTU, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.NTUFinalised);
            base.Browser.Page<WorkflowYesNo>().Confirm(true, true);
            int legalEntityKey = base.Service<ILegalEntityService>().GetMainApplicantLegalEntityWithPersonalLoanOffer(base.GenericKey).LegalEntityKey;
            var idnumber = Service<ILegalEntityService>().GetLegalEntityIDNumber(legalEntityKey);
            base.Browser.Navigate<MenuNode>().CloseLoanNodesCBO();
            this.accountKey = base.Service<IAccountService>().GetMortgageAccountsByLegalEntity(ref this.legalEntityKey, ref idnumber).FirstOrDefault();
            Helper.LoadLegalEntityOnCBO(base.Browser, legalEntityKey, accountKey);
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<WorkflowYesNo>().ClickYes();

            //get the genericKey
            var extRole = base.Service<IExternalRoleService>().GetActiveExternalRoleByLegalEntity(GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client, this.legalEntityKey);
            base.GenericKey = extRole.GenericKey;
            var offerExistsAfterNTU = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.ManageLead);
            Assert.That(offerExistsAfterNTU);
        }

        [Test, Sequential]
        public void when_perform_create_personal_loan_where_lead_is_not_natural_person([Values(LegalEntityTypeEnum.Company,
            LegalEntityTypeEnum.Trust,
            LegalEntityTypeEnum.CloseCorporation)] LegalEntityTypeEnum legalEntityTypeKey)
        {
            this.legalEntityKey = base.Service<ILegalEntityService>().GetMainApplicantLegalEntityWithoutPersonalLoanOffer(legalEntityTypeKey).LegalEntityKey;
            var idnumber = Service<ILegalEntityService>().GetLegalEntityIDNumber(legalEntityKey);
            this.accountKey = base.Service<IAccountService>().GetMortgageAccountsByLegalEntity(ref this.legalEntityKey, ref idnumber).FirstOrDefault();

            Helper.LoadLegalEntityOnCBO(base.Browser, legalEntityKey, accountKey);

            //create lead
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowYesNo>().AssertViewDisplayed();
            base.Browser.Page<WorkflowYesNo>().AssertNotificationDisplayed(@"A personal loan lead cannot be created for a company/trust/cc");
        }

        private void CreateLead()
        {
            this.legalEntityKey = base.Service<ILegalEntityService>().GetMainApplicantLegalEntityWithoutPersonalLoanOffer().LegalEntityKey;
            var idnumber = Service<ILegalEntityService>().GetLegalEntityIDNumber(legalEntityKey);
            this.accountKey = base.Service<IAccountService>().GetMortgageAccountsByLegalEntity(ref this.legalEntityKey, ref idnumber).FirstOrDefault();

            Helper.LoadLegalEntityOnCBO(base.Browser, legalEntityKey, accountKey);

            //create lead
            base.Browser.Navigate<LoanServicingCBO>().ClickCreatePersonalLoanLead();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowYesNo>().AssertViewDisplayed();
            base.Browser.Page<BuildingBlocks.Presenters.CommonPresenters.WorkflowYesNo>().AssertYesNoButtonsDisplayed();

            base.Browser.Page<WorkflowYesNo>().ClickYes();
            var extRole = base.Service<IExternalRoleService>().GetActiveExternalRoleByLegalEntity(GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client, this.legalEntityKey);
            this.generickey = extRole.GenericKey;
            this.legalEntityKey = extRole.LegalEntityKey;
            var id = base.Service<IX2WorkflowService>().GetPersonalLoanInstanceId(this.generickey);
            this.consultant = base.Service<IAssignmentService>().GetADUserNameOfActiveWorkflowRoleAssignment(id);
        }
    }
}