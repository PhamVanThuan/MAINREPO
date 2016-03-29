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
    public class when_application_has_no_alpha_housing_attribute : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationIsAlphaHousing>
    {
        protected static IApplication application;
        protected static IApplicationProduct product;
        Establish context = () =>
        {
            application = An<IApplication>();

			application.WhenToldTo(x => x.HasAttribute(OfferAttributeTypes.AlphaHousing)).Return(false);
            businessRule = new BusinessModel.Rules.Application.ApplicationIsAlphaHousing();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationIsAlphaHousing>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };

        It rule_should_pass = () =>
        {
            messages.WarningMessages.Count.ShouldEqual(0);
        };
    }
}