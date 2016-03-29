using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendAlphaHousingSurveyEmailCommandHandlerSpecs
{
    [Subject(typeof(SendAlphaHousingSurveyEmailCommandHandler))]
    public class when_application_is_alpha_housing : WithFakes
    {
        private static SendAlphaHousingSurveyEmailCommandHandler sendAlphaHousingSurveyEmailCommandHandler;
        private static IDomainMessageCollection messages;
        private static SendAlphaHousingSurveyEmailCommand command;

        private static IApplicationRepository applicationRepository;
        private static ICommonRepository commonRepository;
        private static IMessageService messageService;
        private static IControlRepository controlRepository;

        private Establish context = () =>
        {
            applicationRepository = An<IApplicationRepository>();
            commonRepository = An<ICommonRepository>();
            messageService = An<IMessageService>();
            controlRepository = An<IControlRepository>();

            IControl control = An<IControl>();
            control.WhenToldTo(x => x.ControlNumeric).Return(1);
            controlRepository.WhenToldTo(x => x.GetControlByDescription(SAHL.Common.Constants.ControlTable.AlphaHousingSurvey.IsActivated)).Return(control);

            IAccount account = An<IAccount>();
            account.WhenToldTo(x => x.Key).Return(Param.IsAny<int>());

            ILegalEntity legalEntity = An<ILegalEntity>();
            legalEntity.WhenToldTo(x => x.EmailAddress).Return("emailAddress");

            IApplicationType applicationType = An<IApplicationType>();
            applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.NewPurchaseLoan);

            IApplication application = An<IApplication>();
            application.WhenToldTo(x => x.Account).Return(account);
            application.WhenToldTo(x => x.ApplicationType).Return(applicationType);

            ICorrespondenceTemplate template = An<ICorrespondenceTemplate>();
            template.WhenToldTo(x => x.Template).Return("");

            ICorrespondenceMedium medium = An<ICorrespondenceMedium>();
            medium.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.CorrespondenceMediums.Post);

            IApplicationMailingAddress mailingAddress = An<IApplicationMailingAddress>();
            mailingAddress.WhenToldTo(x => x.CorrespondenceMedium).Return(medium);
            mailingAddress.WhenToldTo(x => x.LegalEntity).Return(legalEntity);

            commonRepository.WhenToldTo(x => x.GetCorrespondenceTemplateByKey(SAHL.Common.Globals.CorrespondenceTemplates.AlphaHousingSurvey)).Return(template);

            IList<IApplicationMailingAddress> mailingList = new List<IApplicationMailingAddress>() { mailingAddress };
            IEventList<IApplicationMailingAddress> mailingAddresses = new EventList<IApplicationMailingAddress>(mailingList);

            IList<IApplicationRole> roleList = new List<IApplicationRole>() { };
            IReadOnlyEventList<IApplicationRole> roles = new ReadOnlyEventList<IApplicationRole>(roleList);

            application.WhenToldTo(x => x.IsAlphaHousing()).Return(true);
            application.WhenToldTo(x => x.GetApplicationRolesByType(SAHL.Common.Globals.OfferRoleTypes.MainApplicant)).Return(roles);
            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            application.WhenToldTo(x => x.ApplicationMailingAddresses).Return(mailingAddresses);

            command = new SendAlphaHousingSurveyEmailCommand(Param.IsAny<int>());
            sendAlphaHousingSurveyEmailCommandHandler = new SendAlphaHousingSurveyEmailCommandHandler(applicationRepository, commonRepository, messageService, controlRepository);
        };

        private Because of = () =>
        {
            sendAlphaHousingSurveyEmailCommandHandler.Handle(messages, command);
        };

        private It should_get_application_by_application_key = () =>
        {
            applicationRepository.WasToldTo(x => x.GetApplicationByKey(Param.IsAny<int>()));
        };

        private It should_get_correspondence_template = () =>
        {
            commonRepository.WasToldTo(x => x.GetCorrespondenceTemplateByKey(SAHL.Common.Globals.CorrespondenceTemplates.AlphaHousingSurvey));
        };

        private It should_generate_password_for_account = () =>
        {
            applicationRepository.WasToldTo(x => x.GeneratePasswordFromAccountNumber(Param.IsAny<int>()));
        };

        private It should_not_send_any_emails = () =>
        {
            messageService.WasNotToldTo(x => x.SendEmailExternal(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>()));
        };

        private It should_not_set_alpha_housing_email_sent_flag_to_true = () =>
        {
            command.AlphaHousingEmailSent.ShouldBeFalse();
        };
    }
}