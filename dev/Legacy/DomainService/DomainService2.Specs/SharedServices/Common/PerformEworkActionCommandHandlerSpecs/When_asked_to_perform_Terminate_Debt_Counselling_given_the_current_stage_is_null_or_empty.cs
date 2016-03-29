using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.SharedServices.Common.PerformEworkActionCommandHandlerSpecs 
{
    public class When_asked_to_perform_Terminate_Debt_Counselling_given_the_current_stage_is_null_or_empty : PerformEWorkActionCommandHandlerSpecBase
    {
        private Establish context = () =>
        {
            eWorkEngine = An<IeWork>();
            debtCounsellingRepo = An<IDebtCounsellingRepository>();

            command = new PerformEWorkActionCommand(FOLDERID, Constants.EworkActionNames.TerminateDebtCounselling, GENERICKEY, ASSIGNEDUSER, String.Empty);
            handler = new PerformEWorkActionCommandHandler(eWorkEngine, debtCounsellingRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_not_ask_the_eWork_client_to_invoke_any_action = () =>
        {
            eWorkEngine.WasNotToldTo(x => x.InvokeAndSubmitAction(Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<string>(), Param.IsAny<Dictionary<string, string>>(), Param.IsAny<string>()));
        };

        It should_return_false = () =>
        {
            command.Result.ShouldBeFalse();
        };
    }
}
