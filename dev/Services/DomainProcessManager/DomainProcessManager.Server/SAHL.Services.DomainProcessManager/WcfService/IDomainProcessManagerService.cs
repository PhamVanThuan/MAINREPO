using System.ServiceModel;
using System.Threading.Tasks;

using SAHL.Core.Services;
using SAHL.Services.Interfaces.DomainProcessManager;

namespace SAHL.Services.DomainProcessManager.WcfService
{
    [ServiceContract]
    public interface IDomainProcessManagerService
    {
        [OperationContract]
        Task<StartDomainProcessResponse> StartDomainProcess(StartDomainProcessCommand command);
    }
}
