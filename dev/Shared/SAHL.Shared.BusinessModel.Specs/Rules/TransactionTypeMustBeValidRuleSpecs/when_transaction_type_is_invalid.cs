using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using System;
using System.Linq;

namespace SAHL.Shared.BusinessModel.Specs.Rules.TransactionTypeMustBeValidRuleSpecs
{
    public class when_transaction_type_is_invalid : WithFakes
    {
        private static ITransactionDataManager transactionDataManager;
        private static TransactionRuleModel ruleModel;
        private static TransactionTypeMustBeValidRule rule;
        private static ISystemMessageCollection messages;
        private static string expectedMessage;

        private Establish context = () =>
        {
            transactionDataManager = An<ITransactionDataManager>();
            var transaction = new PostTransactionModel(121, 99999, 1000m, DateTime.Now.AddDays(-1), "ref2", "system");
            ruleModel = new TransactionRuleModel { TransactionTypeKey = transaction.TransactionTypeKey };
            rule = new TransactionTypeMustBeValidRule(transactionDataManager);
            expectedMessage = "TransactionType provided is not valid";
            messages = SystemMessageCollection.Empty();
            transactionDataManager.WhenToldTo(x => x.DoesTransactionTypeExist(ruleModel.TransactionTypeKey)).Return(false);

        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        private It should_check_if_transaction_type_is_valid = () =>
        {
            transactionDataManager.WasToldTo(x => x.DoesTransactionTypeExist(ruleModel.TransactionTypeKey));
        };

        private It should_return_error_message = () =>
        {
            messages.AllMessages.First().Message.ShouldContain(expectedMessage);
        };
    }
}
