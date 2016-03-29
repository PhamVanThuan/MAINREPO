using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2;
using SAHL.Core.X2.Factories;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.UserRequestStartActivityWithoutSplitCommandHandlerSpecs
{
    public class when_the_activity_onStart_returns_false : WithFakes
    {
        private static AutoMocker<UserRequestStartActivityWithoutSplitCommandHandler> automocker = new NSubstituteAutoMocker<UserRequestStartActivityWithoutSplitCommandHandler>();
        private static UserRequestStartActivityWithoutSplitCommand command;
        private static ISystemMessageCollection expectedMessages;
        private static ISystemMessageCollection returnedMessages;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            expectedMessages = new SystemMessageCollection();
            expectedMessages.AddMessage(An<ISystemMessage>());
            var instanceId = 123465789L;
            var userName = @"SAHL\Test";
            Activity activity = Helper.GetActivity();
            var mapVariables = new Dictionary<string, string>();
            command = new UserRequestStartActivityWithoutSplitCommand(instanceId, userName, activity, false, mapVariables);
            var instance = Helper.GetInstanceDataModel(instanceId);
            var workflow = Helper.GetWorkflowDataModel();
            var state = Helper.GetStateDataModel();
            var process = An<IX2Process>();
            var map = An<IX2Map>();

            var contextualDataProvider = An<IX2ContextualDataProvider>();
            automocker.Get<IMessageCollectionFactory>().WhenToldTo(x => x.CreateEmptyCollection()).Return(expectedMessages);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetInstanceDataModel(Param.IsAny<long>())).Return(instance);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowById(Param.IsAny<int>())).Return(workflow);
            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetStateById(Param.IsAny<int>())).Return(state);

            automocker.Get<IX2ProcessProvider>().WhenToldTo(x => x.GetProcessForInstance(Param.IsAny<long>())).Return(process);

            process.WhenToldTo(x => x.GetWorkflowMap(Param.IsAny<string>())).Return(map);
            map.WhenToldTo(x => x.GetContextualData(Param.IsAny<long>())).Return(contextualDataProvider);

            automocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetWorkflowName(Param.IsAny<InstanceDataModel>())).Return(workflow.Name);

            map.WhenToldTo(x => x.StartActivity(Param.IsAny<InstanceDataModel>(),
                Param.IsAny<IX2ContextualDataProvider>(), Param.IsAny<IX2Params>(), Param.IsAny<ISystemMessageCollection>())).Return(false);

            automocker.Get<IX2ServiceCommandRouter>().
                WhenToldTo(x => x.HandleCommand(Arg.Is<SaveContextualDataCommand>(c => c.InstanceId == command.InstanceId), metadata)
                    .Returns(new SystemMessageCollection()));

            automocker.Get<IX2ServiceCommandRouter>().
                WhenToldTo(x => x.HandleCommand(Arg.Is<SaveInstanceCommand>(c => c.Instance.ID == instance.ID), metadata)
                    .Returns(new SystemMessageCollection()));
        };

        Because of = () =>
        {
            returnedMessages = automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_return_messages = () =>
        {
            returnedMessages.AllMessages.Count().ShouldEqual(1);
        };
    }
}