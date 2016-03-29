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
    public class when_bank_account_data_model_is_valid : WithFakes
    {
        private static BankAccountDataModel bankAccountDataModel;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;
        private static ISystemMessageCollection messages;
        private static SourceBankAccountDetailsShouldFollowCatsFormatRule rule;


        private Establish context = () =>
        {
            bankAccountDataModel = new BankAccountDataModel(1,"510002", "032251401", 1, "Tom", "Tom", DateTime.Now);
            payments = new List<Payment>() { new Payment(bankAccountDataModel, 11m, "".PadRight(30)) };
            messages = SystemMessageCollection.Empty();
            rule = new SourceBankAccountDetailsShouldFollowCatsFormatRule();
            paymentBatch = new PaymentBatch(payments, bankAccountDataModel, 123, "SomeReference");
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, paymentBatch);
        };

        private It should_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

    }
}
