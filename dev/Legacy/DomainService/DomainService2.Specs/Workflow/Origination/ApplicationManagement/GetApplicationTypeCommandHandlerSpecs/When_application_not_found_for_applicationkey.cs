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
    public class When_application_not_found_for_applicationkey : WithFakes
    {
        protected static GetApplicationTypeCommand command;
        protected static GetApplicationTypeCommandHandler commandHandler;
        protected static IDomainMessageCollection messages;

        // Arrange
        Establish context = () =>
            {
                var applicationRepository = An<IApplicationRepository>();
                IApplication application = null;

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
        It should_return_zero = () =>
            {
                int defaultApplicationTypeKey = 0;
                command.ApplicationTypeKey.Equals(defaultApplicationTypeKey);
            };
    }
}