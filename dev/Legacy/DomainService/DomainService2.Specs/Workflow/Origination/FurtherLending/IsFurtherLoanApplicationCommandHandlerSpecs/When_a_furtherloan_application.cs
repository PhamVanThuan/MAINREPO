namespace DomainService2.Specs.Workflow.Origination.FurtherLending.IsFurtherLoanApplicationCommandSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(IsFurtherLoanApplicationCommandHandler), "FurtherLoan Application")]
    public class When_a_furtherloan_application : DomainServiceSpec<IsFurtherLoanApplicationCommand, IsFurtherLoanApplicationCommandHandler>
    {
        static IApplicationReadOnlyRepository appRepo;

        Establish context = () =>
        {
            appRepo = An<IApplicationReadOnlyRepository>();
            appRepo.WhenToldTo(x => x.GetApplicationTypeFromApplicationKey(Param<int>.IsAnything))
                .Return(IsFurtherLoanApplicationCommandHandler.furtherLoanApplicationTypeKey);

            command = new IsFurtherLoanApplicationCommand(Param<int>.IsAnything);
            handler = new IsFurtherLoanApplicationCommandHandler(appRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}