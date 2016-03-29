using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainProcessManager;
using System;
using System.Threading.Tasks;

namespace SAHL.Config.Services.DomainProcessManager.Client
{
    public class DomainProcessManagerClientApi
    {
        private IDomainProcessManagerClient service;
        private StartDomainProcessCommand startCommand;
        private IDataModel dataModel;
        private string eventToWaitFor;

        public DomainProcessManagerClientApi(IDomainProcessManagerClient domainProcessManagerService)
        {
            service = domainProcessManagerService;
        }

        public DomainProcessManagerClientApi DataModel(IDataModel dataModel)
        {
            this.dataModel = dataModel;
            return this;
        }

        public DomainProcessManagerClientApi EventToWaitFor(string eventToWaitFor)
        {
            this.eventToWaitFor = eventToWaitFor;
            return this;
        }

        public async Task<IStartDomainProcessResponse> StartProcess()
        {
            this.CreateCommand();

            var messages = await this.service.StartDomainProcess(startCommand);
            return messages;
        }

        private void CreateCommand()
        {
            if (dataModel == null)
            {
                throw new ArgumentException("Data Model is required");
            }
            if (string.IsNullOrWhiteSpace(eventToWaitFor))
            {
                throw new ArgumentException("Event To Wait for is required");
            }

            startCommand = new StartDomainProcessCommand(dataModel, eventToWaitFor);
        }
    }
}