using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Services
{
    public class ServiceCoordinatorBuilder : IServiceCoordinatorBuilder
    {
        public class ServiceTask
        {
            public ServiceTask(Func<ISystemMessageCollection> serviceAction, Func<ISystemMessageCollection> compensationAction)
            {
                this.ServiceAction = serviceAction;
                this.CompensationAction = compensationAction;
            }

            public ServiceTask(Func<ISystemMessageCollection> serviceAction)
            {
                this.ServiceAction = serviceAction;
            }

            public Func<ISystemMessageCollection> ServiceAction { get; protected set; }

            public Func<ISystemMessageCollection> CompensationAction { get; protected set; }
        }

        private List<ServiceTask> taskSequence;
        private IServiceCoordinatorExecutor executor;

        public IEnumerable<ServiceTask> Tasks { get { return this.taskSequence; } }

        public ServiceCoordinatorBuilder(IServiceCoordinatorExecutor executor, Func<ISystemMessageCollection> serviceAction, Func<ISystemMessageCollection> compensationAction)
        {
            this.executor = executor;
            this.taskSequence = new List<ServiceTask>(new ServiceTask[] { new ServiceTask(serviceAction, compensationAction) });
        }

        public IServiceCoordinatorBuilder Then(Func<ISystemMessageCollection> serviceAction, Func<ISystemMessageCollection> compensationAction)
        {
            this.taskSequence.Add(new ServiceTask(serviceAction, compensationAction));
            return this;
        }

        public IServiceCoordinatorExecutor EndSequence()
        {
            return this.executor;
        }

        public IServiceCoordinatorBuilder ThenWithNoCompensationAction(Func<ISystemMessageCollection> serviceAction)
        {
            this.taskSequence.Add(new ServiceTask(serviceAction));
            return this;
        }
    }
}