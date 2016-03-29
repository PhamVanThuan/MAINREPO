namespace DomainService2.Specs.Workflow.Origination.FurtherLending.IsFurtherAdvanceApplicationCommandSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(IsFurtherAdvanceApplicationCommandHandler), "Non-FurtherAdvance Application")]
    public class When_a_nonfurtheradvance_application : DomainServiceSpec<IsFurtherAdvanceApplicationCommand, IsFurtherAdvanceApplicationCommandHandler>
    {
        static IApplicationReadOnlyRepository appRepo;

        Establish context = () =>
        {
            appRepo = An<IApplicationReadOnlyRepository>();
            appRepo.WhenToldTo(x => x.GetApplicationTypeFromApplicationKey(Param<int>.IsAnything))
                .Return(IsFurtherLoanApplicationCommandHandler.furtherAdvanceApplicationTypeKey - 1);

            command = new IsFurtherAdvanceApplicationCommand(Param<int>.IsAnything);
            handler = new IsFurtherAdvanceApplicationCommandHandler(appRepo);
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