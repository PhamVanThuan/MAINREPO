using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Machine.Specifications;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.UnsecuredLendingConfirmIncomeSpecs
{
    [Subject(typeof(UnsecuredLendingConfirmIncome))]
    public class When_legalentitys_income_is_confirmed : UnsecuredLendingConfirmIncomeSpecBase
    {
        Establish context = () =>
            {
                var applicationUnsecuredLending = CreateMockedApplicationUnsecuredLending(confirmedIncome : true);

                businessRule = new UnsecuredLendingConfirmIncome();

                parameters = new object[]
                    {
                        applicationUnsecuredLending
                    };

                RulesBaseWithFakes<UnsecuredLendingConfirmIncome>.startrule.Invoke();
            };

        Because of = () =>
            {
                businessRule.ExecuteRule(messages, parameters);
            };

        It should_not_return_any_domain_messages = () =>
            {
                messages.Count.ShouldEqual(0);
            };
    }
}
