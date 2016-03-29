using System;
using System.Data;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendEmailToConsultantCommandHandlerSpecs
{
    [Subject(typeof(SendEmailToConsultantForQueryCommandHandler))]
    public class When_sending_internal_mail_for_a_non_existant_workflow_case : WithFakes
    {
        protected static SendEmailToConsultantForQueryCommand command;
        protected static SendEmailToConsultantForQueryCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IMessageService messageService;
        protected static IReasonRepository reasonRepository;
        protected static IX2Repository x2Repository;
        protected static Exception exception;

        // Arrange
        Establish context = () =>
            {
                messages = An<IDomainMessageCollection>();
                x2Repository = An<IX2Repository>();
                reasonRepository = An<IReasonRepository>();
                messageService = An<IMessageService>();

                IInstance instance = null;
                DataTable dataTable = null;

                x2Repository.WhenToldTo(x => x.GetInstanceByKey(Param<int>.IsAnything)).Return(instance);
                x2Repository.WhenToldTo(x => x.GetCurrentConsultantAndAdmin(Param<long>.IsAnything)).Return(dataTable);

                command = new SendEmailToConsultantForQueryCommand(Param<int>.IsAnything, Param<long>.IsAnything, Param<int>.IsAnything);
                handler = new SendEmailToConsultantForQueryCommandHandler(reasonRepository, x2Repository, messageService);
            };

        // Act
        Because of = () =>
            {
                exception = Catch.Exception(() => handler.Handle(messages, command));
            };

        // Assert
        It should_throw_a_null_referrence_exception = () =>
            {
                exception.ShouldBeOfType(typeof(NullReferenceException));
            };
    }
}