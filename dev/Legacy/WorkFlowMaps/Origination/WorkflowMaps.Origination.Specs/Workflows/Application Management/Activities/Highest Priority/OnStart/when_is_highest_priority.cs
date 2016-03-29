using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Highest_Priority.OnStart
{
    [Subject("Activity => Highest_Priority => OnStart")]
    internal class when_is_highest_priority : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IFL fl;

        private Establish context = () =>
        {
            result = false;
            workflowData.ApplicationKey = 1;
            fl = An<IFL>();
            fl.WhenToldTo(x => x.HighestPriority(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Highest_Priority(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_highest_priority = () =>
        {
            fl.WasToldTo(x => x.HighestPriority((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}