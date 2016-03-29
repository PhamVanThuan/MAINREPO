using System;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendEmailToConsultantForValuationDoneCommandHandlerSpecs
{
    [Subject(typeof(SendEmailToConsultantForValuationDoneCommandHandler))]
    public class When_sending_internal_mail_for_a_non_existant_application : WithFakes
    {
        protected static SendEmailToConsultantForValuationDoneCommand command;
        protected static SendEmailToConsultantForValuationDoneCommandHandler handler;
        protected static IDomainMessageCollection messages;
        protected static IApplicationRepository applicationRepository;
        protected static IPropertyRepository propertyRepository;
        protected static IMessageService messageService;
        protected static Exception exception;

        Establish context = () =>
        {
            messageService = An<IMessageService>();
            propertyRepository = An<IPropertyRepository>();
            applicationRepository = An<IApplicationRepository>();
            IApplicationMortgageLoan applicationMortgageLoan = null;

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param<int>.IsAnything)).Return(applicationMortgageLoan);
            command = new SendEmailToConsultantForValuationDoneCommand(Param<int>.IsAnything);
            handler = new SendEmailToConsultantForValuationDoneCommandHandler(applicationRepository, propertyRepository, messageService);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => handler.Handle(messages, command));
        };

        It should_return_an_exception = () =>
        {
            exception.ShouldNotBeNull();
        };

        It should_return_an_exception_of_type_null_referrence = () =>
        {
            exception.ShouldBeOfType(typeof(NullReferenceException));
        };
    }
}