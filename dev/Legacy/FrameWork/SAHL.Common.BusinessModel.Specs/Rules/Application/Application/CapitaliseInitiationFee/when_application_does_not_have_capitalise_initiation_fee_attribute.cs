using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.CapitaliseInitiationFee
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV))]
    public class when_application_does_not_have_capitalise_initiation_fee_attribute : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV>
    {
        protected static IApplication application;
        Establish Context = () =>
        {
            application = An<IApplication>();
            application.WhenToldTo(x => x.HasAttribute(Param.Is(OfferAttributeTypes.CapitaliseInitiationFee))).Return(false);
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
