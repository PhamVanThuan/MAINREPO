using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;

namespace SAHL.Services.Interfaces.CATS.Specs.Models.Payments
{
    public class when_creating_a_payment_with_a_valid_amount : WithFakes
    {
        private static decimal amount;
        private static Payment payment;
        private static BankAccountDataModel bankAccountDataModel;
        private static Exception exception;

        private Establish context = () =>
        {
            amount = 12345;
            bankAccountDataModel = new BankAccountDataModel("", "", 123456789, "", "VP", DateTime.Now);
        };

        private Because of = () =>
        {
            exception = Catch.Exception(() =>
            {
                payment = new Payment(bankAccountDataModel, amount, "SomeReference");
            });
        };

        private It should_contain_no_errors = () =>
         {
             exception.ShouldBeNull();
         };
    }
}
