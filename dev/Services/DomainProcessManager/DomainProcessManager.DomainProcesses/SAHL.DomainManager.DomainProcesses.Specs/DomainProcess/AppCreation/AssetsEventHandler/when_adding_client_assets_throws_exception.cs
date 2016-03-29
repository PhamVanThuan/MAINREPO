using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
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

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AssetsEventHandler
{
    public class when_adding_client_assets_throws_exception : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static FreeTextPostalAddressLinkedToClientEvent addressLinkedEvent;
        private static IEnumerable<ApplicantAssetLiabilityModel> applicantAssetLiabilities;
        private static ApplicantUnListedInvestmentAssetModel unlistedInvestment;
        private static Dictionary<string, int> clientCollection;
        private static Exception runtimeException;
        private static Exception thrownException;        
        private static Guid correlationId;
        private static string friendlyErrorMessage;
        private static int clientKey, applicationNumber;

        private Establish context = () =>
        {
            clientKey = 111;
            applicationNumber = 5546;
            correlationId = Guid.Parse("{C1531D6E-BD6E-4198-BF48-B8ADE56A3B45}");
           
            unlistedInvestment = new ApplicantUnListedInvestmentAssetModel(500000, "ACME co");
            applicantAssetLiabilities = new List<ApplicantAssetLiabilityModel> { unlistedInvestment };
            addressLinkedEvent = new FreeTextPostalAddressLinkedToClientEvent(new DateTime(2014, 1, 1),
                "12", "32", "Mayville", "Kwazulu Natal", "Durban", "4001", AddressFormat.FreeText, clientKey, 14);
            runtimeException = new Exception("Something went wrong");

            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            newPurchaseCreationModel.Applicants.First().ApplicantAssetLiabilities = applicantAssetLiabilities;
            domainProcess.Start(newPurchaseCreationModel, typeof(PostalAddressLinkedToClientEvent).Name);
            friendlyErrorMessage = String.Format("Could not add asset or liability \"{0}\" for applicant with ID Number {1}.",
                            applicantAssetLiabilities.First().AssetLiabilityType.ToString(), domainProcess.DataModel.Applicants.First().IDNumber);
            clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };
            applicationStateMachine.WhenToldTo(x => x.ContainsStateInBreadCrumb(ApplicationState.AllAddressesCaptured)).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);

            clientDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddInvestmentAssetToClientCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                    .Throw(runtimeException);
            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(correlationId);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.HandleEvent(addressLinkedEvent, serviceRequestMetadata));
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