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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.InsertScheduledActivityCommandHandlerSpecs
{
    public class when_inserting_a_scheduled_activity : WithFakes
    {
        private static AutoMocker<InsertScheduledActivityCommandHandler> automocker = new NSubstituteAutoMocker<InsertScheduledActivityCommandHandler>();
        private static InsertScheduledActivityCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static long instanceId = 12;
        private static long activityId = 3;
        private static DateTime activityTime = DateTime.Now;
        private static ActivityDataModel activityDataModel;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                activityDataModel = new ActivityDataModel(3, 2, "name", 2, 12, 13, false, 9, null, "message", null, null, null, string.Empty, null, Guid.NewGuid());
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
                command = new InsertScheduledActivityCommand(instanceId, activityTime, activityDataModel, "workflowProvider");
            };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_insert_a_scheduled_activity = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.Insert<ScheduledActivityDataModel>(Arg.Is<ScheduledActivityDataModel>(c => c.ActivityID == activityId && c.InstanceID == instanceId && c.Time == activityTime)));
            };
    }
}