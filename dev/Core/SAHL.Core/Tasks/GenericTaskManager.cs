using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace SAHL.Core.Tasks
{
    public class GenericTaskManager<T> : IGenericTaskManager<T>
    {
        private BufferBlock<Action<T>> bufferBlock;
        private ActionBlock<Action<T>> actionBlock;
        private ExecutionDataflowBlockOptions actionBlockConfiguration;

        private void Init()
        {
            bufferBlock = new BufferBlock<Action<T>>();
            T obj = default(T);
            actionBlock = new ActionBlock<Action<T>>(a => { a(obj); }, actionBlockConfiguration);
            bufferBlock.LinkTo(actionBlock);
        }

        public GenericTaskManager()
        {
            actionBlockConfiguration = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = 1,
                BoundedCapacity = 1
            };
            Init();
        }

        public GenericTaskManager(ExecutionDataflowBlockOptions actionBlockConfiguration)
        {
            this.actionBlockConfiguration = actionBlockConfiguration;
            Init();
        }

        public Task StartTask(Action<T> action, T obj)
        {
            return Task.Factory.StartNew(() => action(obj));
        }

        public Task<bool> QueueTask(Action<T> action, T obj)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();

            bufferBlock.SendAsync((s) =>
            {
                try
                {
                    action(obj);
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