using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.AssignConsultant.OnComplete
{
    [Subject("Activity => AssignConsultant => OnComplete")]
    internal class when_assignconsultant_is_not_estate_agent : WorkflowSpecApplicationCapture
    {
        private static string message;
        private static bool result;
        private static IWorkflowAssignment client;
        private static string dynamicRole;
        private static string aduserName;
        private static string expectedAduserName;
        private static string expectedStateName;

        private Establish context = () =>
        {
            message = String.Empty;
            dynamicRole = "Branch Consultant D";
            aduserName = String.Empty;
            workflowData.IsEA = false;
            expectedAduserName = @"SAHL\YolandaG";
            expectedStateName = "Check Consultant";
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            client.WhenToldTo(x => x.ResolveDynamicRoleToUserName(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<int>())).Return(expectedAduserName);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_AssignConsultant(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_set_case_owner_name_data_property_to_resolved_branch_consultant = () =>
        {
            workflowData.CaseOwnerName.ShouldBeTheSameAs(expectedAduserName);
        };

        private It should_resolve_branch_consultant_dynamic_role = () =>
        {
            client.WasToldTo(x => x.ResolveDynamicRoleToUserName((IDomainMessageCollection)messages, dynamicRole, instanceData.ID));
        };

        private It should_insert_commissionable_consultant = () =>
        {
            client.WasToldTo(x => x.InsertCommissionableConsultant((IDomainMessageCollection)messages, instanceData.ID, expectedAduserName, workflowData.ApplicationKey, expectedStateName));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}