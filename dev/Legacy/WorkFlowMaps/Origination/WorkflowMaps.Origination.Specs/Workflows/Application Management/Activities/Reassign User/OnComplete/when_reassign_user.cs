using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Reassign_User.OnComplete
{
    [Subject("Activity => Reassign_User => OnComplete")]
    internal class when_reassign_user : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.ApplicationKey = 1;
            workflowData.IsFL = true;
            ((InstanceDataStub)instanceData).ID = 2;
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Reassign_User(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_update_assigned_user_in_idm = () =>
        {
            common.WasToldTo(x => x.UpdateAssignedUserInIDM((IDomainMessageCollection)messages,
                workflowData.ApplicationKey,
                workflowData.IsFL,
                instanceData.ID,
                "Application Management"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}