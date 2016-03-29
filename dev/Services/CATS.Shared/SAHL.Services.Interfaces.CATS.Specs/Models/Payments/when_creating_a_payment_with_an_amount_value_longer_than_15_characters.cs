using Machine.Specifications;
using Machine.Fakes;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.Interfaces.CATS.Specs.Models.Payments
{
    public class when_creating_a_payment_with_an_amount_value_longer_than_15_characters : WithFakes
    {
        private static decimal amount;
        private static Payment payment;
        private static Exception exception;
        private static BankAccountDataModel bankAccountDataModel;

        private Establish context = () =>
        {
            amount = 1234567891234567M;
            bankAccountDataModel = new BankAccountDataModel("","",120992,"VP","",DateTime.Now);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                payment = new Payment(bankAccountDataModel,amount, "".PadRight(30));
            });
        };

        private It should_throw_an_appropriate_exception = () =>
        {
            exception.ShouldContainErrorMessage("The Amount must be between 1 and 999999999999999.");
        };
    }
}
