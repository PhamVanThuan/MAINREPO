using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetApplicationTypeCommandHandlerSpecs
{
    [Subject(typeof(GetApplicationTypeCommandHandler))]
    public class When_application_found_for_applicationkey : DomainServiceSpec<GetApplicationTypeCommand, GetApplicationTypeCommandHandler>
    {
        protected static int applicationTypeKey = 1;

        // Arrange
        Establish context = () =>
            {
                var application = An<IApplication>();
                var applicationType = An<IApplicationType>();

                applicationType.WhenToldTo(x => x.Key).Return(applicationTypeKey);
                application.WhenToldTo(x => x.ApplicationType).Return(applicationType);

                var applicationRepository = An<IApplicationRepository>();

                command = new GetApplicationTypeCommand(Param<int>.IsAnything);
                handler = new GetApplicationTypeCommandHandler(applicationRepository);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_return_applicationTypeKey = () =>
            {
                command.ApplicationTypeKeyResult.Equals(applicationTypeKey);
            };
    }
}