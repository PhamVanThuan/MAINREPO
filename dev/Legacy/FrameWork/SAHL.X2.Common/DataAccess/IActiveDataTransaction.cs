using System;
using System.Transactions;

namespace SAHL.X2.Common.DataAccess
{
    public interface IActiveDataTransaction : IDisposable
    {
        Guid ID { get; }

        ITransactionContext Context { get; }

        Transaction CurrentTransaction { get; }

        bool IsStarted { get; }

        bool HasCastleManagerAttached { get; set; }
    }
}