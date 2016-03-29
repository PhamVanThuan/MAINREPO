using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CreateContextualDataCommandHandlerSpecs
{
    public class when_creating_contextual_data : WithFakes
    {
        private static CreateContextualDataCommand command;
        private static AutoMocker<CreateContextualDataCommandHandler> automocker = new NSubstituteAutoMocker<CreateContextualDataCommandHandler>();
        private static IX2ContextualDataProvider contextualData;
        private static InstanceDataModel instance;
        private static Dictionary<string, string> mapVariables = new Dictionary<string, string>();
        static ServiceRequestMetadata metadata;

        Establish context = () =>
        {
            contextualData = An<IX2ContextualDataProvider>();
            instance = new InstanceDataModel(1231564, 1, null, "Test", "Test", "Test", 1, "TestUser", DateTime.Now, DateTime.Now,
                null, null, "TestUser", null, null, null, null);
            mapVariables.Add("ApplicationKey", "12345");
            command = new CreateContextualDataCommand(instance, contextualData, mapVariables);
        };

        Because of = () =>
        {
            automocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        It should_call_insert_data_on_the_contextual_data_provider_using_the_instance_and_map_variables_from_the_command = () =>
        {
            contextualData.WasToldTo(x => x.InsertData(instance.ID, mapVariables));
        };
    }
}