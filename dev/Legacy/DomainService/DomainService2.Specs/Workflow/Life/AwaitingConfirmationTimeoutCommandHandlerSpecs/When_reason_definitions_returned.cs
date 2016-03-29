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
    public class When_reason_definitions_returned : DomainServiceSpec<AwaitingConfirmationTimeoutCommand, AwaitingConfirmationTimeoutCommandHandler>
    {
        static IStageDefinitionRepository stageDefinitionRepository;
        static IReasonRepository reasonRepository;
        static ILifeRepository lifeRepository;

        Establish context = () =>
        {
            IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();
            lifeRepository = An<ILifeRepository>();

            ILookupRepository lookupRepository = An<ILookupRepository>();
            stageDefinitionRepository = An<IStageDefinitionRepository>();
            reasonRepository = An<IReasonRepository>();
            IReasonDefinition reasonDefinition = An<IReasonDefinition>();
            IReadOnlyEventList<IReasonDefinition> reasonDefinitions = new StubReadOnlyEventList<IReasonDefinition>(new IReasonDefinition[] { reasonDefinition });
            IADUser adUser = An<IADUser>();
            IReason reason = An<IReason>();
            ICallback callback = An<ICallback>();
            IGenericKeyType genericKeyType = An<IGenericKeyType>();

            adUser.WhenToldTo(x => x.ADUserName).Return(Param<string>.IsAnything);
            organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param<string>.IsAnything)).Return(adUser);
            reasonRepository.WhenToldTo(x => x.GetReasonDefinitionsByReasonDescriptionKey(Param<ReasonTypes>.IsAnything, Param<int>.IsAnything)).Return(reasonDefinitions);
            reasonDefinition.WhenToldTo(x => x.ReasonDescription.Description).Return(Param<string>.IsAnything);
            reasonRepository.WhenToldTo(x => x.CreateEmptyReason()).Return(reason);
            lifeRepository.WhenToldTo(x => x.CreateEmptyCallback()).Return(callback);
            lookupRepository.WhenToldTo(x => x.GenericKeyType.ObjectDictionary[Param<string>.IsAnything]).Return(genericKeyType);

            command = new AwaitingConfirmationTimeoutCommand(Param<int>.IsAnything);
            handler = new AwaitingConfirmationTimeoutCommandHandler(organisationStructureRepository, lifeRepository, reasonRepository, stageDefinitionRepository, lookupRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_save_stage_transition = () =>
        {
            stageDefinitionRepository.WasToldTo(x => x.SaveStageTransition(Param<int>.IsAnything
                        , Param<int>.IsAnything
                        , Param<string>.IsAnything
                        , Param<string>.IsAnything
                        , Param<IADUser>.IsAnything));
        };

        It should_create_empty_reason = () =>
        {
            reasonRepository.WasToldTo(x => x.CreateEmptyReason());
        };

        It should_save_reason = () =>
        {
            reasonRepository.WasToldTo(x => x.SaveReason(Param<IReason>.IsAnything));
        };

        It should_create_empty_callback = () =>
        {
            lifeRepository.WasToldTo(x => x.CreateEmptyCallback());
        };

        It should_save_callback = () =>
        {
            lifeRepository.WasToldTo(x => x.SaveCallback(Param<ICallback>.IsAnything));
        };
    }
}