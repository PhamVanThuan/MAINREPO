using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ComcorpPropertyDetailsHandler
{
    public class when_adding_comcorp_property_fails : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static Exception runtimeException;
        private static Exception thrownException;
        private static Guid correlationId;

        private Establish context = () =>
        {
            applicationNumber = 12;

            int clientKey = 100;

            correlationId = Guid.Parse("{C1531D6E-BD6E-4198-BF48-B8ADE56A3B45}");

            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;

            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(new Guid());

            var identityNumber = domainProcess.DataModel.Applicants.First().IDNumber;
            var clientCollection = new Dictionary<string, int> { { identityNumber, clientKey } };

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            combGuidGenerator.WhenToldTo(x => x.Generate()).Return(correlationId);
            runtimeException = new Exception("Comcorp property detail exception");
            propertyDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddComcorpOfferPropertyDetailsCommand>(), Param.IsAny<IServiceRequestMetadata>())).
                                                    Throw(runtimeException);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.AddComCorpPropertyDetails());
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