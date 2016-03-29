using System;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SaveApplicationCommandHandlerSpecs
{
    [Subject(typeof(SaveApplicationCommandHandler))]
    public class When_saving_an_application_that_exists_and_validation_fails : WithFakes
    {
        protected static IApplicationRepository applicationRepository;
        protected static SaveApplicationCommand command;
        protected static SaveApplicationCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static Exception exception;

        // Arrange
        Establish context = () =>
        {
            messages = An<IDomainMessageCollection>();
            IApplication application = An<IApplication>();
            applicationRepository = An<IApplicationRepository>();

            command = new SaveApplicationCommand(Param<int>.IsAnything, Param<bool>.IsAnything);
            handler = new SaveApplicationCommandHandler(applicationRepository);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
            applicationRepository.WhenToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything)).Throw(new DomainValidationException());
        };

        // Act
        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        // Assert
        It should_save_the_application = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
        };

        // Assert
        It should_throw_a_domain_validation_exception = () =>
        {
            exception.ShouldBeOfType(typeof(DomainValidationException));
        };

        //Assert
        It should_return_false = () =>
        {
            command.Result = false;
        };
    }
}