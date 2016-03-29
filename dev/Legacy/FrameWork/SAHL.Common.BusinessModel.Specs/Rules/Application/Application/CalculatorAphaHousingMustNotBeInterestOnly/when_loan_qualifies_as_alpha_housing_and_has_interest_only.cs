using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.AphaHousingMustNotBeInterestOnly
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan))]
    public class when_loan_qualifies_as_alpha_housing_and_has_interest_only : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan>
    {
        protected static IApplicationRepository applicationRepository;
        protected static int productKey;
        protected static bool interestOnly;

        Establish Context = () =>
        {
            productKey = (int)SAHL.Common.Globals.Products.VariableLoan;
            interestOnly = true;

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(x => x.IsAlphaHousingLoan(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<double>())).Return(true);

            businessRule = new BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan(applicationRepository);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustNotBeInterestOnlyLoan>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, productKey, Param.IsAny<int>(), Param.IsAny<double>(), Param.IsAny<int>(), interestOnly);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}
