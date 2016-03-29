using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;

using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CreateInstanceCommandHandlerSpecs
{
    public class when_creating_an_instance_with_a_parent : WithFakes
    {
        private static AutoMocker<CreateInstanceCommandHandler> autoMocker = new NSubstituteAutoMocker<CreateInstanceCommandHandler>();
        private static CreateInstanceCommand command;
        private static Activity activity;
        private static WorkFlowDataModel workflowDataModel;
        private static string processName = "Process";
        private static string workflowName = "Workflow";
        private static string activityName = "Activity";
        private static string userName = "UserName";
        private static string workflowProviderName = "engine.Node1";
        private static long parentInstanceId = 99;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<CreateInstanceCommandHandler>();
            activity = new Activity(1, "activity", null, "", 1, "created", 1, false);
            command = new CreateInstanceCommand(processName, workflowName, activityName, userName, workflowProviderName, parentInstanceId);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            workflowDataModel = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storage", "offerkey", 1, "default sibject", null);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<Activity>(Param.IsAny<ActivityByNameAndWorkflowNameSqlStatement>())).Return(activity);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflowDataModel);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_and_activity = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<Activity>(Param.IsAny<ActivityByNameAndWorkflowNameSqlStatement>()));
        };

        private It should_get_and_workflowdatamodel = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        private It should_create_an_instance_record_with_parent_set = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Insert<InstanceDataModel>(Arg.Is<InstanceDataModel>(i =>
                i.WorkFlowID == activity.WorkflowId &&
                i.ParentInstanceID == parentInstanceId &&
                i.ReturnActivityID == null &&
                i.SourceInstanceID == null &&
                i.StateID == null &&// its a create!
                i.WorkFlowProvider == workflowProviderName &&
                i.CreatorADUserName == userName)));
            //readWriteSqlRepository.WasToldTo(x => x.Insert<InstanceDataModel>(Param.IsAny<InstanceDataModel>()));
        };
    }
}