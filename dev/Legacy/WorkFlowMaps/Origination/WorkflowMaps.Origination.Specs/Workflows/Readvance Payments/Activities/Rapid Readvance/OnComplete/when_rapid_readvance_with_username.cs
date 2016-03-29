using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Rapid_Readvance.OnComplete
{
    [Subject("Activity => Rapid_Readvance => OnComplete")]
    internal class when_rapid_readvance_with_username : WorkflowSpecReadvancePayments
    {
        private static bool result;
        private static List<string> usersToDeactivate = new List<string>();
        private static IWorkflowAssignment assignmentClient;
        private static string adUserName = @"SAHL\FLAPPProcuser";
        private static string lastAdUserName = @"SAHL\FLSuser";

        private Establish context = () =>
        {
            result = false;
            ((InstanceDataStub)instanceData).SourceInstanceID = 1;
            assignmentClient = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignmentClient);
            usersToDeactivate.Add("FL Processor D");
            usersToDeactivate.Add("FL Supervisor D");
            usersToDeactivate.Add("FL Manager D");
            usersToDeactivate.Add("RV Manager D");
            usersToDeactivate.Add("RV Admin D");
            assignmentClient.Expect(x => x.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct((IDomainMessageCollection)messages, (Int64)instanceData.SourceInstanceID, "FL Processor D", 157, out adUserName)).OutRef(@"SAHL\FLAPPProcuser");
            assignmentClient.WhenToldTo(x => x.GetLastUserToWorkOnCaseAcrossInstances((IDomainMessageCollection)messages, instanceData.ID, (Int64)instanceData.SourceInstanceID, 921, "FL Supervisor D", "Readvance Payments")).Return(@"SAHL\FLSuser");
        };

        private Because of = () =>
        {
            var message = String.Empty;
            result = workflow.OnCompleteActivity_Rapid_Readvance(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_users_for_instance_and_process = () =>
        {
            assignmentClient.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, usersToDeactivate, SAHL.Common.Globals.Process.Origination));
        };

        private It should_reassign_case_by_process_aduserName = () =>
        {
            assignmentClient.WasToldTo(x => x.ReassignCaseToUserByProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, adUserName, 157, 857, "Setup Payment", SAHL.Common.Globals.Process.Origination));
        };

        private It should_not_Reactive_User_Or_Round_Robin_For_OSKey_By_Process = () =>
        {
            assignmentClient.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID, "Setup Payment", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_reassign_case_by_process_lastAduserName = () =>
        {
            assignmentClient.WasToldTo(x => x.ReassignCaseToUserByProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, lastAdUserName, 155, 921, "Rapid Decision", SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}