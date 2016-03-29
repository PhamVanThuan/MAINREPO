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

using SAHL.X2Engine2.ViewModels;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CreateInstanceCommandHandlerSpecs
{
    public class when_creating_a_case_for_a_workflow_activity : WithFakes
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
        private static long sourceInstanceId = 99;
        private static int returnActivityId = 12;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static Dictionary<string, object> parameters = new Dictionary<string, object>();
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<CreateInstanceCommandHandler>();
            activity = new Activity(1, "activity", null, "", 1, "created", 1, false);
            command = new CreateInstanceCommand(processName, workflowName, activityName, userName, workflowProviderName, null, sourceInstanceId, returnActivityId);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            workflowDataModel = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storage", "offerkey", 11, "default sibject", null);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<Activity>(Param.IsAny<ActivityByNameAndWorkflowNameSqlStatement>())).Return(activity);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflowDataModel);
            parameters.Add("PrimaryKey", workflowDataModel.ID);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_the_create_activity_by_using_the_activity_name_and_the_workflow_name = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<Activity>(Param.IsAny<ActivityByNameAndWorkflowNameSqlStatement>()));
        };

        private It should_get_the_workflow_data_model_that_the_case_is_being_created_for = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Arg.Is<object>(anonymousObject => anonymousObject.CheckValue(parameters))));
        };

        private It should_create_an_instance_record_with_sourceinstance_and_returnactivity_set = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Insert<InstanceDataModel>(Arg.Is<InstanceDataModel>(i =>
                i.WorkFlowID == activity.WorkflowId &&
                i.ParentInstanceID == null &&
                i.ReturnActivityID == returnActivityId &&
                i.SourceInstanceID == sourceInstanceId &&
                i.StateID == null &&// its a create!
                i.WorkFlowProvider == workflowProviderName &&
                i.CreatorADUserName == userName)));
            //readWriteSqlRepository.WasToldTo(x => x.Insert<InstanceDataModel>(Param.IsAny<InstanceDataModel>()));
        };
    }
}