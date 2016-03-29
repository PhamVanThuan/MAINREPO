using SAHL.Config.Services.DomainProcessManager.Client;
using SAHL.Core.Data;
using SAHL.Core.Exchange;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.Communications;
using SAHL.Services.Interfaces.Communications.Commands;
using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;
using SAHL.Services.Interfaces.PollingManager;
using SAHL.Services.PollingManager.Helpers;
using SAHL.Services.PollingManager.Validators;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SAHL.Services.PollingManager.Handlers
{
    public class LossControlPolledExchangeHandler : IPolledHandler
    {
        private Timer HandlerTimer
        { get; set; }

        private IPolledHandlerSettings polledHandlerSettings;
        private IExchangeMailboxHelper exchangeMailboxHelper;
        private IExchangeProvider exchangeProvider;
        private IExchangeProviderCredentials exchangeProviderCredentials;
        private ILossControlAccountHelper lossControlAccountHelper;
        private ILossControlMailHelper lossControlMailHelper;
        private ILossControlMailValidator lossControlMailValidator;
        private ICommunicationsServiceClient communicationsServiceClient;
        private IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory;
        private ICombGuid combGuid;

        private ILogger logger;
        private ILoggerSource loggerSource;

        public LossControlPolledExchangeHandler(
              IPolledHandlerSettings polledHandlerSettings
            , IExchangeMailboxHelper exchangeMailboxHelper
            , IExchangeProvider exchangeProvider
            , IExchangeProviderCredentials exchangeProviderCredentials
            , ILossControlMailHelper lossControlMailHelper
            , ILossControlMailValidator lossControlMailValidator
            , ILossControlAccountHelper lossControlAccountHelper
            , ILogger logger
            , ILoggerSource loggerSource
            , ICombGuid combGuid
            , ICommunicationsServiceClient communicationsServiceClient
            , IDomainProcessManagerClientApiFactory domainProcessManagerClientApiFactory
        )
        {
            this.polledHandlerSettings = polledHandlerSettings;
            this.exchangeMailboxHelper = exchangeMailboxHelper;
            this.exchangeProvider = exchangeProvider;
            this.exchangeProviderCredentials = exchangeProviderCredentials;
            this.lossControlMailHelper = lossControlMailHelper;
            this.lossControlMailValidator = lossControlMailValidator;
            this.lossControlAccountHelper = lossControlAccountHelper;
            this.logger = logger;
            this.loggerSource = loggerSource;
            this.combGuid = combGuid;
            this.communicationsServiceClient = communicationsServiceClient;
            this.domainProcessManagerClientApiFactory = domainProcessManagerClientApiFactory;
        }

        public void Initialise()
        {
            HandlerTimer = new Timer(polledHandlerSettings.TimerInterval);
            HandlerTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            HandlerTimer.Stop();
        }

        public void Start()
        {
            HandlerTimer.Start();
            logger.LogStartup(loggerSource, Thread.CurrentPrincipal.Identity.Name, "Start", "Loss Control Polled Exchange Handler Started");

#if DEBUG
            Console.WriteLine("{0}\t{1}", Thread.CurrentPrincipal.Identity.Name, "Loss Control Polled Exchange Handler Started");
#endif
        }

        public void Stop()
        {
            HandlerTimer.Stop();
            logger.LogStartup(loggerSource, Thread.CurrentPrincipal.Identity.Name, "Stop", "Loss Control Polled Exchange Handler Stopped");

#if DEBUG
            Console.WriteLine("{0}\t{1}", Thread.CurrentPrincipal.Identity.Name, "Loss Control Polled Exchange Handler Stopped");
#endif
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Stop();
            exchangeMailboxHelper.UpdateMessages();
            HandlePolledEvent();
            Start();
        }

        public void HandlePolledEvent()
        {
            foreach (var emailMessage in exchangeMailboxHelper.EmailMessages)
            {
                if (IsValidMail(emailMessage))
                {
                    //Get the account number and associated errors
                    var accountNumberWithErrorMessages = lossControlAccountHelper.StripAccountNumber('-', emailMessage.Subject);
                    var accountNumber = accountNumberWithErrorMessages.Item1;
                    var associatedExtractionErrors = accountNumberWithErrorMessages.Item2;

                    if (associatedExtractionErrors.Any())
                    {
                        //send out error msgs email to sender
                        SendUnsuccessfulInvoiceEmail(emailMessage);
                    }
                    else
                    {
                        IDataModel dataModel = lossControlMailHelper.GetPreProcessedThirdPartyInvoice(accountNumber, emailMessage);
                        // start the required domain process
                        this.domainProcessManagerClientApiFactory.Create()
                            .DataModel(dataModel)
                            .EventToWaitFor("FireAndForget")
                            .StartProcess();
                    }
                }
                else
                {
                    //send out error msgs email to sender
                    
                    SendUnsuccessfulInvoiceEmail(emailMessage);
                }

                // To deal with modifying a collection one is iterating might mark messages as read and archive outside foreach
                exchangeMailboxHelper.MoveMessage(exchangeProviderCredentials.ArchiveFolder, emailMessage);
            }
        }

        private ISystemMessageCollection SendUnsuccessfulInvoiceEmail(IMailMessage receivedMail)
        {
            IServiceRequestMetadata serviceRequestMetadata = new ServiceRequestMetadata();
            var emailModel = new UnSuccessfulInvoiceSubmissionEmailModel(receivedMail.From, receivedMail.Subject, MailPriority.High, lossControlMailValidator.Errors, receivedMail.Attachments);
            IEmailTemplate<IEmailModel> emailTemplate = new InvoiceEmailTemplate(InvoiceTemplateType.UnSuccessfulInvoiceEmailTemplate, emailModel);
            var command = new SendInternalEmailCommand(combGuid.Generate(), emailTemplate);
            return communicationsServiceClient.PerformCommand(command, serviceRequestMetadata);
        }

        private bool IsValidMail(IMailMessage receivedMail)
        {
            if (!lossControlMailValidator.ValidateMail(receivedMail))
            {
                foreach (string error in lossControlMailValidator.Errors)
                {
                    logger.LogError(loggerSource, Thread.CurrentPrincipal.Identity.Name, "LossControlPolledExchangeHandler", error);

#if DEBUG
                    Console.WriteLine("{0}\t{1}\n{2}", Thread.CurrentPrincipal.Identity.Name, "Loss Control Polled Exchange Handler Error", error);
#endif
                }

                return false;
            }
            return true;
        }
    }
}