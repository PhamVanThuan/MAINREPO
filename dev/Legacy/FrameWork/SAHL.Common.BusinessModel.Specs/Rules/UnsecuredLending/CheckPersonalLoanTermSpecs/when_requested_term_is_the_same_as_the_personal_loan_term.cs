using System.Text;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Specs.Rules.CheckPersonalLoanTermSpecs
{
	[Subject(typeof(CheckPersonalLoanTerm))]
	public class when_requested_term_is_the_same_as_the_personal_loan_term : RulesBaseWithFakes<CheckPersonalLoanTerm>
	{
		protected static ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;
		Establish context = () =>
		{
			var personalLoanterm = 1;
			var maxLoanTerm = 1;
			creditCriteriaUnsecuredLendingRepository = An<ICreditCriteriaUnsecuredLendingRepository>();
			businessRule = new CheckPersonalLoanTerm(creditCriteriaUnsecuredLendingRepository);

			var creditCriteriaUnsecuredLending = An<ICreditCriteriaUnsecuredLending>();
			creditCriteriaUnsecuredLending.WhenToldTo(x => x.Term).Return(maxLoanTerm);
			creditCriteriaUnsecuredLendingRepository.WhenToldTo(x => x.GetCreditCriteriaUnsecuredLendingList()).Return(new ReadOnlyEventList<ICreditCriteriaUnsecuredLending>(new ICreditCriteriaUnsecuredLending[] { creditCriteriaUnsecuredLending }));

			parameters = new object[] { personalLoanterm };
			RulesBaseWithFakes<CheckPersonalLoanTerm>.startrule.Invoke();
		};

		Because of = () =>
		{
			businessRule.ExecuteRule(messages, parameters);
		};

		It rule_should_fail = () =>
		{
			messages.Count.ShouldEqual(0);
		};
	}
}
