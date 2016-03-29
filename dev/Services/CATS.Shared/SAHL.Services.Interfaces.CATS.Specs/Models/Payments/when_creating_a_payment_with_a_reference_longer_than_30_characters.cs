using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.Interfaces.CATS.Models;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.Interfaces.CATS.Specs.Models.Payments
{
    public class when_creating_a_payment_with_a_reference_longer_than_30_characters : WithFakes
    {
        private static string reference;
        private static Payment payment;
        private static BankAccountDataModel bankAccountDataModel;
        private static decimal amount;
        private static Exception ex;

        private Establish context = () =>
        {
            amount = 120992.0M;
            bankAccountDataModel = new BankAccountDataModel(1, "", "", 12091992, "", "VP", DateTime.Now);
            reference = "".PadRight(31, 'l');

        };

        private Because of = () =>
        {
            ex = Catch.Exception(() =>
            {
                payment = new Payment(bankAccountDataModel, amount, reference);
            });
        };

        private It should_throw_an_appropriate_exception = () =>
        {
            ex.ShouldContainErrorMessage("Payment reference length should not be longer than 30 characters.");
        };
    }        
}
