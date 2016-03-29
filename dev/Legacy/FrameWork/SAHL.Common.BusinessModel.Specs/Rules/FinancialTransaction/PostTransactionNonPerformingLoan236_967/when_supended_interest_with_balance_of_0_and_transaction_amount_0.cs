using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;


namespace SAHL.Common.BusinessModel.Specs.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967
{
	public class when_supended_interest_with_balance_of_0_and_transaction_amount_0 : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967>
	{
		protected static IAccount account;
		protected static double loanTransactionAmount;
		protected static IFinancialService financialService;
		protected static IFinancialService childFinancialService;
		protected static IAccountStatus financialServiceAccountStatus;
		protected static IAccountStatus childFinancialServiceAccountStatus;
		protected static IFinancialServiceType financialServiceType;
		protected static IFinancialServiceType childFinancialServiceType;
		protected static Exception exception;
		Establish context = () =>
		{
			account = An<IAccount>();
			financialService = An<IFinancialService>();
			childFinancialService = An<IFinancialService>();
			financialServiceAccountStatus = An<IAccountStatus>();
			childFinancialServiceAccountStatus = An<IAccountStatus>();
			financialServiceType = An<IFinancialServiceType>();
			childFinancialServiceType = An<IFinancialServiceType>();
			loanTransactionAmount = 0d;

			financialServiceType.WhenToldTo(x => x.Key).Return((int)FinancialServiceTypes.VariableLoan);
			childFinancialServiceType.WhenToldTo(x => x.Key).Return((int)FinancialServiceTypes.ArrearBalance);

			financialService.WhenToldTo(x => x.FinancialServiceType).Return(financialServiceType);
			childFinancialService.WhenToldTo(x => x.FinancialServiceType).Return(childFinancialServiceType);

			financialService.WhenToldTo(x => x.FinancialServiceParent).Return((IFinancialService)null);
			childFinancialService.WhenToldTo(x => x.FinancialServiceParent).Return(financialService);

			financialServiceAccountStatus.WhenToldTo(x => x.Key).Return((int)AccountStatuses.Open);
			childFinancialServiceAccountStatus.WhenToldTo(x => x.Key).Return((int)AccountStatuses.Open);

			financialService.WhenToldTo(x => x.AccountStatus).Return(financialServiceAccountStatus);
			childFinancialService.WhenToldTo(x => x.AccountStatus).Return(childFinancialServiceAccountStatus);

			account.WhenToldTo(x => x.FinancialServices).Return(new EventList<IFinancialService>(new IFinancialService[] { financialService, childFinancialService }));

			businessRule = new SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, account, loanTransactionAmount);
		};

		It should_not_fail = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
