using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.CATS.CommandHandlers;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Models;
using SAHL.Services.Interfaces.CATS.Commands;
using SAHL.Services.Interfaces.CATS.Enums;
using SAHL.Services.Interfaces.CATS.Events;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;
using System;
using System.Collections.Generic;
using System.IO;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    public class when_processing_a_valid_batch : WithCoreFakes
    {
        private static ProcessCATSPaymentBatchCommandHandler commandHandler;
        private static ProcessCATSPaymentBatchCommand command;
        private static ICATSManager catsHelper;
        private static int batchNumber;
        private static ICATSDataManager dataManager;
        private static ISystemMessageCollection message;
        private static List<CATSPaymentBatchItemDataModel> batchItems;
        private static List<DetailedPayment> paymentTrans;
        private static List<PaymentBatch> paymentBatches;
        private static CATsEnvironment catsEnvironment;
        private static string fullFileNamePrefix, timestampedfullFileName, fullFileLocation;
        private static string SSVSUserID;
        private static CATSPaymentBatchTypeDataModel catsPaymentBatchType;
        private static ICatsAppConfigSettings catsAppConfigSettings;
        private static int batchSequenceNumber;

        private Establish context = () =>
             {
                 catsAppConfigSettings = An<ICatsAppConfigSettings>();
                 eventRaiser = An<IEventRaiser>();
                 fullFileLocation = @"c:\\temp";
                 fullFileNamePrefix = "filename";
                 timestampedfullFileName = fullFileLocation + fullFileNamePrefix + "201509121200";
                 catsEnvironment = CATsEnvironment.Live;
                 SSVSUserID = "SHL04";
                 catsPaymentBatchType = new CATSPaymentBatchTypeDataModel("ThirdpartyInvoice", SSVSUserID,
                     fullFileNamePrefix, (int)catsEnvironment, 1);
                 batchNumber = 10001;
                 catsHelper = An<ICATSManager>();
                 serviceRequestMetaData = new ServiceRequestMetadata();
                 serviceCommandRouter = An<IServiceCommandRouter>();
                 dataManager = An<ICATSDataManager>();
                 command = new ProcessCATSPaymentBatchCommand(batchNumber);
                 commandHandler = new ProcessCATSPaymentBatchCommandHandler(dataManager, serviceCommandRouter, catsHelper, eventRaiser, catsAppConfigSettings);
                 catsHelper.WhenToldTo(x => x.TimestampFileName(catsAppConfigSettings.CATSOutputFileLocation + fullFileNamePrefix)).Return(timestampedfullFileName);
                 message = SystemMessageCollection.Empty();
                 batchSequenceNumber = 1194;

                 var CATSPaymentBatchItemDataModel1 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, 100, 1001, 11110, "SAHL", "SAHL      SPV 32", "Strauss Dally", "Eternal Reference", "attorneyemail@firm.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel2 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, 1001, 11110, 41, "SAHL", "SAHL      SPV 32", "Straus Dally", "External Reference", "attorneyemail@firm.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel3 = new CATSPaymentBatchItemDataModel(1, 45, 1114, 21.0m, 1001, 11110, 41, "SAHL", "SAHL      SPV 32", "Straus Dally", "External Reference", "attorneyemail@firm.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel4 = new CATSPaymentBatchItemDataModel(1, 45, 1114, 21.0m, 1001, 11110, 41, "SAHL", "SAHL      SPV 32", "Straus Dally", "External Reference", "attorneyemail@firm.co.za", 2004, true);
                 batchItems = new List<CATSPaymentBatchItemDataModel>()
                 {
                    CATSPaymentBatchItemDataModel1
                    ,CATSPaymentBatchItemDataModel2
                    ,CATSPaymentBatchItemDataModel3
                    ,CATSPaymentBatchItemDataModel4
                 };
                 paymentTrans = new List<DetailedPayment>()
                 {
                    new  DetailedPayment(CATSPaymentBatchItemDataModel1, null, null)
                    ,new  DetailedPayment(CATSPaymentBatchItemDataModel1, null, null)
                    ,new  DetailedPayment(CATSPaymentBatchItemDataModel1, null, null)
                    ,new  DetailedPayment(CATSPaymentBatchItemDataModel1, null, null)
                 };

                 paymentBatches = new List<PaymentBatch>(){new PaymentBatch(
                    new List<Payment>
                    {
                        new Payment(
                        new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                        , 5000M,"1492225 Disburse")
                        ,new Payment(
                        new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                        , 5000M,"1492225 Disburse")
                        ,new Payment(
                        new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                        , 5000M,"1492225 Disburse")
                        ,new Payment(
                        new BankAccountDataModel("632005","9212020837", (int)ACBType.Current, "P. Miller", null,null)
                        , 5000M,"1492225 Disburse")
                    }
                    , new BankAccountDataModel("42826", "51403153".PadRight(34, '2'), (int)ACBType.Current, "SA Home Loans (Pty) Limited", null, null)
                    , 5000M, "Reference")};

                 dataManager.WhenToldTo(x => x.GetPaymentBatchLineItemsByBatchKey(batchNumber)).Return(batchItems);
                 catsHelper.WhenToldTo(x => x.GenerateDetailedPayment(batchItems)).Return(paymentTrans);
                 catsHelper.WhenToldTo(x => x.GroupPaymentsBySourceandThenTargetBankAccounts(paymentTrans)).Return(paymentBatches);
                 serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<GenerateCatsFileCommand>
                     .Matches(y => y.PaymentBatch == paymentBatches), serviceRequestMetaData)).Return(message);
                 dataManager.WhenToldTo(x => x.GetBatchTypeInfo(batchNumber)).Return(catsPaymentBatchType);
                 dataManager.WhenToldTo(x => x.GetCATSPaymentBatchSequenceNumber()).Return(batchSequenceNumber);
             };

        private Because of = () =>
             {
                 message = commandHandler.HandleCommand(command, serviceRequestMetaData);
             };

        private It Should_get_payments_batch_type_info_by_batch_number = () =>
         {
             dataManager.WasToldTo(x => x.GetBatchTypeInfo(batchNumber));
         };

        private It Should_get_payments_batch_line_items_by_batch_number = () =>
         {
             dataManager.WasToldTo(x => x.GetPaymentBatchLineItemsByBatchKey(batchNumber));
         };

        private It Should_generate_cats_payments = () =>
         {
             catsHelper.WasToldTo(x => x.GenerateDetailedPayment(batchItems));
         };

        private It Should_group_disbursements_by_source_bank_account__number = () =>
         {
             catsHelper.WasToldTo(x => x.GroupPaymentsBySourceandThenTargetBankAccounts(paymentTrans));
         };

        private It Should_add_timestamp_to_filename = () =>
         {
             catsHelper.WasToldTo(x => x.TimestampFileName(catsAppConfigSettings.CATSOutputFileLocation + fullFileNamePrefix));
         };

        private It should_get_batch_sequence_number = () =>
         {
             dataManager.WasToldTo(x => x.GetCATSPaymentBatchSequenceNumber());
         };

        private It Should_generate_a_cats_file_from_cats_payment_batcheis = () =>
         {
             serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<GenerateCatsFileCommand>
                 .Matches(y => y.PaymentBatch == paymentBatches
                 && y.CATsEnvironment.Equals(CATsEnvironment.Live)
                 && y.OutputFileName.Equals(timestampedfullFileName)
                 && y.FileSequenceNo == batchSequenceNumber
                 && y.Profile.Equals(SSVSUserID)), serviceRequestMetaData));
         };

        private It Should_update_batch_status_to_processed = () =>
         {
             dataManager.WasToldTo(x => x.CloseCatsPaymentBatch(batchNumber, (int)CATSPaymentBatchStatus.Processed
                 , batchSequenceNumber, Path.GetFileName(timestampedfullFileName)));
         };

        private It Should_set_the_batch_sequence_number = () =>
         {
             dataManager.WasToldTo(x => x.SetCatsPaymentBatchSequenceNumber(batchSequenceNumber));
         };

        private It Should_raise_a_cats_batch_processed_event = () =>
         {
             eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<CATSPaymentBatchProcessedEvent>(), batchNumber, (int)GenericKeyType.CATSPaymentBatch, serviceRequestMetaData));
         };

        private It Should_not_return_error_messages = () =>
         {
             message.HasErrors.ShouldBeFalse();
         };
    }
}