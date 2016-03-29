using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SAHL.Core.Tasks
{
    public class TaskManager : ITaskManager
    {
        private BufferBlock<Action> bufferBlock;
        private ActionBlock<Action> actionBlock;
        private ExecutionDataflowBlockOptions actionBlockConfiguration;

        private void Init()
        {
            bufferBlock = new BufferBlock<Action>();
            actionBlock = new ActionBlock<Action>(a => a(), actionBlockConfiguration);
            bufferBlock.LinkTo(actionBlock);
        }

        public TaskManager()
        {
            actionBlockConfiguration = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 1,
                BoundedCapacity = 1
            };
            Init();
        }

        public TaskManager(ExecutionDataflowBlockOptions actionBlockConfiguration)
        {
            this.actionBlockConfiguration = actionBlockConfiguration;
            Init();
        }

        public Task StartTask(Action action)
        {
            return Task.Factory.StartNew(action);
        }

        public Task<bool> QueueTask(Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            bufferBlock.SendAsync(() =>
            {
                try
                {
                    action();
                    taskCompletionSource.SetResult(true);
                }
                catch (Exception exception)
                {
                    taskCompletionSource.SetException(exception);
                }
            });

            return taskCompletionSource.Task;
        }
    }
}