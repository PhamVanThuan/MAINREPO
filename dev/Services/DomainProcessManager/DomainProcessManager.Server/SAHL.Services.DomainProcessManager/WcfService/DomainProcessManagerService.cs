using SAHL.Core;
using SAHL.Core.Services;
using SAHL.Services.DomainProcessManager.CommandHandlers;
using SAHL.Services.Interfaces.DomainProcessManager;
using System;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DomainProcessManager.WcfService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class DomainProcessManagerService : IDomainProcessManagerService
    {
        private readonly IIocContainer iocContainer;

        public DomainProcessManagerService(IIocContainer iocContainer)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            this.iocContainer = iocContainer;
        }

        public Task<StartDomainProcessResponse> StartDomainProcess(StartDomainProcessCommand command)
        {
            var taskCompletionSource = new TaskCompletionSource<StartDomainProcessResponse>();

            try
            {
                var handler = new StartDomainServiceCommandHandler(this.iocContainer);

                var handleResult = handler.HandleCommand(command);
                if (handleResult.HasErrors)
                {
                    var errorMessage = new StringBuilder();
                    foreach (var currentMessage in handleResult.AllMessages)
                    {
                        errorMessage.AppendLine(currentMessage.Message);
                    }

                    throw new Exception(errorMessage.ToString());
                }

                taskCompletionSource.SetResult(command.Result);
            }
            catch (Exception runtimeException)
            {
                taskCompletionSource.SetException(runtimeException);
            }

            return taskCompletionSource.Task;
        }
    }
}