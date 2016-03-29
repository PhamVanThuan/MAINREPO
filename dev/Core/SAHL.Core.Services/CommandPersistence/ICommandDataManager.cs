using System;
using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public interface ICommandDataManager
    {
        int SaveCommand(ICommand command, string serviceName, string contextValues);

        void SetCommandFailed(int commandKey);

        void SetCommandCompleted(int commandKey);

        ICommand GetCommandByCommandKey(int commandKey);

        ICommand CreateCommand(string commandJson, DateTime? commandInsertDate = null);

        IEnumerable<int> GetAllPendingCommands(string serviceName);

        IEnumerable<int> GetAllFailedCommands(string serviceName);

        IContextDetails GetHostContextForCommandKey(int commandKey, Func<string, IContextDetails> deserialiseFunc);

        void MarkCommandUnAuthenticated(int commandKey);

        void MarkCommandUnAuthorized(int commandKey);

        void SetCommandCompleteAndAuthorised(int commandKey);
    }
}