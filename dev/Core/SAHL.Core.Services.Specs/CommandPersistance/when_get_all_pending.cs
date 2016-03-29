using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Cuttlefish;
using SAHL.Core.Data.Models.Cuttlefish.SqlStatements.CommandPersistance;
using SAHL.Core.Services.CommandPersistence;
using SAHL.Core.Services.CommandPersistence.CommandRetryPolicy;
using SAHL.Core.Testing.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services.Specs.CommandPersistance
{
    public class when_get_all_pending : WithFakes
    {
        private static CommandDataModel commandDataModel = new CommandDataModel(string.Empty, DateTime.Now, "machineName", "serviceName", false, false, string.Empty, false, false);

        private static FakeDbFactory dbFactory;
        public static ICommandDataManager CommandDataManager;
        public static ICommandSession CommandSession;
        public static ICommand Command;
        public static ICommandSessionFactory CommandSessionFactory;
        public static ICommandRetryPolicy CommandRetryPolicy;
        public static IHostedService HostedService;
        public static Func<string, IContextDetails> DeserialiseFunc;
        public static IContextDetails ContextDetails;
        public static KeyValuePair<string, string> Pair = new KeyValuePair<string, string>("testKey", "testValue");

        private Establish that = () =>
        {
            dbFactory = new FakeDbFactory();
            CommandDataManager = An<CommandDataManager>(dbFactory);
            Command = An<ICommand>();
            Command.CommandJson = "";
            Command.CommandDate = DateTime.Now;
            CommandRetryPolicy = An<CommandRetryPolicyNone>();
            CommandSession = An<CommandSession>(CommandDataManager, CommandRetryPolicy);
            DeserialiseFunc = s => An<IContextDetails>();
            CommandRetryPolicy = An<ICommandRetryPolicy>();
            HostedService = An<IHostedService>();
            CommandSessionFactory = An<CommandSessionFactory>(CommandDataManager, CommandRetryPolicy, HostedService);
            ContextDetails = new ContextDetails
            {
                Username = "test",
                ContextValues =
                    new List<KeyValuePair<string, string>> { Pair }
            };
            CommandSessionFactory.ServiceName = string.Empty;
            DeserialiseFunc = s => ContextDetails;

            dbFactory.FakedDb.InEventContext().WhenToldTo(x => x.Select(Arg.Any<GetCommandsThatArePendingStatement>())).Return(new List<CommandKeyModel>() { new CommandKeyModel { CommandKey = 1 } });
            dbFactory.FakedDb.InEventContext().WhenToldTo(x => x.SelectOneWhere<CommandDataModel>(Arg.Any<string>(), Arg.Any<object>())).Return(commandDataModel);
            dbFactory.FakedDb.InEventContext().WhenToldTo(x => x.SelectOne(Arg.Any<ISqlStatement<CommandHostContextModel>>())).Return(new CommandHostContextModel { ContextValues = "representsJson" });
        };

        private Because of = () =>
        {
            CommandSessionFactory.GetAllPending(DeserialiseFunc);
        };

        private It should_get_all_pending_commandkeys = () =>
        {
            CommandDataManager.WasToldTo(x => x.GetAllPendingCommands(string.Empty));
        };

        private It should_call_database_for_commandkey = () =>
        {
            dbFactory.FakedDb.InEventContext().WasToldTo(x => x.Select(Arg.Any<GetCommandsThatArePendingStatement>()));
        };

        private It should_load_command_from_database = () =>
        {
            CommandSession.WasToldTo(x => x.LoadCommandForDatabase(Arg.Any<int>(), DeserialiseFunc));
        };

        private It should_create_new_command_manager = () =>
        {
            CommandSessionFactory.WasToldTo(x => x.CreateNewCommandManager(1, DeserialiseFunc));
        };

        private It should_call_the_function_to_deserialise_json_command = () =>
        {
            CommandSession.RunAsUsername.ShouldEqual(ContextDetails.Username);
        };

        private It should_should_set_the_data_values_for_context = () =>
        {
            CommandSession.ContextDetails.First().ShouldEqual(Pair);
        };

        private It should_call_database_for_command_details = () =>
        {
            dbFactory.FakedDb.InEventContext().WasToldTo(x => x.SelectOneWhere<CommandDataModel>(Arg.Any<string>(), Arg.Any<object>()));
        };
    }
}