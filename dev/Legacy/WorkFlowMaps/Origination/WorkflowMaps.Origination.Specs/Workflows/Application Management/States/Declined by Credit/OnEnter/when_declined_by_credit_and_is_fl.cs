using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.Collections.Interfaces;
using WorkflowMaps.ApplicationManagement.Specs;
using X2DomainService.Interface.Common;
using X2DomainService.Interface.WorkflowAssignment;

namespace WorkflowMaps.Origination.Specs.Workflows.Application_Management.States.Declined_by_Credit.OnEnter
{
    [Subject("State => Declined_by_Credit => OnEnter")]
    internal class when_declined_by_credit_and_is_fl : WorkflowSpecApplicationManagement
    {
        private static IWorkflowAssignment client;
        private static ICommon common;
        private static bool result;
        private static int managerOSKey;

        private Establish context = () =>
        {
            result = false;
            workflowData.IsFL = true;
            managerOSKey = 1;
            client = An<IWorkflowAssignment>();
            domainServiceLoader.RegisterMockForType<IWorkflowAssignment>(client);
            common = An<ICommon>();
            domainServiceLoader.RegisterMockForType<ICommon>(common);
            client.WhenToldTo(x => x.GetBranchManagerOrgStructureKey(Param.IsAny<IDomainMessageCollection>(), Param.IsAny<int>())).Return(managerOSKey);
        };

        private Because of = () =>
        {
            result = workflow.OnEnter_Declined_by_Credit(instanceData, workflowData, paramsData, messages);
        };

        private It should_update_offer_status = () =>
        {
            common.WasToldTo(x => x.UpdateOfferStatus((IDomainMessageCollection)messages, workflowData.ApplicationKey, 5, -1));
        };

        private It should_reactivate_fl_processors = () =>
        {
            client.WasToldTo(x => x.ReactiveUserOrRoundRobinForOSKeyByProcess((IDomainMessageCollection)messages, "FL Processor D", workflowData.ApplicationKey, 157, instanceData.ID, "Declined By Credit",
                                                                              SAHL.Common.Globals.Process.Origination, (int)SAHL.Common.Globals.RoundRobinPointers.FLProcessor));
        };

        private It should_create_new_revision = () =>
        {
            common.WasToldTo(x => x.CreateNewRevision((IDomainMessageCollection)messages, workflowData.ApplicationKey));
        };

        private It should_return_true = () =>
        {
            result.ShouldBeTrue();
        };

        private It should_not_reactivate_branch_users_for_origination = () =>
        {
            client.WasNotToldTo(x => x.ReActivateBranchUsersForOrigination((IDomainMessageCollection)messages, instanceData.ID, workflowData.AppCapIID, workflowData.ApplicationKey,
                                                                           "Declined by Credit", SAHL.Common.Globals.Process.Origination));
        };

        private It should_not_assign_branch_manager = () =>
        {
            client.WasNotToldTo(x => x.AssignBranchManagerForOrgStrucKey((IDomainMessageCollection)messages, instanceData.ID, "Branch Manager D", managerOSKey, workflowData.ApplicationKey,
                                                                         "Declined By Credit", SAHL.Common.Globals.Process.Origination));
        };
    }
}