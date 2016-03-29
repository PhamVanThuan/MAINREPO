using DomainService2.SharedServices.Common;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace DomainService2.Specs.SharedServices.Common.GetApplicationTypeCommandHandlerSpecs
{
    [Subject(typeof(GetApplicationTypeCommandHandler))]
    public class When_application_not_found_for_applicationkey : DomainServiceSpec<GetApplicationTypeCommand, GetApplicationTypeCommandHandler>
    {
        // Arrange
        Establish context = () =>
            {
                var applicationRepository = An<IApplicationRepository>();
                IApplication application = null;

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
        It should_return_zero = () =>
            {
                int defaultApplicationTypeKey = 0;
                command.ApplicationTypeKeyResult.Equals(defaultApplicationTypeKey);
            };
    }
}