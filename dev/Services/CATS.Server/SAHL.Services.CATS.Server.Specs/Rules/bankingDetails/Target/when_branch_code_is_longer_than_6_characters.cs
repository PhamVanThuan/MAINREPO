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

namespace SAHL.Services.CATS.Server.Specs.Rules.bankingDetails.Target
{
    public class when_branch_code_is_longer_than_6_characters : WithFakes
    {
        private static BankAccountDataModel targetBankAccountDataModel;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;
        private static ISystemMessageCollection messages;
        private static TargetBankAccountDetailsShouldFollowCatsFormatRule rule;
        private static string branchCode;


        private Establish context = () =>
        {
            branchCode = "".PadRight(7, '*');
            var bankAccountDataModel = new BankAccountDataModel(1, "5001", "032251401", 1, "Tom", "Tom", DateTime.Now);
            targetBankAccountDataModel = new BankAccountDataModel(1,branchCode, "032251401", 1, "Tom", "Tom", DateTime.Now);
            payments = new List<Payment>() { new Payment(targetBankAccountDataModel, 11m, "".PadRight(30)) };
            messages = SystemMessageCollection.Empty();
            rule = new TargetBankAccountDetailsShouldFollowCatsFormatRule();
            paymentBatch = new PaymentBatch(payments, bankAccountDataModel, 123, "SomeReference");
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, paymentBatch);
        };

        private It should_return_branch_code_too_long_error_message = () =>
        {
            messages.ErrorMessages()
                .Where(X => X.Message.Equals("Invalid branch code: " + branchCode + ". The target account's branch code should not be longer than 6 characters."))
            .ShouldNotBeEmpty();
        };

    }
}
