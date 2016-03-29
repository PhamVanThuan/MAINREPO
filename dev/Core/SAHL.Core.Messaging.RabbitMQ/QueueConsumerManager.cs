using SAHL.Core.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.RabbitMQ
{
    public class QueueConsumerManager : IQueueConsumerManager
    {
        private IQueueConnectionFactory connectionFactory;
        private IQueueConsumerFactory queueConsumerFactory;
        private List<TaskHolder> workers;
        private IQueueConnection connection;
        private IRunnableTaskManager runnableTaskManager;

        public QueueConsumerManager(IQueueConnectionFactory connectionFactory, IQueueConsumerFactory queueConsumerFactory, IRunnableTaskManager runnableTaskManager)
        {
            this.workers = new List<TaskHolder>();
            this.connectionFactory = connectionFactory;
            this.queueConsumerFactory = queueConsumerFactory;
            this.runnableTaskManager = runnableTaskManager;
        }

        public void StartConsumers(IEnumerable<QueueConsumerConfiguration> initalConsumerConfiguration)
        {
            connection = this.connectionFactory.CreateConnection();

            connection.Connect();

            foreach (var consumerConfig in initalConsumerConfiguration)
            {
                for (int i = 0; i < consumerConfig.InitialNumberOfConsumers; i++)
                {
                    var cancellation = this.runnableTaskManager.BuildTaskCancellation();

                    var consumer = this.queueConsumerFactory.CreateConsumer(connection, consumerConfig.ExchangeName, consumerConfig.QueueName, consumerConfig.WorkAction, cancellation.IsCancelled, consumerConfig.ExchangeName + "-" + consumerConfig.QueueName + "-" + i.ToString());

                    var newTask = this.runnableTaskManager.CreateTask(consumer.Consume, cancellation.Token, TaskCreationOptions.LongRunning);

                    this.workers.Add(new TaskHolder(newTask, cancellation));
                }
            }

            foreach (var taskHolder in this.workers)
            {
                taskHolder.RunnableTask.Start();
            }
        }

        public void StopAllConsumers()
        {
            foreach (var kvp in this.workers)
            {
                kvp.TaskCancelCancellation.Cancel();
            }

            try
            {
                Task.WaitAll(this.workers.Select(x => x.RunnableTask.Task).ToArray());
            }
            catch (AggregateException ex)
            {
                // don't throw if its just a task that has been cancelled before it starts
                ex.Handle(e => e is TaskCanceledException);
            }

            this.workers.Clear();
        }

        private bool isDisposing;

        public void Dispose()
        {
            if (!isDisposing)
            {
                isDisposing = true;

                StopAllConsumers();
                if (this.connection != null)
                {
                    this.connection.Dispose();
                    this.connection = null;
                }
            }
        }
    }

    public class TaskHolder
    {
        public TaskHolder(IRunnableTask task, IRunnableTaskCancellation cancel)
        {
            this.RunnableTask = task;
            this.TaskCancelCancellation = cancel;
        }

        public IRunnableTask RunnableTask { get; protected set; }

        public IRunnableTaskCancellation TaskCancelCancellation { get; protected set; }
    }
}