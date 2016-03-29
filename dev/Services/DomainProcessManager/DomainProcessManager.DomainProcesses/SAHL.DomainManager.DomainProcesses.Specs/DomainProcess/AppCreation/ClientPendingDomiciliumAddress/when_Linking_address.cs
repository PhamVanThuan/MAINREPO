using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.DomicilumAddress
{
    public class when_linking_address : WithNewPurchaseDomainProcess
    {
        private static ResidentialStreetAddressLinkedToClientEvent residentialAddressLinkedEvent;
        private static int applicationNumber;

        private static int clientKey, clientAddressKey;

        private Establish context = () =>
            {
                clientKey = 15;
                clientAddressKey = 50;
                applicationNumber = 400;

                domainProcess.ProcessState = applicationStateMachine;
                var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
                domainProcess.DataModel = applicationCreationModel;
                var applicant = applicationCreationModel.Applicants.First();

                var address = applicant.Addresses.First(x => x.AddressFormat == AddressFormat.Street);
                residentialAddressLinkedEvent = new ResidentialStreetAddressLinkedToClientEvent(new DateTime(2014, 11, 15), address.UnitNumber, address.BuildingNumber,
                    address.BuildingName, address.StreetNumber, address.StreetName, address.Suburb,
                    address.City, address.Province, address.PostalCode, clientKey, clientAddressKey);

                var clientCollection = new Dictionary<string, int> { { applicant.IDNumber, clientKey } };
                var clientDomicilumAddressCollection = new Dictionary<string, int> { { applicant.IDNumber, clientAddressKey } };

                applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
                applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
                applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
                applicationStateMachine.WhenToldTo(x => x.ClientDomicilumAddressCollection).Return(clientDomicilumAddressCollection);
            };

        private Because of = () =>
            {
                domainProcess.HandleEvent(residentialAddressLinkedEvent, serviceRequestMetadata);
            };

        private It should_perform_AddClientAddressAsPendingDomiciliumCommand_command = () =>
            {
                clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddClientAddressAsPendingDomiciliumCommand>.Matches(m =>
                   m.ClientAddressAsPendingDomiciliumModel.ClientAddresskey == clientAddressKey &&
                   m.ClientAddressAsPendingDomiciliumModel.ClientKey == clientKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
            };

        private It should_not_report_non_critical_error = () =>
            {
                applicationStateMachine.WasNotToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, Param.IsAny<Guid>()));
            };
    }
}