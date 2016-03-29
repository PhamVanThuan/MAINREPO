using Machine.Specifications;
using Machine.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.Has30YearTermDisqualification
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTermDisqualification))]
    public class when_application_has_a_disqualification : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTermDisqualification>
    {
        protected static IApplication application;

        Establish Context = () =>
        {
            application = An<IApplication>();

            application.WhenToldTo(x => x.HasAttribute(Param.IsAny<OfferAttributeTypes>())).Return(true);

            businessRule = new BusinessModel.Rules.Application.ApplicationHas30YearTermDisqualification();

            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHas30YearTermDisqualification>.startrule.Invoke();

        };
        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };
        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}
