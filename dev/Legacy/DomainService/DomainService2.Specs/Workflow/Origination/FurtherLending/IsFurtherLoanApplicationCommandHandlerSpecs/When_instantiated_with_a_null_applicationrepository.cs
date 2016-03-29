namespace DomainService2.Specs.Workflow.Origination.FurtherLending.IsFurtherLoanApplicationCommandHandlerSpecs
{
    using System;
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(IsFurtherLoanApplicationCommandHandler), "OnConstruction")]
    public class When_instantiated_with_a_null_applicationrepository : WithFakes
    {
        static IsFurtherLoanApplicationCommand command;
        static IsFurtherLoanApplicationCommandHandler handler;
        static IApplicationReadOnlyRepository appRepo;
        static Exception exception;

        Establish context = () =>
        {
            appRepo = null;
            command = new IsFurtherLoanApplicationCommand(Param<int>.IsAnything);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler = new IsFurtherLoanApplicationCommandHandler(appRepo));
        };

        It should_throw_a_ArgumentNullException = () =>
        {
            exception.ShouldBeOfType<ArgumentNullException>();
        };

        It should_set_the_exception_parametername_property = () =>
        {
            ArgumentNullException argEx = exception as ArgumentNullException;
            argEx.ParamName.ShouldEqual<string>(Strings.ArgApplicationReadOnlyRepository);
        };
    }
}