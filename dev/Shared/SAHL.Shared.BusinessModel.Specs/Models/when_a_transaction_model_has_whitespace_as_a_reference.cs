using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Shared.BusinessModel.Models;

namespace SAHL.Shared.BusinessModel.Specs.Models
{
    public class when_a_transaction_model_has_whitespace_as_a_reference : WithFakes
    {
        private static string reference;
        private static PostTransactionModel transactionModel;
        private static Exception ex;

        private Establish context = () =>
         {
             reference = "      ";
         };

        private Because of = () =>
         {
             ex = Catch.Exception(() =>
             {
                 transactionModel = new PostTransactionModel(1, 1, 500.00M, DateTime.Now, reference, "userId");
             });
         };

        private It should_throw_an_exception_when_constructing_the_model = () =>
         {
             ex.Message.ShouldEqual("Reference is required");
         };
    }
}