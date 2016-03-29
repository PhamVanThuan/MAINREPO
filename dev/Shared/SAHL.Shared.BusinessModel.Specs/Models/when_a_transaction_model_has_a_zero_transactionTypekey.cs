using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Specs.Models
{
    public class when_a_transaction_model_has_a_zero_transactiontypekey : WithFakes
    {
        private static int transactionTypeKey;
        private static PostTransactionModel transactionModel;
        private static Exception ex;

        private Establish context = () =>
         {
             transactionTypeKey = 0;
         };

        private Because of = () =>
         {
             ex = Catch.Exception(() => {
                 transactionModel = new PostTransactionModel(1, transactionTypeKey, 500.00M, DateTime.Now, "reference", "userId");
             });
         };

        private It should_throw_an_exception_when_constructing_the_model = () =>
         {
             ex.Message.ShouldEqual("TransactionTypeKey must be greater than 0");
         };

    }
}