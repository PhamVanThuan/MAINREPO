using System;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.AphaHousingMustNotBeInterestOnly
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan))]
    public class when_application_is_alpha_housing_and_has_interest_only : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan>
    {
        protected static IApplication application;

        Establish Context = () =>
        {
            application = An<IApplication>();
			application.WhenToldTo(x => x.HasFinancialAdjustment(Globals.FinancialAdjustmentTypeSources.InterestOnly)).Return(true);
            application.WhenToldTo(x => x.IsAlphaHousing()).Return(true);

			businessRule = new BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}