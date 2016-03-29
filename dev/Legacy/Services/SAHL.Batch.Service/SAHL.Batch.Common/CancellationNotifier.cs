using System.Threading;

namespace SAHL.Batch.Common
{
    public class CancellationNotifier : ICancellationNotifier
    {
        private CancellationTokenSource cancellationTokenSource;

        public CancellationNotifier()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        public bool IsCancellationRequested
        {
            get
            {
                return cancellationTokenSource.IsCancellationRequested;
            }
        }

        public CancellationToken GetTokenInstance()
        {
            return cancellationTokenSource.Token;
        }
    }
}