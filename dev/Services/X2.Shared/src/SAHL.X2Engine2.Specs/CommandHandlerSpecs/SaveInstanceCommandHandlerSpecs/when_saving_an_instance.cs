using System;
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.SaveInstanceCommandHandlerSpecs
{
    public class when_saving_an_instance : WithFakes
    {
        private static AutoMocker<SaveInstanceCommandHandler> automocker = new NSubstituteAutoMocker<SaveInstanceCommandHandler>();
        private static SaveInstanceCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static InstanceDataModel instance;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
                command = new SaveInstanceCommand(instance);
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_update_the_instance = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.Update<InstanceDataModel>(Arg.Is<InstanceDataModel>(c => c.ID == instance.ID && c.StateID == instance.StateID)));
            };
    }
}