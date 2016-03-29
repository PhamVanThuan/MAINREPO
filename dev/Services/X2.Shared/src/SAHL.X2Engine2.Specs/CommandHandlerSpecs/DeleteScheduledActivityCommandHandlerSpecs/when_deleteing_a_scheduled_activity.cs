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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.DeleteScheduledActivityCommandHandlerSpecs
{
    public class when_deleteing_a_scheduled_activity : WithFakes
    {
        private static long instanceId = 78;
        private static int activityId = 13;
        private static AutoMocker<DeleteScheduledActivityCommandHandler> automocker = new NSubstituteAutoMocker<DeleteScheduledActivityCommandHandler>();
        private static DeleteScheduledActivityCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
                command = new DeleteScheduledActivityCommand(instanceId, activityId);
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_delete_a_scheduled_activity_with_an_instanceId_and_activityId = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.DeleteWhere<ScheduledActivityDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
            };
    }
}