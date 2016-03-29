using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Collections.Generic;

namespace DomainService2.Specs.SharedServices.Common.PerformEworkActionCommandHandlerSpecs
{
    public class When_asked_to_perform_X2_Archive : PerformEWorkActionCommandHandlerSpecBase
    {
        private Establish context = () =>
        {
            eWorkEngine = An<IeWork>();
            debtCounsellingRepo = An<IDebtCounsellingRepository>();

            command = new PerformEWorkActionCommand(FOLDERID, Constants.EworkActionNames.X2ARCHIVE, GENERICKEY, ASSIGNEDUSER, CURRENTSTAGE);
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

        It should_ask_the_eWork_client_to_invoke_the_X2_Archive_action = () =>
        {
            eWorkEngine.WasToldTo(x => x.InvokeAndSubmitAction(Param.IsAny<string>(), Param<string>.Matches(c => c == command.EFolderID), Param.Is<string>(Constants.EworkActionNames.X2ARCHIVE), Param.IsAny<Dictionary<string, string>>(), Param.IsAny<string>()));
        };

        It should_return_true = () =>
        {
            command.Result.ShouldEqual(true);
        };
    }
}
