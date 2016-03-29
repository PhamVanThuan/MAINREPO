using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Archive_FL.OnEnter
{
    [Subject("State => Archive_FL => OnEnter")]
    internal class when_archive_fl_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IFL fl;
        private static int appkey;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            appkey = workflowData.ApplicationKey;
            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Archive_FL(instanceData, workflowData, paramsData, messages);
        };

        private It should_complete_unhold_next_application = () =>
        {
            fl.WasToldTo(x => x.FLCompleteUnholdNextApplicationWhereApplicable((IDomainMessageCollection)messages, appkey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}