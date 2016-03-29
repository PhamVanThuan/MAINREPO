using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationMailingAddressHandler
{
    public class when_adding_mailing_address_fails : WithNewPurchaseDomainProcess
    {
        private static ResidentialStreetAddressLinkedToClientEvent residentialAddressLinkedEvent;
        private static int applicationNumber;
        private static int clientKey, clientAddressKey;
        private static ApplicationMailingAddressModel mailingAddressModel;
        private static Exception runtimeException;
        private static Guid correlationId;
        private static Exception thrownException;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;
            clientAddressKey = 173;

            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;

            domainProcess.DataModel = applicationCreationModel;
            mailingAddressModel = applicationCreationModel.ApplicationMailingAddress;

            var streetAddress = applicationCreationModel.Applicants.First().Addresses.First(x => x.AddressFormat == AddressFormat.Street);
            residentialAddressLinkedEvent = new ResidentialStreetAddressLinkedToClientEvent(DateTime.Now, streetAddress.UnitNumber, streetAddress.BuildingNumber,
                streetAddress.BuildingName, streetAddress.StreetNumber, streetAddress.StreetName, streetAddress.Suburb, streetAddress.City, streetAddress.Province,
               streetAddress.PostalCode, clientKey, clientAddressKey);

            var IDNumber = domainProcess.DataModel.Applicants.First().IDNumber;
            var clientCollection = new Dictionary<string, int> { { IDNumber, clientKey } };

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.MailingClientAddressKey).Return(clientAddressKey);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
            runtimeException = new Exception("The end is near...");
            applicationDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddApplicationMailingAddressCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                    .Throw(runtimeException);
            correlationId = Guid.Parse("{C1531D6E-BD6E-4198-BF48-B8ADE56A3B45}");
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(correlationId);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.Handle(residentialAddressLinkedEvent, serviceRequestMetadata));
        };

        private It should_not_throw_an_exception = () =>
        {
            thrownException.ShouldBeNull();
        };

        private It should_fire_the_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, Param.IsAny<Guid>()));
        };

        private It should_record_the_command_failure = () =>
        {
            applicationStateMachine.WasToldTo(x => x.RecordCommandFailed(correlationId));
        };

        private It should_add_the_error_to_the_state_machine_messages = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ExceptionMessages().Any(y => y.Message.Contains(runtimeException.ToString())))));
        };
    }
}