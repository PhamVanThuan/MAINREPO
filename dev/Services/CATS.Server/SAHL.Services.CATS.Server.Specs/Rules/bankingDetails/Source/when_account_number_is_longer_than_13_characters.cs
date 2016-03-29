using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.SystemMessages;
using SAHL.Services.CATS.Rules;
using SAHL.Services.CATS.Utils;
using SAHL.Services.Interfaces.CATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.CATS.Server.Specs.Rules.bankingDetails
{
    public class when_account_number_is_longer_than_13_characters : WithFakes
    {
        private static BankAccountDataModel bankAccountDataModel;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;
        private static ISystemMessageCollection messages;
        private static SourceBankAccountDetailsShouldFollowCatsFormatRule rule;
        private static string accountNumber;


        private Establish context = () =>
        {
            accountNumber = "".PadRight(17, '5');
            bankAccountDataModel = new BankAccountDataModel(1,"50024", accountNumber, 1, "Tom", "Tom", DateTime.Now);
            payments = new List<Payment>() { new Payment(bankAccountDataModel, 11m, "".PadRight(30)) };
            messages = SystemMessageCollection.Empty();
            rule = new SourceBankAccountDetailsShouldFollowCatsFormatRule();
            paymentBatch = new PaymentBatch(payments, bankAccountDataModel, 123, "SomeReference");
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, paymentBatch);
        };

        private It should_return_an_account_number_out_of_range_error_message = () =>
        {
            messages.ErrorMessages()
                .Where(X => X.Message.Equals("Invalid account number: " + accountNumber + ". The source account's account number should not be longer than 13 characters."))
            .ShouldNotBeEmpty();
        };

    }
}
