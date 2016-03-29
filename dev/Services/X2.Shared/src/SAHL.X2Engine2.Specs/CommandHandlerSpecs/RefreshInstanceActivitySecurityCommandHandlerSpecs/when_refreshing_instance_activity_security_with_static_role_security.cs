using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.RefreshInstanceActivitySecurityCommandHandlerSpecs
{
    public class when_refreshing_instance_activity_security_with_static_role_security : WithFakes
    {
        private static AutoMocker<RefreshInstanceActivitySecurityCommandHandler> automocker = new NSubstituteAutoMocker<RefreshInstanceActivitySecurityCommandHandler>();
        private static RefreshInstanceActivitySecurityCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static ActivityDataModel activityDataModel;
        private static List<ActivityDataModel> activityDataModels = new List<ActivityDataModel>();
        private static ActivitySecurityDataModel activitySecurityDataModel;
        private static SecurityGroupDataModel securityGroupDataModel;
        private static StateDataModel stateDataModel;
        private static WorkFlowDataModel workflowDataModel;
        private static string roleName = "staticRoleName";
        private static InstanceDataModel instance;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            activitySecurityDataModel = new ActivitySecurityDataModel(1, 2);
            securityGroupDataModel = new SecurityGroupDataModel(false, roleName, "desc", null, null);
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activityDataModel = new ActivityDataModel(1, 2, "wf", 1, 10, 11, false, 1, null, "message", null, null, null, string.Empty, null, Guid.NewGuid());
            activityDataModels.Add(activityDataModel);
            stateDataModel = new StateDataModel(1, 1, "stateName", 1, true, 1, null, null, null);
            workflowDataModel = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storage", "offerkey", 1, "default sibject", null);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            command = new RefreshInstanceActivitySecurityCommand(instance, null, null);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetUserActivitiesForState((int)instance.StateID)).Return(activityDataModels);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetActivitySecurityForActivity(1)).Return(new List<ActivitySecurityDataModel> { activitySecurityDataModel });
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetSecurityGroup(2)).Return(securityGroupDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById((int)instance.StateID)).Return(stateDataModel);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflow(instance)).Return(workflowDataModel);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_delete_all_existing_instance_activity_security_rows = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.DeleteWhere<InstanceActivitySecurityDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        private It should_get_all_the_user_activities_coming_from_this_state = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetUserActivitiesForState((int)command.Instance.StateID));
        };

        private It should_get_activity_security_rows_for_each_activity = () =>
        {
            automocker.Get<IWorkflowDataProvider>().WasToldTo(x => x.GetActivitySecurityForActivity(activityDataModel.ID));
        };

        private It should_insert_a_new_instanceactivitysecurity_record = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Insert<InstanceActivitySecurityDataModel>(Arg.Any<IEnumerable<InstanceActivitySecurityDataModel>>()));
        };
    }
}