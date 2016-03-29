using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.CreateNewRevisionCommandHandlerSpecs
{
    [Subject(typeof(CreateNewRevisionCommandHandler))]
    public class When_valid_application : DomainServiceSpec<CreateNewRevisionCommand, CreateNewRevisionCommandHandler>
    {
        protected static IApplicationRepository applicationRepository;
        protected static IApplication application;

        Establish context = () =>
            {
                applicationRepository = An<IApplicationRepository>();
                application = An<IApplication>();
                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

                command = new CreateNewRevisionCommand(Param.IsAny<int>());
                handler = new CreateNewRevisionCommandHandler(applicationRepository);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_create_revision = () =>
            {
                application.WasToldTo(x => x.CreateRevision());
            };

        It should_save_application = () =>
            {
                applicationRepository.WasToldTo(x => x.SaveApplication(application));
            };
    }
}