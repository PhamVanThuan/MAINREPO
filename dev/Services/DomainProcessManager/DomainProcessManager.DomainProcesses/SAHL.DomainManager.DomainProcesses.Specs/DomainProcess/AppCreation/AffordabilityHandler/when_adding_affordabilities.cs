using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AffordabilityHandler
{
    public class when_adding_affordabilities : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static int clientKey;
        private static ApplicantAffordabilityModel affordability;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;

            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            affordability = applicationCreationModel.Applicants.First().Affordabilities.First();

            var clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
        };

        private Because of = () =>
        {
            domainProcess.AddAffordabilities(applicationStateMachine, domainProcess.DataModel.Applicants);
        };

        private It should_perform_add_applicant_affordability_command = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformCommand(
                Param<AddApplicantAffordabilitiesCommand>.Matches(m =>
                    m.ApplicantAffordabilityModel.ApplicationNumber == applicationNumber &&
                    m.ApplicantAffordabilityModel.ClientKey == clientKey &&
                    m.ApplicantAffordabilityModel.ClientAffordabilityAssessment.First().AffordabilityType == affordability.AffordabilityType &&
                    m.ApplicantAffordabilityModel.ClientAffordabilityAssessment.First().Amount == (double)affordability.Amount),
                Param.IsAny<DomainProcessServiceRequestMetadata>()));
        };
    }
}