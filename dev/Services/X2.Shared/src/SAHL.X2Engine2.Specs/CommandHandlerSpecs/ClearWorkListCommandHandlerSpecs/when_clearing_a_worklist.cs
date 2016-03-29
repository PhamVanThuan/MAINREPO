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

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.ClearWorkListCommandHandlerSpecs
{
    public class when_clearing_a_worklist : WithFakes
    {
        private static AutoMocker<ClearWorkListCommandHandler> automocker = new NSubstituteAutoMocker<ClearWorkListCommandHandler>();
        private static ClearWorkListCommand command;
        private static IReadWriteSqlRepository readWriteSqlRepository;
        private static long instanceId = 12;
        static ServiceRequestMetadata metadata;

        private Establish context = () =>
        {
            readWriteSqlRepository = MockRepositoryProvider.GetReadWriteRepository();
            command = new ClearWorkListCommand(instanceId);
        };

        private Because of = () =>
            {
                automocker.ClassUnderTest.HandleCommand(command, metadata);
            };

        private It should_remove_existing_worklist_rows_for_this_instance = () =>
            {
                readWriteSqlRepository.WasToldTo(x => x.DeleteWhere<WorkListDataModel>(Param.IsAny<string>(), Param.IsAny<object>()));
            };
    }
}