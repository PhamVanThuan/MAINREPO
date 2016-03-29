using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationCapture.Specs;
using WorkflowMaps.Specs.Common;
using X2DomainService.Interface.Origination;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Capture.Activities.Direct_Consultant.OnStart
{
    [Subject("Activity => Direct_Consultant => OnStart")]
    internal class when_direct_consultant_where_lead_type_is_2 : WorkflowSpecApplicationCapture
    {
        private static bool result;
        private static int expectedManagerOSKey;
        private static IWorkflowAssignment assignment;
        private static IApplicationCapture appCap;
        private static string adUsername;

        private Establish context = () =>
        {
            workflowData.LeadType = 2;
            expectedManagerOSKey = 1;
            result = false;
            adUsername = @"SAHL\BCuser";
            workflowData.ApplicationKey = 2;
            ((InstanceDataStub)instanceData).ID = 3;
            assignment = An<IWorkflowAssignment>();
            assignment.WhenToldTo(x => x.GetBranchManagerOrgStructureKey(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<long>()))
                .Return(expectedManagerOSKey);
            assignment.Expect(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "Branch Consultant D", workflowData.ApplicationKey, 95, instanceData.ID, "Contact Client", SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.DirectConsultant)).Return(adUsername);
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(assignment);
            appCap = An<IApplicationCapture>();
            domainServiceLoader.RegisterMockForType<IApplicationCapture>(appCap);
        };

        private Because of = () =>
        {
            result = workflow.OnStartActivity_Direct_Consultant(instanceData, workflowData, paramsData, messages);
        };

        private It should_reactive_branch_consultant_d_user_or_round_robin_for_oskey_95 = () =>
        {
            assignment.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages,
                "Branch Consultant D",
                workflowData.ApplicationKey,
                95,
                instanceData.ID,
                "Contact Client",
                SAHL.Common.Globals.Process.Origination,
                (int)SAHL.Common.Globals.RoundRobinPointers.DirectConsultant));
        };

        private It should_not_reactive_branch_consultant_d_user_or_round_robin_for_oskey_94 = () =>
        {
            assignment.WasNotToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages,
                "Branch Consultant D",
                workflowData.ApplicationKey,
                94,
                instanceData.ID,
                "Contact Client",
                SAHL.Common.Globals.Process.Origination,
                (int)SAHL.Common.Globals.RoundRobinPointers.DirectConsultant));
        };

        private It should_not_get_branch_manager_org_structure_key = () =>
        {
            assignment.WasNotToldTo(x => x.GetBranchManagerOrgStructureKey((IDomainMessageCollection)messages, instanceData.ID));
        };

        private It should_not_assign_branch_manager_for_org_struc_key = () =>
        {
            assignment.WasNotToldTo(x => x.AssignBranchManagerForOrgStrucKey((IDomainMessageCollection)messages,
                instanceData.ID,
                "Branch Manager D",
                expectedManagerOSKey,
                workflowData.ApplicationKey,
                "Direct Consultant",
                SAHL.Common.Globals.Process.Origination));
        };

        private It should_Create_Commissionable_ConsultantRole = () =>
        {
            appCap.WasToldTo(x => x.CreateCommissionableConsultantRole((IDomainMessageCollection)messages, workflowData.ApplicationKey, adUsername));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };
    }
}