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
    public class when_generating_payment_trans_account_names_are_longer_than_30_char : WithCoreFakes
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
                 soucreAccount = new BankAccountDataModel("632005", "9212020000", (int)ACBType.Current, "The Thekwini Warehousing Conduit (Pty) Ltd", null, null);
                 targetAccount = new BankAccountDataModel("632005", "9212020222", (int)ACBType.Current, "Client's account name that too long", null, null);

                 soucreAccountKey = 1001;
                 targetAccountKey = 11110;
                 catsDataManager = An<ICATSDataManager>();
                 catsAppConfigSettings = An<ICatsAppConfigSettings>();
                 catsHelper = new CATSManager(catsDataManager, An<IFileSystem>(), An<ICatsAppConfigSettings>());
                 message = SystemMessageCollection.Empty();
                 cATSPaymentBatchItemDataModel = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, soucreAccountKey, targetAccountKey, 41, "SAHL", "SAHL      SPV 32", "Straus Daly",
                     "External Reference", "strauss@sd.co.za", 2004, true);

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

        private It Should_tractate_account_namesrecords = () =>
         {
             var paymentTran = results.FirstOrDefault();
             Assert.True(paymentTran.SourceBankAccount.AccountName.Length == 30
                 && paymentTran.TargetBankAccount.AccountName.Length == 30);
         };
    }
}