using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.CapitaliseInitiationFee
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV))]
    public class when_application_has_the_capitalise_initiation_fee_attribute_with_acceptable_ltv : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV>
    {
        protected static IApplication application;
        protected static IApplicationProductSupportsVariableLoanApplicationInformation appProduct;
        protected static IApplicationInformationVariableLoan variableLoanInformation;
        protected static double? ltv = 1.1;

        Establish Context = () =>
        {

            application = An<IApplication>();
            appProduct = An<IApplicationProductSupportsVariableLoanApplicationInformation>();
            variableLoanInformation = An<IApplicationInformationVariableLoan>();

            application.WhenToldTo(x => x.HasAttribute(Param.Is(OfferAttributeTypes.CapitaliseInitiationFee))).Return(true);
            businessRule = new BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV();

            application.WhenToldTo(x => x.CurrentProduct).Return(appProduct);
            appProduct.WhenToldTo(x => x.VariableLoanInformation).Return(variableLoanInformation);
            variableLoanInformation.WhenToldTo(x => x.LTV).Return(ltv);

            IRuleParameter ruleParameter = An<IRuleParameter>();
            ruleParameter.WhenToldTo(x => x.Value).Return("1.0");
            IEventList<IRuleParameter> ruleParams = new EventList<IRuleParameter>(new List<IRuleParameter> { ruleParameter });
            ruleItem.WhenToldTo(x => x.RuleParameters).Return(ruleParams);

            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.CapitaliseInitiationFeeLTV>.startrule.Invoke();

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
