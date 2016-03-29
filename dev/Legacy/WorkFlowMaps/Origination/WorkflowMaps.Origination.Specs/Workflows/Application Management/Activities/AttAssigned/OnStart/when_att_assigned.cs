using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.AttAssigned.OnStart
{
    [Subject("Activity => AttAssigned => OnStart")]
    internal class when_att_assigned : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_AttAssigned(instanceData, workflowData, paramsData, messages);
        };

        private It should_create_account_for_application = () =>
        {
            common.WasToldTo(x => x.CreateAccountForApplication((IDomainMessageCollection)messages, workflowData.ApplicationKey, paramsData.ADUserName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}