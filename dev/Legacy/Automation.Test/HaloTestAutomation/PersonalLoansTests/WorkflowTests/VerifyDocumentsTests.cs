using BuildingBlocks.Assertions;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace PersonalLoansTests.WorkflowTests
{
    [RequiresSTA]
    internal class VerifyDocumentsTests : PersonalLoansWorkflowTestBase<WorkflowYesNo>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
        }

        /// <summary>
        /// This test will perform the Documents Verified action for a case where the legalentity has a pending domicilium address. It will check that the pending domicilium
        /// address is set to active and the case is sent to the Disbursement state assigned to a personal loans supervisor.
        /// </summary>
        [Test]
        public void when_performing_documents_verified_should_set_pending_domicilium_to_active()
        {
            EnsurePersonalLoanCaseAtVerifyDocuments();
            //delete existing legalentity domicilium addresses
            var externalRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(externalRole.LegalEntityKey);
            //insert pending legalentity domicilium address
            var legalEntityAddress = base.Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(externalRole.LegalEntityKey, AddressFormatEnum.Street,
                AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            var pending = base.Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, externalRole.LegalEntityKey, GeneralStatusEnum.Pending);
            base.Service<IApplicationService>().InsertExternalRoleDomicilium(pending.LegalEntityDomiciliumKey, externalRole.ExternalRoleKey, base.GenericKey);
            //perform documents verified
            string expectedPLSupervisor = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, RoundRobinPointerEnum.PLSupervisor);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentsVerified);
            base.View.Confirm(true, true);
            //assertions
            X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Disbursement);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, expectedPLSupervisor,
                WorkflowStates.PersonalLoansWF.Disbursement, Workflows.PersonalLoans);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DocumentsVerified);

            LegalEntityAssertions.AssertLegalEntityDomicilium(legalEntityAddress, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// This test will perform the Documents Verified action for a case where the legalentity has a current active domicilium address. It will check that the current active
        /// domicilium address remains active and that the case is sent to the Disbursement state assigned to a personal loans supervisor.
        /// </summary>
        [Test]
        public void when_performing_documents_verified_should_leave_current_domicilium_active()
        {
            EnsurePersonalLoanCaseAtVerifyDocuments();
            //delete existing legalentity domicilium addresses
            var externalRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(externalRole.LegalEntityKey);
            //insert active legalentity domicilium address
            var legalEntityAddress = base.Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(externalRole.LegalEntityKey, AddressFormatEnum.Street,
                AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            base.Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityAddressKey, externalRole.LegalEntityKey, GeneralStatusEnum.Active);
            //perform documents verified
            string expectedPLSupervisor = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, RoundRobinPointerEnum.PLSupervisor);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentsVerified);
            base.View.Confirm(true, true);
            //assertions
            X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Disbursement);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, expectedPLSupervisor,
                WorkflowStates.PersonalLoansWF.Disbursement, Workflows.PersonalLoans);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DocumentsVerified);

            LegalEntityAssertions.AssertLegalEntityDomicilium(legalEntityAddress, GeneralStatusEnum.Active);
        }

        /// <summary>
        /// This test will perform the Documents Verified action for a case where the legalentity has a current active domicilium address and as well as a pending domicilium address.
        /// It will check that the current active domicilium address is set to inactive, that the pending domicilium address is set to active and the case is sent to the Disbursement
        /// state assigned to a personal loans supervisor.
        /// </summary>
        [Test]
        public void when_performing_documents_verified_should_set_active_domicilium_inactive_and_pending_domicilium_to_active()
        {
            EnsurePersonalLoanCaseAtVerifyDocuments();
            //delete existing legalentity domicilium addresses
            var externalRole = base.Service<IExternalRoleService>().GetFirstActiveExternalRole(base.GenericKey, GenericKeyTypeEnum.Offer_OfferKey, ExternalRoleTypeEnum.Client);
            base.Service<ILegalEntityAddressService>().DeleteLegalEntityDomiciliumAddress(externalRole.LegalEntityKey);
            //insert active legalentity domicilium address
            var activeLegalEntityAddress = base.Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(externalRole.LegalEntityKey, AddressFormatEnum.Street,
                AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            base.Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(activeLegalEntityAddress.LegalEntityAddressKey, externalRole.LegalEntityKey, GeneralStatusEnum.Active);
            //insert pending legalentity domicilium address
            var pendingLegalEntityAddress = base.Service<ILegalEntityAddressService>().InsertLegalEntityAddressByAddressType(externalRole.LegalEntityKey, AddressFormatEnum.Street,
                AddressTypeEnum.Residential, GeneralStatusEnum.Active);
            var pending = base.Service<ILegalEntityAddressService>().InsertLegalEntityDomiciliumAddress(pendingLegalEntityAddress.LegalEntityAddressKey, externalRole.LegalEntityKey, GeneralStatusEnum.Pending);
            base.Service<IApplicationService>().InsertExternalRoleDomicilium(pending.LegalEntityDomiciliumKey, externalRole.ExternalRoleKey, base.GenericKey);
            //perform documents verified
            string expectedPLSupervisor = Service<IAssignmentService>().GetUserForReactivateOrRoundRobinAssignment(base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, RoundRobinPointerEnum.PLSupervisor);
            base.Browser.ClickAction(WorkflowActivities.PersonalLoans.DocumentsVerified);
            base.View.Confirm(true, true);
            //assertions
            X2Assertions.AssertWorkflowInstanceExistsForState(Workflows.PersonalLoans, base.GenericKey, WorkflowStates.PersonalLoansWF.Disbursement);
            WorkflowRoleAssignmentAssertions.AssertWorkflowRoleAssignment(base.InstanceID, base.GenericKey, WorkflowRoleTypeEnum.PLSupervisorD, expectedPLSupervisor,
                WorkflowStates.PersonalLoansWF.Disbursement, Workflows.PersonalLoans);
            StageTransitionAssertions.AssertStageTransitionCreated(base.GenericKey, StageDefinitionStageDefinitionGroupEnum.PersonalLoans_DocumentsVerified);
            //active legalentity domicilium is set to inactive
            LegalEntityAssertions.AssertLegalEntityDomicilium(activeLegalEntityAddress, GeneralStatusEnum.Inactive);
            //pending legalentity domicilium is set to active
            LegalEntityAssertions.AssertLegalEntityDomicilium(pendingLegalEntityAddress, GeneralStatusEnum.Active);
        }

        private void EnsurePersonalLoanCaseAtVerifyDocuments()
        {
            //Check that the we can still use the PL application last searched for.
            var exists = base.Service<IX2WorkflowService>().OfferExistsAtState(base.GenericKey, WorkflowStates.PersonalLoansWF.VerifyDocuments);
            if (!exists && base.GenericKey != 0)
                base.FindCaseAtState(WorkflowStates.PersonalLoansWF.VerifyDocuments, WorkflowRoleTypeEnum.PLAdminD);
            if (base.GenericKey == 0)
                base.FindCaseAtState(WorkflowStates.PersonalLoansWF.VerifyDocuments, WorkflowRoleTypeEnum.PLAdminD);
        }
    }
}