using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SAHL.Services.Cuttlefish.Worker.Shared
{
    public class MessageWorkerManager : SAHL.Services.Cuttlefish.Worker.Shared.IMessageWorkerManager
    {
        private List<TaskHolder> workers;
        private ICuttlefishServiceConfiguration cuttlefishServiceConfiguration;

        public MessageWorkerManager(ICuttlefishServiceConfiguration cuttlefishServiceConfiguration)
        {
            this.cuttlefishServiceConfiguration = cuttlefishServiceConfiguration;
            this.workers = new List<TaskHolder>();
        }

        public void StartWorker(string messageExchangeName, string messageQueueName, int numberOfWorkersToStart, Action<string> workerAction)
        {
            for (int i = 0; i < numberOfWorkersToStart; i++)
            {
                QueueConsumer qc = new QueueConsumer(this.cuttlefishServiceConfiguration.MessageBusServer,
                    messageExchangeName,
                    messageQueueName,
                    this.cuttlefishServiceConfiguration.MessageBusServerUserName,
                    this.cuttlefishServiceConfiguration.MessageBusServerPassword, workerAction);

                CancellationToken cancel = new CancellationToken();
                Task newTask = new Task(qc.Consume, cancel);

                this.workers.Add(new TaskHolder(newTask, cancel));
                newTask.Start();
            }
        }
    }

    public class TaskHolder
    {
        public TaskHolder(Task task, CancellationToken cancel)
        {
            this.Task = task;
            this.Cancel = cancel;
        }

        public Task Task { get; protected set; }

        public CancellationToken Cancel { get; protected set; }
    }
}