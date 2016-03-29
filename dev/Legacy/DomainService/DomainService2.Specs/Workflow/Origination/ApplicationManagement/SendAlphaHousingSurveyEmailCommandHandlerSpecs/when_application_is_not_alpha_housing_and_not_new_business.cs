using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.SendAlphaHousingSurveyEmailCommandHandlerSpecs
{
    [Subject(typeof(SendAlphaHousingSurveyEmailCommandHandler))]
    public class when_application_is_not_alpha_housing_and_not_new_business : WithFakes
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

            IApplicationType applicationType = An<IApplicationType>();
            applicationType.WhenToldTo(x => x.Key).Return((int)SAHL.Common.Globals.OfferTypes.FurtherAdvance);

            IApplication application = An<IApplication>();
            application.WhenToldTo(x => x.IsAlphaHousing()).Return(false);
            application.WhenToldTo(x => x.ApplicationType).Return(applicationType);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);

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

        private It should_not_get_correspondence_template = () =>
        {
            commonRepository.WasNotToldTo(x => x.GetCorrespondenceTemplateByKey(SAHL.Common.Globals.CorrespondenceTemplates.AlphaHousingSurvey));
        };

        private It should_not_generate_password_for_account = () =>
        {
            applicationRepository.WasNotToldTo(x => x.GeneratePasswordFromAccountNumber(Param.IsAny<int>()));
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