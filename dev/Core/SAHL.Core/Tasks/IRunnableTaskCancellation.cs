using System.Threading;

namespace SAHL.Core.Tasks
{
    public interface IRunnableTaskCancellation
    {
        CancellationToken Token { get; }

        void Cancel();

        bool IsCancelled();
    }
}