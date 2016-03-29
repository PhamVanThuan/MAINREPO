using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Shared.BusinessModel.Managers;
using SAHL.Shared.BusinessModel.Models;
using SAHL.Shared.BusinessModel.Rules;
using System.Linq;

namespace SAHL.Shared.BusinessModel.Specs.Rules.FinancialTransactionServiceKeyMustExistSpec
{
    public class when_financial_transaction_service_key_exists : WithFakes
    {
        private static ITransactionDataManager transactionDataManager;
        private static FinancialTransactionServiceKeyMustExistRule rule;
        private static TransactionRuleModel ruleModel;
        private static ISystemMessageCollection messages;

        Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            transactionDataManager = An<ITransactionDataManager>();
            rule = new FinancialTransactionServiceKeyMustExistRule(transactionDataManager);
            var transaction = new ReversalTransactionModel(1408, "SystemUser");
            ruleModel = new TransactionRuleModel { TransactionKey = transaction.FinancialTransactionKey };

            transactionDataManager.WhenToldTo(x => x.DoesFinancialTransactionKeyExist(ruleModel.TransactionKey)).Return(true);
        };

        Because of = () =>
        {
            rule.ExecuteRule(messages, ruleModel);
        };

        It should_check_if_the_financial_transaction_key_exists = () =>
        {
            transactionDataManager.WasToldTo(x => x.DoesFinancialTransactionKeyExist(ruleModel.TransactionKey));
        };

        It should_not_return_any_messages = () =>
        {
            messages.AllMessages.Count().ShouldEqual(0);
        };
    }
}
