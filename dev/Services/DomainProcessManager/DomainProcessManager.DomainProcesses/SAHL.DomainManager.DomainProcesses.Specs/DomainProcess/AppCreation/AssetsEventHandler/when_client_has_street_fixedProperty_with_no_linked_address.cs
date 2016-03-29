using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.AddressDomain.Events;
using SAHL.Services.Interfaces.AddressDomain.Model;
using SAHL.Services.Interfaces.AddressDomain.Queries;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AssetsEventHandler
{
    public class when_client_has_street_fixedProperty_with_no_linked_address : WithNewPurchaseDomainProcess
    {
        private static ApplicantFixedPropertyAssetModel fixedPropertyAsset;

        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static FreeTextPostalAddressLinkedToClientEvent addressLinkedEvent;
        private static IEnumerable<ApplicantAssetLiabilityModel> applicantAssetLiabilities;
        private static Dictionary<string, int> clientCollection;
        private static int clientKey, applicationNumber, clientAddressKey;

        private Establish context = () =>
        {
            clientKey = 100;
            applicationNumber = 173;
            clientAddressKey = 14;
            var fixedPropertyAddressModel = ApplicationCreationTestHelper.PopulateFreeTextResidentialAddressModel();
            fixedPropertyAsset = new ApplicantFixedPropertyAssetModel(fixedPropertyAddressModel, 2500000, 800000, new DateTime(1999, 12, 26));
            applicantAssetLiabilities = new List<ApplicantAssetLiabilityModel> { fixedPropertyAsset };

            addressLinkedEvent = new FreeTextPostalAddressLinkedToClientEvent(new DateTime(2014, 1, 1),
                "12",
                "32",
                "Mayville",
                "Kwazulu Natal",
                "Durban",
                "4001",
                AddressFormat.FreeText,
                clientKey,
                clientAddressKey);

            newPurchaseCreationModel =
                ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            newPurchaseCreationModel.Applicants.First().ApplicantAssetLiabilities = applicantAssetLiabilities;
            domainProcess.DataModel = newPurchaseCreationModel;
            domainProcess.ProcessState = applicationStateMachine;

            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);

            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(105);
            clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);

            var freeTextqueryResult = new ServiceQueryResult<GetClientFreeTextAddressQueryResult>(
                new GetClientFreeTextAddressQueryResult[] { });
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()))
                .Return<GetClientFreeTextAddressQuery>(rqst =>
                {
                    rqst.Result = freeTextqueryResult;
                    return new SystemMessageCollection();
                });
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(addressLinkedEvent, serviceRequestMetadata);
        };

        private It should_check_if_street_fixedProperty_address_is_linked_to_client = () =>
        {
            addressDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetClientFreeTextAddressQuery>()));
        };

        private It should_not_add_the_fixed_property_asset_to_client = () =>
        {
            clientDomainService.WasNotToldTo(x => x.PerformCommand(Param.IsAny<AddFixedPropertyAssetToClientCommand>(),
                Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_go_into_non_critical_error_state = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, Param.IsAny<Guid>()));
        };

        private It should_add_an_error_message_to_the_application_state_machine = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(
                Param<ISystemMessageCollection>.Matches(m => m.ErrorMessages().Count() == 1)));
        };
    }
}
