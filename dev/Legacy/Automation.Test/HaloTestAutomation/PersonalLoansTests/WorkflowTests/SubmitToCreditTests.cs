using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.PersonalLoans;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class SubmitToCreditTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestFixtureSetup()
        {
            scriptEngine = new X2ScriptEngine();
            base.OnTestFixtureSetup();
        }

        /// <summary>
        /// when performing application in order the case should round robin assigned to the next credit analyst.
        /// </summary>
        [Test, Repeat(5)]
        public void when_performing_application_in_order_should_round_robin_to_credit_analysts()
        {
            base.GenericKey = Service<IX2WorkflowService>().GetOfferKeyAtStateByType(WorkflowStates.PersonalLoansWF.DocumentCheck, Workflows.PersonalLoans, Common.Enums.OfferTypeEnum.UnsecuredLending,
                string.Empty, (int)Common.Enums.GeneralStatusEnum.Active);
            base.InstanceID = base.Service<IX2WorkflowService>().GetPersonalLoanInstanceId(base.GenericKey);
            var expectedUserToAssignTo = base.Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLCreditAnalystD, RoundRobinPointerEnum.PLCreditAnalyst);
            base.scriptEngine.ExecuteScript(Common.Enums.WorkflowEnum.PersonalLoans, "ApplicationInOrder", base.GenericKey);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLCreditAnalystD, expectedUserToAssignTo, WorkflowStates.PersonalLoansWF.Credit,
                Workflows.PersonalLoans);
        }

        /// <summary>
        /// When performing action application in order, given that there is no legal
        /// entity salutation, the validation should display legal entity salutation required.
        /// </summary>
        [Test, Description("Asserts that the legal entity salutation is required when not present")]
        public void when_performing_application_in_order_should_display_legal_entity_salutation_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(base.GenericKey);
            base.Service<ILegalEntityService>().UpdateLegalEntitySalutationToNull(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Legal Entity Salutation Required");
        }

        /// <summary>
        /// When performing action application in order, given that there are no legal
        /// entity initials, the validation should display legal entity initials required.
        /// </summary>
        [Test, Description("Asserts that the legal entity initials are required when not present")]
        public void when_performing_application_in_order_should_display_legal_entity_initials_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(base.GenericKey);
            base.Service<ILegalEntityService>().UpdateLegalEntityInitialsToNull(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Legal Entity Initials Required");
        }

        /// <summary>
        /// When performing action application in order, given that there is no legal
        /// gender designation, the validation should display legal entity gender required.
        /// </summary>
        [Test, Description("Asserts that the legal entity gender is required when not present")]
        public void when_performing_application_in_order_should_display_legal_entity_gender_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(base.GenericKey);
            base.Service<ILegalEntityService>().UpdateLegalEntityGenderToNull(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Legal Entity Gender Required");
        }

        /// <summary>
        /// When performing action application in order, given that there is no legal
        /// entity marital status, the validation should display legal entity marital status required.
        /// </summary>
        [Test, Description("Asserts that the legal entity marital status is required when not present")]
        public void when_performing_application_in_order_should_display_legal_entity_marital_status_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(base.GenericKey);
            base.Service<ILegalEntityService>().UpdateLegalEntityMaritalStatusToNull(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Legal Entity Marital Status Required");
        }

        /// <summary>
        /// When performing action application in order, given that there is no legal
        /// entity citizenship type, the validation should display legal entity citizenship type required.
        /// </summary>
        [Test, Description("Asserts that the legal entity citizenship type is required when not present")]
        public void when_performing_application_in_order_should_display_legal_entity_citizenship_type_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(base.GenericKey);
            base.Service<ILegalEntityService>().UpdateLegalEntityCitizenTypeToEmpty(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Legal Entity Citizen Type Required");
        }

        /// <summary>
        /// When performing action application in order, given that there is no legal
        /// entity date of birth, the validation should display legal entity date of birth required.
        /// </summary>
        [Test, Description("Asserts that the legal entity date of birth is required when not present")]
        public void when_performing_application_in_order_should_display_legal_entity_date_of_birth_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var legalEntityKey = base.Service<IApplicationService>().GetLegalEntityKeyFromOfferKey(base.GenericKey);
            base.Service<ILegalEntityService>().UpdateLegalEntityDateOfBirthToEmpty(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Legal Entity Date Of Birth Required");
        }

        /// <summary>
        /// when performing application in order and there is no debit order for the offer an error
        /// message should be displayed to the user.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_debit_order_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.Service<IDebitOrdersService>().DeleteOfferDebitOrderByOffer(base.GenericKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Application Debit Order Details must be Captured.");
        }

        /// <summary>
        /// when performing application in order and there is no mailing address for the offer the user should
        /// get an error message
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_application_mailing_address_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.Service<IApplicationService>().DeleteOfferMailingAddress(base.GenericKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Each Application must have one valid Application mailing address.");
        }

        /// <summary>
        /// when performing application in order and there is no afforability assessment, the user should see an error message.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_has_no_active_affordability_assessment_message()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();            
            base.Service<IAffordabilityAssessmentService>().DeleteLegalEntityAffordabilityAssessment(base.GenericKey);
            
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();

            base.View.AssertMessageDisplayed("Application does not have an Active Affordability Assessment.");
        }        

        /// <summary>
        /// when performing application in order and the legal entity debt counselling declarations are set to YES the user should get an error message
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_declarations_required()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.ManageApplication, WorkflowRoleTypeEnum.PLConsultantD);
            base.Service<IApplicationService>().DeleteExternalRoleDeclarations(base.GenericKey);
            var legalEntityKey = (from le in base.Service<IExternalRoleService>().GetActiveExternalRoleList(base.GenericKey,
                                                                            GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client)
                                  select le.LegalEntityKey).FirstOrDefault();
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().LegalEntity_ByLegalEntityKey(legalEntityKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.LegalEntityNode>().ApplicationDeclarations(NodeTypeEnum.Update);
            var appDeclarations = new Automation.DataModels.ApplicationDeclaration
            {
                InsolvencyAnswer = "No",
                DateRehabilitatedAnswer = null,
                AdministrationOrderAnswer = "No",
                DateRescindedAnswer = null,
                CurrentUnderDebtCounsellingAnswer = "Yes",
                CurrentDebtRearrangementAnswer = "Yes",
                ConductCreditCheckAnswer = "Yes"
            };
            base.Browser.Page<PersonalLoanApplicationDeclarations>().ApplicationDeclarationUpdate(appDeclarations);
            base.Browser.ClickAction(string.Format("Personal Loan : {0} (Personal Loans)", base.GenericKey));
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentCheck);
            base.View.ClickYes();
            bool offerExists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.DocumentCheck);
            Assert.That(offerExists);
            base.ReloadCase(WorkflowStates.PersonalLoansWF.DocumentCheck, WorkflowRoleTypeEnum.PLAdminD);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Further processing is not allowed due to legal entity debt counselling declarations.");
            //delete declarations
            base.Service<IApplicationService>().DeleteExternalRoleDeclarations(base.GenericKey);
        }

        /// <summary>
        /// when performing application in order and there are no ITC's at all
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_itc_required_where_client_has_none()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.CreateMandatoryDataForSubmissionToCredit();
            var legalEntityKey = (from le in base.Service<IExternalRoleService>().GetActiveExternalRoleList(base.GenericKey,
                                                                         GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client)
                                  select le.LegalEntityKey).FirstOrDefault();
            base.Service<ILegalEntityService>().DeleteITC(legalEntityKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Every Legal Entity must have a valid ITC enquiry.");
        }

        /// <summary>
        /// when performing application in order and there is an ITC older than a month user should get an error message.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_itc_required_where_the_itc_is_more_than_a_month_old()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.CreateMandatoryDataForSubmissionToCredit();
            var legalEntityKey = (from le in base.Service<IExternalRoleService>().GetActiveExternalRoleList(base.GenericKey,
                                                                         GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client)
                                  select le.LegalEntityKey).FirstOrDefault();
            //Remove all the ITC's so the rule can
            base.Service<ILegalEntityService>().UpdateITC(legalEntityKey, DateTime.Now.Subtract(TimeSpan.FromDays(32)));
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Every Legal Entity must have a valid ITC enquiry.");
        }

        /// <summary>
        /// When performing the 'Application in Order' action and there is no domicilium address, the user should get an error message
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_message_where_no_domicilium_present()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.CreateMandatoryDataForSubmissionToCredit();
            var extRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(extRole.LegalEntityKey);

            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("A Domicilium Address must be captured for the Applicant on a Personal Loan application.");
        }

        /// <summary>
        /// When performing the 'Application in Order' action and there is a pending domicilium address, move the case into Credit.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_move_to_credit_where_domicilium_pending()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.CreateMandatoryDataForSubmissionToCredit();
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.Confirm(true, true);
            BuildingBlocks.Assertions.X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Credit);
        }

        /// <summary>
        /// When performing the 'Application in Order' action and there is an active domicilium address, move the case into Credit.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_move_to_credit_where_domicilium_active()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            UpdateDeclarations();
            base.Service<IApplicationService>().InsertEmploymentRecords(this.GenericKey);
            base.Service<IApplicationService>().InsertOfferMailingAddress(this.GenericKey);
            base.Service<IApplicationService>().CleanUpOfferDebitOrder(this.GenericKey);
            base.Service<ILegalEntityService>().InsertITC(this.GenericKey, 4, 2);
            base.Service<IApplicationService>().InsertAffordabilityAssessment((int)AffordabilityAssessmentStatusKey.Unconfirmed, base.GenericKey);
            var externalRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(externalRole.LegalEntityKey);
            var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(externalRole.LegalEntityKey, AddressFormatEnum.Street,
                AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            base.Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, externalRole.LegalEntityKey, GeneralStatusEnum.Active);

            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.Confirm(true, true);
            BuildingBlocks.Assertions.X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Credit);
        }

        /// <summary>
        /// when performing application in order and there is no confirmed income the user should get an error message.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_should_display_confirmed_employment_required()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            var extRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<IEmploymentService>().UpdateAllEmploymentStatus(extRole.LegalEntityKey, EmploymentStatusEnum.Previous);

            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.ClickYes();
            base.View.AssertMessageDisplayed("Employment must be confirmed before the application can be submitted.");
        }

        /// <summary>
        /// when performing application in order and there are no messages it should move to Credit stage.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_without_messages_should_move_to_credit()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.CreateMandatoryDataForSubmissionToCredit();
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.Confirm(true, true);
            BuildingBlocks.Assertions.X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Credit);
        }

        /// <summary>
        /// admin users should be able to submit to credit.
        /// </summary>
        [Test]
        public void when_performing_application_in_order_as_admin_should_move_to_credit_and_write_a_stage_transition()
        {
            EnsurePersonalLoanCaseAtDocumentCheck();
            base.CreateMandatoryDataForSubmissionToCredit();
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.ApplicationinOrder);
            base.View.Confirm(true, true);
            BuildingBlocks.Assertions.X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Credit);
            base.Service<IStageTransitionService>().CheckIfTransitionExists(base.GenericKey, (int)StageDefinitionStageDefinitionGroupEnum.ApplicationInOrder);
        }

        private void EnsurePersonalLoanCaseAtDocumentCheck()
        {
            //Check that the we can still use the PL application last searched for.
            var exists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.DocumentCheck);
            if (!exists && base.GenericKey != 0)
                base.FindCaseAtState(WorkflowStates.PersonalLoansWF.DocumentCheck, WorkflowRoleTypeEnum.PLAdminD);
            if (base.GenericKey == 0)
                base.FindCaseAtState(WorkflowStates.PersonalLoansWF.DocumentCheck, WorkflowRoleTypeEnum.PLAdminD);
        }
    }
}