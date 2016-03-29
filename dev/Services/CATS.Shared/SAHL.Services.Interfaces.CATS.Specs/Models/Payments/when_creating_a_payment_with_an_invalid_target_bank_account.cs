using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Interfaces.CATS.Models;
using System;

namespace SAHL.Services.Interfaces.CATS.Specs.Models.Payments
{
    public class when_creating_a_payment_with_an_invalid_target_bank_account : WithFakes
    {
        private static Payment payment;
        private static BankAccountDataModel bankAccountDataModel;
        private static Exception error;
      
        private Establish context = () =>
        {
            bankAccountDataModel = new BankAccountDataModel("","",120992,"VP","",DateTime.Now);     
        };

        private Because of = () =>
        {
            error = Catch.Exception(() =>
            {
                payment = new Payment(null, 120992, "".PadRight(30));
            });
        };

        private It should_throw_an_appropriate_exception = () =>
        {
            error.ShouldContainErrorMessage("Payment Target bank account should not be provided.");
        };
    }
}
