using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.StateManagement;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.AffordabilityHandler
{
    public class when_adding_affordabilities_throws_error : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;
        private static int clientKey;
        private static string identityNumber;
        private static Exception runtimeException;

        private static Exception thrownException;
        private static string friendlyErrorMessage;

        private Establish context = () =>
        {
            applicationNumber = 12;
            clientKey = 5656;

            runtimeException = new Exception("Could not add affordabilities");
            domainProcess.ProcessState = applicationStateMachine;
            var applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            domainProcess.DataModel = applicationCreationModel;
            identityNumber = applicationCreationModel.Applicants.First().IDNumber;
            friendlyErrorMessage = String.Format("The income/expenditure assessment could not be added for applicant with ID Number: {0}.", identityNumber);

            var clientCollection = new Dictionary<string, int> { { domainProcess.DataModel.Applicants.First().IDNumber, clientKey } };

            applicationStateMachine.WhenToldTo(x => x.ClientCollection).Return(clientCollection);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);

            applicationDomainService.WhenToldTo(x => x.PerformCommand(Param.IsAny<AddApplicantAffordabilitiesCommand>(), Param.IsAny<IServiceRequestMetadata>()))
                                                    .Throw(runtimeException);
        };

        private Because of = () =>
        {
            thrownException = Catch.Exception(() => domainProcess.AddAffordabilities(applicationStateMachine, domainProcess.DataModel.Applicants));
        };

        private It should_fire_the_non_critical_error_trigger = () =>
        {
            applicationStateMachine.WasToldTo(x => x.FireStateMachineTrigger(ApplicationStateTransitionTrigger.NonCriticalErrorReported, Param.IsAny<Guid>()));
        };

        private It should_record_the_command_failure = () =>
        {
            applicationStateMachine.WasToldTo(x => x.RecordCommandFailed(Param.IsAny<Guid>()));
        };

        private It should_add_the_error_and_exception_to_the_state_machine_messages = () =>
        {
            applicationStateMachine.WasToldTo(x => x.AggregateMessages(Param<ISystemMessageCollection>.Matches(m =>
                m.ErrorMessages().Any(y => y.Message.Contains(identityNumber)) &&
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