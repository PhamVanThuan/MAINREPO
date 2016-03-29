using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AddressEventHandler
{
    public class when_linking_property_addr_as_lead_applicant_residential : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static ResidentialStreetAddressLinkedToClientEvent streetAddressAsResidentialAddressLinkedToClientEvent;
        private static int clientKey;
        private static Dictionary<string, int> clientCollection;
        private static Dictionary<string, int> clientDomicilumAddressCollection;
        private static string leadMainApplicantIdNumber;
        private static int clientAddressKey;

        private Establish context = () =>
        {
            clientKey = 10;
            clientAddressKey = 1232;
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            leadMainApplicantIdNumber = applicationCreationModel.Applicants.First().IDNumber;

            DomainModelMapper mapper = new DomainModelMapper();
            mapper.CreateMap<AddressModel, Services.Interfaces.AddressDomain.Model.StreetAddressModel>();
            var streetAddress = mapper.Map(ApplicationCreationTestHelper.PopulatePropertyAddressModel(true));

            streetAddressAsResidentialAddressLinkedToClientEvent = new ResidentialStreetAddressLinkedToClientEvent(DateTime.Now, streetAddress.UnitNumber, streetAddress.BuildingNumber,
                streetAddress.BuildingName, streetAddress.StreetNumber, streetAddress.StreetName, streetAddress.Suburb, streetAddress.City, streetAddress.Province,
               streetAddress.PostalCode, clientKey, clientAddressKey);

            clientCollection = new Dictionary<string, int>();
            clientCollection.Add(leadMainApplicantIdNumber, clientKey);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);

            clientDomicilumAddressCollection = new Dictionary<string, int>();
            applicationStateMachine.WhenToldTo(asm => asm.ClientDomicilumAddressCollection).Return(clientDomicilumAddressCollection);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(streetAddressAsResidentialAddressLinkedToClientEvent, serviceRequestMetadata);
        };

        private It should_fire_the_address_details_captured_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.AddressCaptureConfirmed), Param.IsAny<Guid>()));
        };

        private It should_set_the_application_mailing_address_client_address_key = () =>
        {
            applicationStateMachine.MailingClientAddressKey.ShouldEqual(clientAddressKey);
        };
    }
}