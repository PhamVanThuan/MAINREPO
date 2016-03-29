using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.DebitOrderDayCannotBeAfterThe28thDay
{
    public class when_the_debit_order_day_is_after_the_28th : WithFakes
    {
        private static IDomainRuleManager<ApplicationDebitOrderModel> debitOrderRuleContext;
        private static DebitOrderDayCannotBeAfterThe28thDayRule debitOrderRule;
        private static ApplicationDebitOrderModel debitOrder;
        private static SystemMessageCollection actualMessages;
        private static SystemMessage expectedMessage;
        private static string expectedErrorMessage = "Debit Order Day must be between 1 and 28 days.";
        private static int expectedApplicationNumber = 1234567, expectedDebitOrderDay = 29, expectedClientBankAccountKey = 24189247;

        private Establish context = () =>
        {
            debitOrderRuleContext = new DomainRuleManager<ApplicationDebitOrderModel>();
            expectedMessage = new SystemMessage(expectedErrorMessage, SystemMessageSeverityEnum.Error);
            debitOrderRule = new DebitOrderDayCannotBeAfterThe28thDayRule();
            debitOrderRuleContext.RegisterRule(debitOrderRule);
            debitOrder = new ApplicationDebitOrderModel(expectedApplicationNumber, expectedDebitOrderDay, expectedClientBankAccountKey);
            actualMessages = new SystemMessageCollection();
        };

        private Because of = () =>
        {
            debitOrderRuleContext.ExecuteRules(actualMessages, debitOrder);
        };

        private It should_have_error_messages = () =>
        {
            actualMessages.HasErrors.ShouldBeTrue();
        };

        private It should_have_correct_error_message = () =>
        {
            actualMessages.AllMessages.First().Message.ShouldEqual(expectedMessage.Message);
        };
    }
}