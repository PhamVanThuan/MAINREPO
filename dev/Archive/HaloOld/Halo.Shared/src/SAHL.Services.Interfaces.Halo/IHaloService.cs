using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.Halo
{
    public interface IHaloService
    {
        ISystemMessageCollection PerformCommand<T>(T command) where T : IHaloServiceCommand;
    }
}