using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Employment.SalaryPayDayMandatory
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Employment.EmploymentSalaryPayDayMandatory))]
    public class When_unemployed_should_pass : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Employment.EmploymentSalaryPayDayMandatory>
    {
        protected static IEmploymentUnemployed employment;

        Establish Context = () =>
            {
                employment = An<IEmploymentUnemployed>();

                businessRule = new BusinessModel.Rules.Employment.EmploymentSalaryPayDayMandatory();
                RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Employment.EmploymentSalaryPayDayMandatory>.startrule.Invoke();
            };
        Because of = () =>
            {
                businessRule.ExecuteRule(messages, employment);
            };
        It should_pass = () =>
            {
                messages.Count.ShouldEqual(0);
            };
    }
}
