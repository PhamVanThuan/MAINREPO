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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.DeleteActiveExternalActivityCommandHandlerSpecs
{
    public class when_deleting_an_active_external_activity : WithFakes
    {
        static AutoMocker<DeleteActiveExternalActivityCommandHandler> autoMocker = new NSubstituteAutoMocker<DeleteActiveExternalActivityCommandHandler>();
        private static IReadWriteSqlRepository readWriteSqlRepository;
        static int activeExternalActivityId = 1;
        static DeleteActiveExternalActivityCommand command;
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            command = new DeleteActiveExternalActivityCommand(activeExternalActivityId);
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
        };

        Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_delete_the_active_external_activity_record = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.DeleteByKey<ActiveExternalActivityDataModel, int>(activeExternalActivityId));
        };
    }
}