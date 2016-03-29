using SAHL.Core.Data;

namespace SAHL.Core.DomainProcess
{
    public interface IDomainProcessStartResult
    {
        bool Result { get; }

        IDataModel Data { get; }
    }
}