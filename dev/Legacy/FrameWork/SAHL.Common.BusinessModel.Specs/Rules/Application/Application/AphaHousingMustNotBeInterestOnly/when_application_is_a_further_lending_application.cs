using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.AphaHousingMustNotBeInterestOnly
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan))]
    public class when_application_is_a_further_lending_application : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan>
    {
        protected static IApplication application;
        Establish Context = () =>
        {
            application = An<IApplicationFurtherLending>();
            businessRule = new BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustNotBeInterestOnlyLoan>.startrule.Invoke();

        };
        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };
        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
