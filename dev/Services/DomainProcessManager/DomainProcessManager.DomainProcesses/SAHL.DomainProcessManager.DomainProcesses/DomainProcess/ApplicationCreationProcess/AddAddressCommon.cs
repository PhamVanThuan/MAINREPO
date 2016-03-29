using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.DomainProcess.ApplicationCreationProcess
{
    public abstract partial class ApplicationCreationDomainProcess<T> : DomainProcessBase<T>
        where T : ApplicationCreationModel
    {
        public void AddClientAddresses(IEnumerable<ApplicantModel> applicants)
        {
            foreach (var applicant in applicants)
            {
                int clientKey = applicationStateMachine.ClientCollection[applicant.IDNumber];
                foreach (var address in applicant.Addresses)
                {
                    var addressGuid = this.combGuidGenerator.Generate();
                    try
                    {
                        if (address.AddressFormat == AddressFormat.FreeText)
                        {
                            if (address.AddressType == AddressType.Residential)
                            {
                                AddFreeTextAsResidentialAddress(address, applicant.IDNumber, clientKey, addressGuid);
                            }
                            else if (address.AddressType == AddressType.Postal)
                            {
                                AddFreeTextAsPostalAddress(address, applicant.IDNumber, clientKey, addressGuid);
                            }
                        }
                        else if (address.AddressFormat == AddressFormat.Street)
                        {
                            AddPropertyStreetAddressAsClientResidential(address, applicant.IDNumber, clientKey, addressGuid);
                        }
                    }
                    catch (Exception runtimeException)
                    {
                        string friendlyErrorMessage = string.Format("Address could not be saved for applicant with ID Number: {0}.", applicant.IDNumber);
                        HandleNonCriticalException(runtimeException, friendlyErrorMessage, addressGuid, applicationStateMachine);
                    }
                }
            }
        }

        private void AddFreeTextAsResidentialAddress(AddressModel address, string idNumber, int clientKey, Guid addressGuid)
        {
            var clientAddressKey = GetClientFreeTextAddress(address, clientKey);
            if (clientAddressKey == null)
            {
                LinkFreeTextAsClientResidentialAddress(clientKey, addressGuid, address);
            }
            else
            {
                HandleFreeTextResidentialLinkedToClientConfirmed(addressGuid);
            }
        }

        private void AddPropertyStreetAddressAsClientResidential(AddressModel address, string applicantIdNumber, int clientKey, Guid addressGuid)
        {
            var clientAddress = GetClientStreetAddress(address, clientKey);
            if (clientAddress == null)
            {
                LinkStreetAddressAsClientResidential(clientKey, addressGuid, address);
            }
            else
            {
                HandlePropertyAddressLinkedAsResidentialToClientConfirmed(addressGuid, clientKey, clientAddress.ClientAddressKey);
            }
        }

        private void AddFreeTextAsPostalAddress(AddressModel address, string idNumber, int clientKey, Guid addressGuid)
        {
            var clientAddress = GetClientFreeTextAddress(address, clientKey);
            if (clientAddress == null)
            {
                LinkFreeTextAsClientPostalAddress(clientKey, addressGuid, address);
            }
            else
            {
                HandleFreeTextResidentialLinkedToClientConfirmed(combGuidGenerator.Generate());
            }
        }

        private GetClientFreeTextAddressQueryResult GetClientFreeTextAddress(AddressModel address, int clientKey)
        {
            var streetAddressModel = new SAHL.Services.Interfaces.AddressDomain.Model.FreeTextAddressModel(address.AddressFormat, address.FreeText1,
                address.FreeText2, address.FreeText3, address.FreeText4, address.FreeText5, address.Country);
            GetClientFreeTextAddressQuery query = new GetClientFreeTextAddressQuery(clientKey, streetAddressModel, address.AddressType);
            addressDomainService.PerformQuery(query);
            return query.Result.Results.FirstOrDefault();
        }

        private GetClientStreetAddressQueryResult GetClientStreetAddress(AddressModel address, int clientKey)
        {
            var streetAddressModel = new StreetAddressModel(address.UnitNumber
                , address.BuildingName
                , address.BuildingNumber
                , address.StreetNumber
                , address.StreetName
                , address.Suburb
                , address.City
                , address.City
                , address.PostalCode);
            GetClientStreetAddressQuery query = new GetClientStreetAddressQuery(clientKey, streetAddressModel, address.AddressType);
            addressDomainService.PerformQuery(query);
            return query.Result.Results.FirstOrDefault();
        }

        private void LinkFreeTextAsClientPostalAddress(int clientKey, Guid addressGuid, AddressModel address)
        {
            DomainModelMapper mapper = new DomainModelMapper();
            mapper.CreateMap<AddressModel, Services.Interfaces.AddressDomain.Model.FreeTextAddressModel>();
            Services.Interfaces.AddressDomain.Model.FreeTextAddressModel freeTextPostalAddress = mapper.Map(address);

            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, addressGuid);
            var linkFreeTextAsClientPostalAddressCommand = new LinkFreeTextAddressAsPostalAddressToClientCommand(freeTextPostalAddress, clientKey, addressGuid);

            var serviceMessages = this.addressDomainService.PerformCommand(linkFreeTextAsClientPostalAddressCommand, serviceRequestMetadata);
            CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.AddressCaptured);
        }

        private void LinkFreeTextAsClientResidentialAddress(int clientKey, Guid addressGuid, AddressModel address)
        {
            DomainModelMapper mapper = new DomainModelMapper();
            mapper.CreateMap<AddressModel, Services.Interfaces.AddressDomain.Model.FreeTextAddressModel>();
            Services.Interfaces.AddressDomain.Model.FreeTextAddressModel freeTextResidentialAddress = mapper.Map(address);

            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, addressGuid);
            var linkFreeTextAsClientResidentialAddressCommand = new LinkFreeTextAddressAsResidentialAddressToClientCommand(freeTextResidentialAddress, clientKey, addressGuid);

            var serviceMessages = this.addressDomainService.PerformCommand(linkFreeTextAsClientResidentialAddressCommand, serviceRequestMetadata);
            CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.AddressCaptured);
        }

        protected void LinkStreetAddressAsClientResidential(int clientKey, Guid addressGuid, AddressModel clientStreetAddress)
        {
            DomainModelMapper mapper = new DomainModelMapper();
            mapper.CreateMap<AddressModel, Services.Interfaces.AddressDomain.Model.StreetAddressModel>();
            var streetAddress = mapper.Map(clientStreetAddress);

            var serviceRequestMetadata = new DomainProcessServiceRequestMetadata(this.DomainProcessId, addressGuid);
            var streetAddressCommand = new LinkStreetAddressAsResidentialAddressToClientCommand(streetAddress, clientKey, addressGuid);

            var serviceMessages = this.addressDomainService.PerformCommand(streetAddressCommand, serviceRequestMetadata);
            CheckForNonCriticalErrors(applicationStateMachine, serviceRequestMetadata.CommandCorrelationId, serviceMessages, ApplicationState.AddressCaptured);
        }
    }
}