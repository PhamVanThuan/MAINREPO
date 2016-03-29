using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.GetApplicationTypeCommandHandlerSpecs
{
    [Subject(typeof(GetApplicationTypeCommandHandler))]
    public class When_application_found_for_applicationkey : WithFakes
    {
        protected static GetApplicationTypeCommand command;
        protected static GetApplicationTypeCommandHandler commandHandler;
        protected static IDomainMessageCollection messages;
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
                commandHandler = new GetApplicationTypeCommandHandler(applicationRepository);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            };

        // Act
        Because of = () =>
            {
                commandHandler.Handle(messages, command);
            };

        // Assert
        It should_return_applicationTypeKey = () =>
            {
                command.ApplicationTypeKey.Equals(applicationTypeKey);
            };
    }
}