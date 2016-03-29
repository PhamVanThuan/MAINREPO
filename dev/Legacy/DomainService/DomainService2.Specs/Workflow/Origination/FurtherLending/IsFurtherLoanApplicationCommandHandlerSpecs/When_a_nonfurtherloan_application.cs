namespace DomainService2.Specs.Workflow.Origination.FurtherLending.IsFurtherLoanApplicationCommandHandlerSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(IsFurtherLoanApplicationCommandHandler), "Non-FurtherLoan Application")]
    public class When_a_nonfurtherloan_application : DomainServiceSpec<IsFurtherLoanApplicationCommand, IsFurtherLoanApplicationCommandHandler>
    {
        static IApplicationReadOnlyRepository appRepo;

        Establish context = () =>
        {
            appRepo = An<IApplicationReadOnlyRepository>();
            appRepo.WhenToldTo(x => x.GetApplicationTypeFromApplicationKey(Param<int>.IsAnything))
                .Return(IsFurtherLoanApplicationCommandHandler.furtherLoanApplicationTypeKey - 1);

            command = new IsFurtherLoanApplicationCommand(Param<int>.IsAnything);
            handler = new IsFurtherLoanApplicationCommandHandler(appRepo);
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