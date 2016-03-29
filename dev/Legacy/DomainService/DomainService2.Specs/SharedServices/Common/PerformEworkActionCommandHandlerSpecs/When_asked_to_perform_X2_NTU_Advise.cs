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
    public class When_asked_to_perform_X2_NTU_Advise : PerformEWorkActionCommandHandlerSpecBase
    {
        Establish context = () =>
        {
            eWorkEngine = An<IeWork>();
            debtCounsellingRepo = An<IDebtCounsellingRepository>();

            command = new PerformEWorkActionCommand(FOLDERID, Constants.EworkActionNames.X2NTUAdvise, GENERICKEY, ASSIGNEDUSER, CURRENTSTAGE);
            handler = new PerformEWorkActionCommandHandler(eWorkEngine, debtCounsellingRepo);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_ask_the_eWork_client_to_Login = () =>
        {
            eWorkEngine.WasToldTo(x => x.LogIn(X2LOGIN));
        };

        It should_ask_the_eWork_client_to_invoke_the_X2_NTU_Advise_action_with_an_extra_variable_X2NTUReason = () =>
        {
            eWorkEngine.WasToldTo(x => x.InvokeAndSubmitAction(Param.IsAny<string>(), 
                                                               Param<string>.Matches(c => c == command.EFolderID), 
                                                               Param.Is<string>(Constants.EworkActionNames.X2NTUAdvise),
                                                               Param<Dictionary<string, string>>.Matches(c => c.ContainsKey(Constants.EworkActionNames.X2NTUReason) && c[Constants.EworkActionNames.X2NTUReason] == "19"),
                                                               Param.Is<string>(Constants.EworkActionNames.X2NTUReason)));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldEqual(true);
        };

    }
}
