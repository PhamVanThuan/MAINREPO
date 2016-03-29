using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation
{
    public class when_application_processing_is_competed : WithNewPurchaseDomainProcess
    {
        private static int applicationNumber;

        private Establish context = () =>
        {
            applicationNumber = 11432;
            domainProcess.ProcessState = applicationStateMachine;
            applicationStateMachine.WhenToldTo(x => x.HasProcessCompletedWithCriticalPathFullyCaptured()).Return(true);
            applicationStateMachine.WhenToldTo(x => x.ApplicationNumber).Return(applicationNumber);
        };

        private Because of = () =>
        {
            domainProcess.HandledEvent(serviceRequestMetadata);
        };

        private It should_log_that_the_process_has_completed = () =>
        {
            rawLogger.WasToldTo(l => l.LogInfo(Param.IsAny<LogLevel>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param<string>.Matches(s =>
                s.Contains(string.Format("application number {0}", applicationNumber)) && s.Contains("completed")), null
            ));
        };
    }
}