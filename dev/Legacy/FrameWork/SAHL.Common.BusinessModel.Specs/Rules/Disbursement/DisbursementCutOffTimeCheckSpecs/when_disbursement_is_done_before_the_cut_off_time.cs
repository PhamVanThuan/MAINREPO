using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Rules.Disbursement;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Specs.Rules.Disbursement.DisbursementCutOffTimeCheckSpecs
{
    [Subject(typeof(DisbursementCutOffTimeCheck))]
    public class when_disbursement_is_done_before_the_cut_off_time : RulesBaseWithFakes<DisbursementCutOffTimeCheck>
    {
        private static ICommonRepository commonRepo;

        Establish context = () =>
            {
                commonRepo = An<ICommonRepository>();
                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 0, 0);
                commonRepo.WhenToldTo(x => x.GetTodaysDate()).Return(dt);
                businessRule = new DisbursementCutOffTimeCheck(commonRepo);

                IRuleParameter ruleParameter = An<IRuleParameter>();
                ruleParameter.WhenToldTo(x => x.Value).Return("12:30");
                IEventList<IRuleParameter> ruleParams = new EventList<IRuleParameter>(new List<IRuleParameter>{ruleParameter});
                ruleItem.WhenToldTo(x => x.RuleParameters).Return(ruleParams);
                RulesBaseWithFakes<DisbursementCutOffTimeCheck>.startrule.Invoke();
            };

        Because of = () =>
            {
                businessRule.ExecuteRule(messages, null);
            };

        It should_return_true = () =>
            {
                messages.Count.ShouldEqual<int>(0);
            };
    }
}
