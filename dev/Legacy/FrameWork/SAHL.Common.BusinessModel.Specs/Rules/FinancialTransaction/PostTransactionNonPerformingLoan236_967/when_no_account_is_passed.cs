﻿using System;
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
	public class when_no_account_is_passed : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967>
	{
		protected static Exception exception;
		Establish context = () =>
		{
			businessRule = new SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967();
			RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.FinancialTransaction.PostTransactionNonPerformingLoan236_967>.startrule.Invoke();
		};

		Because of = () =>
		{
			exception = Catch.Exception(() => { businessRule.ExecuteRule(messages, (IAccount)null); });
		};

		It should_throw_argument_exception = () =>
		{
			exception.ShouldBeOfType<ArgumentException>();
		};
	}
}