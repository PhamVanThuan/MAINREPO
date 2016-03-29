using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using X2DomainService.Interface.HelpDesk;

namespace WorkflowMaps.HelpDesk.Specs.Activities.AutoArchive.OnStart
{
    [Subject("Activity => AutoArchive => OnStart")]
    internal class when_help_desk_query_key_data_property_is_greater_than_zero_and_workflow_data_not_updated : WorkflowSpecHelpDesk
    {
        private static bool result;
        private static IHelpDesk client;

        private Establish context = () =>
        {
            result = true;
            client = An<IHelpDesk>();
            workflowData.HelpDeskQueryKey = 1;
            client.WhenToldTo(x => x.X2AutoArchive2AM_Update(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(false);
            domainServiceLoader.RegisterMockForType<IHelpDesk>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_AutoArchive(instanceData, workflowData, paramsData, messages);
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };

        private It should_try_update_helpdesk_data = () =>
        {
            client.WasToldTo(x => x.X2AutoArchive2AM_Update((IDomainMessageCollection)messages, workflowData.HelpDeskQueryKey));
        };
    }
}