using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Signed_LOA_Review.OnEnter
{
    [Subject("State => Signed_LOA_Review => OnEnter")]
    internal class when_signed_loa_review_is_fl : WorkflowSpecApplicationManagement
    {
        private static bool result;
        private static IWorkflowAssignment client;
        private static List<string> roles;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            roles = new List<string>() { "Branch Consultant D", "Branch Admin D", "Branch Manager D", "New Business Processor D", "New Business Manager D" };
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Signed_LOA_Review(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_deactiveusersforinstance = () =>
        {
            client.WasNotToldTo(x => x.DeActiveUsersForInstanceAndProcess((IDomainMessageCollection)messages, instanceData.ID, workflowData.ApplicationKey, roles, SAHL.Common.Globals.Process.Origination));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}