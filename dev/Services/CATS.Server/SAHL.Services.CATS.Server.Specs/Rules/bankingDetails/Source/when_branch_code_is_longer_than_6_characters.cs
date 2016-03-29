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
    public class when_branch_code_is_longer_than_6_characters : WithFakes
    {
        private static BankAccountDataModel bankAccountDataModel;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;
        private static ISystemMessageCollection messages;
        private static SourceBankAccountDetailsShouldFollowCatsFormatRule rule;
        private static string branchCode;


        private Establish context = () =>
        {
            branchCode = "".PadRight(7, '*');
            bankAccountDataModel = new BankAccountDataModel(1, branchCode, "032251401", 1, "Tom", "Tom", DateTime.Now);
            payments = new List<Payment>() { new Payment(bankAccountDataModel, 11m, "".PadRight(30)) };
            messages = SystemMessageCollection.Empty();
            rule = new SourceBankAccountDetailsShouldFollowCatsFormatRule();
            paymentBatch = new PaymentBatch(payments, bankAccountDataModel, 123, "SomeReference");
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, paymentBatch);
        };

        private It should_return_branch_code_too_long_error_message = () =>
        {
            messages.ErrorMessages()
                .Where(X => X.Message.Equals("Invalid branch code: " + branchCode.Trim() + ". The source account's branch code should not be longer than 6 characters."))
            .ShouldNotBeEmpty();
        };

    }
}
