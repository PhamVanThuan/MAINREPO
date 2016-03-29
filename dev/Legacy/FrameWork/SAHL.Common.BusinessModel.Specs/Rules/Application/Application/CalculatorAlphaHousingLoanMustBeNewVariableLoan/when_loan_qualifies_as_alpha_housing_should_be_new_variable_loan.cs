using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.CalculatorAlphaHousingLoanMustBeNewVariableLoan
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustBeNewVariableLoan))]
    public class when_loan_qualifies_as_alpha_housing_should_be_new_variable_loan : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustBeNewVariableLoan>
    {
        protected static IApplicationRepository applicationRepository;
        protected static int productKey;

        Establish Context = () =>
            {
                productKey = (int)SAHL.Common.Globals.Products.Edge;

                applicationRepository = An<IApplicationRepository>();
                applicationRepository.WhenToldTo(x => x.IsAlphaHousingLoan(Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<double>())).Return(true);
                businessRule = new BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustBeNewVariableLoan(applicationRepository);
                RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CalculatorAlphaHousingLoanMustBeNewVariableLoan>.startrule.Invoke();

            };
        Because of = () =>
            {
                businessRule.ExecuteRule(messages, productKey, Param.IsAny<int>(), Param.IsAny<double>(), Param.IsAny<int>());
            };
        It rule_should_fail = () =>
            {
                messages.Count.ShouldEqual(1);
            };
    }
}
