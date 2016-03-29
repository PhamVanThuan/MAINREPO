using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Not_Direct_Consultant.OnStart
{
    [Subject("Activity => Not_Direct_Consultant => OnStart")]
    internal class when_lead_type_is_1_is_not_estate_agent_other_and_assigned_to_is_empty : WorkflowSpecApplicationCapture
    {
        private static bool result;

        private Establish context = () =>
        {
            result = false;
            workflowData.LeadType = 1;
            workflowData.CaseOwnerName = "abc";

            var client = An<IApplicationCapture>();
            client.WhenToldTo(x => x.IsEstateAgentInRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(client);

            var assignment = An<IWorkflowAssignment>();
            string assignedTo;
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD, out assignedTo, workflowData.ApplicationKey, "UserAssignCheck"))
                .OutRef(string.Empty)
                .Return(true)
                .IgnoreArguments();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Not_Direct_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_set_case_owner_name_data_property_to_empty_string = () =>
        {
            workflowData.CaseOwnerName.ShouldBeEmpty();
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}