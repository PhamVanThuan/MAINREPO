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

namespace SAHL.X2Engine2.Specs.NodeHandlerSpecs.ExternalHandlerSpecs
{
    public class when_queueing_up_external_activities_where_activiy_does_not_raise_an_external_actvitity : WithFakes
    {
        private static AutoMocker<QueueUpExternalActivitiesCommandHandler> autoMocker;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static QueueUpExternalActivitiesCommand command;
        private static InstanceDataModel instance;
        private static Activity activity;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            readWriteSqlRepository = An<IReadWriteSqlRepository>();
            instance = new InstanceDataModel(9, 1, null, "name", "subject", "workflowProvider", 1, "CreatorADUserName", DateTime.Now, null, null, null, null, null, null, null, null);
            activity = new Activity(1, "DoSomething", 1, "state", 2, "nextState", 1, false);
            activity.WorkflowId = 1;
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            command = new QueueUpExternalActivitiesCommand(instance, activity);
            autoMocker = new NSubstituteAutoMocker<QueueUpExternalActivitiesCommandHandler>();
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_not_create_an_active_external_activity_record = () =>
        {
            readWriteSqlRepository.WasNotToldTo(x => x.Insert<ActiveExternalActivityDataModel>(Arg.Any<ActiveExternalActivityDataModel>()));
        };
    }
}