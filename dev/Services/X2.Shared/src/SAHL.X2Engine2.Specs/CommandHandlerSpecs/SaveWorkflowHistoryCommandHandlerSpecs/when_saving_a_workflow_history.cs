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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.SaveWorkflowHistoryCommandHandlerSpecs
{
    public class when_saving_a_workflow_history : WithFakes
    {
        private static AutoMocker<SaveWorkflowHistoryCommandHandler> automocker = new NSubstituteAutoMocker<SaveWorkflowHistoryCommandHandler>();
        private static SaveWorkflowHistoryCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static InstanceDataModel instance;
        private static int toStateID = 2, activityID = 3;
        private static string userWhoPerformedTheActivity = "bobthebuilder";
        private static DateTime activityTime = DateTime.Now;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
            {
                instance = new InstanceDataModel(12, 1, null, "name", "subject", "wfdp", 1, "creator", DateTime.Now, null, null, null, null, null, 9, null, null);
                readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
                command = new SaveWorkflowHistoryCommand(instance, toStateID, activityID, userWhoPerformedTheActivity, activityTime);
            };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_save_a_workflow_history = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.Insert<WorkFlowHistoryDataModel>(Arg.Is<WorkFlowHistoryDataModel>(c => c.InstanceID == instance.ID && c.StateID == toStateID && c.ActivityID == activityID)));
            };
    }
}