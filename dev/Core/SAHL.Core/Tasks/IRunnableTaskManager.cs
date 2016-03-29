using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Tasks
{
    public interface IRunnableTaskManager
    {
        IRunnableTask CreateTask(Action action, CancellationToken cancellationToken, TaskCreationOptions options);

        IRunnableTaskCancellation BuildTaskCancellation();
    }
}