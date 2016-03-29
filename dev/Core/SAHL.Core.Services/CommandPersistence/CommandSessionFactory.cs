using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services.CommandPersistence
{
    public class CommandSessionFactory : ICommandSessionFactory
    {
        private readonly ICommandDataManager commandDataManager;
        private readonly ICommandRetryPolicy commandRetryPolicy;

        public string ServiceName { get; set; }

        public CommandSessionFactory(ICommandDataManager commandDataManager, ICommandRetryPolicy commandRetryPolicy, IHostedService hostedService)
        {
            this.commandDataManager = commandDataManager;
            this.commandRetryPolicy = commandRetryPolicy;
            this.ServiceName = hostedService.Name;
        }

        public ICommandSession CreateNewCommandManager(string json)
        {
            ICommandSession commandSession = new CommandSession(commandDataManager, commandRetryPolicy);
            commandSession.LoadCommand(json, ServiceName);
            return commandSession;
        }

        public IEnumerable<ICommandSession> GetAllPending(Func<string, IContextDetails> deserialiseFunc)
        {
            return commandDataManager.GetAllPendingCommands(ServiceName).Select(commandKey => CreateNewCommandManager(commandKey, deserialiseFunc));
        }

        public IEnumerable<ICommandSession> GetAllFailed(Func<string, IContextDetails> deserialiseFunc)
        {
            return commandDataManager.GetAllFailedCommands(ServiceName).Select(commandKey => CreateNewCommandManager(commandKey, deserialiseFunc));
        }

        public IEnumerable<ICommandSession> GetAllFailedAndPending(Func<string, IContextDetails> deserialiseFunc)
        {
            return GetAllFailed(deserialiseFunc).Concat(GetAllPending(deserialiseFunc));
        }

        public IEnumerable<ICommandSession> GetAllFailedAndPending(Func<string, IContextDetails> deserialiseFunc, Func<ICommandSession, bool> wherePredicate)
        {
            return GetAllFailed(deserialiseFunc, wherePredicate).Concat(GetAllPending(deserialiseFunc, wherePredicate));
        }

        public IEnumerable<ICommandSession> GetAllFailed(Func<string, IContextDetails> deserialiseFunc, Func<ICommandSession, bool> wherePredicate)
        {
            return GetAllFailed(deserialiseFunc).Where(wherePredicate);
        }

        public IEnumerable<ICommandSession> GetAllPending(Func<string, IContextDetails> deserialiseFunc, Func<ICommandSession, bool> wherePredicate)
        {
            return GetAllFailed(deserialiseFunc).Where(wherePredicate);
        }

        public ICommandSession CreateNewCommandManager(int commandKey, Func<string, IContextDetails> deserialiseFunc)
        {
            ICommandSession commandSession = new CommandSession(commandDataManager, commandRetryPolicy);
            commandSession.LoadCommandForDatabase(commandKey, deserialiseFunc);
            return commandSession;
        }
    }
}