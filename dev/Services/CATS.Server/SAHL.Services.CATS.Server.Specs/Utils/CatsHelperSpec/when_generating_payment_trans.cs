using Machine.Fakes;
using Machine.Specifications;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    public class when_generating_payment_trans : WithCoreFakes
    {
        private static ICATSManager catsHelper;
        private static List<DetailedPayment> results;
        private static ISystemMessageCollection message;
        private static List<CATSPaymentBatchItemDataModel> catsPaymentBatchItems;
        private static ICatsAppConfigSettings catsAppConfigSettings;
        private static ICATSDataManager catsDataManager;
        private static int soucreAccountKey;
        private static int targetAccountKey;
        private static BankAccountDataModel soucreAccount;
        private static BankAccountDataModel targetAccount;
        private static CATSPaymentBatchItemDataModel cATSPaymentBatchItemDataModel;

        private Establish context = () =>
             {
                 soucreAccount = new BankAccountDataModel("632005", "9212020000", (int)ACBType.Current, "P. Miller", null, null);
                 targetAccount = new BankAccountDataModel("632005", "9212020222", (int)ACBType.Current, "P. Miller", null, null);

                 soucreAccountKey = 1001;
                 targetAccountKey = 11110;
                 catsDataManager = An<ICATSDataManager>();
                 catsAppConfigSettings = An<ICatsAppConfigSettings>();
                 catsHelper = new CATSManager(catsDataManager, An<IFileSystem>(), An<ICatsAppConfigSettings>());
                 message = SystemMessageCollection.Empty();
                 cATSPaymentBatchItemDataModel = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, soucreAccountKey, targetAccountKey, 41, "SAHL", "SAHL      SPV 32",
                     "Straus Daly", "External Reference", "strauss@sd.co.za", 2004, true);

                 catsPaymentBatchItems = new List<CATSPaymentBatchItemDataModel>()
                 {
                    cATSPaymentBatchItemDataModel
                 };
                 catsDataManager.WhenToldTo(x => x.GetBankingAccountByKey(soucreAccountKey)).Return(soucreAccount);
                 catsDataManager.WhenToldTo(x => x.GetBankingAccountByKey(targetAccountKey)).Return(targetAccount);
             };

        private Because of = () =>
             {
                 results = catsHelper.GenerateDetailedPayment(catsPaymentBatchItems);
             };

        private It Should_get_source_bank_account = () =>
         {
             catsDataManager.WasToldTo(x => x.GetBankingAccountByKey(soucreAccountKey));
         };

        private It Should_get_target_bank_account = () =>
         {
             catsDataManager.WasToldTo(x => x.GetBankingAccountByKey(targetAccountKey));
         };

        private It Should_have_a_correct_number_of_records = () =>
         {
             Assert.True(results.Count() == catsPaymentBatchItems.Count());
         };

        private It Should_have_a_correct_records = () =>
         {
             var paymentTran = results.FirstOrDefault();
             Assert.True(paymentTran.SourceBankAccount == soucreAccount
                 && paymentTran.SourceBankAccountKey == soucreAccountKey
                 && paymentTran.TargetBankAccount == targetAccount
                 && paymentTran.GenericKey == cATSPaymentBatchItemDataModel.GenericKey
                 && paymentTran.GenericTypeKey == cATSPaymentBatchItemDataModel.GenericTypeKey
                 && paymentTran.CATSPaymentBatchKey == cATSPaymentBatchItemDataModel.CATSPaymentBatchKey
                 && paymentTran.SahlReferenceNumber == cATSPaymentBatchItemDataModel.SahlReferenceNumber
                 );
         };
    }
}