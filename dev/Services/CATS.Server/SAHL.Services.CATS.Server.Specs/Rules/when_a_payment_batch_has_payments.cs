using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Rules;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.CATS.Server.Specs.Rules
{
    public class when_a_payment_batch_has_payments : WithFakes
    {
        private static BankAccountDataModel bankAccountDataModel;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;
        private static ISystemMessageCollection messages;
        private static APaymentBatchShouldHaveAtleastOnePaymentRule rule;

        private Establish context = () =>
        {
            bankAccountDataModel = new BankAccountDataModel(1, "", "", 12091992, "", "VP", DateTime.Now);
            payments = new List<Payment>() { new Payment(bankAccountDataModel, 11m, "".PadRight(30)) };
            messages = SystemMessageCollection.Empty();
            rule = new APaymentBatchShouldHaveAtleastOnePaymentRule();
            paymentBatch = new PaymentBatch(payments, bankAccountDataModel, 123, "SomeReference");
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, paymentBatch);
        };

        private It should_return_an_error_message = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };
    }
}