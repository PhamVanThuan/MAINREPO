using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reinstate_NTU.OnStart
{
    [Subject("Activity => Reinstate_NTU => OnStart")]
    internal class when_reinstate_ntu : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IApplicationManagement appMan;

        private Establish context = () =>
        {
            result = true;
            workflowData.ApplicationKey = 1;
            workflowData.PreviousState = "PreviousStateTest";
            ((ParamsDataStub)paramsData).IgnoreWarning = true;
            ((ParamsDataStub)paramsData).ADUserName = "ADUserNameTest";
            appMan = An<IApplicationManagement>();
            domainServiceLoader.RegisterMockForType<IApplicationManagement>(appMan);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Reinstate_NTU(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_reinstate_allowed_by_user = () =>
        {
            appMan.WasToldTo(x => x.CheckIfReinstateAllowedByUser((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                workflowData.PreviousState,
                paramsData.IgnoreWarning,
                paramsData.ADUserName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}