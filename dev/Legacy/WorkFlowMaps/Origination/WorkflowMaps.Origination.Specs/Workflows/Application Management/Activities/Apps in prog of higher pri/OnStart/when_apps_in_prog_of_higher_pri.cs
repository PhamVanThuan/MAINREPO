using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Apps_in_prog_of_higher_pri.OnStart
{
    [Subject("Activity => Apps_in_prog_of_higher_pri => OnStart")]
    internal class when_apps_in_prog_of_higher_pri : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static bool expectedResult;

        private Establish context = () =>
        {
            result = false;
            expectedResult = true;
            var client = An<IFL>();
            client.WhenToldTo(x => x.AppsInProgOfHigherPri((IDomainMessageCollection)messages, workflowData.ApplicationKey))
                .Return(expectedResult);
            domainServiceLoader.RegisterMockForType<IFL>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Apps_in_prog_of_higher_pri(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_apps_in_prog_of_higher_pri_return_value = () =>
        {
            result.ShouldEqual(expectedResult);
        };
    }
}