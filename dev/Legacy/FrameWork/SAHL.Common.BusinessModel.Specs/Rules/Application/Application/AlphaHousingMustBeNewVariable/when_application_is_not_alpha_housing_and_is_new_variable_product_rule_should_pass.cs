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

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.AlphaHousingMustBeNewVariable
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan))]
    public class when_application_is_not_alpha_housing_and_is_new_variable_product_rule_should_pass : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan>
    {
        protected static IApplication application;
        protected static IApplicationProduct product;

        Establish Context = () =>
            {
                application = An<IApplication>();
                product = An<IApplicationProduct>();

                product.WhenToldTo(x => x.ProductType).Return(Globals.Products.NewVariableLoan);
                application.WhenToldTo(x => x.IsAlphaHousing()).Return(true);
                application.WhenToldTo(x => x.ProductHistory).Return(new IApplicationProduct[] {product });

                businessRule = new BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan();
                RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.AlphaHousingLoanMustBeNewVariableLoan>.startrule.Invoke();

            };
        Because of = () =>
            {
                businessRule.ExecuteRule(messages, application);
            };
        It rule_should_pass = () =>
            {
                messages.Count.ShouldEqual(0);
            };
    }
}
