using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;

namespace SAHL.Services.Interfaces.CATS.Specs.Payments.PaymentsBatch
{
    public class when_creating_a_payment_batch_without_a_BankAccountDataModel : WithFakes
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
            Reference = "SomeReference";
            bankAccountDataModel = null;

        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {

                paymentBatch = new PaymentBatch(payments, bankAccountDataModel, Amount,Reference);
            });
        };

        private It should_throw_an_appropriate_exception = () =>
        {
            exception.ShouldContainErrorMessage("A source bank account should be provided.");
        };


    }
}
