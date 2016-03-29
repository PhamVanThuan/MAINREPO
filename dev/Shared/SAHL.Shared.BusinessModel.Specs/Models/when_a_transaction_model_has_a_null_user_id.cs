using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Specs.Models
{
    public class when_a_transaction_model_has_a_null_user_id : WithFakes
    {
        private static string userId;
        private static PostTransactionModel transactionModel;
        private static Exception ex;

        private Establish context = () =>
         {
             userId = null;
         };

        private Because of = () =>
         {
             ex = Catch.Exception(() => {
                 transactionModel = new PostTransactionModel(1, 1, 500.00M, DateTime.Now, "reference", userId);
             });
         };

        private It should_throw_an_exception_when_constructing_the_model = () =>
         {
             ex.Message.ShouldEqual("UserId is required");
         };

    }
}