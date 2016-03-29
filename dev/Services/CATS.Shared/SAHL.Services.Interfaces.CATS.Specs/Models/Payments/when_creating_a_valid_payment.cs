using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;
using System;

namespace SAHL.Services.Interfaces.CATS.Specs.Models.Payments
{
    public class when_creating_a_valid_payment : WithFakes
    {
        private static decimal amount;
        private static Payment payment;
        private static BankAccountDataModel bankAccountDataModel;
        private static Exception exception;

        private Establish context = () =>
        {
            amount = 5000000;
            bankAccountDataModel = new BankAccountDataModel("","",120992,"","VP",DateTime.Now);
        };

        private Because of = () =>
         {
             exception = Catch.Exception(() =>
             {
                payment = new Payment(bankAccountDataModel, amount, "SomeReference");
             });
         };

        private It should_not_throw_an_appropriate_exception = () =>
        {
            exception.ShouldBeNull();
        };
    }
}
