using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using Machine.Fakes;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.FurtherLendingAccountForApplicationIsAlphaHousing
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing))]
	public class when_application_is_further_lending_and_alpha_housing : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing>
    {
        protected static IApplication application;
		protected static IAccount account;
		protected static IDetail alphaHousingDetail;
		protected static IDetailType alphaHousingDetailType;

        Establish Context = () =>
        {
            application = An<IApplicationFurtherLending>();
			account = An<IAccount>();
			alphaHousingDetail = An<IDetail>();
			alphaHousingDetailType = An<IDetailType>();

			alphaHousingDetailType.WhenToldTo(x => x.Key).Return((int)DetailTypes.AlphaHousing);
			alphaHousingDetail.WhenToldTo(x => x.DetailType).Return(alphaHousingDetailType);
			IEventList<IDetail> details = new EventList<IDetail>(new IDetail[] { alphaHousingDetail });

			account.WhenToldTo(x => x.Details).Return(details);
			application.WhenToldTo(x => x.Account).Return(account);

			businessRule = new SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.FurtherLending.AccountForFurtherLendingApplicationIsAlphaHousing>.startrule.Invoke();

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
