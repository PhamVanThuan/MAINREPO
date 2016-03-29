using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.ApplicationIsAlphaHousing
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.ApplicationIsAlphaHousing))]
    public class when_application_has_alpha_housing_attribute : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationIsAlphaHousing>
    {
        protected static IApplication application;
        protected static IApplicationProduct product;
        Establish context = () =>
        {
            application = An<IApplication>();
            product = An<IApplicationProduct>();

            product.WhenToldTo(x => x.ProductType).Return(Globals.Products.Edge);
            application.WhenToldTo(x => x.HasAttribute(OfferAttributeTypes.AlphaHousing)).Return(true);

            businessRule = new BusinessModel.Rules.Application.ApplicationIsAlphaHousing();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationIsAlphaHousing>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}