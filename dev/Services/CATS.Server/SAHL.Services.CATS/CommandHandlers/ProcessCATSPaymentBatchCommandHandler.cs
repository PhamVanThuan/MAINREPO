using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Events;
using SAHL.Services.CATS.Managers.CATS;
using System;
using System.IO;

namespace SAHL.Services.CATS.CommandHandlers
{
    public class ProcessCATSPaymentBatchCommandHandler
        : IServiceCommandHandler<ProcessCATSPaymentBatchCommand>
    {
        private ICATSDataManager dataManager;
        private ICATSManager helper;
        private IServiceCommandRouter serviceCommandRouter;
        private IEventRaiser eventRaiser;
        private ICatsAppConfigSettings settings;

        public ProcessCATSPaymentBatchCommandHandler(ICATSDataManager catsDataManager, IServiceCommandRouter serviceCommandRouter
            , ICATSManager catsHelper, IEventRaiser eventRaiser, ICatsAppConfigSettings settings)
        {
            this.dataManager = catsDataManager;
            this.serviceCommandRouter = serviceCommandRouter;
            this.helper = catsHelper;
            this.eventRaiser = eventRaiser;
            this.settings = settings;
        }

        public ISystemMessageCollection HandleCommand(ProcessCATSPaymentBatchCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            var batchType = dataManager.GetBatchTypeInfo(command.CATSPaymentBatchKey);
            var paymentBatchItems = dataManager.GetPaymentBatchLineItemsByBatchKey(command.CATSPaymentBatchKey);
            var paymentTrans = helper.GenerateDetailedPayment(paymentBatchItems);
            var paymentBatch = helper.GroupPaymentsBySourceandThenTargetBankAccounts(paymentTrans);
            var filenameWithTimestamp = helper.TimestampFileName(settings.CATSOutputFileLocation + batchType.CATSFileNamePrefix);
            var batchSequenceNumber = dataManager.GetCATSPaymentBatchSequenceNumber();

            var generateCatsFileCommand =
                new GenerateCatsFileCommand(
                    (CATsEnvironment)batchType.CATSEnvironment
                    , filenameWithTimestamp
                    , batchType.CATSProfile
                    , batchSequenceNumber, DateTime.Now, DateTime.Now, paymentBatch);

            messages.Aggregate(serviceCommandRouter.HandleCommand(generateCatsFileCommand, metadata));

            if (messages.HasErrors)
            {
                dataManager.CloseCatsPaymentBatch(command.CATSPaymentBatchKey, (int)CATSPaymentBatchStatus.Failed, -1, filenameWithTimestamp);
                return messages;
            }

            dataManager.CloseCatsPaymentBatch(command.CATSPaymentBatchKey, (int)CATSPaymentBatchStatus.Processed, batchSequenceNumber, Path.GetFileName(filenameWithTimestamp));

            dataManager.SetCatsPaymentBatchSequenceNumber(batchSequenceNumber);

            var paymentBatchProcessedEvent = new CATSPaymentBatchProcessedEvent(DateTime.Now, command.CATSPaymentBatchKey);
            eventRaiser.RaiseEvent(DateTime.Now, paymentBatchProcessedEvent, command.CATSPaymentBatchKey, (int)GenericKeyType.CATSPaymentBatch, metadata);
            return messages;
        }
    }
}