using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance;
using SAHL.Core.Identity;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Core.Services.Specs.CommandPersistance
{
    public class when_given_a_fail : WithFakes
    {
        private static FakeDbFactory dbFactory;
        public static ICommandDataManager CommandDataManager;
        public static ICommandSession CommandSession;
        public static IHostContext hostContext;
        public static bool HasExecuted;

        Establish that = () =>
        {
            dbFactory = new FakeDbFactory();
            CommandDataManager = An<CommandDataManager>(dbFactory);
            hostContext = An<IHostContext>();
            CommandSession = new CommandSession(CommandDataManager, An<ICommandRetryPolicy>());
            CommandSession.CommandKey = 1;
        };

        private Because of = () =>
        {
            CommandSession.FailCommand(Arg.Any<string>());
        };

        private It should_call_SetCommandFailed = () =>
        {
            CommandDataManager.WasToldTo(x => x.SetCommandFailed(1));
        };

        It should_call_update = () =>
        {
            dbFactory.FakedDb.InEventContext().WasToldTo(x => x.Update<CommandDataModel>(Arg.Is<SetPersistedCommandFailStatement>(y => y.CommandKey == 1)));
        };
    }
}