using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Specs.Rules.DebtCounselling.DebtCounsellingCheckCourtDetailsAndCourtOrderReason
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.DebtCounselling.DebtCounsellingCheckCourtDetailsAndCourtOrderReason))]
    public class when_court_details_and_default_in_terms_of_court_order_reason_exists : DebtCounsellingCheckCourtDetailsAndCourtOrderReasonBase
    {
        Establish context = () =>
        {
            var reason = An<IReason>();
            var reasonDefinition = An<IReasonDefinition>();
            reasonDefinition.WhenToldTo(x => x.Key).Return((int)ReasonDefinitions.DefaultInTermsOfCourtOrder);
            reason.WhenToldTo(x => x.ReasonDefinition).Return(reasonDefinition);
            SetUpDebtCounsellingCase(SAHL.Common.Globals.HearingAppearanceTypes.CourtApplication, SAHL.Common.Globals.HearingTypes.Court, new List<IReason> { reason });
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };

        It should_do = () =>
        {
            reasonRepository.WasToldTo(x => x.GetReasonByGenericKeyAndReasonDefinitionKey(Param.IsAny<int>(), Param.Is<SAHL.Common.Globals.ReasonDefinitions>(SAHL.Common.Globals.ReasonDefinitions.DefaultInTermsOfCourtOrder)));
        };
    }
}
