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
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CreateChildInstanceCommandHandlerSpecs
{
    public class when_creating_a_child_instance : WithFakes
    {
        private static AutoMocker<CreateChildInstanceCommandHandler> autoMocker = new NSubstituteAutoMocker<CreateChildInstanceCommandHandler>();
        private static CreateChildInstanceCommand command;
        private static Activity activity;
        private static WorkFlowDataModel workflowDataModel;
        private static InstanceDataModel parentInstance;
        private static string processName = "Process";
        private static string workflowName = "Workflow";
        private static string activityName = "Activity";
        private static string userName = "UserName";
        private static string workflowProviderName = "engine.Node1";
        private static IReadWriteSqlRepository readWriteSqlRepository;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            autoMocker = new NSubstituteAutoMocker<CreateChildInstanceCommandHandler>();
            activity = new Activity(1, "activity", null, "", 1, "created", 1, false);
            parentInstance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            command = new CreateChildInstanceCommand(parentInstance, activity, workflowProviderName, userName);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            workflowDataModel = new WorkFlowDataModel(1, 1, null, "workflow", DateTime.Now, "storage", "offerkey", 1, "default sibject", null);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(workflowDataModel);
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_get_and_workflowdatamodel = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<WorkFlowDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
        };

        private It should_create_an_instance_record_with_sourceinstance_and_returnactivity_set = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Insert<InstanceDataModel>(Arg.Is<InstanceDataModel>(i =>
                i.WorkFlowID == activity.WorkflowId &&
                i.ParentInstanceID == parentInstance.ID &&
                i.ReturnActivityID == null &&
                i.SourceInstanceID == null &&
                i.StateID == null &&// its a create!
                i.WorkFlowProvider == workflowProviderName &&
                i.CreatorADUserName == userName)));
        };

        private It should_get_an_instance_datamodel = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.SelectOne<InstanceDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
            };
    }
}