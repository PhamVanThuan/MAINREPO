using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.IsComcorpApplicationCommandHandlerSpecs
{
    [Subject(typeof(IsComcorpApplicationCommandHandler))]
    public class when_application_is_comcorp_loan : DomainServiceSpec<IsComcorpApplicationCommand, IsComcorpApplicationCommandHandler>
    {
        private static bool isComcorpLoan;
        private static IApplicationReadOnlyRepository applicationReadOnlyRepository;
        private static IApplication application;

        private Establish context = () =>
        {
            application = An<IApplication>();
            application.WhenToldTo(x => x.IsComcorp())
                       .Return(true);

            applicationReadOnlyRepository = An<IApplicationReadOnlyRepository>();
            applicationReadOnlyRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything))
                           .Return(application);

            command = new IsComcorpApplicationCommand(Param<int>.IsAnything);
            handler = new IsComcorpApplicationCommandHandler(applicationReadOnlyRepository);
        };

        private Because of = () =>
        {
            handler.Handle(messages, command);
            isComcorpLoan = command.Result;
        };

        private It result_should_be_true = () =>
        {
            isComcorpLoan.ShouldBeTrue();
        };
    }
}