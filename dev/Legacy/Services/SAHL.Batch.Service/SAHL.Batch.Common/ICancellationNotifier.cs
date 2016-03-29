using System.Threading;

namespace SAHL.Batch.Common
{
    public interface ICancellationNotifier
    {
        void Cancel();

        bool IsCancellationRequested { get; }

        CancellationToken GetTokenInstance();
    }
}