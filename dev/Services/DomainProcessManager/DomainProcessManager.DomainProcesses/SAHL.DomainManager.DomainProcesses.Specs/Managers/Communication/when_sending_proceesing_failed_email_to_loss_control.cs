using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_proceesing_failed_email_to_loss_control: WithFakes
    {
        private static CommunicationManager communicationManager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;
        private static IServiceRequestMetadata serviceRequestMetadata;

        private static string lossControlEmailAddress;
        private static ISystemMessageCollection systemMessages;
        private static StandardEmailModel emailModel;
        private static StandardEmailTemplate emailTemplate;
        private static SendInternalEmailCommand command;
        private static List<IMailAttachment> attachments;

        private Establish context = () =>
        {
            
            settings = An<ICommunicationManagerSettings>();
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            serviceRequestMetadata=An<IServiceRequestMetadata>();
            communicationManager = new CommunicationManager(communicationsService, combGuidGenerator, settings);

            attachments = new List<IMailAttachment> { new MailAttachment { AttachmentName = "test123.pdf", ContentAsBase64 = "Some Content=", AttachmentType = "pdf" } };
            emailModel = new StandardEmailModel(settings.AttorneyInvoiceFailuresEmailAddress, "Failed: " + "3234 - ", "Failed: please resend", MailPriority.High, attachments);
            emailTemplate = new StandardEmailTemplate(emailModel);
            command = new SendInternalEmailCommand(combGuidGenerator.Generate(), emailTemplate);

            lossControlEmailAddress = "losscontrol@sahomeloans.com";
            settings.WhenToldTo(x => x.AttorneyInvoiceFailuresEmailAddress).Return(lossControlEmailAddress);

            systemMessages = SystemMessageCollection.Empty();
            systemMessages.AddMessage(new SystemMessage("Failed: Something bad just happened!", SystemMessageSeverityEnum.Error));
        };

        private Because of = () =>
        {
            communicationManager.SendProcessingFailedEmailToLossControl(emailModel.Subject, attachments, systemMessages);
        };

        private It should_send_process_failed_email_to_loss_control = () =>
        {
            communicationsService.WasToldTo(x => x.PerformCommand(Param.IsAny<SendInternalEmailCommand>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}
