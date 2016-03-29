using System;

namespace SAHL.Services.Cuttlefish.Worker.Shared
{
    public interface IQueueConsumer
    {
        void Consume(Action<string> workAction);
    }
}