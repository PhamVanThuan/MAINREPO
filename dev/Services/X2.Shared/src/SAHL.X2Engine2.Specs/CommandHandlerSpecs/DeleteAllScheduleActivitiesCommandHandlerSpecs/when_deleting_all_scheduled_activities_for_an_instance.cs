using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.ViewModels.SqlStatement;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.DeleteAllScheduleActivitiesCommandHandlerSpecs
{
    public class when_deleting_all_scheduled_activities_for_an_instance : WithFakes
    {
        private static AutoMocker<DeleteAllScheduleActivitiesCommandHandler> automocker;
        private static DeleteAllScheduleActivitiesCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static long InstanceId = 16;
        private static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                command = new DeleteAllScheduleActivitiesCommand(InstanceId);
                automocker = new NSubstituteAutoMocker<DeleteAllScheduleActivitiesCommandHandler>();
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_ = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.DeleteWhere<ScheduledActivityDataModel>(Arg.Is<string>(match1 => match1 == "InstanceID=@InstanceID"), Arg.Any<object>()));
        };
    }
}