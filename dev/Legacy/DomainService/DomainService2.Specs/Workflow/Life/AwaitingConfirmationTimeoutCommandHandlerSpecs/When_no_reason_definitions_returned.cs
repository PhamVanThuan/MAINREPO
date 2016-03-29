namespace DomainService2.Specs.Workflow.Life.AwaitingConfirmationTimeoutCommandHandlerSpecs
{
    using DomainService2.Specs.DomainObjects;
    using DomainService2.Workflow.Life;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.Common.Globals;

    [Subject(typeof(AwaitingConfirmationTimeoutCommandHandler))]
    public class When_no_reason_definitions_returned : DomainServiceSpec<AwaitingConfirmationTimeoutCommand, AwaitingConfirmationTimeoutCommandHandler>
    {
        static IStageDefinitionRepository stageDefinitionRepository;

        Establish context = () =>
        {
            IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();
            ILifeRepository lifeRepository = An<ILifeRepository>();
            IReasonRepository reasonRepository = An<IReasonRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            IReadOnlyEventList<IReasonDefinition> reasonDefinitions = new StubReadOnlyEventList<IReasonDefinition>();

            reasonRepository.WhenToldTo(x => x.GetReasonDefinitionsByReasonDescriptionKey(Param<ReasonTypes>.IsAnything, Param<int>.IsAnything)).Return(reasonDefinitions);

            command = new AwaitingConfirmationTimeoutCommand(Param<int>.IsAnything);
            handler = new AwaitingConfirmationTimeoutCommandHandler(organisationStructureRepository, lifeRepository, reasonRepository, stageDefinitionRepository, lookupRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_save_stage_transition = () =>
        {
            stageDefinitionRepository.WasNotToldTo(x => x.SaveStageTransition(Param<int>.IsAnything
                        , Param<int>.IsAnything
                        , Param<string>.IsAnything
                        , Param<string>.IsAnything
                        , Param<IADUser>.IsAnything));
        };
    }
}