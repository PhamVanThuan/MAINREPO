using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.Activities.Client_Accepts.OnComplete
{
    [Subject("Activity => Client_Accepts => OnComplete")]
    internal class when_client_accepts : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static string message;
        private static IWorkflowAssignment assignment;
        private static ICommon common;
        private static List<string> dys;

        private Establish context = () =>
        {
            result = false;
            message = string.Empty;
            dys = new List<string>() { "Branch Consultant D", "Branch Admin D", "Branch Manager D" };

            assignment = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);

            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
        };

        private Because of = () =>
        {
            result = workflow.OnCompleteActivity_Client_Accepts(instanceData, workflowData, paramsData, messages, ref message);
        };

        private It should_perform_pricing_for_risk = () =>
        {
            common.WasToldTo(x => x.PricingForRisk((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_deactive_branch_consultant_d_branch_admin_d_and_branch_manager_d_users_for_instance = () =>
        {
            assignment.WasToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, dys, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}