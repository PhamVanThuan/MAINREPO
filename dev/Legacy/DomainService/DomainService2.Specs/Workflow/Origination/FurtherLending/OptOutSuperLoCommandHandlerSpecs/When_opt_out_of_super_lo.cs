namespace DomainService2.Specs.Workflow.Origination.FurtherLending.OptOutSuperLoCommandHandlerSpecs
{
    using DomainService2.Workflow.Origination.FurtherLending;
    using Machine.Fakes;
    using Machine.Specifications;
    using SAHL.Common.BusinessModel.Interfaces.Repositories;

    [Subject(typeof(OptOutSuperLoCommandHandler))]
    public class When_opt_out_of_super_lo : DomainServiceSpec<OptOutSuperLoCommand, OptOutSuperLoCommandHandler>
    {
        static IApplicationRepository applicationRepository;

        Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();

            command = new OptOutSuperLoCommand(Param<int>.IsAnything, Param<string>.IsAnything);
            handler = new OptOutSuperLoCommandHandler(applicationRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_create_offer_roles_not_in_account = () =>
        {
            applicationRepository.WasToldTo(x => x.OptOutOfSuperLo(Param<int>.IsAnything, Param<string>.IsAnything, Param<int>.IsAnything));
        };
    }
}