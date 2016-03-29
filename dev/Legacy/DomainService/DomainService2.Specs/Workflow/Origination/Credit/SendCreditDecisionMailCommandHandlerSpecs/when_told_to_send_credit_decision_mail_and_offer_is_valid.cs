using DomainService2.Specs.DomainObjects;
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
using System.Data;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.Workflow.Origination.Credit.SendCreditDecisionMailCommandHandlerSpecs
{
    [Subject(typeof(SendCreditDecisionMailCommandHandler))]
    public class when_told_to_send_credit_decision_mail_and_offer_is_valid : WithFakes
    {
        static IDomainMessageCollection message;
        static SendCreditDecisionMailCommand command;
        static SendCreditDecisionMailCommandHandler commandHandler;
        static IMessageService messageService;
        static ICommonRepository commonRepository;
        static IApplicationRepository applicationRepository;
        static IApplication application;
        static IApplicationType applicationType;
        static ICorrespondenceTemplate correspondenceTemplate;
        static ILegalEntity admin;
        static IApplicationRole adminRole;
        static IApplicationRoleType adminRoleType;
        static ILegalEntity consultant;
        static IApplicationRole consultantRole;
        static IApplicationRoleType consultantRoleType;
        static IGeneralStatus activeStatus;
        const string adminEmail = "admin@sahomeloans.com";
        const string consultantEmail = "consultant@sahomeloans.com";


        Establish context = () =>
        {
            command = new SendCreditDecisionMailCommand(Param.IsAny<long>(), "", Param.IsAny<int>());
            applicationRepository = An<IApplicationRepository>();
            commonRepository = An<ICommonRepository>();
            messageService = An<IMessageService>();


            activeStatus = An<IGeneralStatus>();
            activeStatus.WhenToldTo(x => x.Key).Return((int)GeneralStatuses.Active);

            admin = An<ILegalEntity>();
            admin.WhenToldTo(x => x.EmailAddress).Return(adminEmail);
            adminRole = An<IApplicationRole>();
            adminRoleType = An<IApplicationRoleType>();
            adminRoleType.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.BranchAdminD);
            adminRole.WhenToldTo(x => x.GeneralStatus).Return(activeStatus);
            adminRole.WhenToldTo(x => x.ApplicationRoleType).Return(adminRoleType);
            adminRole.WhenToldTo(x => x.LegalEntity).Return(admin);

            consultant = An<ILegalEntity>();
            consultant.WhenToldTo(x => x.EmailAddress).Return(consultantEmail);
            consultantRole = An<IApplicationRole>();
            consultantRoleType = An<IApplicationRoleType>();
            consultantRoleType.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.BranchConsultantD);
            consultantRole.WhenToldTo(x => x.GeneralStatus).Return(activeStatus);
            consultantRole.WhenToldTo(x => x.ApplicationRoleType).Return(consultantRoleType);
            consultantRole.WhenToldTo(x => x.LegalEntity).Return(consultant);

            application = An<IApplication>();
            applicationType = An<IApplicationType>();
            applicationType.WhenToldTo(x => x.Key).Return(6);
            application.WhenToldTo(x => x.ApplicationType).Return(applicationType);

            IReadOnlyEventList<IApplicationRole> roles = new StubReadOnlyEventList<IApplicationRole>(new IApplicationRole[] {adminRole, consultantRole});

            application.WhenToldTo(x => x.ApplicationRoles).Return(roles);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

            correspondenceTemplate = An<ICorrespondenceTemplate>();
            correspondenceTemplate.WhenToldTo(x => x.Subject).Return("");
            correspondenceTemplate.WhenToldTo(x => x.Template).Return("");
            commonRepository.WhenToldTo(x => x.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.CreditDecisionInternal)).Return(correspondenceTemplate);

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

        It should_ask_common_repository_for_correspondence_template = () =>
        {
            commonRepository.WasToldTo(x => x.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.CreditDecisionInternal));
        };

        It should_send_the_email_to_the_branch_consultant = () =>
        {
            messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.Is<string>(consultantEmail), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

        It should_send_the_email_to_the_branch_admin = () =>
        {
            messageService.WasToldTo(x => x.SendEmailInternal(Param.IsAny<string>(), Param.Is<string>(adminEmail), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

    }
}
