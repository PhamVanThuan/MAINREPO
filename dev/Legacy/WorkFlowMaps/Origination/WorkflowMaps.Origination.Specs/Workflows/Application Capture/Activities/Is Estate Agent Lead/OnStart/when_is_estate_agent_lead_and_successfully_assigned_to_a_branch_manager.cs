using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Is_Estate_Agent_Lead.OnStart
{
    [Subject("Activity => Is_Estate_Agent_Lead => OnStart")]
    internal class when_is_estate_agent_lead_and_successfully_assigned_to_a_branch_manager : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string expectedAssignedTo;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;
        private static int callCount;

        private Establish context = () =>
        {
            result = false;
            callCount = 0;
            workflowData.LeadType = 0;
            expectedAssignedTo = "ExpectedAssignedTo";
            ((InstanceDataStub)instanceData).CreatorADUserName = "ExpectedCreatorADUserName";
            ((InstanceDataStub)instanceData).ID = 1;
            workflowData.ApplicationKey = 2;
            string assignedTo;
            assignment = An<IWorkflowAssignment>();
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, "Branch Consultant D", out assignedTo, workflowData.ApplicationKey, "Estate Agent Lead Assign"))
                .OutRef(string.Empty)
                .Return(false)
                .IgnoreArguments()
                .Repeat.Twice();
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, "Branch Manager D", out assignedTo, workflowData.ApplicationKey, "Estate Agent Lead Assign"))
                .OutRef(expectedAssignedTo)
                .Return(false)
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            appCap.WhenToldTo(x => x.IsEstateAgent(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Estate_Agent_Lead(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_is_estate_agent = () =>
        {
            appCap.WasToldTo(x => x.IsEstateAgent((IDomainMessageCollection)messages, instanceData.CreatorADUserName));
        };

        private It should_assign_creator_as_branch_manager = () =>
        {
            callCount.ShouldEqual(1);
        };

        private It should_set_case_owner_name_property_to_assigned_to = () =>
        {
            workflowData.CaseOwnerName.ShouldEqual(expectedAssignedTo);
        };

        private It should_set_isea_property_to_true = () =>
        {
            workflowData.IsEA.ShouldBeTrue();
        };

        private It should_insert_commissionable_consultant = () =>
        {
            assignment.WasToldTo(x => x.InsertCommissionableConsultant((IDomainMessageCollection)messages,
                instanceData.ID,
                expectedAssignedTo,
                workflowData.ApplicationKey,
                "Estate Agent Lead Assign"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}