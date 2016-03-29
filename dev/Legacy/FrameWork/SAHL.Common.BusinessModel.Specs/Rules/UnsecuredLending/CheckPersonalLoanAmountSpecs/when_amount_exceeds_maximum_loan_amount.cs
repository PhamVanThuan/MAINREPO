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

namespace SAHL.Common.BusinessModel.Specs.Rules.CheckPersonalLoanAmountSpecs
{
    [Subject(typeof(CheckPersonalLoanAmount))]
    public class when_amount_exceeds_maximum_loan_amount : RulesBaseWithFakes<CheckPersonalLoanAmount>
    {
        protected static ICreditCriteriaUnsecuredLendingRepository creditCriteriaUnsecuredLendingRepository;
        Establish context = () =>
        {
            var personalLoanAmount = 2D;
            var maxLoanAmount = 1D;
            creditCriteriaUnsecuredLendingRepository = An<ICreditCriteriaUnsecuredLendingRepository>();
            businessRule = new CheckPersonalLoanAmount(creditCriteriaUnsecuredLendingRepository);

            var creditCriteriaUnsecuredLending = An<ICreditCriteriaUnsecuredLending>();
            creditCriteriaUnsecuredLending.WhenToldTo(x => x.MaxLoanAmount).Return(maxLoanAmount);
            creditCriteriaUnsecuredLendingRepository.WhenToldTo(x => x.GetCreditCriteriaUnsecuredLendingList()).Return(new ReadOnlyEventList<ICreditCriteriaUnsecuredLending>(new ICreditCriteriaUnsecuredLending[] { creditCriteriaUnsecuredLending }));

            parameters = new object[] { personalLoanAmount }; //legal entity key
            RulesBaseWithFakes<CheckPersonalLoanAmount>.startrule.Invoke();
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