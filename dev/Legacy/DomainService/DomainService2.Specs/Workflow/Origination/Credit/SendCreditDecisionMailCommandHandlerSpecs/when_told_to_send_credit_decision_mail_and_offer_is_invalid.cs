using DomainService2.Workflow.Origination.Credit;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.Origination.Credit.SendCreditDecisionMailCommandHandlerSpecs
{
    [Subject(typeof(SendCreditDecisionMailCommandHandler))]
    public class when_told_to_send_credit_decision_mail_and_offer_is_invalid : WithFakes
    {
        static IDomainMessageCollection message;
        static SendCreditDecisionMailCommand command;
        static SendCreditDecisionMailCommandHandler commandHandler;
        static IMessageService messageService;
        static ICommonRepository commonRepository;
        static IApplicationRepository applicationRepository;
        static IApplication application;
        static IApplicationType applicationType;


        Establish context = () =>
        {
            command = new SendCreditDecisionMailCommand(Param.IsAny<long>(), Param.IsAny<string>(), Param.IsAny<int>());
            applicationRepository = An<IApplicationRepository>();
            commonRepository = An<ICommonRepository>();
            messageService = An<IMessageService>();
            application = An<IApplication>();
            applicationType = An<IApplicationType>();
            applicationType.WhenToldTo(x => x.Key).Return(2);
            application.WhenToldTo(x => x.ApplicationType).Return(applicationType);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

            commandHandler = new SendCreditDecisionMailCommandHandler(messageService, commonRepository, applicationRepository);
        };

        Because of = () =>
        {
            commandHandler.Handle(message, command);
        };

        It should_ask_application_repository_for_application_by_key = () =>
        {
            applicationRepository.WasToldTo(x => x.GetApplicationByKey(Param.IsAny<int>()));
        };

        It should_not_ask_common_repository_for_correspondence_template = () =>
        {
            commonRepository.WasNotToldTo(x => x.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.CreditDecisionInternal));
        };

        //It should_get_not_current_consultant_and_admin = () =>
        //{
        //    workflowAssignmentRepository.WasNotToldTo(x => x.GetCurrentConsultantAndAdmin(Param.IsAny<long>()));
        //};

        It should_not_tell_the_message_to_send_email = () =>
        {
            messageService.WasNotToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

    }
}
