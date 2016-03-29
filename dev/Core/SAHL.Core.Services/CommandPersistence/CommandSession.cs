using System;
using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public class CommandSession : ICommandSession
    {
        private ICommandDataManager CommandDataManager;
        private IContextDetails contextDetails;

        public string RunAsUsername
        {
            get { return contextDetails != null ? contextDetails.Username : default(string); }
        }

        public IEnumerable<KeyValuePair<string, string>> ContextDetails
        {
            get
            {
                return contextDetails != null ? contextDetails.ContextValues : default(IEnumerable<KeyValuePair<string, string>>);
            }
        }

        public int CommandKey { get; set; }

        public ICommandRetryPolicy CommandRetryPolicy { get; protected set; }

        public string ServiceName { get; set; }

        public ICommand Command { get; protected set; }

        public CommandSession(ICommandDataManager commandDataManager, ICommandRetryPolicy commandRetryPolicy)
        {
            this.CommandDataManager = commandDataManager;
            this.CommandRetryPolicy = commandRetryPolicy;
        }

        public void PersistCommand(string serialisedContextDetails)
        {
            CommandKey = CommandDataManager.SaveCommand(Command, ServiceName, serialisedContextDetails);
        }

        public void LoadCommandForDatabase(int commandKey, Func<string, IContextDetails> deserialiseFunc)
        {
            this.CommandKey = commandKey;

            contextDetails = CommandDataManager.GetHostContextForCommandKey(commandKey, deserialiseFunc);
            this.Command = CommandDataManager.GetCommandByCommandKey(commandKey);
        }

        public void LoadCommand(string json, string serviceName)
        {
            ServiceName = serviceName;
            Command = CommandDataManager.CreateCommand(json);
        }

        public void MarkUnAuthenticated()
        {
            CommandDataManager.MarkCommandUnAuthenticated(CommandKey);
        }

        public void MarkUnAuthorized()
        {
            CommandDataManager.MarkCommandUnAuthorized(CommandKey);
        }

        public void CompleteCommandAuthorised()
        {
            CommandDataManager.SetCommandCompleteAndAuthorised(CommandKey);
        }

        public void FailCommand(string errorMessage)
        {
            CommandDataManager.SetCommandFailed(CommandKey);
        }

        public void CompleteCommand()
        {
            CommandDataManager.SetCommandCompleted(CommandKey);
        }
    }
}