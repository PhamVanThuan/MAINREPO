using SAHL.Core.SystemMessages;

namespace SAHL.Services.Interfaces.UserProfile
{
    public interface IUserProfileService
    {
        ISystemMessageCollection PerformCommand<T>(T Command) where T : IUserProfileServiceCommand;
    }
}