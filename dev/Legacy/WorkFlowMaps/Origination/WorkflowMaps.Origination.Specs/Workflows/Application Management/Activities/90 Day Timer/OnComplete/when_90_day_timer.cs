using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities._90_Day_Timer.OnComplete
{
    [Subject("Activity => _90_Day_Timer => OnComplete")]
    internal class when_90_day_timer : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IApplicationManagement client;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            client = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_90_Day_Timer(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_activate_ntu_from_watchdog_timer = () =>
        {
            client.WasToldTo(x => x.ActivateNTUFromWatchdogTime((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}