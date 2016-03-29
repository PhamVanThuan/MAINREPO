using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Services.Cuttlefish.Managers
{
    public class QueueConsumerManager : IQueueConsumerManager
    {
        private List<TaskHolder> workers;
        private IQueueConsumerFactory queueConsumerFactory;

        public QueueConsumerManager(IQueueConsumerFactory queueConsumerFactory)
        {
            this.workers = new List<TaskHolder>();
            this.queueConsumerFactory = queueConsumerFactory;
        }

        public void StartConsumer(string queueExchangeName, string queueName, int numberOfWorkersToStart, IQueueConsumerWorker consumerWorker)
        {
            for (int i = 0; i < numberOfWorkersToStart; i++)
            {
                CancellationTokenSource cancellation = new CancellationTokenSource();
                Func<bool> shouldCancel = () => { return cancellation.IsCancellationRequested; };
                IQueueConsumer consumer = this.queueConsumerFactory.CreateQueueConsumer(queueExchangeName, queueName, consumerWorker.ProcessMessage, shouldCancel);

                Task newTask = new Task(consumer.Consume, cancellation.Token);

                this.workers.Add(new TaskHolder(newTask, cancellation));
                newTask.Start();
            }
        }

        public int GetNumberOfRunningConsumers()
        {
            return this.workers.Count;
        }

        public void StopAllConsumers()
        {
            foreach (var kvp in this.workers)
            {
                kvp.Cancel.Cancel();
            }

            try
            {
                Task.WaitAll(this.workers.Select(x => x.Task).ToArray(), 6000);
            }
            catch (AggregateException ex)
            {
                // don't throw if its just a task that has been cancelled before it starts
                ex.Handle(e => e is TaskCanceledException);
                Console.WriteLine("TaskFailure.");
            }

            this.workers.Clear();
        }
    }

    public class TaskHolder
    {
        public TaskHolder(Task task, CancellationTokenSource cancel)
        {
            this.Task = task;
            this.Cancel = cancel;
        }

        public Task Task { get; protected set; }

        public CancellationTokenSource Cancel { get; protected set; }
    }
}