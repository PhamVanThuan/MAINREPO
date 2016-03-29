﻿using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.Data;
using SAHL.Core.Exchange;
using SAHL.Core.Exchange.Provider;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
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
    public class polled_event_handles_a_mail_with_no_attachment : WithFakes
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
        private static IDataModel dataModel;
        private static List<string> missingAttachmentErrorMessages;

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
            missingAttachmentErrorMessages = new List<string> { "Third party invoce email missing attachment." };

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
            emailMessage = new MailMessage();
            emailMessage.From = "attorney@practice.co.za";
            emailMessage.To = "losscontrol@sahl.com";
            emailMessage.Subject = "My Invoice 1";
            emailMessage.Body = "Test body";

            emailMessage.Attachments = new List<IMailAttachment>();
            expectedNumberOfAttachments = emailMessage.Attachments.Count;

            exchangeMailboxHelper.WhenToldTo(x => x.EmailMessages).Return(new List<IMailMessage> { emailMessage });
            lossControlMailValidator.WhenToldTo(x => x.ValidateMail(Param.IsAny<IMailMessage>())).Return(false);
            lossControlMailValidator.WhenToldTo(y => y.Errors).Return(missingAttachmentErrorMessages);

            lossControlAccountHelper.WhenToldTo(x => x.StripAccountNumber(Param.IsAny<char>(), Param.IsAny<string>()))
                .Return(new Tuple<int, List<string>>(expectedAccountNumber, missingAttachmentErrorMessages));

            attorneyInvoiceProcessModel = new ReceiveAttorneyInvoiceProcessModel(
                  expectedAccountNumber
                , emailMessage.DateRecieved
                , emailMessage.From
                , emailMessage.Subject
                , string.Empty
                , string.Empty
                , string.Empty
                , string.Empty
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

        private It should_log_the_error = () =>
        {
            logger.WasToldTo(l => l.LogError(
                  Param.IsAny<ILoggerSource>()
                , Param.IsAny<string>()
                , Param<string>.Matches(m => m.Equals("LossControlPolledExchangeHandler", StringComparison.Ordinal))
                , Param<string>.Matches(m => m.Equals(missingAttachmentErrorMessages.First(), StringComparison.Ordinal))
                , null
            ));
        };

        private It should_move_the_mail_to_an_archive = () =>
        {
            exchangeMailboxHelper.WasToldTo(x => x.MoveMessage(exchangeProviderCredentials.ArchiveFolder, emailMessage));
        };

        private It should_notify_sender_about_invalid_mail_format = () =>
        {
            communicationsServiceClient.WasToldTo(x =>
                x.PerformCommand(Param<SendInternalEmailCommand>
                .Matches(y => y.EmailTemplate.EmailModel.To == emailMessage.From
                           && y.EmailTemplate.EmailModel.Subject.Contains(emailMessage.Subject)
                           && y.EmailTemplate.EmailModel.Attachments.Count() == expectedNumberOfAttachments)
                , Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}