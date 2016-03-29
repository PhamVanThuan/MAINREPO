using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UpdateWorkListCommandHandlerSpecs
{
    public class when_rebuilding_the_worklist_with_no_security_group_for_state : WithFakes
    {
        private static StructureMap.AutoMocking.AutoMocker<RebuildUserWorkListCommandHandler> autoMocker;

        private static RebuildUserWorkListCommand rebuildUserWorklistCommand;
        private static IX2Map map;
        private static InstanceDataModel instance;
        private static StateWorkListDataModel stateWorkList;
        private static StateWorkListDataModel[] stateWorkLists;
        private static Exception exception;
        private static string activityMessage;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RebuildUserWorkListCommandHandler>();

            activityMessage = "username";
            map = An<IX2Map>();
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(instance.ID)).Return(instance);

            stateWorkList = new StateWorkListDataModel(int.MinValue, int.MinValue);
            stateWorkLists = new StateWorkListDataModel[] { stateWorkList };

            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateWorkList(instance.StateID.Value)).Return(stateWorkLists);
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSecurityGroup(stateWorkList.SecurityGroupID)).Return((SecurityGroupDataModel)null);

            rebuildUserWorklistCommand = new RebuildUserWorkListCommand(instance.ID, activityMessage);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() => { autoMocker.ClassUnderTest.HandleCommand(rebuildUserWorklistCommand, metadata); });
        };

        private It should_throw_an_exception_with_message_security_group_not_found_for_workflow = () =>
        {
            exception.Message.ShouldBeEqualIgnoringCase(String.Format("The security group ({0}) could not be found", stateWorkList.SecurityGroupID));
        };
    }
}