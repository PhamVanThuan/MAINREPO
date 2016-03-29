using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AffordabilityHandler
{
    public class when_handling_applicant_affordability_event : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel newPurchaseCreationModel;
        private static ApplicantAffordabilitiesAddedEvent applicantAffordabilitiesAddedEvent;
        private static int clientKey, applicationNumber;

        private Establish context = () =>
        {
            clientKey = 1234;
            applicationNumber = 133;

            newPurchaseCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.Start(newPurchaseCreationModel, typeof(NewPurchaseApplicationAddedEvent).Name);

            var clientAffordabilityAssessment = new List<Services.Interfaces.ApplicationDomain.Models.AffordabilityTypeModel> {
                new Services.Interfaces.ApplicationDomain.Models.AffordabilityTypeModel(AffordabilityType.BondPayments, 50000000, "This is a description")
            };

            applicantAffordabilitiesAddedEvent = new ApplicantAffordabilitiesAddedEvent(DateTime.Now, clientKey, applicationNumber, clientAffordabilityAssessment);
        };

        private Because of = () =>
        {
            domainProcess.HandleEvent(applicantAffordabilitiesAddedEvent, serviceRequestMetadata);
        };

        private It should_raise_applicant_affordability_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.AffordabilityDetailCaptureConfirmed, applicantAffordabilitiesAddedEvent.Id));
        };
    }
}