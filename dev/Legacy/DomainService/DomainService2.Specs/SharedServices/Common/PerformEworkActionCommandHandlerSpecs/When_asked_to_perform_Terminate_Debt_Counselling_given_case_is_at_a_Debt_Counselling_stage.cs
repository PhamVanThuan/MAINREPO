using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;

namespace DomainService2.Specs.SharedServices.Common.PerformEworkActionCommandHandlerSpecs
{
    public class When_asked_to_perform_Terminate_Debt_Counselling_given_case_is_at_a_Debt_Counselling_stage : PerformEWorkActionCommandHandlerSpecBase
    {
        private static string currentStage = "Debt Counselling (Arrears)";
        private static string eWorkUser;

        private Establish context = () =>
        {
            eWorkEngine = An<IeWork>();
            debtCounsellingRepo = An<IDebtCounsellingRepository>();

            command = new PerformEWorkActionCommand(FOLDERID, Constants.EworkActionNames.TerminateDebtCounselling, GENERICKEY, ASSIGNEDUSER, currentStage);
            handler = new PerformEWorkActionCommandHandler(eWorkEngine, debtCounsellingRepo);

            eWorkUser = command.AssignedUser.Replace(@"SAHL\", "");
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };


        It should_ask_the_eWork_client_to_invoke_the_Terminate_Debt_Counselling_action_with_extra_parameter_UserToDo = () =>
        {
            eWorkEngine.WasToldTo(x => x.InvokeAndSubmitAction(Param.IsAny<string>(),
                                                               Param<string>.Matches(c => c == command.EFolderID),
                                                               Param.Is<string>(Constants.EworkActionNames.TerminateDebtCounselling),
                                                               Param<Dictionary<string, string>>.Matches(c => c.ContainsKey("UserToDo") && c["UserToDo"] == eWorkUser),
                                                               Param.IsAny<string>()));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}
