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
    public class When_account_has_alpha_housing_details : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing>
    {
        private static IAccount account;

        Establish context = () =>
            {
                account = An<IAccount>();

                IDetailClass alphaHousingDetailClass = An<IDetailClass>();
                alphaHousingDetailClass.WhenToldTo(x => x.Key).Return((int)DetailClasses.AlphaHousing);
                IDetailType alphaHousingDetailType = An<IDetailType>();
                alphaHousingDetailType.WhenToldTo(x => x.DetailClass).Return(alphaHousingDetailClass);
                IDetail detail = An<IDetail>();
                detail.WhenToldTo(x => x.DetailType).Return(alphaHousingDetailType);

                IDetailClass anotherDetailClass = An<IDetailClass>();
                anotherDetailClass.WhenToldTo(x => x.Key).Return((int)DetailClasses.CivilServants);
                IDetailType anotherDetailType = An<IDetailType>();
                anotherDetailType.WhenToldTo(x => x.DetailClass).Return(anotherDetailClass);
                IDetail anotherDetail = An<IDetail>();
                anotherDetail.WhenToldTo(x => x.DetailType).Return(anotherDetailType);

                IEventList<IDetail> details = new EventList<IDetail>(new IDetail[] { detail, anotherDetail });

                account.WhenToldTo(x => x.Details).Return(details);

                businessRule = new BusinessModel.Rules.Account.AccountIsAlphaHousing();
                RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Account.AccountIsAlphaHousing>.startrule.Invoke();
            };

        Because of = () =>
            {
                businessRule.ExecuteRule(messages, account);
            };

        It should_add_a_message = () =>
            {
                messages.Count.ShouldEqual(1);
            };
    }
}