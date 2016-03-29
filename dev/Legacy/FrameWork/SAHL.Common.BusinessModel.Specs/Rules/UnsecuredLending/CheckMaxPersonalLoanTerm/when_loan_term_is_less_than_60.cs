using System.Collections.Generic;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckMaxPersonalLoanTerm
{
	[Subject(typeof(SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckMaxPersonalLoanTerm))]
	public class when_loan_term_is_less_than_60 : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckMaxPersonalLoanTerm>
	{
		protected static IAccountPersonalLoan personalLoanAccount;
		Establish context = () =>
		{
			personalLoanAccount = An<IAccountPersonalLoan>();
			businessRule = new SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan.CheckMaxPersonalLoanTerm();
			parameters = new object[]{
				personalLoanAccount,
				10
			};
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, parameters);
		};

		It rule_should_pass = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
