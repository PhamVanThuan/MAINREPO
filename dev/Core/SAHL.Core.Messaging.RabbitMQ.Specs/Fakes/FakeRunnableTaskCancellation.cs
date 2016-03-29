using SAHL.Core.Tasks;
using System.Threading;

namespace SAHL.Core.Messaging.RabbitMQ.Specs.Fakes
{
    public class FakeRunnableTaskCancellation : IRunnableTaskCancellation
    {
        public FakeRunnableTaskCancellation()
        {
            this.Token = new CancellationToken();
        }

        public CancellationToken Token
        {
            get;protected set;
        }

        public void Cancel()
        {
            WasCancelled = true;
        }

        public bool IsCancelled()
        {
            return WasCancelled;
        }

        public bool WasCancelled
        {
            get; protected set;
        }
    }
}