using System.Threading;

namespace SAHL.Core.Tasks
{
    public class RunnableTaskCancellation : IRunnableTaskCancellation
    {
        private CancellationTokenSource tokenSource;

        public RunnableTaskCancellation()
        {
            this.tokenSource = new CancellationTokenSource();
        }

        public CancellationToken Token
        {
            get
            {
                return this.tokenSource.Token;
            }
        }

        public void Cancel()
        {
            this.tokenSource.Cancel();
        }

        public bool IsCancelled()
        {
            return this.tokenSource.IsCancellationRequested;
        }
    }
}