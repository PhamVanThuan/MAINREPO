using SAHL.Core.SystemMessages;

namespace SAHL.Core.Services
{
    public interface IServiceCommandResult
    {
        SystemMessageCollection CommandMessages { get; }
    }

    public interface IServiceCommandResult<T> : IServiceCommandResult
    {
        T ReturnedData { get; }
    }
}