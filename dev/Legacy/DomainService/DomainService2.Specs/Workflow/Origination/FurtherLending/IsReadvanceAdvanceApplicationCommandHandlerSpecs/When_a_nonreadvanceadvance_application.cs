namespace DomainService2.Specs.Workflow.Origination.FurtherLending.IsReadvanceAdvanceApplicationCommandHandlerSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(IsReadvanceAdvanceApplicationCommandHandler), "Non-ReadvanceAdvance Application")]
    public class When_a_nonreadvanceadvance_application : DomainServiceSpec<IsReadvanceAdvanceApplicationCommand, IsReadvanceAdvanceApplicationCommandHandler>
    {
        static IApplicationReadOnlyRepository appRepo;

        Establish context = () =>
        {
            appRepo = An<IApplicationReadOnlyRepository>();
            appRepo.WhenToldTo(x => x.GetApplicationTypeFromApplicationKey(Param<int>.IsAnything))
                .Return(IsReadvanceAdvanceApplicationCommandHandler.readvanceAdvanceApplicationTypeKey - 1);

            command = new IsReadvanceAdvanceApplicationCommand(Param<int>.IsAnything);
            handler = new IsReadvanceAdvanceApplicationCommandHandler(appRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}