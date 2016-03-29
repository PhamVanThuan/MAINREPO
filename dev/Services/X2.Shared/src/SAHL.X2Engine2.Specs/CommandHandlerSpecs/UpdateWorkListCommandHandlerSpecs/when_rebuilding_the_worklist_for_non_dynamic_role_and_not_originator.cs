using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UpdateWorkListCommandHandlerSpecs
{
    public class when_rebuilding_the_worklist_and_not_dynamic : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RebuildUserWorkListCommandHandler> autoMocker;

        private static RebuildUserWorkListCommand rebuildUserWorklistCommand;
        private static IX2Map map;
        private static InstanceDataModel instance;
        private static SecurityGroupDataModel securityGroup;
        private static StateWorkListDataModel stateWorkList;
        private static StateWorkListDataModel[] stateWorkLists;
        private static string activityMessage;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RebuildUserWorkListCommandHandler>();

            activityMessage = "activityMessage";
            map = An<IX2Map>();
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instance.ID)).Return(instance);

            securityGroup = new SecurityGroupDataModel(false, "name", "description", null, null);

            stateWorkList = new StateWorkListDataModel(int.MinValue, int.MinValue);
            stateWorkLists = new StateWorkListDataModel[] { stateWorkList };

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateWorkList(instance.StateID.Value)).Return(stateWorkLists);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSecurityGroup(stateWorkList.SecurityGroupID)).Return(securityGroup);

            rebuildUserWorklistCommand = new RebuildUserWorkListCommand(instance.ID, activityMessage);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(rebuildUserWorklistCommand, metadata);
        };

        private It should_clear_the_existing_worklist_for_the_instance = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Is<ClearWorkListCommand>(command => command.InstanceID == instance.ID), metadata));
        };

        private It should_add_a_new_worklist_entry_for_user_in_security_group = () =>
        {
            autoMocker.Get<IX2ServiceCommandRouter>().WasToldTo(x => x.HandleCommand(Arg.Any<CreateWorkListCommand>(), metadata));
        };
    }
}