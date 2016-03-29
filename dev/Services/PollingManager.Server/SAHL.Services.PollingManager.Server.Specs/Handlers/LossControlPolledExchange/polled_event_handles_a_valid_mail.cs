using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.Data;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.DomainProcessManager;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager.Handlers;
using SAHL.Services.PollingManager.Helpers;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ExchangeManager.Server.Specs.Handlers.LossControlPolledExchange
{
    public class loss_control_polled_exchange_handler_polled_event_handles_a_valid_mail : WithFakes
    {
        private static IExchangeMailboxHelper exchangeMailboxHelper;
        private static IExchangeProvider exchangeProvider;
        private static IExchangeProviderCredentials exchangeProviderCredentials;
        private static ILossControlMailHelper lossControlMailHelper;
        private static ILossControlMailValidator lossControlMailValidator;
        private static ILossControlAccountHelper lossControlAccountHelper;
        private static ILogger logger;
        private static ILoggerSource loggerSource;
        private static ICombGuid combGuid;
        private static ICommunicationsServiceClient communicationsServiceClient;
        private static IDomainProcessManagerClient domainProcessManagerService;
        private static IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private static IPolledHandlerSettings polledHandlerSettings;
        private static IPolledHandler lossControlPolledExchangeHandler;
        private static IMailMessage emailMessage;
        private static int expectedNumberOfAttachments, expectedAccountNumber;
        private static ReceiveAttorneyInvoiceProcessModel attorneyInvoiceProcessModel;
        private static char accountNumberSeperator;
        private static IDataModel dataModel;

        private Establish context = () =>
        {
            polledHandlerSettings = An<IPolledHandlerSettings>();
            exchangeMailboxHelper = An<IExchangeMailboxHelper>();
            exchangeProvider = An<IExchangeProvider>();
            exchangeProviderCredentials = An<IExchangeProviderCredentials>();
            lossControlMailHelper = An<ILossControlMailHelper>();
            lossControlMailValidator = An<ILossControlMailValidator>();
            lossControlAccountHelper = An<ILossControlAccountHelper>();
            domainProcessManagerClientApiFactory = An<IDomainProcessManagerClientApiFactory>();
            domainProcessManagerService = An<IDomainProcessManagerClient>();
            dataModel = An<IDataModel>();

            logger = An<ILogger>();
            loggerSource = An<ILoggerSource>();
            combGuid = An<ICombGuid>();
            communicationsServiceClient = An<ICommunicationsServiceClient>();

            lossControlPolledExchangeHandler = new LossControlPolledExchangeHandler(
                  polledHandlerSettings
                , exchangeMailboxHelper
                , exchangeProvider
                , exchangeProviderCredentials
                , lossControlMailHelper
                , lossControlMailValidator
                , lossControlAccountHelper
                , logger
                , loggerSource
                , combGuid
                , communicationsServiceClient
                , domainProcessManagerClientApiFactory
                );

            expectedAccountNumber = 12345;
            accountNumberSeperator = '-';
            emailMessage = new MailMessage();
            emailMessage.From = "attorney@practice.co.za";
            emailMessage.To = "losscontrol@sahl.com";
            emailMessage.Subject = string.Format("{0}{1} 56666 My Invoice 1", expectedAccountNumber, accountNumberSeperator);
            emailMessage.Body = "Test body";

            emailMessage.Attachments = new List<IMailAttachment>
                                        {
                                            new MailAttachment()
                                            {
                                                  AttachmentName = "May Invoice.pdf"
                                                , AttachmentType = "pdf"
                                                , ContentAsBase64 = "VGhpcyBpcyBhIHRlc3QgYXR0YWNoZW1lbnQ="
                                            }
                                        };
            expectedNumberOfAttachments = emailMessage.Attachments.Count;

            exchangeMailboxHelper.WhenToldTo(x => x.EmailMessages).Return(new List<IMailMessage> { emailMessage });
            lossControlMailValidator.WhenToldTo(x => x.ValidateMail(Param.IsAny<IMailMessage>())).Return(true);

            lossControlAccountHelper.WhenToldTo(x => x.StripAccountNumber(
                  Param.IsAny<char>()
                , Param.IsAny<string>())
                ).Return(new Tuple<int, List<string>>(expectedAccountNumber, new List<string>()));

            attorneyInvoiceProcessModel = new ReceiveAttorneyInvoiceProcessModel(
                  expectedAccountNumber
                , emailMessage.DateRecieved
                , emailMessage.From
                , emailMessage.Subject
                , "MyInvoice1.pdf"
                , "pdf"
                , string.Empty
                , emailMessage.Attachments.First().ContentAsBase64
            );
            lossControlMailHelper.WhenToldTo(y => y.GetPreProcessedThirdPartyInvoice(Param.IsAny<int>(), Param.IsAny<IMailMessage>())).Return(attorneyInvoiceProcessModel);

            domainProcessManagerClientApiFactory.WhenToldTo(x => x.Create()).Return(new DomainProcessManagerClientApi(domainProcessManagerService));
        };

        private Because of = () =>
        {
            lossControlPolledExchangeHandler.HandlePolledEvent();
        };

        private It should_validate_received_email_message = () =>
        {
            lossControlMailValidator.WasToldTo(v => v.ValidateMail(Param<IMailMessage>.Matches(
                m => m.From.Equals(emailMessage.From, StringComparison.Ordinal)
                  && m.To.Equals(emailMessage.To, StringComparison.Ordinal)
                  && m.Body.Equals(emailMessage.Body, StringComparison.Ordinal)
                  && m.Subject.Equals(emailMessage.Subject, StringComparison.Ordinal)
                  && m.Attachments.Count == expectedNumberOfAttachments
                )));
        };

        private It should_get_the_account_number_from_email_subject = () =>
        {
            lossControlAccountHelper.WasToldTo(x => x.StripAccountNumber(
                  Param<char>.Matches(m => m.Equals(accountNumberSeperator))
                , Param<string>.Matches(m => m.Equals(emailMessage.Subject, StringComparison.Ordinal)))
                );
        };

        private It should_prepare_invoice_for_processing = () =>
        {
            lossControlMailHelper.WasToldTo(x => x.GetPreProcessedThirdPartyInvoice(Param<int>.Matches(y => y.Equals(expectedAccountNumber)),
                Param<IMailMessage>.Matches(m => m.Attachments.Count == expectedNumberOfAttachments
                && m.Subject.Equals(emailMessage.Subject, StringComparison.Ordinal))));
        };

        private It should_start_the_third_party_invoice_processor = () =>
        {
            domainProcessManagerService.WasToldTo(x => x.StartDomainProcess(
                Param<StartDomainProcessCommand>.Matches(
                   m => m.StartEventToWaitFor == "FireAndForget"
                     && m.DataModel is ReceiveAttorneyInvoiceProcessModel
                )));
        };

        private It should_move_the_mail_to_an_archive = () =>
        {
            exchangeMailboxHelper.WasToldTo(x => x.MoveMessage(exchangeProviderCredentials.ArchiveFolder, emailMessage));
        };
    }
}