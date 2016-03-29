using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.LegalEntity.HasAccountInArrearsInLast6Months
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.LegalEntity.HasAccountInArrearsInLast6Months))]
	public class when_legal_entity_has_loan_in_arrears_in_last_6_months : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.HasAccountInArrearsInLast6Months>
	{
		protected static float amount = 200;
		protected static int monthsInArrears = 6;
		protected static ILegalEntity legalEntity;
		protected static IAccount account;
		protected static IRole accountRole;
		protected static IRoleType roleType;
		protected static AccountTypes accountType;
		Establish context = () =>
		{
			var hasBeenInArrears = true;
			accountType = AccountTypes.MortgageLoan;

			account = An<IAccount>();
			legalEntity = An<ILegalEntity>();
			accountRole = An<IRole>();
			roleType = An<IRoleType>();

			roleType.WhenToldTo(x => x.Key).Return((int)RoleTypes.MainApplicant);
			legalEntity.WhenToldTo(x => x.Roles).Return(new EventList<IRole>(new IRole[] { accountRole }));
			accountRole.WhenToldTo(x => x.Account).Return(account);
			accountRole.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
			accountRole.WhenToldTo(x => x.RoleType).Return(roleType);
			account.WhenToldTo(x => x.AccountType).Return(accountType);
			account.WhenToldTo(x => x.HasBeenInArrears(monthsInArrears, amount)).Return(hasBeenInArrears);

			businessRule = new BusinessModel.Rules.LegalEntity.HasAccountInArrearsInLast6Months();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.LegalEntity.HasAccountInArrearsInLast6Months>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, legalEntity);
		};

		It rule_should_fail = () =>
		{
			messages.Count.ShouldEqual(1);
		};

		It it_should_run_rule_for_last_6_months = () =>
		{
			account.WasToldTo(x => x.HasBeenInArrears(monthsInArrears, amount));
		};
	}
}
