using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Communication;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Communication
{
    public class when_sending_third_party_manual_archive_email : WithFakes
    {
        private static CommunicationManager manager;
        private static ICommunicationsServiceClient communicationsService;
        private static ICombGuid combGuidGenerator;
        private static ICommunicationManagerSettings settings;

        private static string invoiceProcessorEmailAddress;
        private static List<string> stuckThirdPartyInvoicesReferenceNumbers;

        private static Guid domainProcessId;

        private Establish context = () =>
        {
            communicationsService = An<ICommunicationsServiceClient>();
            combGuidGenerator = An<ICombGuid>();
            settings = An<ICommunicationManagerSettings>();
            manager = new CommunicationManager(communicationsService, combGuidGenerator, settings);

            domainProcessId = combGuidGenerator.Generate();

            stuckThirdPartyInvoicesReferenceNumbers = new List<string> { "SAHL-2015/07/382", "SAHL-2015/07/372", "SAHL-2015/07/359" };

            invoiceProcessorEmailAddress = "_ToBeDecided@sahomeloans.com";
            settings.WhenToldTo(x => x.ThirdPartyInvoiceProcessorEmailAddress).Return(invoiceProcessorEmailAddress);
        };

        private Because of = () =>
        {
            manager.SendOperatorRequestForManualArchive(stuckThirdPartyInvoicesReferenceNumbers);
        };

        private It should_send_an_email_to_invoice_processor = () =>
        {
            communicationsService.WasToldTo(x => x.PerformCommand(Param<SendInternalEmailCommand>.Matches(m =>
                m.EmailTemplate.EmailModel.To == invoiceProcessorEmailAddress &&
                m.EmailTemplate.GetType() == typeof(StandardEmailTemplate) &&
                ((StandardEmailModel)m.EmailTemplate.EmailModel).Body.Contains(stuckThirdPartyInvoicesReferenceNumbers.Aggregate((i, j) => i + ", " + j))),
                Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}