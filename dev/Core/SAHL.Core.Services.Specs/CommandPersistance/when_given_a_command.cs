using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Identity;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Testing.Fakes;
using System;

namespace SAHL.Core.Services.Specs.CommandPersistance
{
    public class when_given_a_command : WithFakes
    {
        private static FakeDbFactory dbFactory;
        public static CommandDataManager CommandDataManager;
        public static ICommandSession CommandSession;
        public static ICommand Command;
        public static IHostContext hostContext;
        public static string commandJson;
        public static string ServiceName;
        public static string ContextDetailsJson;
        public static bool HasExecuted;

        private Establish context = () =>
        {
            commandJson = "";
            ServiceName = "";
            ContextDetailsJson = "";
            dbFactory = new FakeDbFactory();
            CommandDataManager = An<CommandDataManager>(dbFactory);
            hostContext = An<IHostContext>();
            CommandSession = new CommandSession(CommandDataManager, An<ICommandRetryPolicy>());
            Command = new Command
            {
                CommandDate = DateTime.Now,
                CommandJson = "Some Value"
            };

            CommandSession.LoadCommand(commandJson, ServiceName);
        };

        private Because of = () =>
        {
            CommandSession.PersistCommand(ContextDetailsJson);
        };

        private It should_give_command_to_command_data_manager = () =>
        {
            CommandDataManager.WasToldTo(x => x.SaveCommand(Command, ServiceName, ContextDetailsJson));
        };

        private It should_call_insert = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Insert(Param<CommandDataModel>.IsAnything));
        };
    }
}