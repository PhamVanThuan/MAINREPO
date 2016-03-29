using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Specs.Rules.Credit.ApplicationEmploymentTypeConfirmation
{
    class when_an_application_employment_type_has_been_confirmed_for_given_instance : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Workflow.CheckEmploymentTypeConfirmed>
    {
        protected static IX2Repository x2Repository;
        protected static long instanceId;

        Establish context = () =>
        {
            instanceId = Param.IsAny<long>();

            x2Repository = An<IX2Repository>();
            x2Repository.WhenToldTo(x => x.HasInstancePerformedActivity
                                                (
                                                    instanceId
                                                  , SAHL.Common.Constants.WorkFlowActivityName.ConfirmApplicationEmploymentType)
                                                ).Return(true);

            businessRule = new SAHL.Common.BusinessModel.Rules.Workflow.CheckEmploymentTypeConfirmed(x2Repository);
            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Workflow.CheckEmploymentTypeConfirmed>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, Param.IsAny<long>());
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
