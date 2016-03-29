using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.DebitOrderDayCannotBeAfterThe28thDay
{
    public class when_the_debit_day_is_prior_to_the_28th_day : WithFakes
    {
        private static IDomainRuleManager<ApplicationDebitOrderModel> debitOrderRuleContext;
        private static DebitOrderDayCannotBeAfterThe28thDayRule debitOrderRule;
        private static ApplicationDebitOrderModel debitOrder;
        private static SystemMessageCollection messages;
        private static int expectedApplicationNumber = 1234567, expectedDebitOrderDay = 27, expectedClientBankAccountKey = 24189247;

        private Establish context = () =>
        {
            debitOrderRuleContext = new DomainRuleManager<ApplicationDebitOrderModel>();
            debitOrderRule = new DebitOrderDayCannotBeAfterThe28thDayRule();
            debitOrderRuleContext.RegisterRule(debitOrderRule);
            debitOrder = new ApplicationDebitOrderModel(expectedApplicationNumber, expectedDebitOrderDay, expectedClientBankAccountKey);
            messages = new SystemMessageCollection();
        };

        private Because of = () =>
        {
            debitOrderRuleContext.ExecuteRules(messages, debitOrder);
        };

        private It should_have_not_return_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

    }
}