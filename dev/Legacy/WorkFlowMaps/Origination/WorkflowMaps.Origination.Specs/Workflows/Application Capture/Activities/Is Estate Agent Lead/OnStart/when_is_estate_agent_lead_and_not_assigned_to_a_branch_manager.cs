using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using System;
using WorkflowMaps.ApplicationCapture.Specs;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Is_Estate_Agent_Lead.OnStart
{
    [Subject("Activity => Is_Estate_Agent_Lead => OnStart")]
    internal class when_is_estate_agent_lead_and_not_assigned_to_a_branch_manager : WorkflowSpecApplicationCapture
    {
        private static Exception expectedException;
        private static string expectedAssignedTo;
        private static int expectedManagerOSKey;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;

        private Establish context = () =>
        {
            workflowData.LeadType = 0;
            expectedAssignedTo = string.Empty;
            assignment = An<IWorkflowAssignment>();
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, "Branch Consultant D", out expectedAssignedTo, workflowData.ApplicationKey, "Estate Agent Lead Assign"))
                .OutRef(expectedAssignedTo)
                .Return(true)
                .IgnoreArguments();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            appCap.WhenToldTo(x => x.IsEstateAgent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            expectedException = Catch.Exception(() => workflow.OnStartActivity_Is_Estate_Agent_Lead(instanceData, workflowData, paramsData, messages));
        };

        private It should_throw_exception = () =>
        {
            expectedException.Message.ShouldEqual("Estate Agent User but not a manager/admin/consultant, cannot assign");
        };
    }
}