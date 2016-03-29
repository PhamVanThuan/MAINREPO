using SAHL.Core.DomainProcess;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        , IDomainProcessEvent<ResidentialStreetAddressLinkedToClientEvent>
        , IDomainProcessEvent<FreeTextPostalAddressLinkedToClientEvent>
        , IDomainProcessEvent<FreeTextResidentialAddressLinkedToClientEvent>
        where T : ApplicationCreationModel
    {
        public void Handle(FreeTextResidentialAddressLinkedToClientEvent freeTextResidentialAddressLinkedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            HandleFreeTextResidentialLinkedToClientConfirmed(freeTextResidentialAddressLinkedToClientEvent.Id);
        }

        public void Handle(FreeTextPostalAddressLinkedToClientEvent freeTextPostalAddressLinkedToClientEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            HandleFreeTextPostalLinkedToClientConfirmed(freeTextPostalAddressLinkedToClientEvent.Id);
        }

        public void Handle(ResidentialStreetAddressLinkedToClientEvent propertyAddressAsResidentialAddressLinkedEvent, IServiceRequestMetadata serviceRequestMetadata)
        {
            HandlePropertyAddressLinkedAsResidentialToClientConfirmed(
                  propertyAddressAsResidentialAddressLinkedEvent.Id
                , propertyAddressAsResidentialAddressLinkedEvent.ClientKey
                , propertyAddressAsResidentialAddressLinkedEvent.ClientAddressKey);
        }

        private void HandleFreeTextPostalLinkedToClientConfirmed(Guid postalAddressConfirmationId)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, postalAddressConfirmationId);

            OnAllAddressesAdded(applicationStateMachine);
        }

        private void HandleFreeTextResidentialLinkedToClientConfirmed(Guid freeTextResidentialAddressLinkedConfirmationId)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, freeTextResidentialAddressLinkedConfirmationId);

            OnAllAddressesAdded(applicationStateMachine);
        }

        private void HandlePropertyAddressLinkedAsResidentialToClientConfirmed(Guid propertyAddressLinkedAsResidentialConfirmationId, int clientKey, int clientAddressKey)
        {
            applicationStateMachine.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AddressCaptureConfirmed, propertyAddressLinkedAsResidentialConfirmationId);
            var clientIdNumber = applicationStateMachine.ClientCollection.First(c => c.Value == clientKey).Key;
            var applicant = DataModel.Applicants.First(a => a.IDNumber == clientIdNumber);

            if (!applicationStateMachine.MailingClientAddressKey.HasValue && applicant.ApplicantRoleType == Core.BusinessModel.Enums.LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant)
            {
                applicationStateMachine.MailingClientAddressKey = clientAddressKey;
            }
            var addedAddress = applicant.Addresses.Where(add => add.IsDomicilium).FirstOrDefault();
            if (addedAddress != null && addedAddress.IsDomicilium)
            {
                PopulateDictionary(applicationStateMachine.ClientDomicilumAddressCollection, applicant.IDNumber, clientAddressKey);
            }
            OnAllAddressesAdded(applicationStateMachine);
        }

        public void OnAllAddressesAdded(IApplicationStateMachine applicationStateMachine)
        {
            if (applicationStateMachine.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured))
            {
                var applicants = this.DataModel.Applicants;

                AddApplicationMailingAddress();

                LinkAddressAsPendingDomicilum(applicationStateMachine, applicants);

                ConditionallyAddAssetsToClient(applicationStateMachine);
            }
        }
    }
}