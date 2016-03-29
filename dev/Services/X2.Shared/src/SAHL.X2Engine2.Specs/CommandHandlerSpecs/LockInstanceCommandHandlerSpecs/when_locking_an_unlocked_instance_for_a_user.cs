using System;
using Machine.Fakes;
using Machine.Specifications;
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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.LockInstanceCommandHandlerSpecs
{
    public class when_locking_an_unlocked_instance_for_a_user : WithFakes
    {
        private static InstanceDataModel instanceDataModel;
        private static long instanceId;
        private static Activity activity;
        private static AutoMocker<LockInstanceCommandHandler> automocker = new NSubstituteAutoMocker<LockInstanceCommandHandler>();
        private static LockInstanceCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            instanceId = 12;
            activity = new Activity(1, "activity", 1, "state1", 2, "state2", 1, false);
            instanceDataModel = new InstanceDataModel(instanceId, 1, null, "name", "subject", string.Empty, 1, "creator", DateTime.Now, null, null, null, null, null, null, null, null);
            command = new LockInstanceCommand(instanceDataModel, "userName", activity);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_update_the_instance = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Update<InstanceDataModel>(instanceDataModel));
        };
    }
}