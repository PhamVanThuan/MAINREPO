using System;
using System.Collections.Generic;

namespace SAHL.Core.SystemMessages
{
    public interface ISystemMessageCollection : IDisposable
    {
        void AddMessage(ISystemMessage messageToAdd);

        void AddMessages(IEnumerable<ISystemMessage> messagesToAdd);

        IEnumerable<ISystemMessage> AllMessages { get; }

        void Clear();

        IEnumerable<ISystemMessage> ErrorMessages();

        IEnumerable<ISystemMessage> InfoMessages();

        IEnumerable<ISystemMessage> WarningMessages();

        IEnumerable<ISystemMessage> ExceptionMessages();

        IEnumerable<ISystemMessage> DebugMessages();

        bool HasErrors { get; }

        bool HasWarnings { get; }

        bool HasExceptions { get; }

        void Aggregate(ISystemMessageCollection messageCollection);

        SystemMessage[] CopyMessages();
    }
}