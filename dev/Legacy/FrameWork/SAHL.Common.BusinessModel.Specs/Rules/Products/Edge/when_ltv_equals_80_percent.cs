using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Products.Edge
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Products.ApplicationProductEdgeLTV))]
    public class when_ltv_equals_80_percent : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Products.ApplicationProductEdgeLTV>
    {
        protected static IApplicationProductEdge_IApplicationMortgageLoan applicationEdge;
        protected static IApplicationProductEdge applicationProductEdge;
        protected static IApplicationInformationVariableLoan applicationInformationVariableLoan;

        Establish Context = () =>
        {
            applicationInformationVariableLoan = An<IApplicationInformationVariableLoan>();
            applicationInformationVariableLoan.WhenToldTo(x => x.LTV).Return(0.8);

            applicationProductEdge = An<IApplicationProductEdge>();
            applicationProductEdge.WhenToldTo(x => x.VariableLoanInformation).Return(applicationInformationVariableLoan);

            applicationEdge = An<IApplicationProductEdge_IApplicationMortgageLoan>();
            applicationEdge.WhenToldTo(x => x.CurrentProduct).Return(applicationProductEdge);

            businessRule = new BusinessModel.Rules.Products.ApplicationProductEdgeLTV();

            IRuleParameter ruleParameter = An<IRuleParameter>();
            ruleParameter.WhenToldTo(x => x.Value).Return("0.80");
            IEventList<IRuleParameter> ruleParams = new EventList<IRuleParameter>(new List<IRuleParameter> { ruleParameter });
            ruleItem.WhenToldTo(x => x.RuleParameters).Return(ruleParams);

            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Products.ApplicationProductEdgeLTV>.startrule.Invoke();
        };
        Because of = () =>
        {
            businessRule.ExecuteRule(messages, applicationEdge);
        };
        It should_fail = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}