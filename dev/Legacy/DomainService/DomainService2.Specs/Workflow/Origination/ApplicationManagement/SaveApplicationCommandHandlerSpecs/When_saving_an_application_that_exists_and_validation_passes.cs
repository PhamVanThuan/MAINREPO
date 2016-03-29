using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SaveApplicationCommandHandlerSpecs
{
    [Subject(typeof(SaveApplicationCommandHandler))]
    public class When_saving_an_application_that_exists_and_validation_passes : WithFakes
    {
        protected static IApplicationRepository applicationRepository;
        protected static SaveApplicationCommand command;
        protected static SaveApplicationCommandHandler handler;
        protected static IDomainMessageCollection messages;

        // Arrange
        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                IApplication application = An<IApplication>();
                applicationRepository = An<IApplicationRepository>();

                command = new SaveApplicationCommand(Param<int>.IsAnything, Param<bool>.IsAnything);
                handler = new SaveApplicationCommandHandler(applicationRepository);

                applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(application);
                applicationRepository.WhenToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
            };

        // Act
        Because of = () =>
            {
                handler.Handle(messages, command);
            };

        // Assert
        It should_save_the_application = () =>
            {
                applicationRepository.WasToldTo(x => x.SaveApplication(Param<IApplication>.IsAnything));
            };

        //Assert
        It should_return_true = () =>
            {
                command.Result = true;
            };
    }
}