using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.DomainProcess;
using SAHL.Core.SystemMessages;
using SAHL.Services.DomainProcessManager.Services;
using SAHL.Services.Interfaces.DomainProcessManager;
using System;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DomainProcessManager.CommandHandlers
{
    public class StartDomainServiceCommandHandler //: IServiceCommandHandler<StartDomainProcessCommand>
    {
        private readonly IIocContainer _iocContainer;

        public StartDomainServiceCommandHandler(IIocContainer iocContainer)
        {
            if (iocContainer == null) { throw new ArgumentNullException("iocContainer"); }
            _iocContainer = iocContainer;
        }

        public ISystemMessageCollection HandleCommand(StartDomainProcessCommand command)
        {
            if (command == null) { throw new ArgumentNullException("command"); }
            var messageCollection = new SystemMessageCollection();

            try
            {
                var domainProcess = this.GetDomainProcess(command.DataModel);

                this.AddDomainProcessToDomainProcessQueue(domainProcess);
                var domainProcessStartResult = this.StartDomainProcessProcessing(domainProcess, command.DataModel, command.StartEventToWaitFor).Result;

                var startDomainProcessResponse = new StartDomainProcessResponse(domainProcessStartResult.Result,
                                                                                domainProcessStartResult.Data);
                command.Result = startDomainProcessResponse;
            }
            catch (Exception runtimeException)
            {
                messageCollection.AddMessage(new SystemMessage(this.BuildErrorMessage(runtimeException), SystemMessageSeverityEnum.Exception));
            }

            return messageCollection;
        }

        private string BuildErrorMessage(Exception runtimeException)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(runtimeException.Message);
            if (runtimeException.InnerException != null)
            {
                errorMessage.AppendLine(this.BuildErrorMessage(runtimeException.InnerException));
            }

            return errorMessage.ToString();
        }

        private IDomainProcess GetDomainProcess<TModel>(TModel dataModel) where TModel : class, IDataModel
        {
            var domainProcessGenericType = typeof(IDomainProcess<>);
            var genericType = domainProcessGenericType.MakeGenericType(dataModel.GetType());

            var domainProcess = _iocContainer.GetInstance(genericType) as IDomainProcess;
            if (domainProcess == null)
            {
                throw new InstanceNotFoundException(string.Format("Unable to find the Domain Process [IDomainProcess<{0}>]", dataModel.GetType().Name));
            }

            return domainProcess;
        }

        private void AddDomainProcessToDomainProcessQueue(IDomainProcess domainProcess)
        {
            var queueService = this.GetDomainProcessQueueService();
            queueService.AddDomainProcess(domainProcess);
        }

        private IDomainProcessCoordinatorService GetDomainProcessQueueService()
        {
            var domainProcessQueueService = _iocContainer.GetInstance<IDomainProcessCoordinatorService>();
            if (domainProcessQueueService == null)
            {
                throw new InstanceNotFoundException("Unable to find the Domain Process Queue Service[{0}]");
            }

            return domainProcessQueueService;
        }

        private async Task<IDomainProcessStartResult> StartDomainProcessProcessing<TModel>(IDomainProcess domainProcess, TModel dataModel, string eventToWaitFor) 
            where TModel : class, IDataModel
        {
            return await domainProcess.Start(dataModel, eventToWaitFor);
        }
    }
}