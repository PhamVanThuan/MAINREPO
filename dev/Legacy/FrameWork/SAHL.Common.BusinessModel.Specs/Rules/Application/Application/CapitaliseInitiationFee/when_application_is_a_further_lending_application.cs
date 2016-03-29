using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.CapitaliseInitiationFee
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV))]
    public class when_application_is_a_further_lending_application : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV>
    {
        protected static IApplication application;
        Establish Context = () =>
        {
            application = An<IApplicationFurtherLending>();
            businessRule = new BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV();
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV>.startrule.Invoke();

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
