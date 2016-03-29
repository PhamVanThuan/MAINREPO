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
    public class when_acb_type_number_is_longer_than_1_character : WithFakes
    {
        private static BankAccountDataModel bankAccountDataModel;
        private static PaymentBatch paymentBatch;
        private static List<Payment> payments;
        private static ISystemMessageCollection messages;
        private static SourceBankAccountDetailsShouldFollowCatsFormatRule rule;
        private static int acbTypeNumber;


        private Establish context = () =>
        {
            acbTypeNumber = 11;
            bankAccountDataModel = new BankAccountDataModel(1,"50024","03214001", acbTypeNumber,"Tom", "Tom", DateTime.Now);
            payments = new List<Payment>() { new Payment(bankAccountDataModel, 11m, "".PadRight(30)) };
            messages = SystemMessageCollection.Empty();
            rule = new SourceBankAccountDetailsShouldFollowCatsFormatRule();
            paymentBatch = new PaymentBatch(payments, bankAccountDataModel, 123, "SomeReference");
            
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, paymentBatch);
        };

        private It should_return_an_acb_type_number_out_of_range_error_message = () =>
        {
            messages.ErrorMessages()
                .Where(X => X.Message.Equals("Invalid acb type number: " + acbTypeNumber + ". The source account's acb type number should be in the range of [0 - 9]."))
            .ShouldNotBeEmpty();
        };

    }
}
