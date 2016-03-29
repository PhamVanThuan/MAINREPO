using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System.Web.Mvc;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class NonSAHLClientCreateLeadTests : PersonalLoansWorkflowTestBase<PersonalLoanLead>
    {
        protected string idNumber { get; set; }
        
        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser = new TestBrowser(TestUsers.PersonalLoanConsultant1, TestUsers.Password);

            // Get a case to work with
            idNumber = IDNumbers.GetNextIDNumber().ToString();

            // Click Personal Loan action
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().PersonalLoansMenu(base.Browser);
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            Service<IWatiNService>().KillAllIEProcesses();
        }
        

        /// <summary>
        /// Assert that the following information is set: TYPE: Natural Personal; ROLE: Lead-Main Applicant
        /// </summary>
        [Test]
        public void when_the_presenter_renders_the_expected_default_information_is_set()
        {
            WatiNAssertions.AssertSelectedOption("NaturalPerson", base.View.ddlLeadType);
            WatiNAssertions.AssertSelectedOption("Lead - Main Applicant", base.View.ddlLeadRole);
        }

        /// <summary>
        /// Assert that the required fields are captured for the lead to be created
        /// </summary>
        [Test]
        public void when_action_lead_capture_is_performed_without_any_required_fields_captured_validation_halts_processing()
        {
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddPersonalLoanLegalEntity(OfferRoleTypes.LeadMainApplicant, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, false, ButtonTypeEnum.CreateLead);
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity First Name Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity Surname Required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("At least one contact detail (Email, Home, Work or Cell Number) is required");
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists("Legal Entity ID Number Required For South African Citizens");
        }

        /// <summary>
        /// Assert that the case is created and moved to stage manage lead
        /// </summary>
        [Test]
        public void when_action_lead_capture_is_performed_the_case_is_created_and_moves_to_stage_manage_lead()
        {
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddPersonalLoanLegalEntity(OfferRoleTypes.LeadMainApplicant, idNumber, null, null, "TestPersonalLoanLead_FirstName", "TestPersonalLoanLead_SurName", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "test@personalLoanLead.com", false, false, false, false, false, ButtonTypeEnum.CreateLead);
            var legalEntityKey = Service<ILegalEntityService>().GetLegalEntityKeyByIdNumber(idNumber);
            int offerKey = Service<IApplicationService>().GetOfferByLegalEntityKey(legalEntityKey);
            var offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(offerKey, WorkflowStates.PersonalLoansWF.ManageLead);
            Assert.That(offerExists);
        }

        /// <summary>
        /// Assert that the case is assigned to the consultant who created the lead
        /// </summary>
        [Test]
        public void when_action_lead_capture_is_performed_the_case_is_assigned_to_the_consultant_who_created_the_lead()
        {
            base.Browser.Page<LegalEntityDetailsLeadApplicantAdd>().AddPersonalLoanLegalEntity(OfferRoleTypes.LeadMainApplicant, idNumber, null, null, "TestPersonalLoanLead_FirstName", "TestPersonalLoanLead_SurName", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "test@personalLoanLead.com", false, false, false, false, false, ButtonTypeEnum.CreateLead);
            var legalEntityKey = Service<ILegalEntityService>().GetLegalEntityKeyByIdNumber(idNumber);
            int offerKey = Service<IApplicationService>().GetOfferByLegalEntityKey(legalEntityKey);
            var id = base.Service<IX2WorkflowService>().GetPersonalLoanInstanceId(offerKey);
            var consultant = base.Service<IAssignmentService>().GetADUserNameOfActiveWorkflowRoleAssignment(id);
            X2Assertions.AssertWorklistOwner(offerKey, WorkflowStates.PersonalLoansWF.ManageLead, Workflows.PersonalLoans, consultant);
        }
    }
}
