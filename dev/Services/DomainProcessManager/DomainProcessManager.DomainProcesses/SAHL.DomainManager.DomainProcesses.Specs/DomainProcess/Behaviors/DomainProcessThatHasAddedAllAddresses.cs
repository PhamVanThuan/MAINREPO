using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain;
using SAHL.Services.Interfaces.AddressDomain.Commands;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Behaviors
{
    [Behaviors]
    public class DomainProcessThatHasAddedAllAddresses
    {
        protected static IAddressDomainServiceClient addressDomainService;
        protected static IClientDomainServiceClient clientDomainService;
        protected static IApplicationStateMachine applicationStateMachine;
        protected static IApplicationDomainServiceClient applicationDomainService;
        protected static ApplicantModel applicant;
        protected static int applicationNumber, clientKey, streetClientAddressKey, clientDomiciliumKey;

        private It should_add_a_residential_address = () =>
        {
            var streetAddress = applicant.Addresses.First(a => a.AddressType == AddressType.Residential && a.AddressFormat == AddressFormat.FreeText);
            addressDomainService.WasToldTo(x => x.PerformCommand(
                Param<LinkFreeTextAddressAsResidentialAddressToClientCommand>.Matches(m =>
                m.ClientKey == clientKey &&
                m.FreeTextAddressModel.FreeText1 == streetAddress.FreeText1 &&
                m.FreeTextAddressModel.FreeText2 == streetAddress.FreeText2 &&
                m.FreeTextAddressModel.AddressFormat == AddressFormat.FreeText),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_a_postal_address = () =>
        {
            var postalAddress = applicant.Addresses.First(a => a.AddressType == AddressType.Postal && a.AddressFormat == AddressFormat.FreeText);
            addressDomainService.WasToldTo(x => x.PerformCommand(
                Param<LinkFreeTextAddressAsPostalAddressToClientCommand>.Matches(m =>
                m.ClientKey == clientKey &&
                m.FreeTextAddressModel.FreeText1 == postalAddress.FreeText1 &&
                m.FreeTextAddressModel.FreeText2 == postalAddress.FreeText2 &&
                m.FreeTextAddressModel.AddressFormat == AddressFormat.FreeText),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_link_the_street_address_as_property_address = () =>
        {
            var propertyAddress = applicant.Addresses.First(a => a.AddressFormat == AddressFormat.Street);
            addressDomainService.WasToldTo(x => x.PerformCommand(Param<LinkStreetAddressAsResidentialAddressToClientCommand>.Matches(m =>
                m.StreetAddressModel.StreetName == propertyAddress.StreetName &&
                m.StreetAddressModel.StreetNumber == propertyAddress.StreetNumber &&
                m.ClientKey == clientKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_check_if_the_freetext_address_is_linked_to_client = () =>
        {
            addressDomainService.WasToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()));
        };

        private It should_add_a_client_domicilium_address = () =>
        {
            clientDomainService.WasToldTo(x => x.PerformCommand(Param<AddClientAddressAsPendingDomiciliumCommand>.Matches(m =>
                m.ClientAddressAsPendingDomiciliumModel.ClientKey == clientKey &&
                m.ClientAddressAsPendingDomiciliumModel.ClientAddresskey == streetClientAddressKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_add_the_client_domicilium_address_to_the_state_machine = () =>
        {
            applicationStateMachine.ClientDomicilumAddressCollection[applicant.IDNumber].ShouldEqual(streetClientAddressKey);
            addressDomainService.WasToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()));
            applicationStateMachine.ClientDomicilumAddressCollection.ContainsKey(applicant.IDNumber).ShouldBeTrue();
        };

        private It should_add_the_property_address_as_the_application_mailing_address_to_the_state_machine = () =>
        {
            applicationStateMachine.MailingClientAddressKey.ShouldEqual(streetClientAddressKey);
        };

        private It should_link_the_client_domicilium_address_to_the_applicant = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<LinkDomiciliumAddressToApplicantCommand>.Matches(m =>
                m.ApplicationNumber == applicationNumber &&
                m.ClientKey == clientKey &&
                m.ApplicantDomicilium.ClientDomiciliumKey == clientDomiciliumKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };

        private It should_link_the_mailing_address_to_the_application = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(Param<AddApplicationMailingAddressCommand>.Matches(
                m => m.ApplicationNumber == applicationNumber &&
                m.model.ClientAddressKey == streetClientAddressKey),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}