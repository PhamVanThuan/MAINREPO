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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.RemoveCloneInstanceCommandSpecs
{
    public class when_removing_a_clone_instance : WithFakes
    {
        private static AutoMocker<RemoveCloneInstanceCommandHandler> automocker = new NSubstituteAutoMocker<RemoveCloneInstanceCommandHandler>();
        private static RemoveCloneInstanceCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static long instanceID = 1;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            instanceID = 1231564L;
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            command = new RemoveCloneInstanceCommand(instanceID);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_call_deleteByKey_on_the_sqlRepository = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.DeleteByKey<InstanceDataModel, long>(instanceID));
        };
    }
}