﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ReadvancePayments.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Readvance_Payments.Activities.Decline.OnComplete
{
    [Subject("Activity => Decline => OnComplete")]
    internal class when_previous_fl_supervisor_is_active : WorkflowSpecReadvancePayments
    {
        private static IWorkflowAssignment wfa;
        private static string message;
        private static string adUserName;
        private static bool result;
        private static List<string> roles = new List<string> { "FL Processor D", "FL Supervisor D", "FL Manager D" };

        private Establish context = () =>
        {
            ((InstanceDataStub)instanceData).SourceInstanceID = 1;
            result = false;
            adUserName = @"SAHL\FLSUser";
            wfa = An<IWorkflowAssignment>();
            wfa.WhenToldTo(x => x.GetLastUserToWorkOnCaseAcrossInstances(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>(), Param.IsAny<long>(),
                Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(adUserName);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(wfa);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Decline(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_deactivate_the_roles_for_the_instance = () =>
        {
            wfa.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey,
                roles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_assign_a_fl_supervisor = () =>
        {
            wfa.WasToldTo(x => x.ReassignCaseToUser((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, adUserName, 155, 921,
                "Rapid Decision"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}