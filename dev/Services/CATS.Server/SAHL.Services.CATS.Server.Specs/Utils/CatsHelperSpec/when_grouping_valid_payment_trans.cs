using Machine.Specifications;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Models;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    public class when_grouping_valid_payment_trans : WithCoreFakes
    {
        private static ICATSManager catsHelper;
        private static List<PaymentBatch> results;
        private static ISystemMessageCollection message;
        private static List<DetailedPayment> paymentTrans;
        private static ICatsAppConfigSettings catsAppConfigSettings;
        private static ICATSDataManager catsDataManager;
        private static int soucreAccountKey;
        private static int soucreAccountKey1;
        private static int targetAccountKey;
        private static BankAccountDataModel soucreAccount, soucreAccount1, targetAccount;
        private static string targetAccountReference, sourceAccountReference, sourceAccountReference1;

        private Establish context = () =>
             {
                 targetAccountReference = "External Reference";
                 sourceAccountReference = "SAHL      SPV 32";
                 sourceAccountReference1 = "SAHL      SPV 31";
                 soucreAccount = new BankAccountDataModel("632005", "9212020000", (int)ACBType.Current, "P. Miller", null, null);
                 soucreAccount1 = new BankAccountDataModel("632005", "9212020111", (int)ACBType.Current, "P. Miller", null, null);
                 targetAccount = new BankAccountDataModel("632005", "9212020222", (int)ACBType.Current, "P. Miller", null, null);

                 soucreAccountKey = 1001;
                 soucreAccountKey1 = 1002;
                 targetAccountKey = 11110;
                 catsDataManager = An<ICATSDataManager>();
                 catsAppConfigSettings = An<ICatsAppConfigSettings>();
                 catsHelper = new CATSManager(catsDataManager, An<IFileSystem>(), An<ICatsAppConfigSettings>());
                 message = SystemMessageCollection.Empty();
                 var CATSPaymentBatchItemDataModel1 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, soucreAccountKey, targetAccountKey, 41, "SAHL-90",
                     sourceAccountReference, "Straus Daly", targetAccountReference, "strauss@sd.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel2 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 23.0m, soucreAccountKey1, targetAccountKey, 41, "SAHL-90",
                     sourceAccountReference1, "Straus Daly", targetAccountReference, "strauss@sd.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel3 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, soucreAccountKey1, targetAccountKey, 41, "SAHL-90",
                     sourceAccountReference1, "Straus Daly", targetAccountReference, "strauss@sd.co.za", 2004, true);
                 var CATSPaymentBatchItemDataModel4 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 26.0m, soucreAccountKey, targetAccountKey, 41, "SAHL-90",
                     sourceAccountReference, "Straus Daly", targetAccountReference, "strauss@sd.co.za", 2004, true);

                 paymentTrans = new List<DetailedPayment>()
                 {
                    new  DetailedPayment(CATSPaymentBatchItemDataModel1, soucreAccount, targetAccount)
                    ,new  DetailedPayment(CATSPaymentBatchItemDataModel2, soucreAccount1, targetAccount)
                    ,new  DetailedPayment(CATSPaymentBatchItemDataModel3, soucreAccount1, targetAccount)
                    ,new  DetailedPayment(CATSPaymentBatchItemDataModel4, soucreAccount, targetAccount)
                 };
             };

        private Because of = () =>
             {
                 results = catsHelper.GroupPaymentsBySourceandThenTargetBankAccounts(paymentTrans);
             };

        private It Should_have_a_correct_batch_amount = () =>
         {
             Assert.True(results.Where(x => x.SourceAccount == soucreAccount).FirstOrDefault().Amount == 47.0m);
             Assert.True(results.Where(x => x.SourceAccount == soucreAccount1).FirstOrDefault().Amount == 44.0m);
         };

        private It Should_have_a_correct_source_reference = () =>
         {
             Assert.True(results.Where(x => x.SourceAccount == soucreAccount).FirstOrDefault().Reference.Equals(sourceAccountReference));
             Assert.True(results.Where(x => x.SourceAccount == soucreAccount1).FirstOrDefault().Reference.Equals(sourceAccountReference1));
         };

        private It Should_have_a_correct_target_reference = () =>
         {
             Assert.False(results.Where(x => x.SourceAccount == soucreAccount).FirstOrDefault().Payments.Any(y => y.Reference.Equals(targetAccountReference)));
             Assert.False(results.Where(x => x.SourceAccount == soucreAccount1).FirstOrDefault().Payments.Any(y => y.Reference.Equals(targetAccountReference)));
         };

        private It Should_have_a_correct_number_of_payments_in_each_batch = () =>
         {
             Assert.True(results.Where(x => x.SourceAccount == soucreAccount).FirstOrDefault().Payments.Count() == 1);
             Assert.True(results.Where(x => x.SourceAccount == soucreAccount1).FirstOrDefault().Payments.Count() == 1);
         };

        private It Should_two_payment_batches = () =>
         {
             results.Count().Equals(2).ShouldBeTrue();
         };
    }
}