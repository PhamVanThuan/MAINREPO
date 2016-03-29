using SAHL.Core.Data;

namespace SAHL.Services.Interfaces.DomainProcessManager
{
    public interface IStartDomainProcessResponse
    {
        bool Result { get; }
        IDataModel Data { get; }
    }
}