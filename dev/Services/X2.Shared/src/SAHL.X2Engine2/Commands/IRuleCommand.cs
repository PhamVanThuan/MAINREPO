using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public interface IRuleCommand : IServiceCommand
    {
        bool Result { get; }

        string Message { get; }
    }
}