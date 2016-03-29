using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;
using System;

namespace SAHL.Services.Interfaces.CATS.Specs.Models.Payments
{
    public class when_creating_a_payment_with_an_invalid_amount : WithFakes
    {
        private static Payment payment;
        private static BankAccountDataModel bankAccountDataModel;
        private static decimal amount;
        private static Exception exception;

        private Establish context = () =>
        {
            bankAccountDataModel = new BankAccountDataModel("", "", 120992, "VP", "", DateTime.Now);
            amount = -123456789M;
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                payment = new Payment(bankAccountDataModel, amount, "".PadRight(30));
            });
        };

        private It should_throw_an_appropriate_error = () =>
        {
            exception.ShouldContainErrorMessage("The Amount must be between 1 and 999999999999999.");
        };
    }
}