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
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    public class when_processing_generating_a_file_returns_errors : WithCoreFakes
    {
        private static ProcessCATSPaymentBatchCommandHandler commandHandler;
        private static ProcessCATSPaymentBatchCommand command;
        private static ICATSManager catsHelper;
        private static int batchNumber;
        private static ICATSDataManager dataManager;
        private static ISystemMessageCollection errormessages;
        private static List<CATSPaymentBatchItemDataModel> batchItem;
        private static List<DetailedPayment> paymentTrans;
        private static List<PaymentBatch> paymentBatches;
        private static CATsEnvironment catsEnvironment;
        private static string fullFileNamePrefix, timestampedfullFileName, fullFileLocation;
        private static string SSVSUserID;
        private static ICatsAppConfigSettings catsAppConfigSettings;

        private Establish context = () =>
             {
                 catsAppConfigSettings = An<ICatsAppConfigSettings>();
                 eventRaiser = An<IEventRaiser>();
                 fullFileLocation = @"c:\\temp";
                 fullFileNamePrefix = "filename";
                 timestampedfullFileName = fullFileLocation + fullFileNamePrefix + "201509121200";
                 catsEnvironment = CATsEnvironment.Live;
                 SSVSUserID = "SHL04";
                 batchNumber = 10001;
                 catsHelper = An<ICATSManager>();
                 serviceRequestMetaData = new ServiceRequestMetadata();
                 serviceCommandRouter = An<IServiceCommandRouter>();
                 dataManager = An<ICATSDataManager>();
                 command = new ProcessCATSPaymentBatchCommand(batchNumber);
                 commandHandler = new ProcessCATSPaymentBatchCommandHandler(dataManager, serviceCommandRouter, catsHelper, eventRaiser, catsAppConfigSettings);
                 var catsPaymentBatchType = new CATSPaymentBatchTypeDataModel("ThirdpartyInvoice", SSVSUserID, fullFileNamePrefix, (int)catsEnvironment, 1);
                 dataManager.WhenToldTo(x => x.GetBatchTypeInfo(batchNumber)).Return(catsPaymentBatchType);
                 messages = SystemMessageCollection.Empty();
                 errormessages = SystemMessageCollection.Empty();
                 errormessages.AddMessage(new SystemMessage("error occurred", SystemMessageSeverityEnum.Error));
                 var CATSPaymentBatchItemDataModel1 = new CATSPaymentBatchItemDataModel(1, 54, 112, 21.0m, 1001, 11110, batchNumber, "SAHLr", "SAHL      SPV 32", "Straus Dally", "Straus Dally", "straus@sd.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel2 = new CATSPaymentBatchItemDataModel(4, 54, 112, 21.0m, 1001, 11110, batchNumber, "SAHLr", "SAHL      SPV 32", "Straus Dally", "Straus Dally", "straus@sd.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel3 = new CATSPaymentBatchItemDataModel(5, 54, 112, 21.0m, 1001, 11110, batchNumber, "SAHLr", "SAHL      SPV 32", "Straus Dally", "Straus Dally", "straus@sd.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel4 = new CATSPaymentBatchItemDataModel(8, 54, 112, 21.0m, 1001, 11110, batchNumber, "SAHLr", "SAHL      SPV 32", "Straus Dally", "Straus Dally", "straus@sd.co.za", 2004, true);
                 batchItem = new List<CATSPaymentBatchItemDataModel>()
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

                 dataManager.WhenToldTo(x => x.GetPaymentBatchLineItemsByBatchKey(batchNumber)).Return(batchItem);
                 catsHelper.WhenToldTo(x => x.TimestampFileName(catsAppConfigSettings.CATSOutputFileLocation + fullFileNamePrefix)).Return(timestampedfullFileName);
                 catsHelper.WhenToldTo(x => x.GenerateDetailedPayment(batchItem)).Return(paymentTrans);
                 catsHelper.WhenToldTo(x => x.GroupPaymentsBySourceandThenTargetBankAccounts(paymentTrans)).Return(paymentBatches);
                 serviceCommandRouter.WhenToldTo(x => x.HandleCommand(Param<GenerateCatsFileCommand>
                     .Matches(y => y.PaymentBatch == paymentBatches), serviceRequestMetaData)).Return(errormessages);
             };

        private Because of = () =>
             {
                 messages = commandHandler.HandleCommand(command, serviceRequestMetaData);
             };

        private It Should_get_payments_batch_line_items_by_batch_number = () =>
         {
             dataManager.WasToldTo(x => x.GetPaymentBatchLineItemsByBatchKey(batchNumber));
         };

        private It Should_generate_cats_payments = () =>
         {
             catsHelper.WasToldTo(x => x.GenerateDetailedPayment(batchItem));
         };

        private It Should_group_disbursements_by_source_bank_account__number = () =>
         {
             catsHelper.WasToldTo(x => x.GroupPaymentsBySourceandThenTargetBankAccounts(paymentTrans));
         };

        private It Should_add_timestamp_to_filename = () =>
         {
             catsHelper.WasToldTo(x => x.TimestampFileName(catsAppConfigSettings.CATSOutputFileLocation + fullFileNamePrefix));
         };

        private It Should_generate_a_cats_file_from_cats_payment_batcheis = () =>
         {
             serviceCommandRouter.WasToldTo(x => x.HandleCommand(Param<GenerateCatsFileCommand>
                 .Matches(y => y.PaymentBatch == paymentBatches
                 && y.CATsEnvironment.Equals(CATsEnvironment.Live)
                 && y.OutputFileName.Equals(timestampedfullFileName)
                 && y.Profile.Equals(SSVSUserID)), serviceRequestMetaData));
         };

        private It Should_update_batch_status_to_failed = () =>
         {
             dataManager.WasToldTo(x => x.CloseCatsPaymentBatch(batchNumber, (int)CATSPaymentBatchStatus.Failed, -1, timestampedfullFileName));
         };

        private It Should_not_return_error_messages = () =>
         {
             messages.ErrorMessages().Any(x => x.Message.Contains("error occurred")).ShouldBeTrue();
         };
    }
}