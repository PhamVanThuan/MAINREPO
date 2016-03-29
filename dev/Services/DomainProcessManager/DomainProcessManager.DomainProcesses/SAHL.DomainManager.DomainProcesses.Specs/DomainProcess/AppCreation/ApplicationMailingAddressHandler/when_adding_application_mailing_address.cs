using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationMailingAddressHandler
{
    public class when_adding_application_mailing_address : WithNewPurchaseDomainProcess
    {
        private static ResidentialStreetAddressLinkedToClientEvent residentialAddressLinkedEvent;
        private static int applicationNumber;
        private static int clientKey, clientAddressKey;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;
            clientAddressKey = 173;

            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel =
                ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            domainProcess.DataModel = applicationCreationModel;

            var address = applicationCreationModel.Applicants.First().Addresses.First(x => x.AddressFormat == AddressFormat.Street);
            residentialAddressLinkedEvent = new ResidentialStreetAddressLinkedToClientEvent(new DateTime(2014, 11, 15),
                address.UnitNumber,
                address.BuildingNumber,
                address.BuildingName,
                address.StreetNumber,
                address.StreetName,
                address.Suburb,
                address.City,
                address.Province,
                address.PostalCode,
                clientKey,
                clientAddressKey);

            var idNumber = domainProcess.DataModel.Applicants.First().IDNumber;
            var clientCollection = new Dictionary<string, int>();
            clientCollection.Add(idNumber, clientKey);

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.MailingClientAddressKey).Return(clientAddressKey);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(Arg.Is(ApplicationState.AllAddressesCaptured))).Return(true);
        };

        private Because of = () =>
        {
            domainProcess.Handle(residentialAddressLinkedEvent, serviceRequestMetadata);
        };

        private It should_ensure_that_all_addresses_have_been_captured = () =>
        {
            applicationStateMachine.WasToldTo(x => x.ContainsStateInBreadCrumb(Arg.Is(ApplicationState.AllAddressesCaptured)));
        };

        private It should_add_a_correct_application_mailing_address = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Arg.Is<AddApplicationMailingAddressCommand>(y =>
                y.ApplicationNumber == applicationNumber &&
                    y.model.ClientAddressKey == clientAddressKey),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
