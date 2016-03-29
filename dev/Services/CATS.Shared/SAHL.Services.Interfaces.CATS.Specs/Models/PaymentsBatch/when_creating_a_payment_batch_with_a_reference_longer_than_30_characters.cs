using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.CATS.Specs.Payments.PaymentsBatch
{
    public class when_creating_a_paymentBatch_with_a_reference_longer_than_30_characters : WithFakes
    {
        private static BankAccountDataModel bankAccountDataModel;
        private static decimal Amount;
        private static string Reference;
        private static Exception exception;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;

        private Establish context = () =>
        {
            Amount = 123456789M;
            Reference = "".PadRight(31, 'l');
            bankAccountDataModel = new BankAccountDataModel(1, "", "", 12091992, "", "VP", DateTime.Now);
            payments = new List<Payment>() { new Payment(bankAccountDataModel, 11m, "".PadRight(30)) };
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                paymentBatch = new PaymentBatch(null,bankAccountDataModel,Amount,Reference);
            });
        };

        private It should_throw_an_long_reference_exception = () =>
        {
            exception.ShouldContainErrorMessage("Payment batch reference length should not be longer than 30 characters.");
        };


    }
}
