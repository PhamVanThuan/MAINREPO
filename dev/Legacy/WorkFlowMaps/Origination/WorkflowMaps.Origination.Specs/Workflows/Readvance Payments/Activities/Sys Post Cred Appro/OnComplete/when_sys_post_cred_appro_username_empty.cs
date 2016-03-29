using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Sys_Post_Cred_Appro.OnComplete
{
    [Subject("State => Sys_Post_Cred_Appro => OnComplete")]
    internal class when_sys_post_cred_appro_username_empty : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment workflowAssignment;
        private static ICommon common;
        private static string aduserName;
        private static string lastAdUserName;

        private Establish context = () =>
        {
            result = false;
            ((InstanceDataStub)instanceData).SourceInstanceID = 1;
            aduserName = @"SAHL\FLAppProcUser";
            lastAdUserName = @"SAHL\FLSUser";
            workflowAssignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(workflowAssignment);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            workflowAssignment.Expect(x => x.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct((IDomainMessageCollection)messages, (Int64)instanceData.SourceInstanceID, "FL Processor D", 157, out aduserName)).OutRef("");
            workflowAssignment.WhenToldTo(x => x.GetLastUserToWorkOnCaseAcrossInstances((IDomainMessageCollection)messages, instanceData.ID, (Int64)instanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments")).Return("");
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Sys_Post_Cred_Appro(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_get_applicationkey_from_source_instanceID = () =>
        {
            common.WasToldTo(x => x.GetApplicationKeyFromSourceInstanceID((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_get_case_name = () =>
        {
            common.WasToldTo(x => x.GetCaseName((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_not_reassign_case_to_user_by_process_adusername = () =>
        {
            workflowAssignment.WasNotToldTo(x => x.ReassignCaseToUserByProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, aduserName, 157, 857, "Setup Payment", SAHL.Common.Globals.Process.Origination));
        };

        private It should_reactive_user_or_round_robin_for_oskey_by_process = () =>
        {
            workflowAssignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_not_reassign_case_to_user_by_process_lastadusername = () =>
        {
            workflowAssignment.WasNotToldTo(x => x.ReassignCaseToUserByProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, lastAdUserName, 157, 857, "Setup Payment", SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}