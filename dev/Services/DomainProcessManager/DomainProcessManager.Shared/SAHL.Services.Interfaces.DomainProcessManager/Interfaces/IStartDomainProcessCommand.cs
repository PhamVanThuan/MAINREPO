using SAHL.Core.Data;
using SAHL.Core.Services;

namespace SAHL.Services.Interfaces.DomainProcessManager
{
    public interface IStartDomainProcessCommand
    {
        IDataModel DataModel { get; }
        string StartEventToWaitFor { get; }
        StartDomainProcessResponse Result { get; set;  }
    }
}
