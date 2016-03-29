using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Is_Estate_Agent_Consultant.OnStart
{
    [Subject("Activity => Is_Estate_Agent_Consultant => OnStart")]
    internal class when_lead_type_is_1_and_is_estate_agent_consultant : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string expectedAssignedTo;
        private static IApplicationCapture appCap;
        private static IWorkflowAssignment assignment;
        private static string assignedTo;
        private static int callCount;

        private Establish context = () =>
        {
            result = false;
            expectedAssignedTo = "ExpectedAssignedTo";
            workflowData.LeadType = 1;
            assignedTo = string.Empty;
            callCount = 0;
            workflowData.IsEA = false;
            workflowData.CaseOwnerName = string.Empty;
            appCap = An<IApplicationCapture>();
            ((InstanceDataStub)instanceData).CreatorADUserName = "ExpectedCreatorADUserName";
            appCap.WhenToldTo(x => x.IsEstateAgentInRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(true);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
            assignment = An<IWorkflowAssignment>();
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD, out assignedTo, workflowData.ApplicationKey, "Check Source And LeadType"))
                .OutRef(expectedAssignedTo)
                .Return(true)
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Estate_Agent_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_check_if_estate_agent_is_in_consultant_role = () =>
        {
            appCap.WasToldTo(x => x.IsEstateAgentInRole((IDomainMessageCollection)messages, instanceData.CreatorADUserName, SAHL.Common.OrganisationStructure.Consultant));
        };

        private It should_assign_creator_as_branch_consultant_d_dynamic_role = () =>
        {
            callCount.ShouldEqual(1);
        };

        private It should_set_case_owner_name_data_property = () =>
        {
            workflowData.CaseOwnerName.ShouldEqual(expectedAssignedTo);
        };

        private It should_set_isEA_data_property_to_true = () =>
        {
            workflowData.IsEA.ShouldBeTrue();
        };

        private It should_insert_commissionable_consultant = () =>
        {
            assignment.WasToldTo(x => x.InsertCommissionableConsultant((IDomainMessageCollection)messages,
                instanceData.ID,
                expectedAssignedTo,
                workflowData.ApplicationKey,
                "Estate Agent Consultant Assign"));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}