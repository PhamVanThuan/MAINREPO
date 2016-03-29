using SAHL.Shared.DataAccess;
using SAHL.Shared.Messages;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SAHL.Batch.Common
{
    public interface IRepository
    {
        void Save<TMessage>(TMessage message) where TMessage : IBatchMessage;
                
        void Update<TMessage>(TMessage message, GenericStatuses genericStatus) where TMessage : IBatchMessage;

        IEnumerable<TMessage> Load<TMessage>(GenericStatuses genericStatus, int numberOfAttemptsLimit) where TMessage : IBatchMessage;
    }
}