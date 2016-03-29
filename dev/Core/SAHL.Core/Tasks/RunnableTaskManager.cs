using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Tasks
{
    public class RunnableTaskManager : IRunnableTaskManager
    {
        public IRunnableTaskCancellation BuildTaskCancellation()
        {
            return new RunnableTaskCancellation();
        }

        public IRunnableTask CreateTask(Action action, CancellationToken cancellationToken, TaskCreationOptions options)
        {
            return new RunnableTask(action, cancellationToken, options);
        }
    }
}