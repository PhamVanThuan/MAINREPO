using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Direct_Consultant.OnStart
{
    [Subject("Activity => Direct_Consultant => OnComplete")]
    internal class when_direct_consultant_where_lead_type_is_not_2 : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static int expectedManagerOSKey;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;

        private Establish context = () =>
        {
            result = true;
            workflowData.LeadType = 0;
            expectedManagerOSKey = 1;
            workflowData.ApplicationKey = 2;
            ((InstanceDataStub)instanceData).ID = 3;
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.GetBranchManagerOrgStructureKey(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>()))
                .Return(expectedManagerOSKey);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Direct_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_not_reactive_branch_consultant_d_user_or_round_robin = () =>
        {
            assignment.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<long>(),
                Param.IsAny<string>(),
                Param.IsAny<SAHL.Common.Globals.Process>(),
                Param.IsAny<int>()));
        };

        private It should_not_get_branch_manager_org_structure_key = () =>
        {
            assignment.WasNotToldTo(x => x.GetBranchManagerOrgStructureKey(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>()));
        };

        private It should_not_assign_branch_manager_for_org_struc_key = () =>
        {
            assignment.WasNotToldTo(x => x.AssignBranchManagerForOrgStrucKey(Param.IsAny<IDomainMessageCollection>(),
                Param.IsAny<long>(),
                Param.IsAny<string>(),
                Param.IsAny<int>(),
                Param.IsAny<int>(),
                Param.IsAny<string>(),
                Param.IsAny<SAHL.Common.Globals.Process>()));
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}