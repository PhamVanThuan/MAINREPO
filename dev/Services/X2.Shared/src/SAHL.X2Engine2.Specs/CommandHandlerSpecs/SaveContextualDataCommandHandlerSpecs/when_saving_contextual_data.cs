using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.SaveContextualDataCommandHandlerSpecs
{
    public class when_saving_contextual_data : WithFakes
    {
        private static AutoMocker<SaveContextualDataCommandHandler> automocker = new NSubstituteAutoMocker<SaveContextualDataCommandHandler>();
        private static SaveContextualDataCommand command;
        private static IX2ContextualDataProvider contextualDataProvider;
        private static IX2ContextualDataProvider contextualData;
        private static long instanceID;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            contextualData = An<IX2ContextualDataProvider>();
            instanceID = 1231564L;
            command = new SaveContextualDataCommand(contextualData, instanceID);
        };

        private Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_call_save_data_on_the_contextual_data_provider_using_the_instanceID_from_the_command = () =>
        {
            contextualData.WasToldTo(x => x.SaveData(instanceID));
        };
    }
}