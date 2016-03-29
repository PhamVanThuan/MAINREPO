using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetApplicationStatusCommandHandlerSpecs
{
    [Subject(typeof(GetApplicationStatusCommandHandler))]
    public class When_application_found_for_applicationkey : DomainServiceSpec<GetApplicationStatusCommand, GetApplicationStatusCommandHandler>
    {
        protected static int applicationStatusKey = 1;

        Establish context = () =>
            {
                var application = An<IApplication>();
                var applicationStatus = An<IApplicationStatus>();

                applicationStatus.WhenToldTo(x => x.Key).Return(applicationStatusKey);
                application.WhenToldTo(x => x.ApplicationStatus).Return(applicationStatus);

                var applicationRepository = An<IApplicationRepository>();

                command = new GetApplicationStatusCommand(Param<int>.IsAnything);
                handler = new GetApplicationStatusCommandHandler(applicationRepository);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            };

        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        It should_return_applicationStatusKey = () =>
            {
                command.ApplicationStatusKeyResult.Equals(applicationStatusKey);
            };
    }
}