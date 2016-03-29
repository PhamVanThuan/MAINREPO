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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CheckInstanceIsNotLockedCommandHandlerSpecs
{
    public class when_checking_an_instance_is_not_already_locked_and_it_is : WithFakes
    {
        private static AutoMocker<CheckInstanceIsNotLockedCommandHandler> autoMocker;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static CheckInstanceIsNotLockedCommand command;
        private static InstanceDataModel instance;
        private static Activity activity;
        private static string userName = "userName";
        private static InstanceDataModel instanceDataModel;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            instanceDataModel = new InstanceDataModel(1, null, "name", "subject", "", 1, "creator", System.DateTime.Now, null, null, DateTime.Now, "OtherUser", 1, 1, null, null);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(1, "Create Instance", null, "", 1, "Created", 1, false);
            command = new CheckInstanceIsNotLockedCommand(instance.ID, activity.ActivityID, userName);
            readWriteSqlRepository.WhenToldTo(x => x.SelectOne<InstanceDataModel>(Param.IsAny<string>(), Param.IsAny<object>())).Return(instanceDataModel);
            autoMocker = new NSubstituteAutoMocker<CheckInstanceIsNotLockedCommandHandler>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_check_the_instance_is_locked_for_the_current_instance_and_activity_for_user = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.SelectOne<InstanceDataModel>(Param.IsAny<string>(), Arg.Any<object>()));
        };

        private It should_return_false = () =>
        {
            command.Result.ShouldEqual(false);
        };
    }
}