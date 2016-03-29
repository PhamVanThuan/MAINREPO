using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Core.Tasks
{
    public class RunnableTask : IRunnableTask
    {
        public RunnableTask(Action action, CancellationToken cancellationToken, TaskCreationOptions options)
        {
            this.Task = new Task(action, cancellationToken, options);
        }

        public void Start()
        {
            this.Task.Start();
        }

        public Task Task
        {
            get;
            protected set;
        }
    }
}