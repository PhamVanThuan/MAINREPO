using DomainService2.Workflow.HelpDesk;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.Workflow.HelpDesk.X2AutoArchive2AM_UpdateCommandHandlerSpecs
{
    [Subject(typeof(X2AutoArchive2AM_UpdateCommandHandler))]
    public class When_x2_auto_archive_2am_update : DomainServiceSpec<X2AutoArchive2AM_UpdateCommand, X2AutoArchive2AM_UpdateCommandHandler>
    {
        protected static IHelpDeskRepository helpDeskRepository;

        Establish context = () =>
        {
            helpDeskRepository = An<IHelpDeskRepository>();

            command = new X2AutoArchive2AM_UpdateCommand(Param.IsAny<int>());
            handler = new X2AutoArchive2AM_UpdateCommandHandler(helpDeskRepository);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_call_repository_method_to_archive_case = () =>
        {
            helpDeskRepository.WasToldTo(x => x.X2AutoArchive2AM_Update(Param.IsAny<int>()));
        };
    }
}