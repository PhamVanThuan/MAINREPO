using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
using System.Collections.Generic;
using System.Timers;

namespace SAHL.Services.PollingManager.Server.Specs.Handlers.LossControlPolledExchange
{
    public class when_the_timer_elapses : WithFakes
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
        private static int expectedNumberOfAttachments, expectedAccountNumber;
        private static char accountNumberSeperator;
        private static IDataModel dataModel;
        private static object source;
        private static ElapsedEventArgs args;
        private static IMailMessage mailMessage;

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
            mailMessage = new MailMessage();
            mailMessage.From = "attorney@practice.co.za";
            mailMessage.To = "losscontrol@sahl.com";
            mailMessage.Subject = string.Format("{0}{1} 56666 My Invoice 1", expectedAccountNumber, accountNumberSeperator);
            mailMessage.Body = "Test body";
            lossControlMailValidator.WhenToldTo(x => x.Errors).Return(new List<string>() { "errors" });
            mailMessage.Attachments = new List<IMailAttachment>
                                        {
                                            new MailAttachment()
                                            {
                                                  AttachmentName = "May Invoice.pdf"
                                                , AttachmentType = "pdf"
                                                , ContentAsBase64 = "VGhpcyBpcyBhIHRlc3QgYXR0YWNoZW1lbnQ="
                                            }
                                        };

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
            exchangeMailboxHelper.WhenToldTo(x => x.EmailMessages).Return(new List<IMailMessage> { mailMessage });
            //we are not testing the behaviour inside the polled event, just that it happens.
            lossControlMailValidator.WhenToldTo(x => x.ValidateMail(Param.IsAny<IMailMessage>())).Return(false);
            polledHandlerSettings.WhenToldTo(x => x.TimerInterval).Return(1000);
        };

        private Because of = () =>
        {
            lossControlPolledExchangeHandler.Initialise();
            lossControlPolledExchangeHandler.OnTimedEvent(source, args);
        };

        private It should_stop_the_timer = () =>
        {
            logger.WasToldTo(x => x.LogStartup(Arg.Any<ILoggerSource>(), Arg.Any<string>(), "Stop", Arg.Any<string>(), null));
        };

        private It should_update_the_email_messages = () =>
        {
            exchangeMailboxHelper.WasToldTo(x => x.UpdateMessages());
        };

        private It should_handle_the_polled_event = () =>
        {
            lossControlMailValidator.WasToldTo(x => x.ValidateMail(mailMessage));
        };

        private It should_start_the_timer = () =>
        {
            logger.WasToldTo(x => x.LogStartup(Arg.Any<ILoggerSource>(), Arg.Any<string>(), "Start", Arg.Any<string>(), null));
        };
    }
}