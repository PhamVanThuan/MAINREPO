using System;
using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public interface ICommandSession
    {
        void PersistCommand(string serialisedContextDetails);

        int CommandKey { get; set; }

        string ServiceName { get; set; }

        ICommandRetryPolicy CommandRetryPolicy { get; }

        ICommand Command { get; }

        string RunAsUsername { get; }

        IEnumerable<KeyValuePair<string, string>> ContextDetails { get; }

        void FailCommand(string errorMessage);

        void CompleteCommand();

        void LoadCommandForDatabase(int commandKey, Func<string, IContextDetails> deserialiseFunc);

        void LoadCommand(string json, string serviceName);

        void MarkUnAuthenticated();

        void MarkUnAuthorized();

        void CompleteCommandAuthorised();
    }
}