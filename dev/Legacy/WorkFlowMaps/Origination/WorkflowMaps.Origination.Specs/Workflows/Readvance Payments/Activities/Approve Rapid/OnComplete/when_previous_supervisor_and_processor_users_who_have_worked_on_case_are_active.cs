﻿using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Approve_Rapid.OnComplete
{
    [Subject("Activity => Approve_Rapid => OnComplete")]
    internal class when_previous_supervisor_and_processor_users_who_have_worked_on_case_are_active : WorkflowSpecReadvancePayments
    {
        private static ICommon commonClient;
        private static IWorkflowAssignment wfa;
        private static string message;
        private static string adUserName;
        private static string prevSupervisor;
        private static bool result;
        private static List<string> roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };

        private Establish context = () =>
            {
                ((InstanceDataStub)instanceData).SourceInstanceID = 1;
                prevSupervisor = @"SAHL\FLSUser";
                commonClient = An<ICommon>();
                adUserName = @"SAHL\FLAppProcUser";
                wfa = An<IWorkflowAssignment>();
                result = false;
                wfa.Expect(x => x.GetUsersWhoWorkedOnThisCaseInOtherMapsWhereUserIsStillActiveAndInSameOrgStruct((IDomainMessageCollection)messages, instanceData.SourceInstanceID.Value, "FL Processor D", 157, out adUserName))
                    .OutRef(@"SAHL\FLAppProcUser");
                wfa.WhenToldTo(x => x.GetLastUserToWorkOnCaseAcrossInstances(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(),
                    Param.IsAny<long>(), Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>())).Return(prevSupervisor);
                domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
                domainServiceLoader.RegisterMockForType<ICommon>(commonClient);
            };

        private Because of = () =>
            {
                result = workflow.OnCompleteActivity_Approve_Rapid(instanceData, workflowData, paramsData, messages, ref message);
            };

        private It should_return_true = () =>
            {
                result.ShouldBeTrue();
            };

        private It should_deactivate_the_further_lending_users_for_the_instance = () =>
            {
                wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles, SAHL.Common.Globals.Process.Origination));
            };

        private It should_update_the_offer_status_to_accepted = () =>
            {
                commonClient.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 1, 3));
            };

        private It should_assign_the_case_to_the_previous_FL_processor = () =>
            {
                wfa.WasToldTo(x => x.ReassignCaseToUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, adUserName,
                    157, 857, "Rapid Decision"));
            };

        private It should_assign_the_case_to_the_previous_FL_supervisor = () =>
            {
                wfa.WasToldTo(x => x.ReassignCaseToUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, prevSupervisor,
                    155, 921, "Rapid Decision"));
            };
    }
}