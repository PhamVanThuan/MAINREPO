using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.ApplicationHas30YearTerm
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTerm))]
    public class when_application_has_30yr_attribute : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTerm>
    {
        protected static IApplication application;
        Establish Context = () =>
        {
            application = An<IApplication>();
            businessRule = new BusinessModel.Rules.Application.ApplicationHas30YearTerm();
            application.WhenToldTo(x => x.HasAttribute(Param.IsAny<OfferAttributeTypes>())).Return(true);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTerm>.startrule.Invoke();

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
