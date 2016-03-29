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

namespace SAHL.Core.Services.Specs.CommandPersistance
{
    public class when_get_all_failed_and_pending_with_predicate : WithFakes
    {
        private static readonly CommandDataModel commandModel = new CommandDataModel(string.Empty, DateTime.Now, string.Empty, string.Empty, false, false, string.Empty, false, false);
        private static FakeDbFactory dbFactory;
        public static ICommandDataManager CommandDataManager;
        public static ICommandSession CommandSession;
        public static ICommand Command;
        public static CommandSessionFactory CommandSessionFactory;
        public static ICommandRetryPolicy CommandRetryPolicy;
        public static IHostedService HostedService;
        public static IContextDetails ContextDetails;
        public static Func<string, IContextDetails> DeserialiseFunc;
        public static Func<ICommandSession, bool> WherePredicate;
        public static KeyValuePair<string, string> Pair = new KeyValuePair<string, string>("testKey", "testValue");

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            CommandDataManager = An<CommandDataManager>(dbFactory);
            Command = An<ICommand>();
            Command.CommandJson = "";
            Command.CommandDate = DateTime.Now;
            CommandRetryPolicy = An<CommandRetryPolicyNone>();
            CommandSession = An<CommandSession>(CommandDataManager, CommandRetryPolicy);
            WherePredicate = x => true;
            IHostedService hostedService = An<IHostedService>();
            CommandRetryPolicy = An<ICommandRetryPolicy>();
            CommandSessionFactory = An<CommandSessionFactory>(CommandDataManager, CommandRetryPolicy, hostedService);

            CommandSessionFactory.ServiceName = string.Empty;
            ContextDetails = new ContextDetails { Username = "testuser", ContextValues = new List<KeyValuePair<string, string>> { Pair } };
            DeserialiseFunc = s => ContextDetails;
            dbFactory.FakedDb.InEventContext()
                .WhenToldTo(x => x.Select(Arg.Any<GetCommandsThatHaveFailedStatement>()))
                .Return(new List<CommandKeyModel> { new CommandKeyModel { CommandKey = 1 } });
            dbFactory.FakedDb.InEventContext()
                .WhenToldTo(x => x.SelectOneWhere<CommandDataModel>(Arg.Any<string>(), Arg.Any<object>()))
                .Return(commandModel);
            dbFactory.FakedDb.InEventContext()
                .WhenToldTo(x => x.SelectOne(Arg.Any<ISqlStatement<CommandHostContextModel>>()))
                .Return(new CommandHostContextModel { ContextValues = "representsJson" });
        };

        private Because of = () =>
        {
            CommandSessionFactory.GetAllFailedAndPending(DeserialiseFunc, WherePredicate);
        };

        private It should_Call_Get_All_Pending_With_Predicate = () =>
        {
            CommandSessionFactory.WasToldTo(x => x.GetAllPending(DeserialiseFunc, WherePredicate));
        };

        private It should_Call_Get_All_Failed_With_Predicate = () =>
        {
            CommandSessionFactory.WasToldTo(x => x.GetAllFailed(DeserialiseFunc, WherePredicate));
        };

        private It should_Call_Get_All_Pending = () =>
        {
            CommandSessionFactory.WasToldTo(x => x.GetAllPending(DeserialiseFunc));
        };

        private It should_Call_Get_All_Failed = () =>
        {
            CommandSessionFactory.WasToldTo(x => x.GetAllFailed(DeserialiseFunc));
        };
    }
}