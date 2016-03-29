using System.Threading.Tasks;


namespace SAHL.Services.Interfaces.DomainProcessManager
{
    public interface IDomainProcessManagerClient
    {
        Task<IStartDomainProcessResponse> StartDomainProcess(StartDomainProcessCommand command);
    }
}
