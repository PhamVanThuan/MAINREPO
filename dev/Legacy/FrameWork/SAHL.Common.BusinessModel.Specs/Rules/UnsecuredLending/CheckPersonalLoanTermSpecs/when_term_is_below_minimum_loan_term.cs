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

namespace SAHL.Common.BusinessModel.Specs.Rules.CheckPersonalLoanTermSpecs
{
    [Subject(typeof(CheckPersonalLoanTerm))]
    public class when_term_is_below_minimum_loan_term : RulesBaseWithFakes<CheckPersonalLoanTerm>
    {
        protected static ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;

        Establish context = () =>
        {
            var personalLoanterm = 0;
            var minimumLoanTerm = 1;
            creditCriteriaUnsecuredLendingRepository = An<ICreditCriteriaUnsecuredLendingRepository>();
            businessRule = new CheckPersonalLoanTerm(creditCriteriaUnsecuredLendingRepository);

            var creditCriteriaUnsecuredLending = An<ICreditCriteriaUnsecuredLending>();
            creditCriteriaUnsecuredLending.WhenToldTo(x => x.Term).Return(minimumLoanTerm);
            creditCriteriaUnsecuredLendingRepository.WhenToldTo(x => x.GetCreditCriteriaUnsecuredLendingList())
                .Return(new ReadOnlyEventList<ICreditCriteriaUnsecuredLending>(new ICreditCriteriaUnsecuredLending[] { creditCriteriaUnsecuredLending }));

            parameters = new object[] { personalLoanterm };
            RulesBaseWithFakes<CheckPersonalLoanTerm>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}