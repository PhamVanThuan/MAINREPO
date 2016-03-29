using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.DeleteInstanceCommandHandlerSpecs
{
    public class when_deleting_an_instance : WithFakes
    {
        private static AutoMocker<DeleteInstanceCommandHandler> automocker;
        private static DeleteInstanceCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static long instanceId = 99;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                automocker = new NSubstituteAutoMocker<DeleteInstanceCommandHandler>();
                command = new DeleteInstanceCommand(instanceId);
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_delete_the_instance = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.DeleteByKey<InstanceDataModel, long>(instanceId));
            };
    }
}