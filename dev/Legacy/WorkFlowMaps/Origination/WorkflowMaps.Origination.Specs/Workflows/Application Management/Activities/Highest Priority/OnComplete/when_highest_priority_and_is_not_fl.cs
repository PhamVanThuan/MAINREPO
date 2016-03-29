using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Highest_Priority.OnComplete
{
    [Subject("Activity => Highest_Priority => OnComplete")]
    internal class when_highest_priority_and_is_not_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IFL fl;
        private static IWorkflowAssignment assignement;
        private static ICommon common;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            workflowData.IsFL = false;

            fl = An<IFL>();
            domainServiceLoader.RegisterMockForType<IFL>(fl);

            assignement = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignement);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Highest_Priority(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_not_deactive_users_for_instance_and_process = () =>
        {
            assignement.WasNotToldTo(x => x.DeActiveUsersForInstanceAndProcess(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<int>(),
                Param.IsAny<List<string>>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_not_round_robin_assign_other_fl_cases = () =>
        {
            assignement.WasNotToldTo(x => x.RoundRobinAndAssignOtherFLCases((IDomainMessageCollection)messages, workflowData.ApplicationKey, "FL Processor D", 157, instanceData.ID, "QA",
                (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_not_add_account_memo_message_on_reciept_of_application = () =>
        {
            fl.WasNotToldTo(x => x.AddAccountMemoMessageOnReceiptOfApplication(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<string>()));
        };

        private It should_not_update_assigned_user_in_idm = () =>
        {
            common.WasNotToldTo(x => x.UpdateAssignedUserInIDM(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<int>(),
                Param.IsAny<bool>(),
                Param.IsAny<long>(),
                Param.IsAny<string>()));
        };

        private It should_return_true = () =>
        {
            result.ShouldEqual(true);
        };
    }
}