using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;

using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Testing.Fakes;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.CreateWorkListCommandHandlerSpecs
{
    public class when_creating_a_worklist : WithFakes
    {
        private static AutoMocker<CreateWorkListCommandHandler> autoMocker = new NSubstituteAutoMocker<CreateWorkListCommandHandler>();
        private static CreateWorkListCommand command;
        private static WorkListDataModel workListDataModel;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();

            command = new CreateWorkListCommand(new WorkListDataModel(1, "adusername", DateTime.Now, "message"));
        };

        private Because of = () =>
        {
            autoMocker.ClassUnderTest.HandleCommand(command, metadata);
        };

        private It should_create_a_worklist_entry_for_the_dynamic_role = () =>
        {
            readWriteSqlRepository.WasToldTo(x => x.Insert<WorkListDataModel>(Param.IsAny<IEnumerable<WorkListDataModel>>()));
        };
    }
}