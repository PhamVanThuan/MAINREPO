using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AddressEventHandler
{
    public class when_free_text_is_linked_as_postal_address : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static FreeTextPostalAddressLinkedToClientEvent freeTextPostalAddressLinkedToClientEvent;
        private static int clientKey, clientAddressKey;
        private static Dictionary<string, int> clientCollection;

        private Establish context = () =>
        {
            clientKey = 10;
            clientAddressKey = 123;

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(Core.BusinessModel.Enums.OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            var freeTextPostal = ApplicationCreationTestHelper.PopulateFreeTextPostalAddressModel();
            freeTextPostalAddressLinkedToClientEvent = new FreeTextPostalAddressLinkedToClientEvent(
                DateTime.Now
                , freeTextPostal.FreeText1
                , freeTextPostal.FreeText2
                , freeTextPostal.FreeText3
                , freeTextPostal.FreeText4
                , freeTextPostal.FreeText5
                , freeTextPostal.Country
                , freeTextPostal.AddressFormat
                , clientKey
                , clientAddressKey);

            clientCollection = new Dictionary<string, int>();
            clientCollection.Add(domainProcess.DataModel.Applicants.First().IDNumber, clientKey);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);

            int? mailingClientAddressKey = 7312;
            applicationStateMachine.WhenToldTo(x => x.MailingClientAddressKey).Return(mailingClientAddressKey);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(freeTextPostalAddressLinkedToClientEvent, serviceRequestMetadata);
        };

        private It should_fire_the_address_details_captured_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(Arg.Is<ApplicationStateTransitionTrigger>(y => y == ApplicationStateTransitionTrigger.AddressCaptureConfirmed), Param.IsAny<Guid>()));
        };
    }
}