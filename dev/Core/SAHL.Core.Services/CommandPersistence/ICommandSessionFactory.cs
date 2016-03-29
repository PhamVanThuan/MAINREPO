using System;
using System.Collections.Generic;

namespace SAHL.Core.Services.CommandPersistence
{
    public interface ICommandSessionFactory
    {
        ICommandSession CreateNewCommandManager(string json);

        ICommandSession CreateNewCommandManager(int commandKey, Func<string, IContextDetails> deserialiseFunc);

        IEnumerable<ICommandSession> GetAllPending(Func<string, IContextDetails> deserialiseFunc);

        IEnumerable<ICommandSession> GetAllFailed(Func<string, IContextDetails> deserialiseFunc);

        string ServiceName { get; set; }

        IEnumerable<ICommandSession> GetAllFailedAndPending(Func<string, IContextDetails> deserialiseFunc);

        IEnumerable<ICommandSession> GetAllFailedAndPending(Func<string, IContextDetails> deserialiseFunc, Func<ICommandSession, bool> wherePredicate);
    }
}