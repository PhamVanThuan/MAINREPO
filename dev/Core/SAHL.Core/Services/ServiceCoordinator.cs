using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Services
{
    public class ServiceCoordinator : IServiceCoordinator, IServiceCoordinatorExecutor
    {
        private ServiceCoordinatorBuilder builder;

        public ServiceCoordinator()
        {
        }

        public IServiceCoordinatorBuilder StartSequence(Func<ISystemMessageCollection> serviceAction, Func<ISystemMessageCollection> compensationAction)
        {
            builder = new ServiceCoordinatorBuilder(this, serviceAction, compensationAction);

            return builder;
        }

        public SystemMessages.ISystemMessageCollection Run()
        {
            var index = 0;
            List<SAHL.Core.Services.ServiceCoordinatorBuilder.ServiceTask> compensatableTasks = new List<ServiceCoordinatorBuilder.ServiceTask>();

            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            // step through the builder
            foreach (var task in this.builder.Tasks)
            {
                // track what we may need to compensate
                try
                {
                    var svcMessages = task.ServiceAction();
                    if (svcMessages.HasErrors)
                    {
                        messages.Aggregate(svcMessages);
                        messages.Aggregate(this.RollbackTasks(compensatableTasks));
                        return messages;
                    }
                    compensatableTasks.Add(task);
                    index++;
                }
                catch (Exception ex)
                {
                    messages.AddMessage(new SystemMessage(ex.Message, SystemMessageSeverityEnum.Exception));
                    messages.Aggregate(this.RollbackTasks(compensatableTasks));
                    return messages;
                }
            }
            return messages;
        }

        protected ISystemMessageCollection RollbackTasks(IList<SAHL.Core.Services.ServiceCoordinatorBuilder.ServiceTask> compensatableTasks)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            compensatableTasks.Reverse();
            foreach (var compTask in compensatableTasks)
            {
                try
                {
                    var svcMessages = compTask.CompensationAction();
                    if (svcMessages.HasErrors)
                    {
                        messages.Aggregate(svcMessages);
                    }
                }
                catch (Exception ex)
                {
                    // log that the compensation failed
                    messages.AddMessage(new SystemMessage(ex.Message, SystemMessageSeverityEnum.Exception));
                }
            }

            return messages;
        }
    }
}