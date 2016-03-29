using SAHL.Config.Services.DomainProcessManager.Client.DomainProcessManagerWcfService;
using SAHL.Services.Interfaces.DomainProcessManager;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SAHL.Config.Services.DomainProcessManager.Client
{
    public class DomainProcessManagerClient : IDomainProcessManagerClient
    {
        private NameValueCollection nameValueCollection;

        public DomainProcessManagerClient(NameValueCollection nameValueCollection)
        {
            this.nameValueCollection = nameValueCollection;
            this.SetupWcfService();
        }

        public Uri WcfServiceUri
        {
            get
            {
                string settingsValue = nameValueCollection["DomainProcessManagerWcfServiceAddress"];
                return string.IsNullOrEmpty(settingsValue) ? null : new Uri(settingsValue);
            }
        }

        public DomainProcessManagerServiceClient WcfService { get; private set; }

        public Task<IStartDomainProcessResponse> StartDomainProcess(StartDomainProcessCommand command)
        {
            var taskCompletionSource = new TaskCompletionSource<IStartDomainProcessResponse>();

            try
            {
                if ((this.WcfService == null)) { this.SetupWcfService(); }

                var startDomainProcessResult = this.WcfService.StartDomainProcess(command);
                if (!startDomainProcessResult.Result)
                {
                    throw new Exception("Failed to start the Domain Process");
                }

                taskCompletionSource.SetResult(startDomainProcessResult);
            }
            catch (Exception runtimeException)
            {
                taskCompletionSource.SetException(runtimeException);
            }

            return taskCompletionSource.Task;
        }

        private void SetupWcfService()
        {
            this.WcfService = new DomainProcessManagerServiceClient();
            this.WcfService.Endpoint.Address = new EndpointAddress(this.WcfServiceUri);
        }
    }
}