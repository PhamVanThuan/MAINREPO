using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ClientPendingDomiciliumAddress
{
    public class when_handling_event_model_exception : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber, clientKey, clientAddressKey;
        private static ResidentialStreetAddressLinkedToClientEvent residentialAddressLinkedEvent;
        private static Exception thrownException, runtimeException;
        private static string friendlyErrorMessage;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;
            clientAddressKey = 173;

            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            var address = applicationCreationModel.Applicants.First().Addresses.First(x => x.AddressFormat == AddressFormat.Street);
            residentialAddressLinkedEvent = new ResidentialStreetAddressLinkedToClientEvent(new DateTime(2014, 11, 15), address.UnitNumber, address.BuildingNumber,
                address.BuildingName, address.StreetNumber, address.StreetName, address.Suburb,
                address.City, address.Province, address.PostalCode, clientKey, clientAddressKey);

            runtimeException = new Exception("an error occured.");

            var clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };
            var clientDomicilumAddressCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientAddressKey } };
            friendlyErrorMessage = string.Format("The Pending Domicilium Address could not be saved for Applicant with ID Number:", domainProcess.DataModel.Applicants.First().IDNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ClientDomicilumAddressCollection).Return(clientDomicilumAddressCollection);
            clientDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddClientAddressAsPendingDomiciliumCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                    .Throw(runtimeException);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.Handle(residentialAddressLinkedEvent, serviceRequestMetadata));
        };

        private It should_contain_an_error_message = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ErrorMessages().Any(y => y.Message.Contains(friendlyErrorMessage)) &&
                m.ExceptionMessages().Any(y => y.Message.Contains(runtimeException.ToString())))));
        };

        private It should_log_the_error_message = () =>
        {
            rawLogger.WasToldTo(x => x.LogError(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()
               , Param<string>.Matches(m => m.Contains(friendlyErrorMessage)), null));
        };

        private It should_not_throw_an_exception = () =>
        {
            thrownException.ShouldBeNull();
        };
    }
}