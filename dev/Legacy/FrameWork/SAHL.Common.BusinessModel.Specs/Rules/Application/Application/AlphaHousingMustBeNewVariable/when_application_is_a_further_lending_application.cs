using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.AlphaHousingMustBeNewVariable
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan))]
    public class when_application_is_a_further_lending_application : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan>
    {
        protected static IApplication application;
        Establish Context = () =>
        {
            application = An<IApplicationFurtherLending>();
            businessRule = new BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan>.startrule.Invoke();

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
