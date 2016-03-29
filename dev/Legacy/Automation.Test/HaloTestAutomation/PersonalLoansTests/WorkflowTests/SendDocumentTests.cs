using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    public class SendDocumentTests : PersonalLoansWorkflowTestBase<CorrespondenceProcessing>
    {
        protected override void OnTestStart()
        {
            base.FindCaseAtState(WorkflowStates.PersonalLoansWF.LegalAgreements, WorkflowRoleTypeEnum.PLConsultantD);
            base.Browser.Page<BasePageAssertions>().AssertNode("Send Documents", "Send Documents action is available but not selectable.");
            base.OnTestStart();
        }

        protected override void OnTestTearDown()
        {
            base.OnTestTearDown();
            base.Browser.Dispose();
        }

        /// <summary>
        /// This test will perform the Send Documents action for a case where the legalentity has a pending domicilium address. It will check that both the Legal Agreements and
        /// Domicilium Election Form are sent and that the case is sent to the Verify Documents state assigned to a personal loan admin.
        /// </summary>
        [Test]
        public void when_performing_send_documents_should_send_legal_agreement_and_domicilium_election_form()
        {
            base.Service<IApplicationService>().InsertExternalRoleDomicilium(base.GenericKey);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.SendDocuments);
            base.View.AssertViewDisplayed("PL_Correspondence_SendDocuments_With_Domicilium_Election_Form");
            string expectedPLAdmin = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLAdminD, RoundRobinPointerEnum.PLAdmin);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.VerifyDocuments);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_SendDocuments);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLAdminD, expectedPLAdmin,
                WorkflowStates.PersonalLoansWF.VerifyDocuments, Workflows.PersonalLoans);
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Email, checkDataSTOR: true);
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.DomiciliumElectionForm, CorrespondenceMedium.Email, checkDataSTOR: true);
        }

        /// <summary>
        /// This test will perform the Send Documents action for a case where the legalentity has an active domicilium address. It will check that only the Legal Agreements are sent and
        /// that the case is sent to the Verify Documents state assigned to a personal loan admin.
        /// </summary>
        [Test]
        public void when_performing_send_documents_should_send_legal_agreement()
        {
            var externalRole = Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(externalRole.LegalEntityKey);
            var legalEntityAddress = Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(externalRole.LegalEntityKey, AddressFormatEnum.Street,
                AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            base.Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, externalRole.LegalEntityKey, GeneralStatusEnum.Active);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.SendDocuments);
            base.View.AssertViewDisplayed("PL_Correspondence_SendDocuments");
            string expectedPLAdmin = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLAdminD, RoundRobinPointerEnum.PLAdmin);
            base.View.SendCorrespondence(CorrespondenceSendMethodEnum.Email);
            X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.VerifyDocuments);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_SendDocuments);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLAdminD, expectedPLAdmin,
                WorkflowStates.PersonalLoansWF.VerifyDocuments, Workflows.PersonalLoans);
            CorrespondenceAssertions.AssertCorrespondenceRecordAdded(base.GenericKey, CorrespondenceReports.PersonalLoanLegalAgreement, CorrespondenceMedium.Email, checkDataSTOR: true);
            CorrespondenceAssertions.AssertCorrespondenceRecordNotAdded(base.GenericKey, CorrespondenceReports.DomiciliumElectionForm, CorrespondenceMedium.Email);
        }
    }
}