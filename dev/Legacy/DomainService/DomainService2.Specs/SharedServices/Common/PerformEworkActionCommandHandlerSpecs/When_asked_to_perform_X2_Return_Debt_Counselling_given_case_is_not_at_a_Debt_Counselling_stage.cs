using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;

namespace DomainService2.Specs.SharedServices.Common.PerformEworkActionCommandHandlerSpecs
{
    public class When_asked_to_perform_X2_Return_Debt_Counselling_given_case_is_not_at_a_Debt_Counselling_stage : PerformEWorkActionCommandHandlerSpecBase
    {
        private static string currentStage = "Send Letter";

        private Establish context = () =>
        {
            eWorkEngine = An<IeWork>();
            debtCounsellingRepo = An<IDebtCounsellingRepository>();

            command = new PerformEWorkActionCommand(FOLDERID, Constants.EworkActionNames.X2ReturnDebtCounselling, GENERICKEY, ASSIGNEDUSER, currentStage);
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

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}
