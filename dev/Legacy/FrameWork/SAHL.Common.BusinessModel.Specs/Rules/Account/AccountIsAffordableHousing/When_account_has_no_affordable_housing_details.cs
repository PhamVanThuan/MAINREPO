using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Account.AccountIsAlphaHousing
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing))]
    public class When_account_has_no_alpha_housing_details : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing>
    {
        private static IAccount account;

        Establish context = () =>
        {
            account = An<IAccount>();

            IDetailClass aDetailClass = An<IDetailClass>();

            IDetailType aDetailType = An<IDetailType>();
            aDetailType.WhenToldTo(x => x.DetailClass).Return(aDetailClass);
            IDetail aDetail = An<IDetail>();
            aDetail.WhenToldTo(x => x.DetailType).Return(aDetailType);

            IEventList<IDetail> details = new EventList<IDetail>(new IDetail[] { aDetail });

            account.WhenToldTo(x => x.Details).Return(details);

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