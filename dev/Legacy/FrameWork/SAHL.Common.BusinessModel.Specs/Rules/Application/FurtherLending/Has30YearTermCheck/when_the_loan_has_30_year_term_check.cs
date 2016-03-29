using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Application.FurtherLending;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.FurtherLending.Has30YearTermCheck
{
    [Subject(typeof(LoanHas30YearTermAndRemainingInstalmentsCheck))]
    public class when_the_loan_has_30_year_term_check : RulesBaseWithFakes<LoanHas30YearTermAndRemainingInstalmentsCheck>
    {
        private static IMortgageLoanAccount mortgageLoanAccount;
        private static IEventList<IFinancialService> financialServices;
        private static IMortgageLoan mortgageLoan;

        private Establish context = () =>
        {
            mortgageLoanAccount = An<IMortgageLoanAccount>();
            mortgageLoanAccount.WhenToldTo(x => x.IsThirtyYearTerm).Return(true);

            mortgageLoan = An<IMortgageLoan>();
            mortgageLoan.WhenToldTo(x => x.FinancialServiceType.Key).Return((int)FinancialServiceTypes.VariableLoan);
            mortgageLoan.WhenToldTo(x => x.RemainingInstallments).Return(241);

            financialServices = new EventList<IFinancialService>();
            financialServices.Add(new DomainMessageCollection(), mortgageLoan);

            mortgageLoanAccount.WhenToldTo(x => x.FinancialServices).Return(financialServices);

            businessRule = new LoanHas30YearTermAndRemainingInstalmentsCheck();
            RulesBaseWithFakes<LoanHas30YearTermAndRemainingInstalmentsCheck>.startrule.Invoke();
        };

        private Because of = () =>
        {
            RuleResult = businessRule.ExecuteRule(messages, mortgageLoanAccount);
        };

        private It should_rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}