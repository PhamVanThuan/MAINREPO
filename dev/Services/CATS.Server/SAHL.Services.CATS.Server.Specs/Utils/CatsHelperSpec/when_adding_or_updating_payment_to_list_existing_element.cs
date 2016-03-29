using Machine.Specifications;
using NUnit.Framework;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Models;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Services.CATS.Managers.CATS;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.HandlerSpec
{
    public class when_adding_or_updating_payment_to_list_existing_element : WithCoreFakes
    {
        private static ICATSManager catsHelper;
        private static ISystemMessageCollection message;
        private static List<Payment> payments;
        private static List<Payment> results;
        private static DetailedPayment paymentTran;
        private static int soucreAccountKey;
        private static int targetAccountKey;
        private static BankAccountDataModel soucreAccount, targetAccount;
        private static ICATSDataManager catsDataManager;
        private static string targetAccountReference, sourceAccountReference;

        private Establish context = () =>
            {
                payments = new List<Payment>();
                targetAccountReference = "External Reference";
                sourceAccountReference = "SAHL      SPV 32";
                soucreAccount = new BankAccountDataModel("632005", "9212020000", (int)ACBType.Current, "P. Miller", null, null);
                targetAccount = new BankAccountDataModel("632005", "9212020222", (int)ACBType.Current, "P. Miller", null, null);

                soucreAccountKey = 1001;
                targetAccountKey = 11110;
                catsDataManager = An<ICATSDataManager>();
                catsHelper = new CATSManager(catsDataManager, An<IFileSystem>(), An<ICatsAppConfigSettings>());
                message = SystemMessageCollection.Empty();
                var CATSPaymentBatchItemDataModel1 = new CATSPaymentBatchItemDataModel(1, 54, 1114, 21.0m, soucreAccountKey, targetAccountKey, 41, "SAHL-90",
                    sourceAccountReference, "Straus Daly", targetAccountReference, "strauss@sd.co.za", 2004, true);

                paymentTran = new DetailedPayment(CATSPaymentBatchItemDataModel1, soucreAccount, targetAccount);

            };

        private Because of = () =>
            {
                results = catsHelper.AddPaymentToPaymentsListSummedByTargetBankAccount(payments, paymentTran, targetAccountReference);
            };

        private It Should_add_a_payment = () =>
        {
            results.Count().ShouldEqual(1);
        };

        private It Should_have_a_correct_source_reference = () =>
        {
            results.Any(x => x.Amount == paymentTran.Amount && x.Reference.Equals(targetAccountReference)
                && x.TargetAccount == targetAccount).ShouldBeTrue();
        };
    }
}