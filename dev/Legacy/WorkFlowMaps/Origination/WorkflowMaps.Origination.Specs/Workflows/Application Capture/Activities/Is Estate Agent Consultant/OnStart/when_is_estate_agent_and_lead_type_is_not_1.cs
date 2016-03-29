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
    internal class when_is_estate_agent_and_lead_type_is_not_1 : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static string expectedAssignedTo;
        private static IApplicationCapture appCap;
        private static IWorkflowAssignment assignment;
        private static string assignedTo;
        private static int callCount;

        private Establish context = () =>
        {
            result = true;
            expectedAssignedTo = "ExpectedAssignedTo";
            workflowData.LeadType = 2;
            assignedTo = string.Empty;
            callCount = 0;
            workflowData.IsEA = false;
            workflowData.CaseOwnerName = string.Empty;
            appCap = An<IApplicationCapture>();
            ((InstanceDataStub)instanceData).CreatorADUserName = "ExpectedCreatorADUserName";
            appCap.WhenToldTo(x => x.IsEstateAgentInRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>()))
                .Return(false);
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
            assignment = An<IWorkflowAssignment>();
            assignment.Expect(x => x.AssignCreateorAsDynamicRole((IDomainMessageCollection)messages, instanceData.ID, SAHL.Common.ApplicationCapture.WorkflowRoles.BranchConsultantD, out assignedTo, workflowData.ApplicationKey, "Check Source And LeadType"))
                .OutRef(expectedAssignedTo)
                .Return(true)
                .IgnoreArguments()
                .WhenCalled((y) => { callCount++; });
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Is_Estate_Agent_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_check_if_estate_agent_has_branch_consultant_d_role = () =>
        {
            appCap.WasNotToldTo(x => x.IsEstateAgentInRole(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_not_assign_creator_as_branch_consultant_d_dynamic_role = () =>
        {
            callCount.ShouldEqual(0);
        };

        private It should_not_set_case_owner_name_data_property = () =>
        {
            workflowData.CaseOwnerName.ShouldBeEmpty();
        };

        private It should_not_set_isEA_data_property_to_true = () =>
        {
            workflowData.IsEA.ShouldBeFalse();
        };

        private It should_not_insert_commissionable_consultant = () =>
        {
            assignment.WasNotToldTo(x => x.InsertCommissionableConsultant(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<string>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}