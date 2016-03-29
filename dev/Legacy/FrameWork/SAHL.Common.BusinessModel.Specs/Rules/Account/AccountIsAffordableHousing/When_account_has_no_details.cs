using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.Account.AccountIsAlphaHousing
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing))]
    public class When_account_has_no_details : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing>
    {
        private static IAccount account;

        Establish context = () =>
        {
            account = An<IAccount>();
            businessRule = new BusinessModel.Rules.Account.AccountIsAlphaHousing();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, account);
        };

        It should_not_add_any_messages = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}